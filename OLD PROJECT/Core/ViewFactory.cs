using EELBALL_TRACKER.Models;
using EELBALL_TRACKER.Views;
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
        //This is very sloppy, but I couldn't get the VM to open a View without looping through InitializeComponent
        //Could probably download something like LightMVVM but im so far along with this I dont want to complicate
        //it when I'm so close
        public RelayCommand CmdOpenDialog;
        public RelayCommand CmdCloseDialog;
        public RelayCommand CmdOpen;
        public Window WindowToOpen;
        public string Value { get; set; } //made generic so this could be reused anywhere
        public string Value2 { get; set; }
        public Action GetData { get; set; }
        public ViewFactory(string windowToOpen)
        {
            switch (windowToOpen)
            {
                case"LeaderboardWindow":
                    WindowToOpen = new LeaderboardWindow();
                    break;
                case "CategoryParamAddWindow":
                    WindowToOpen = new CategoryParamAddWindow();
                    break;
            }
            CmdOpenDialog = new RelayCommand(o => WindowToOpen.ShowDialog());
            CmdOpen = new RelayCommand(o => WindowToOpen.Show());
            CmdCloseDialog = new RelayCommand(o => WindowToOpen.Close());
        }
        public void ShowDialog() //TODO update this method to show any kind of modal, not just CategoryParamAdds
        {
            WindowToOpen.ShowDialog();
            var x = WindowToOpen as CategoryParamAddWindow;
            Value = x.CB_Category;
            Value2 = x.TB_Value;
        }
        public void Show()
        {
            WindowToOpen.Show();
        }
        public void Close()
        {
            WindowToOpen.Close();
        }
    }
}
