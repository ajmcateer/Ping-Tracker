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
using Ping_Tracker_MVVM.Model;
using MaterialDesignThemes.Wpf;

namespace Ping_Tracker.ViewModel
{

    public class MainViewModel : ViewModelBase
    {

        private string _currentPage;
        private List<PageModel> _pageModels;

        SnackbarMessageQueue _snackbar;

        public SnackbarMessageQueue SnackBarTest
        {
            get
            {
                return _snackbar;
            }
            set
            {
                _snackbar = value;
                RaisePropertyChanged("SnackBar");
            }
        }

        public string CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value;
                RaisePropertyChanged("Average");
            }
        }
        public List<PageModel> PageModel
        {
            get
            {
                return _pageModels;
            }
            set
            {
                _pageModels = value;
                RaisePropertyChanged("PageModel");
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(ISnackbarMessageQueue snack)
        {
            PageModel = new List<PageModel>();
            PageModel.Add(new PageModel("Ping", "..\\Views\\PingUI.xaml"));
            PageModel.Add(new PageModel("Options", "..\\Views\\Options.xaml"));
            CurrentPage = PageModel[0].Content;
            SnackBarTest = (SnackbarMessageQueue)snack;
            SnackBarTest.Enqueue("Test");
        }
    }
}