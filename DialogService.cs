using System;
using EELBALL_TRACKER.Views;
using EELBALL_TRACKER;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace EELBALL_TRACKER.ViewModels
{
    /*
     * * My understanding of MVVM is the VM should not talk to the view. When sending the view commands
     * the view needs an abstraction. in this case, IDialogService.
     * However, I can't get IDialogeService to tell the view to close. Looks like i might need to use a NuGet package MVVM Light?
     
    !!THIS DOES NOT WORK
    !!This triggers an error in VMCategoryParamAdd because Close is null
    !!I've been trying to find a solution forever but I've been spending hours on getting data from a simple dialogbox.



    public interface IDialogService
    {
        void ShowDialog();
        Action Close { get; set; }
    }


     


    internal class DialogService :IDialogService, INotifyPropertyChanged
    {
        CategoryParamAddWindow CW;

        public event PropertyChangedEventHandler PropertyChanged;

        public Action Close { get; set; }
        public DialogService()
        {
            
        }
        public void ShowDialog()
        {
            CW = new CategoryParamAddWindow();
            CW.ShowDialog();
        }
        public void CloseWindow()
        {
            Close?.Invoke();
        }
    }
    */
}
