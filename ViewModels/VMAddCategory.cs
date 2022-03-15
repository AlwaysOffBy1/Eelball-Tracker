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
        public VMAddCategory() 
        {
            CategoryAddWindow x = new CategoryAddWindow();
            x.Closing += PushDataAndClose;
            x.btn_Cancel.Click +=PushDataAndClose; //need to add event to btn_Cancel and btn_Confirm. winforms loooks really easy right now
            x.ShowDialog();

        }
        private void PushDataAndClose(object sender, CancelEventArgs e)
        {
            string source = sender.ToString();
            switch (source)
            {
                case "Thrower":
                    break;
                case "Ball":
                    break;
                case "Contestant":
                    break;
                case "Result":
                    break;
            }
            Task.Delay(1000);
        }
    }
}
