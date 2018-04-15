using MaterialDesignThemes.Wpf;
using Ping_Tracker;
using Ping_Tracker.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ping_Tracker_MVVM.Views
{
    /// <summary>
    /// Interaction logic for PingUI.xaml
    /// </summary>
    public partial class PingUI : UserControl
    {
        //MainViewModel _viewModel = new MainViewModel();

        public PingUI()
        {
            InitializeComponent();
            //base.DataContext = _viewModel;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            btnStop.IsEnabled = true;
            btnStart.IsEnabled = false;
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
