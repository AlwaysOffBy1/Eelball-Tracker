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
        public string CB_Category;
        public string TB_Value;
        public CategoryParamAddWindow()
        {
            InitializeComponent();
            Loaded += InvokeList;
        }

        private void InvokeList(object sender, RoutedEventArgs e)
        {
            if(DataContext is VMAddCategoryParam vm)
            {
                vm.Close += () =>
                {
                    this.Close();
                };
                vm.PassData += () =>
                {
                    CB_Category = vm.Category;
                    TB_Value = vm.Value;
                };
            }
            
        }
    }
}
