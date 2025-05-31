using DataAccess.AuthProviders;
using DataAccess.AuthProviders.Facebook;
using DataAccess.AuthProviders.Github;
using DataAccess.AuthProviders.LinkedIn;
using DataAccess.AuthProviders.Twitter;
using DataAccess.AutoChecker;
using DataAccess.Service.Components;
using DataAccess.Service.Interfaces;
using DataAccess.ServiceProxy;
using DrinkDb_Auth.AuthProviders.Google;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using DrinkDb_Auth.ServiceProxy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data;
using WinUIApp.ProxyServices;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});

DependencyInjection(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
static void DependencyInjection(WebApplicationBuilder builder)
{
    // Still needed for the controllers, they require new functions in services and repos, so I didn't bother doing it now
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    builder.Services.AddDbContextFactory<AppDbContext>(options => options.UseSqlServer(connectionString));
    builder.Services.AddScoped<AppDbContext>(sp => sp.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext());

    string apiRoute = "http://localhost:5078/";

    builder.Services.AddSingleton<ISessionService, SessionServiceProxy>(sp => new SessionServiceProxy(apiRoute));
    builder.Services.AddSingleton<IAuthenticationService>(sp => new AuthenticationServiceProxy(apiRoute));
    builder.Services.AddSingleton<IUserService>(sp => new UserServiceProxy(apiRoute));
    builder.Services.AddSingleton<ICheckersService>(sp => new OffensiveWordsServiceProxy(apiRoute));
    builder.Services.AddSingleton<IReviewService>(sp => new ReviewsServiceProxy(apiRoute));
    builder.Services.AddSingleton<IUpgradeRequestsService>(sp => new UpgradeRequestsServiceProxy(apiRoute));
    builder.Services.AddSingleton<IDrinkModificationRequestService>(sp => new DrinkModificationRequestServiceProxy(apiRoute));
    builder.Services.AddSingleton<IRolesService, RolesProxyService>(sp => new RolesProxyService(apiRoute));
    builder.Services.AddSingleton<IAutoCheck, AutoCheckerProxy>(sp => new AutoCheckerProxy(apiRoute));
    builder.Services.AddSingleton<IBasicAuthenticationProvider>(sp => new BasicAuthenticationProviderServiceProxy(apiRoute));
    builder.Services.AddSingleton<ITwoFactorAuthenticationService>(sp => new TwoFactorAuthenticationServiceProxy(apiRoute));
    builder.Services.AddSingleton<IDrinkService, ProxyDrinkService>(sp => new ProxyDrinkService(apiRoute));

    builder.Services.AddSingleton<LinkedInLocalOAuthServer>(sp =>
        new LinkedInLocalOAuthServer("http://localhost:8891/"));
    builder.Services.AddSingleton<GitHubLocalOAuthServer>(sp =>
        new GitHubLocalOAuthServer("http://localhost:8890/"));
    builder.Services.AddSingleton<FacebookLocalOAuthServer>(sp =>
        new FacebookLocalOAuthServer("http://localhost:8888/"));

    builder.Services.AddSingleton<ILinkedInLocalOAuthServer>(sp =>
        sp.GetRequiredService<LinkedInLocalOAuthServer>());
    builder.Services.AddSingleton<IGitHubLocalOAuthServer>(sp =>
        sp.GetRequiredService<GitHubLocalOAuthServer>());
    builder.Services.AddScoped<FacebookOAuth2Provider>(sp =>
                        new FacebookOAuth2Provider(
                            sp.GetRequiredService<ISessionService>(),
                            sp.GetRequiredService<IUserService>()));
    builder.Services.AddSingleton<IFacebookLocalOAuthServer>(sp =>
        sp.GetRequiredService<FacebookLocalOAuthServer>());

    builder.Services.AddScoped<IGitHubHttpHelper, GitHubHttpHelper>();
    builder.Services.AddScoped<GitHubOAuth2Provider>(sp =>
        new GitHubOAuth2Provider(
            sp.GetRequiredService<IUserService>(),
            sp.GetRequiredService<ISessionService>()));
    builder.Services.AddScoped<IGitHubOAuthHelper>(sp =>
        new GitHubOAuthHelper(
            sp.GetRequiredService<GitHubOAuth2Provider>(),
            sp.GetRequiredService<GitHubLocalOAuthServer>()));
    builder.Services.AddScoped<IGoogleOAuth2Provider, GoogleOAuth2Provider>();
    builder.Services.AddScoped<IFacebookOAuthHelper, FacebookOAuthHelper>();

    builder.Services.AddScoped<LinkedInOAuth2Provider>();
    builder.Services.AddScoped<ILinkedInOAuthHelper>(sp =>
        new LinkedInOAuthHelper(
            "86j0ikb93jm78x",
            "WPL_AP1.pg2Bd1XhCi821VTG.+hatTA==",
            "http://localhost:8891/auth",
            "openid profile email",
            sp.GetRequiredService<LinkedInOAuth2Provider>()));

    builder.Services.AddScoped<IVerify, Verify2FactorAuthenticationSecret>();
    builder.Services.AddScoped<ITwitterOAuth2Provider, TwitterOAuth2Provider>(sp =>
        new TwitterOAuth2Provider(
            sp.GetRequiredService<IUserService>(),
            sp.GetRequiredService<ISessionService>()));
}