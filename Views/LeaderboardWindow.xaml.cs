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
    /// Interaction logic for LeaderboardWindow.xaml
    /// </summary>
    public partial class LeaderboardWindow : Window
    {


        public LeaderboardWindow()
        {
            InitializeComponent();
            Loaded += GetCommands;
        }
        private void GetCommands(object sender, RoutedEventArgs e)
        {
            if (DataContext is VMThrow vm)
            {
                vm.CloseWindow += () =>
                {
                    this.Close();
                };
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            e.Equals(true);
            this.Hide();
        }

        //UI ANIMATIONS
        //Not sure where to do these. Could put them in XAML but some of them are pretty complex
        //The simpler animations will go in XAML, and the complex ones here
    }
}
