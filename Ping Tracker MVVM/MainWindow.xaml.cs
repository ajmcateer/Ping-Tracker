using System.Windows;
using Ping_Tracker.ViewModel;
using System;
using GalaSoft.MvvmLight.Messaging;
using PeanutButter.Toast;

namespace Ping_Tracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        /// 
        MainViewModel _viewModel = new MainViewModel();

        DateTime lastUsedTime = new DateTime();

        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();

            base.DataContext = _viewModel;

            Messenger.Default.Register<string>(this, "Toast", ShowToast);
        }

        private void ShowToast(string obj)
        {
            var time = DateTime.Now;
            string title = "Title";
            string message = "Message";
            ToastTypes type = ToastTypes.Warning;

            TimeSpan span = time.Subtract(lastUsedTime);

            if (span.Minutes < 2)
                return;

            if (obj == "HighPing")
            {
                title = "High Ping";
                message = "Ping is over 500";
                type = ToastTypes.Warning;
                lastUsedTime = DateTime.Now;
            }
            else if(obj == "Failure")
            {
                title = "Ping Timeout";
                message = "Ping Timed out";
                type = ToastTypes.Error;
                lastUsedTime = DateTime.Now;
            }

            Toaster toaster = new Toaster();
            try
            {
                toaster.Show(title, message, type);
            }
            catch (Exception ex)
            {

            }
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Options opt = new Options(_viewModel);
            opt.ShowDialog();
        }
    }
}