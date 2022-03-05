using EELBALL_TRACKER.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Windows;

namespace EELBALL_TRACKER
{
    internal class VMThrow : INotifyPropertyChanged
    {
        private Throw currentThrow;
        public Throw CurrentThrow
        {
            get
            {
                return currentThrow;
            }
            set
            {
                OnPropertyRaised("CurrentThrow");
                currentThrow = value;
            }
        }
        public ObservableCollection<Throw> RecentThrows {
            get{return this.recentThrows;}
            set
            {
                OnPropertyRaised("RecentThrows");
                this.recentThrows = value;
            } 
        }
        private ObservableCollection<Throw> recentThrows;
        public ObservableCollection<string> TypesOfBalls
        {
            get => typesOfBalls;
            set
            {
                typesOfBalls = value;
                OnPropertyRaised("TypesOfBalls");
            }
        }
        private ObservableCollection<string> typesOfBalls;

        public ObservableCollection<string> Throwers
        {
            get => throwers;
            set
            {
                throwers = value;
                OnPropertyRaised("Throwers");
            }
        }
        private ObservableCollection<string> throwers;

        public int ThrowCount;
        public ObservableCollection<string> Contestants{
            get{return contestants;}
            set
            {
                OnPropertyRaised("Contestants");
                this.contestants = value;
            }
        }
        private ObservableCollection<string> contestants;
        public RelayCommand CmdRecordResult { get; set; }
        public DatabaseModel DatabaseModel { get; set; }

        private bool isUsingIO;
        public bool IsUsingIO { get { return isUsingIO; }
            set
            {
                isUsingIO = value;
                OnPropertyRaised("IsUsingIO");
            } 
        }

        public VMThrow()
        {
            DatabaseModel = new DatabaseModel();
                Contestants = new ObservableCollection<string>(DatabaseModel.PlayerList);
                TypesOfBalls = new ObservableCollection<string>(DatabaseModel.TypeList);
                Throwers = new ObservableCollection<string>(DatabaseModel.ThrowerList);
                ThrowCount = DatabaseModel.ThrowCount;
                
            RecentThrows = new ObservableCollection<Throw>();
            CurrentThrow = new Throw(ThrowCount+1);
            CmdRecordResult = new RelayCommand(o => { RecordResult(o); }, new Func<bool>(() => ShouldCommandsBeActive()) );

        }
        private bool ShouldCommandsBeActive() { return !IsUsingIO; } //reaaaaally just want to bind RelayCommand.CanExecute to a bool, but i dont think thats possible?
        public async void RecordResult(object stringSource)
        {
            IsUsingIO = true;
            Throw t = new Throw
                (
                    CurrentThrow.Thrower,
                    CurrentThrow.Type,
                    CurrentThrow.For,
                    CurrentThrow.PaidBy,
                    (string)stringSource,
                    CurrentThrow.ID
                );
            CurrentThrow.ID += 1;
            RecentThrows.Add(t); //Is there a way to automatically add values to array without a call? Like a binding on the UI?
            IsUsingIO = !await RecordResultAsync(t);
        }
        private async Task<bool> RecordResultAsync(Throw t)
        {
            await Task.Run(() => DatabaseModel.AppendDatabase(t));
            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyRaised(string propertyname = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
}
