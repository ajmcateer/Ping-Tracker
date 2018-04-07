using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Ping_Tracker.Model;
using System.Threading;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Linq;
using LiveCharts;
using PeanutButter.Toast;
using GalaSoft.MvvmLight.Messaging;
using System.Configuration;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
using System.Threading.Tasks;
using System.Net.Http;
using LiveCharts.Wpf;

namespace Ping_Tracker.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        public RelayCommand StartCommand { get; private set; }
        public RelayCommand StopCommand { get; private set; }

        private bool threadStop = false;

        private Thread thread;

        private List<long> _averagePings;

        private CustomQueue _pingQueue;
        private string _URL;
        private string _extIP;
        private string _status;
        private string _average;

        private int _pingInterval;
        private int _pingsToSave;
        private string _pingsSent;

        private SynchronizationContext uiContext = SynchronizationContext.Current;

        public LimitedSeries dataPoints { get; private set; }

        public CustomQueue PingQueue
        {
            get
            {
                if (_pingQueue == null)
                    _pingQueue = new CustomQueue(PingsToSave);

                return _pingQueue;
            }
            set
            {
                _pingQueue = value;
                RaisePropertyChanged("PingQueue");
            }
        }

        public string Average
        {
            get
            {
                return _average;
            }
            set
            {
                _average = Math.Round(Convert.ToDouble(value), 2).ToString();
                RaisePropertyChanged("Average");
            }
        }

        public string ExtIP
        {
            get
            {
                return _extIP;
            }
            set
            {
                _extIP = value;
                RaisePropertyChanged("ExtIP");
            }
        }

        public string URL
        {
            get
            {
                return _URL;
            }
            set
            {
                _URL = value;
                RaisePropertyChanged("URL");
            }
        }

        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                RaisePropertyChanged("Status");
            }
        }

        public List<long> AveragePings
        {
            get
            {
                if (_averagePings == null)
                    _averagePings = new List<long>();

                return _averagePings;
            }
            set
            {
                _averagePings = value;
                RaisePropertyChanged("AveragePings");
            }
        }

        public int PingInterval
        {
            get
            {
                return _pingInterval;
            }
            set
            {
                _pingInterval = value;
                Ping_Tracker_MVVM.Properties.Settings.Default.PingInterval = _pingInterval;
                RaisePropertyChanged("PingInterval");
            }
        }

        public string PingsSent
        {
            get
            {
                return _pingsSent;
            }
            set
            {
                _pingsSent = value;
                RaisePropertyChanged("PingsSent");
            }
        }

        public int PingsToSave
        {
            get
            {
                return _pingsToSave;
            }
            set
            {
                _pingsToSave = value;
                Ping_Tracker_MVVM.Properties.Settings.Default.PingsToSave = _pingsToSave;
                RaisePropertyChanged("PingsToSave");
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            AveragePings = new List<long>();

            PingInterval = Ping_Tracker_MVVM.Properties.Settings.Default.PingInterval;
            PingsToSave = Ping_Tracker_MVVM.Properties.Settings.Default.PingsToSave;

            Console.WriteLine(PingInterval.ToString());
            Console.WriteLine(PingsToSave.ToString());

            dataPoints = new LimitedSeries(PingsToSave);

            var point = new LineSeries
            {
                Title = "Ping Times",
                Values = new ChartValues<double> { 0 },
                PointGeometry = DefaultGeometries.None
            };

            //add our series to our SeriesCollection
            dataPoints.Series.Add(point);

            this.StartCommand = new RelayCommand(Start);
            this.StopCommand = new RelayCommand(Stop);

            Task.Run(async () =>
            {
                ExtIP = await GetExtIP();
            });
        }

        /// <summary>
        /// Gets external IP at startup
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetExtIP()
        {
            string responseString = "";

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(@"http://ipecho.net/plain");

                if (response.IsSuccessStatusCode)
                {
                    // by calling .Result you are performing a synchronous call
                    var responseContent = response.Content;

                    // by calling .Result you are synchronously reading the result
                    responseString = await responseContent.ReadAsStringAsync();
                }
            }

            return string.Concat("External IP: ", responseString);
        }

        private void Start()
        {
            threadStop = false;
            thread = new Thread(() => ThreadAction());
            thread.Start();
        }

        /// <summary>
        /// New thread used to ping the selected server
        /// and calculate statistics
        /// </summary>
        private void ThreadAction()
        {
            Console.WriteLine("Thread Started");

            PingQueue = new CustomQueue(100);

            if (!Uri.IsWellFormedUriString(URL, UriKind.RelativeOrAbsolute))
            {
                Console.WriteLine("URL Failed");
                return;
            }

            int Pings = 0;

            while (!threadStop)
            {
                Ping p = new Ping();
                PingReply pr = p.Send(URL);

                AveragePings.Add(pr.RoundtripTime);

                uiContext.Send(x => PingQueue.AddToQueue(pr.RoundtripTime.ToString()), null);
                uiContext.Send(x => dataPoints.AddToQueue((double)pr.RoundtripTime), null);

                Average = GetAverage(AveragePings);
                Status = pr.Status.ToString();

                if(pr.RoundtripTime > 500)
                {
                    uiContext.Send(x => Messenger.Default.Send("HighPing", "Toast"), null); 
                }
                if (Status != "Success")
                {
                    uiContext.Send(x => Messenger.Default.Send("Failure", "Toast"), null);
                }

                Pings++;

                PingsSent = "Pings Sent: " + Pings.ToString();

                Thread.Sleep(PingInterval*1000);
            }
            Console.WriteLine("Thread Stopped");
        }

        /// <summary>
        /// averages a list of longs
        /// </summary>
        /// <param name="Pings"></param>
        /// <returns>average of the list of longs converted to a string</returns>
        private string GetAverage(List<long> Pings)
        {
            return Pings.Average().ToString();
        }

        /// <summary>
        /// stops the thread
        /// </summary>
        private void Stop()
        {
            threadStop = true;
        }
    }
}