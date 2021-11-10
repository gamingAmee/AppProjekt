using AppProjekt.Models;
using AppProjekt.Views;
using Microcharts;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace AppProjekt.ViewModels
{
    public class TelemetricsViewModel : BaseViewModel
    {
        public ObservableCollection<Telemetrics> Telemetrics { get; }
        public Command LoadItemsCommand { get; }

        public TelemetricsViewModel()
        {
            Title = "Browse";
            Telemetrics = new ObservableCollection<Telemetrics>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }


        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Telemetrics.Clear();
                var telemetrics = await _telemetricsService.GetTelemetricsAsync();
                foreach (var item in telemetrics)
                {
                    Telemetrics.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        //public Chart TempChart
        //{

        //    get
        //    {
        //        var entries = Telemetrics.Select(p => p.Temperatur).AsEnumerable();
        //        return new LineChart { Entries = entries };
        //    }

        //}
    }
}