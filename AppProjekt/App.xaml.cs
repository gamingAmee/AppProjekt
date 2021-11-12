using AppProjekt.Services;
using AppProjekt.Views;
using MonkeyCache.FileStore;
using Repository;
using System;
using TinyIoC;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppProjekt
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            var container = TinyIoCContainer.Current;
            container.Register<IGenericRepository, GenericRepository>();
            container.Register<ITelemetricsService, TelemetricsService>();

            Barrel.ApplicationId = AppInfo.PackageName;
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
