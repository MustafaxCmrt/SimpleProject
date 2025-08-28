using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using SimpleProject.Domain;
using SimpleProject.Services;
using SimpleProject.WebAdmin;
using SimpleProject.WebAdmin.Providers;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using SimpleProject.Data;
using SimpleProject.Domain.Entities;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using SimpleProject.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

//builder.WebHost.UseUrls("http://0.0.0.0:8080");
builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(30); });

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IUserAccessor, UserAccessor>();
builder.Services.AddSingleton<IValidationAttributeAdapterProvider, ValidationAdapterProvider>();

var cs = builder.Configuration.GetConnectionString("Default") ?? builder.Configuration["ConnectionString"];
builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(cs));

StartUp.ConfigureServices(builder.Services, builder.Configuration);

builder.Services.AddScoped<IQrService, QrService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IAnimalService, AnimalService>();
builder.Services.AddScoped<ICollarService, CollarService>();
builder.Services.AddScoped<IFoundReportService, FoundReportService>();
builder.Services.AddScoped<IQrOwnershipService, QrOwnershipService>();
builder.Services.AddScoped<IQrTransferService, QrTransferService>();
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(typeof(SessionFilterAttribute));
    options.Filters.Add(typeof(DateConvertFilterAttribute));
    options.Filters.Add(new ResponseCacheAttribute()
    {
        Duration = 0,
        Location = ResponseCacheLocation.None,
        NoStore = true
    });
}).AddViewOptions(options =>
{
    options.HtmlHelperOptions.FormInputRenderMode = FormInputRenderMode.AlwaysUseCurrentCulture;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SimpleProject API v1");
        c.RoutePrefix = "swagger";
    });
}

var supportedCultures = new List<CultureInfo>() { new("tr-TR") };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(supportedCultures.First()),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
};
var cookieRequestCultureProvider =
    localizationOptions.RequestCultureProviders.OfType<CookieRequestCultureProvider>().First();
cookieRequestCultureProvider.CookieName = Consts.CookieLang;
localizationOptions.RequestCultureProviders.Clear();
localizationOptions.RequestCultureProviders.Add(cookieRequestCultureProvider);

app.UseRequestLocalization(localizationOptions);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

if (!string.IsNullOrEmpty(AppSettings.Current.UploadPath))
{
    app.UseStaticFiles(new StaticFileOptions()
    {
        FileProvider = new PhysicalFileProvider(AppSettings.Current.UploadPath),
        RequestPath = "/upload"
    });
}

app.UseRouting();

app.UseSession();
app.UseCookiePolicy(new CookiePolicyOptions()
{
    Secure = CookieSecurePolicy.SameAsRequest
});

app.UseAuthorization();

app.MapControllers();

app.MapGet("/home/langjs", () =>
    Results.Content("window.t=(k)=>k;", "application/javascript"));

app.MapGet("/health/db", async (AppDbContext db) =>
{
    try
    {
        var ok = await db.Database.CanConnectAsync();
        var dbName = db.Database.GetDbConnection().Database;
        var users = await db.Set<AdminUser>().CountAsync();
        return Results.Ok(new { ok, db = dbName, users });
    }
    catch (Exception ex)
    {
        return Results.Json(new { ok = false, error = ex.Message });
    }
});

app.MapGet("/health/ado", async (IConfiguration cfg) =>
{
    var s = cfg.GetConnectionString("Default") ?? cfg["ConnectionString"];
    try
    {
        await using var con = new SqlConnection(s);
        await con.OpenAsync();
        return Results.Ok(new { ok = true, db = con.Database, server = con.ServerVersion });
    }
    catch (Exception ex)
    {
        return Results.Json(new { ok = false, error = ex.Message, used = s });
    }
});
app.MapPost("/diag/setpass", async (string u, string p, AppDbContext db) =>
{
    var shaUtf8 = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(p))).ToLowerInvariant();
    var rows = await db.Database.ExecuteSqlRawAsync(
        "UPDATE dbo.AdminUser SET Password = {0} WHERE UserName = {1}", shaUtf8, u);
    return Results.Ok(new { updated = rows, saved = shaUtf8.Substring(0, 10) });
});
app.MapGet("/diag/login", async (string u, string p, AppDbContext db) =>
{
    var user = await db.Set<AdminUser>()
        .Where(x => x.UserName == u)
        .Select(x => new { x.Id, x.UserName, x.Status, x.Deleted, x.Password })
        .FirstOrDefaultAsync();

    if (user == null) return Results.Ok(new { found = false });

    var sha = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(p)));
    var shaLower = sha.ToLowerInvariant();

    var stored = user.Password ?? "";
    var storedTrim = stored.Trim();

    var plainOk = string.Equals(storedTrim, p, StringComparison.Ordinal);
    var shaOk = string.Equals(storedTrim, sha, StringComparison.OrdinalIgnoreCase)
                || string.Equals(storedTrim, shaLower, StringComparison.Ordinal);

    return Results.Ok(new
    {
        found = true,
        user.Id,
        user = user.UserName,
        user.Status,
        user.Deleted,
        storedPrefix = storedTrim.Substring(0, Math.Min(10, storedTrim.Length)),
        storedLen = storedTrim.Length,
        sha = shaLower.Substring(0, 10),
        plainOk,
        shaOk
    });
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();