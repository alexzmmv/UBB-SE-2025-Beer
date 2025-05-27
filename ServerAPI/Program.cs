using DataAccess.AuthProviders.Facebook;
using DataAccess.AuthProviders;
using DataAccess.AuthProviders.Github;
using DataAccess.AuthProviders.LinkedIn;
using DataAccess.AutoChecker;
using DataAccess.Repository;
using DataAccess.Service;
using DataAccess.Service.Interfaces;
using DrinkDb_Auth.AuthProviders.Google;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using IRepository;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data;
using WinUiApp.Data.Interfaces;
using WinUIApp.WebAPI.Repositories;
using WinUIApp.WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("DrinkDbClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5078/");
});

// Add services to the container.

builder.Services.AddScoped<IDrinkDTOService, DrinkService>();
builder.Services.AddScoped<IReviewService, ReviewsService>();
builder.Services.AddScoped<IRatingDTOService, RatingDTOService>();
builder.Services.AddScoped<IDrinkRepository, DrinkRepository>();

builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<IReviewsRepository, ReviewsRepository>();

builder.Services.AddScoped<IAppDbContext, AppDbContext>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.MigrationsAssembly("DataAccess")
    ));

builder.Services.AddScoped<ISessionRepository, SessionRepository>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOffensiveWordsRepository, OffensiveWordsRepository>();
builder.Services.AddScoped<IUpgradeRequestsRepository, UpgradeRequestsRepository>();
builder.Services.AddScoped<IRolesRepository, RolesRepository>();
builder.Services.AddScoped<IOffensiveWordsRepository, OffensiveWordsRepository>();

builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUpgradeRequestsService, UpgradeRequestsService>();
builder.Services.AddScoped<IRolesService, RolesService>();
builder.Services.AddScoped<IAuthenticationService>(sp => new AuthenticationService(
    sp.GetRequiredService<ISessionRepository>(),
    sp.GetRequiredService<IUserRepository>(),
    sp.GetRequiredService<LinkedInLocalOAuthServer>(),
    sp.GetRequiredService<GitHubLocalOAuthServer>(),
    sp.GetRequiredService<FacebookLocalOAuthServer>(),
    sp.GetRequiredService<IBasicAuthenticationProvider>()));

builder.Services.AddScoped<LinkedInLocalOAuthServer>(sp =>
    new LinkedInLocalOAuthServer("http://localhost:8891/"));
builder.Services.AddScoped<GitHubLocalOAuthServer>(sp =>
    new GitHubLocalOAuthServer("http://localhost:8890/"));
builder.Services.AddScoped<FacebookLocalOAuthServer>(sp =>
    new FacebookLocalOAuthServer("http://localhost:8888/"));

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
builder.Services.AddScoped<FacebookOAuth2Provider>(sp =>
                    new FacebookOAuth2Provider(
                        sp.GetRequiredService<ISessionService>(),
                        sp.GetRequiredService<IUserService>()));
builder.Services.AddScoped<IFacebookOAuthHelper, FacebookOAuthHelper>();
builder.Services.AddScoped<LinkedInOAuth2Provider>(sp =>
    new LinkedInOAuth2Provider(
        sp.GetRequiredService<IUserService>(),
        sp.GetRequiredService<ISessionService>()));
builder.Services.AddScoped<ILinkedInOAuthHelper>(sp => new LinkedInOAuthHelper(
    "86j0ikb93jm78x",
    "WPL_AP1.pg2Bd1XhCi821VTG.+hatTA==",
    "http://localhost:8891/auth",
    "openid profile email",
    sp.GetRequiredService<LinkedInOAuth2Provider>()));

builder.Services.AddScoped<IAutoCheck, AutoCheck>(sp => new AutoCheck(sp.GetRequiredService<IOffensiveWordsRepository>()));
builder.Services.AddScoped<ICheckersService, CheckersService>();
builder.Services.AddScoped<IBasicAuthenticationProvider>(sp =>
    new BasicAuthenticationProvider(sp.GetRequiredService<IUserService>()));
builder.Services.AddScoped<ITwoFactorAuthenticationService, TwoFactorAuthenticationService>();

builder.Services.AddScoped<ILinkedInLocalOAuthServer>(sp =>
new LinkedInLocalOAuthServer("http://localhost:8891/"));

builder.Services.AddScoped<IGitHubLocalOAuthServer>(sp =>
    new GitHubLocalOAuthServer("http://localhost:8890/"));

builder.Services.AddScoped<IFacebookLocalOAuthServer>(sp =>
    new FacebookLocalOAuthServer("http://localhost:8888/"));

var app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    GitHubLocalOAuthServer gitHubServer = scope.ServiceProvider.GetRequiredService<GitHubLocalOAuthServer>();
    _ = gitHubServer.StartAsync();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
