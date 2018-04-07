using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ping_Tracker_MVVM.Infrastructure
{
    /// <summary>
    /// https://stackoverflow.com/questions/28966819/mvvm-light-5-0-how-to-use-the-navigation-service?answertab=votes#tab-top
    /// </summary>
    public interface IFrameNavigationService : INavigationService
    {
        object Parameter { get; }
    }
}
