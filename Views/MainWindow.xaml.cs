using System;
using System.Windows;


namespace EELBALL_TRACKER
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
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
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
    }
}
