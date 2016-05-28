using Ping_Tracker.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Shapes;

namespace Ping_Tracker
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        public Options(MainViewModel mvm)
        {
            InitializeComponent();
            this.DataContext = mvm;
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            Ping_Tracker_MVVM.Properties.Settings.Default.Save();

            this.Close();
        }
    }
}
