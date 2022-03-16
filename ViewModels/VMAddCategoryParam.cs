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
    //This VM breaks the rules of MVVM. I instantiate the view since using a DialogService wasn't working.
    //Instead of rewriting everything from scratch, I'll let a VM talk to a simple Dialog. Sorry MVVM. 
    //For some mroe details go to DialogService.cs
    internal class VMAddCategoryParam: INotifyPropertyChanged
    {
        public string Category { get; set; }
        public string Value { get; set; }

        CategoryParamAddWindow view;//Yes this breaks the rules of MVVM. DialogService.cs has more info
        public RelayCommand CmdButtonClick { get; set; }
        public RelayCommand CmdOpenForm { get; set; }
        public RelayCommand CmdCloseForm { get; set; }
        public Action Close { get; set; }

        public VMAddCategoryParam() 
        {
            view = new CategoryParamAddWindow();
            CmdOpenForm = new RelayCommand(o => view.ShowDialog());
            CmdCloseForm = new RelayCommand(o => PushDataAndClose(o));
        }
        private void PushDataAndClose(object source)
        {
            
            Task.Delay(1000);
            view.Close();
            
            
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
