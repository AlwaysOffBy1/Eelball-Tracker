using System;
using EELBALL_TRACKER.Views;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EELBALL_TRACKER.ViewModels
{
    public interface IDialogService
    {
        void ShowDialog();
        Action Close { get; set; }
    }




    internal class DialogService :IDialogService
    {
        CategoryParamAddWindow CW;
        public Action Close { get; set; }
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
}
