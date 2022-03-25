using EELBALL_TRACKER.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Animation;


using System.Threading;

namespace EELBALL_TRACKER.ViewModels
{
    internal class VMLeaderboard : INotifyPropertyChanged
    {
        public Point[] PointList = new Point[]
        {
            new Point(0,20),
            new Point(30,20),
            new Point(60, 20),
            new Point(90, 20),
            new Point(120,20)
        };
        

        public event PropertyChangedEventHandler PropertyChanged;
        public DoubleAnimation MoveContestantPlace;
        public RelayCommand CmdMoveToPlace { get; set; }
        public List<string> PlaceList //TODO override add method to add textbox to leaderboardwindow
        {
            get => _placeList;
            set
            {
                _placeList = value;
                OnPropertyRaised("PlaceList");
            }
        }  
        private List<string> _placeList;
        public VMLeaderboard()
        {
            
            CmdMoveToPlace = new RelayCommand(MoveToPlace);
        }
        public void MoveToPlace(string control, int place)
        {

        }

        private void OnPropertyRaised(string propertyname = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
}
