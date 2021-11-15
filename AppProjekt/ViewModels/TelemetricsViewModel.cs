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
            }
        }

        private Chart tempChart;
        public Chart TempChart { get { return tempChart; } set { SetProperty(ref tempChart, value); } }

        private Chart humChart;
        public Chart HumChart { get { return humChart; } set { SetProperty(ref humChart, value); } }
        #endregion

        public Command LoadItemsCommand { get; }
        public Command UpdateChartsCommand { get; }

        public TelemetricsViewModel()
        {
            Title = "Telemetrics";
            Telemetrics = new ObservableCollection<Telemetrics>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            UpdateChartsCommand = new Command(() => UpdateCharts(_selectedTime));
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
                var temp = float.Parse(item.Temperatur);
                var tempEntry = new ChartEntry((float)temp)
                {
                    Label = item.Timestamp.ToString(),
                    ValueLabel = item.Temperatur,
                };
                var hum = float.Parse(item.Humidity);
                var humEntry = new ChartEntry(hum)
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
                LineMode = LineMode.Spline,
                LabelOrientation = Orientation.Vertical,
                ValueLabelOrientation = Orientation.Horizontal,
                LabelTextSize = 30
            };
            HumChart = new LineChart()
            {
                Entries = HumEntries,
                PointSize = 10,
                LineMode = LineMode.Spline,
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
                        var temp = float.Parse(item.Temperatur);
                        var tempEntry = new ChartEntry(temp)
                        {
                            Label = item.Timestamp.ToString(),
                            ValueLabel = item.Temperatur,
                        };
                        var hum = float.Parse(item.Humidity);
                        var humEntry = new ChartEntry(hum)
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
                        LineMode = LineMode.Spline,
                        LabelOrientation = Orientation.Vertical,
                        ValueLabelOrientation = Orientation.Horizontal,
                        LabelTextSize = 30
                    };
                    HumChart = new LineChart()
                    {
                        Entries = HumEntries,
                        PointSize = 10,
                        LineMode = LineMode.Spline,
                        LabelOrientation = Orientation.Vertical,
                        ValueLabelOrientation = Orientation.Horizontal,
                        LabelTextSize = 30
                    };
                    break;
                case "10 mins":
                    foreach (var item in Telemetrics.Where(t => t.Timestamp >= lastTelemetrics.Timestamp.AddMinutes(-10)).OrderByDescending(t => t.Timestamp))
                    {
                        var temp = float.Parse(item.Temperatur);
                        var tempEntry = new ChartEntry(temp)
                        {
                            Label = item.Timestamp.ToString(),
                            ValueLabel = item.Temperatur,
                        };
                        var hum = float.Parse(item.Humidity);
                        var humEntry = new ChartEntry(hum)
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
                        LineMode = LineMode.Spline,
                        LabelOrientation = Orientation.Vertical,
                        ValueLabelOrientation = Orientation.Vertical,
                        LabelTextSize = 30
                    };
                    HumChart = new LineChart()
                    {
                        Entries = HumEntries,
                        PointSize = 10,
                        LineMode = LineMode.Spline,
                        LabelOrientation = Orientation.Vertical,
                        ValueLabelOrientation = Orientation.Vertical,
                        LabelTextSize = 30
                    };
                    break;
                case "1 hour":
                    var items = Telemetrics.Where(t => t.Timestamp >= lastTelemetrics.Timestamp.AddMinutes(-60)).OrderByDescending(t => t.Timestamp).GroupBy(t => t.Timestamp.Minute / TimeSpan.FromMinutes(10).Minutes)
                       .Select(g => new
                       {
                           Temp = g.Average(p => Convert.ToDecimal(p.Temperatur)),
                           Hum = g.Average(h => Convert.ToDecimal(h.Humidity)),
                           Timestamp = g.Key
                       });
                    foreach (var item in items)
                    {
                        var tempEntry = new ChartEntry((float)item.Temp)
                        {
                            Label = item.Timestamp.ToString(),
                            ValueLabel = item.Temp.ToString("N"),
                        };
                        var humEntry = new ChartEntry((float)item.Hum)
                        {
                            Label = item.Timestamp.ToString(),
                            ValueLabel = item.Hum.ToString("N"),
                        };
                        TempEntries.Add(tempEntry);
                        HumEntries.Add(humEntry);
                    }
                    TempChart = new LineChart()
                    {
                        Entries = TempEntries,
                        PointSize = 10,
                        LineMode = LineMode.Spline,
                        LabelOrientation = Orientation.Vertical,
                        ValueLabelOrientation = Orientation.Vertical,
                        LabelTextSize = 30
                    };
                    HumChart = new LineChart()
                    {
                        Entries = HumEntries,
                        PointSize = 10,
                        LineMode = LineMode.Spline,
                        LabelOrientation = Orientation.Vertical,
                        ValueLabelOrientation = Orientation.Vertical,
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

