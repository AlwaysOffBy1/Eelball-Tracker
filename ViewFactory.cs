using EELBALL_TRACKER.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EELBALL_TRACKER
{
    internal class ViewFactory
    {
        public RelayCommand CmdOpenDialog;
        public RelayCommand CmdCloseDialog;
        Window WindowToOpen;
        public ViewFactory(Window windowToOpen)
        {
            WindowToOpen = windowToOpen;
            CmdOpenDialog = new RelayCommand(o => WindowToOpen.ShowDialog()); //?!?! DOES THIS WORK!??!
            CmdCloseDialog = new RelayCommand(o => WindowToOpen.Close());
        }
    }
}
