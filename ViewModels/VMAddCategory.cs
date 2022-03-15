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
    internal class VMAddCategory
    {
        public string Category;
        public string Value;
        public CategoryAddWindow AddWindow;
        public RelayCommand CmdButtonClick { get; set; }
        public VMAddCategory() 
        {
            CmdButtonClick = new RelayCommand(o => { PushDataAndClose(o); });
            AddWindow = new CategoryAddWindow();
            AddWindow.Owner = this;
            AddWindow.ShowDialog();            
            
        }
        public void Show()
        {

        }
        private void PushDataAndClose(object source)
        {
            

            if (source.ToString().Equals("OK"))
            {
                Value = AddWindow.tb_Value.Text;
                Category = AddWindow.cb_Category.Text;
            }
            AddWindow.Close();
            Task.Delay(1000);
        }
    }
}
