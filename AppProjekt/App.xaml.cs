using AppProjekt.Services;
using AppProjekt.Views;
using Repository;
using System;
using TinyIoC;
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
