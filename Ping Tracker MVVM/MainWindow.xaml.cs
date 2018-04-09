using System.Windows;
using Ping_Tracker.ViewModel;
using System;
using GalaSoft.MvvmLight.Messaging;
using PeanutButter.Toast;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

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

            Messenger.Default.Register<string>(this, "Toast", ShowToast);
        }

        /// <summary>
        /// uses the peanutbutter.dll file to display toast notifications
        /// </summary>
        /// <param name="Error"></param>
        private void ShowToast(string Error)
        {
            var time = DateTime.Now;
            string title = "Title";
            string message = "Message";
            ToastTypes type = ToastTypes.Warning;

            TimeSpan span = time.Subtract(lastUsedTime);

            if (span.Minutes < 2)
                return;

            if (Error == "HighPing")
            {
                title = "High Ping";
                message = "Ping is over 500";
                type = ToastTypes.Warning;
                lastUsedTime = DateTime.Now;
            }
            else if(Error == "Failure")
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
                MessageBox.Show(String.Concat("An error occured trying to show an alert, The alert was: ", message), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //until we had a StaysOpen glag to Drawer, this will help with scroll bars
            var dependencyObject = Mouse.Captured as DependencyObject;
            while (dependencyObject != null)
            {
                if (dependencyObject is ScrollBar) return;
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }

            MenuToggleButton.IsChecked = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}