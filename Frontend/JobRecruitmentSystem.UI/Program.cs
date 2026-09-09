using System.Net.Http.Headers;
using System.Text;
using JobRecruitmentSystem.UI.Filters;
using JobRecruitmentSystem.UI.Services;
using JobRecruitmentSystem.UI.Services.Localization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<RequireCompanyProfileFilter>();
});
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<ILocalizer, JsonLocalizer>();

builder.Services.AddHttpClient<ApiClient>(client =>
{
    var baseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7279/api";
    client.BaseAddress = new Uri(baseUrl.EndsWith('/') ? baseUrl : baseUrl + "/");
    // Plain HttpClient sends no User-Agent by default. Some shared-hosting
    // edge layers / WAFs treat that as bot-like traffic and block it even
    // though a real browser hitting the same URL sails through fine.
    client.DefaultRequestHeaders.UserAgent.ParseAdd("JobRecruitmentSystem-UI/1.0 (+server-to-server)");

    // TEMPORARY — SmarterASP.NET's trial hosting gates every request (even
    // server-to-server ones) behind its own IIS-level HTTP Basic Authentication,
    // separate from and in front of our own JWT auth. A browser hitting the
    // site directly gets prompted once and remembers it; this server-side
    // HttpClient never sees that prompt, so it must send the trial credentials
    // itself. That occupies the standard Authorization header, so our own JWT
    // is carried in a custom "X-Access-Token" header instead — see
    // ApiClient.AttachToken and the matching JwtBearerEvents.OnMessageReceived
    // on the API side. REMOVE this whole block (and set TrialSiteAuth:Enabled
    // to false, or delete the section) once the trial period ends / a real
    // domain is attached and the Basic Auth gate goes away.
    var trialAuthRaw = builder.Configuration["TrialSiteAuth:Enabled"];
    var trialAuthEnabled = builder.Configuration.GetValue<bool>("TrialSiteAuth:Enabled");
    Console.WriteLine($"[TrialSiteAuth] raw='{trialAuthRaw ?? "<null>"}' parsed={trialAuthEnabled}");

    if (trialAuthEnabled)
    {
        var trialUser = builder.Configuration["TrialSiteAuth:Username"] ?? string.Empty;
        var trialPassword = builder.Configuration["TrialSiteAuth:Password"] ?? string.Empty;
        var basicValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{trialUser}:{trialPassword}"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicValue);
        Console.WriteLine($"[TrialSiteAuth] Basic Authorization header set for user '{trialUser}' (len={trialUser.Length}).");
    }
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
        // Site is only ever served over HTTPS in production (SmarterASP.NET),
        // and the UI runs as its own IIS application under /ui, so the cookie's
        // path is relative to that app's own root ("/") regardless of the
        // externally-visible subfolder URL.
        options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
            ? CookieSecurePolicy.SameAsRequest
            : CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.Path = "/";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
