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
        private ObservableCollection<ChartEntry> Entries = new ObservableCollection<ChartEntry>();
        public List<string> TimePick
        {
            get
            {
                return new List<string> { "1 min", "10 mins", "20 mins" };
            }
        }

        private string _selectedTime;

        public string SelectedTime
        {
            get { return _selectedTime; }
            set
            {
                SetProperty(ref _selectedTime, value);

            }
        }


        private Chart tempChart;
        public Chart TempChart { get { return tempChart; } set { SetProperty(ref tempChart, value); } }


        public Command LoadItemsCommand { get; }

        public TelemetricsViewModel()
        {
            Title = "Telemetrics";
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
                CreateTempChart();
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

        public void CreateTempChart()
        {
            foreach (var item in Telemetrics.OrderByDescending(t => t.Timestamp).Take(10))
            {
                var test = Convert.ToDecimal(item.Temperatur);
                var entry = new ChartEntry((float)test)
                {
                    Label = item.Timestamp.ToString(),
                    ValueLabel = item.Temperatur,
                };
                Entries.Add(entry);
            }
            TempChart = new LineChart()
            {
                Entries = Entries,
                PointSize = 10,
                LineSize = 10,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                LabelTextSize = 30
            };
        }

        //void UpdateTempChart(string time)
        //{
        //    Entries.Clear();
        //    foreach (var item in Telemetrics.Where(t => t.Timestamp.Minute == 60))
        //    {
        //        var test = Convert.ToDecimal(item.Temperatur);
        //        var entry = new ChartEntry((float)test)
        //        {
        //            Label = item.Timestamp.ToString(),
        //            ValueLabel = item.Temperatur,
        //        };
        //        Entries.Add(entry);
        //    }
        //}

        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}

