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
        #region Props
        public ObservableCollection<Telemetrics> Telemetrics { get; }
        private ObservableCollection<ChartEntry> TempEntries = new ObservableCollection<ChartEntry>();
        private ObservableCollection<ChartEntry> HumEntries = new ObservableCollection<ChartEntry>();

        public List<string> TimePick
        {
            get
            {
                return new List<string> { "1 min", "10 mins", "1 hour" };
            }
        }
        private string _selectedTime;
        public string SelectedTime
        {
            get { return _selectedTime; }
            set
            {
                SetProperty(ref _selectedTime, value);
                UpdateCharts(_selectedTime);
            }
        }

        private Chart tempChart;
        public Chart TempChart { get { return tempChart; } set { SetProperty(ref tempChart, value); } }

        private Chart humChart;
        public Chart HumChart { get { return humChart; } set { SetProperty(ref humChart, value); } }
        #endregion

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
                CreateCharts();
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


        #region Chart
        public void CreateCharts()
        {
            foreach (var item in Telemetrics.OrderByDescending(t => t.Timestamp).Take(10))
            {
                var temp = Convert.ToDecimal(item.Temperatur);
                var tempEntry = new ChartEntry((float)temp)
                {
                    Label = item.Timestamp.ToString(),
                    ValueLabel = item.Temperatur,
                };
                var hum = Convert.ToDecimal(item.Humidity);
                var humEntry = new ChartEntry((float)hum)
                {
                    Label = item.Timestamp.ToString(),
                    ValueLabel = item.Humidity
                };
                TempEntries.Add(tempEntry);
                HumEntries.Add(humEntry);

            }
            TempChart = new LineChart()
            {
                Entries = TempEntries,
                PointSize = 10,
                LineSize = 10,
                LabelOrientation = Orientation.Vertical,
                ValueLabelOrientation = Orientation.Horizontal,
                LabelTextSize = 30
            };
            HumChart = new LineChart()
            {
                Entries = HumEntries,
                PointSize = 10,
                LineSize = 10,
                LabelOrientation = Orientation.Vertical,
                ValueLabelOrientation = Orientation.Horizontal,
                LabelTextSize = 30
            };
        }

        void UpdateCharts(string time)
        {
            TempEntries.Clear();
            HumEntries.Clear();
            var lastTelemetrics = Telemetrics.LastOrDefault();
            switch (time)
            {
                case "1 min":
                    foreach (var item in Telemetrics.Where(t => t.Timestamp.Minute == lastTelemetrics.Timestamp.Minute).OrderByDescending(t => t.Timestamp))
                    {
                        var temp = Convert.ToDecimal(item.Temperatur);
                        var tempEntry = new ChartEntry((float)temp)
                        {
                            Label = item.Timestamp.ToString(),
                            ValueLabel = item.Temperatur,
                        };
                        var hum = Convert.ToDecimal(item.Humidity);
                        var humEntry = new ChartEntry((float)hum)
                        {
                            Label = item.Timestamp.ToString(),
                            ValueLabel = item.Humidity,
                        };
                        TempEntries.Add(tempEntry);
                        HumEntries.Add(humEntry);
                    }
                    TempChart = new LineChart()
                    {
                        Entries = TempEntries,
                        PointSize = 10,
                        LineSize = 10,
                        LabelOrientation = Orientation.Vertical,
                        ValueLabelOrientation = Orientation.Horizontal,
                        LabelTextSize = 30
                    };
                    HumChart = new LineChart()
                    {
                        Entries = HumEntries,
                        PointSize = 10,
                        LineSize = 10,
                        LabelOrientation = Orientation.Vertical,
                        ValueLabelOrientation = Orientation.Horizontal,
                        LabelTextSize = 30
                    };
                    break;
                case "10 mins":
                    foreach (var item in Telemetrics.Where(t => t.Timestamp.Minute <= lastTelemetrics.Timestamp.Minute).OrderByDescending(t => t.Timestamp))
                    {
                        var temp = Convert.ToDecimal(item.Temperatur);
                        var tempEntry = new ChartEntry((float)temp)
                        {
                            Label = item.Timestamp.ToString(),
                            ValueLabel = item.Temperatur,
                        };
                        var hum = Convert.ToDecimal(item.Humidity);
                        var humEntry = new ChartEntry((float)hum)
                        {
                            Label = item.Timestamp.ToString(),
                            ValueLabel = item.Humidity,
                        };
                        TempEntries.Add(tempEntry);
                        HumEntries.Add(humEntry);
                    }
                    TempChart = new LineChart()
                    {
                        Entries = TempEntries,
                        PointSize = 10,
                        LineSize = 10,
                        LabelOrientation = Orientation.Vertical,
                        ValueLabelOrientation = Orientation.Horizontal,
                        LabelTextSize = 30
                    };
                    HumChart = new LineChart()
                    {
                        Entries = HumEntries,
                        PointSize = 10,
                        LineSize = 10,
                        LabelOrientation = Orientation.Vertical,
                        ValueLabelOrientation = Orientation.Horizontal,
                        LabelTextSize = 30
                    };
                    break;
                case "1 hour":
                    foreach (var item in Telemetrics.Where(t => t.Timestamp.Hour <= lastTelemetrics.Timestamp.Hour).OrderByDescending(t => t.Timestamp))
                    {
                        var temp = Convert.ToDecimal(item.Temperatur);
                        var tempEntry = new ChartEntry((float)temp)
                        {
                            Label = item.Timestamp.ToString(),
                            ValueLabel = item.Temperatur,
                        };
                        var hum = Convert.ToDecimal(item.Humidity);
                        var humEntry = new ChartEntry((float)hum)
                        {
                            Label = item.Timestamp.ToString(),
                            ValueLabel = item.Humidity,
                        };
                        TempEntries.Add(tempEntry);
                        HumEntries.Add(humEntry);
                    }
                    TempChart = new LineChart()
                    {
                        Entries = TempEntries,
                        PointSize = 10,
                        LineSize = 10,
                        LabelOrientation = Orientation.Vertical,
                        ValueLabelOrientation = Orientation.Horizontal,
                        LabelTextSize = 30
                    };
                    HumChart = new LineChart()
                    {
                        Entries = HumEntries,
                        PointSize = 10,
                        LineSize = 10,
                        LabelOrientation = Orientation.Vertical,
                        ValueLabelOrientation = Orientation.Horizontal,
                        LabelTextSize = 30
                    };
                    break;
                default:
                    break;
            }
        }

        #endregion
        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}

