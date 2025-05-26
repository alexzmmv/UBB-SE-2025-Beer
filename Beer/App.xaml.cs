using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using WinUIApp.ProxyServices;
using WinUIApp.Services.DummyServices;

namespace WinUIApp
{
    public partial class App : Application
    {
        private Window? window;
        public App()
        {
            this.InitializeComponent();
        }

        private static IServiceProvider? serviceProvider;
        public static T GetService<T>() where T : class
        {
            return serviceProvider!.GetRequiredService<T>();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            ServiceCollection services = new ServiceCollection();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton<IRatingService, ProxyRatingService>();
            services.AddSingleton<IReviewService, ProxyReviewService>();
            services.AddSingleton<IUserService, UserService>();

            serviceProvider = services.BuildServiceProvider();

            this.window = new Views.MainWindow();
            this.window.Activate();
        }

    }
}