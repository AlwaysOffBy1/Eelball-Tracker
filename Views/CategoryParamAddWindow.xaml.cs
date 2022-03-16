using EELBALL_TRACKER.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EELBALL_TRACKER.Views
{
    /// <summary>
    /// Interaction logic for CategoryParamAddWindow.xaml
    /// </summary>
    public partial class CategoryParamAddWindow : Window
    {
        public CategoryParamAddWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        //Still wrapping my head around this. Not ok to reference View in VM, but this is somehow better?
        {
            if(DataContext is IDialogService vm)
            {
                vm.Close += () => { vm.Close(); };
            }
        }
    }
}
