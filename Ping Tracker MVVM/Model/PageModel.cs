using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ping_Tracker_MVVM.Model
{
    public class PageModel : INotifyPropertyChanged
    {
        private string _name;
        private string _content;
        public event PropertyChangedEventHandler PropertyChanged;

        public PageModel(string name, string content)
        {
            Name = name;
            Content = content;
        }

        public string Name
        {
            get {
                    return _name;
                }
            set {
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
        }

        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                NotifyPropertyChanged("Content");
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
