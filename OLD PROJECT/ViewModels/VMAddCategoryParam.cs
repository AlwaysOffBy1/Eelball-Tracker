using EELBALL_TRACKER.Models;
using EELBALL_TRACKER.Views;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EELBALL_TRACKER.ViewModels
{
    internal class VMAddCategoryParam: INotifyPropertyChanged
    {
        public string Category { get; set; }
        public string Value { get; set; }
        public bool CommitData = false;
        public RelayCommand CmdButtonClick { get; set; }
        public RelayCommand CmdOpenForm { get; set; }
        public RelayCommand CmdCloseForm { get; set; }
        public Action Close { get; set; }
        public Action<bool> PassData { get; set; }
        public VMAddCategoryParam() 
        {
            CmdCloseForm = new RelayCommand(o => PushDataAndClose(o));
        }
        private void PushDataAndClose(object stringSource)
        {
            CommitData = stringSource.ToString().Equals("OK");
            PassData.Invoke(CommitData);
            Close?.Invoke();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised(string propertyname = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
}
