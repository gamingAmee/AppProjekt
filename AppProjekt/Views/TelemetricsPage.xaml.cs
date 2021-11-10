using AppProjekt.Models;
using AppProjekt.ViewModels;
using AppProjekt.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppProjekt.Views
{
    public partial class TelemetricsPage : ContentPage
    {
        TelemetricsViewModel _viewModel;

        public TelemetricsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new TelemetricsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}