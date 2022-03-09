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
        
        public Throw CurrentThrow
        {
            get
            {
                return currentThrow;
            }
            set
            {
                //when selection changed, CurrentThrow does not update. something is wrong here.
                OnPropertyRaised("CurrentThrow");
                currentThrow = value;
            }
        }
        private Throw currentThrow;
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
        public RelayCommand CmdSelectPaidBy { get; set; }
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
            //Pull in data from XML file and add action listeners to the .add method.
            DatabaseModel = new DatabaseModel();
                Contestants = new ObservableCollection<string>(DatabaseModel.PlayerList);
                    Contestants.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(AddToObservableCollection);
                TypesOfBalls = new ObservableCollection<string>(DatabaseModel.TypeList);
                    TypesOfBalls.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(AddToObservableCollection);
                Throwers = new ObservableCollection<string>(DatabaseModel.ThrowerList);
                    Throwers.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(AddToObservableCollection);
                ThrowCount = DatabaseModel.ThrowCount;
                
            RecentThrows = new ObservableCollection<Throw>();
            CurrentThrow = new Throw(ThrowCount+1);
            CmdRecordResult = new RelayCommand(o => { RecordResult(o); }, new Func<bool>(() => ShouldCommandsBeActive()) );
            CmdSelectPaidBy = new RelayCommand(o => { SelectPaidBy(o); }); //for a small app like this i know it seems kinda silly to use commands instead of just triggers, but i really need the practice

        }
        public void SelectPaidBy(object stringSource){ CurrentThrow.PaidBy = (string)stringSource;}
        private bool ShouldCommandsBeActive() { return !IsUsingIO; } //reaaaaally just want to bind RelayCommand.CanExecute to a bool, but i dont think thats possible?
        private async void RecordResult(object stringSource) //TODO saw online NOT to do async voids, and instead use async Tasks. 
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
        private void AddToObservableCollection(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs args)
        {
            MessageBox.Show(sender.ToString()); //TODO add UI to add items. 
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
