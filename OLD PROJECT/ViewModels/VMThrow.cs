using EELBALL_TRACKER.Models;
using EELBALL_TRACKER;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.CompilerServices;

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
                OnPropertyRaised();
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
                OnPropertyRaised();
            }
        }
        private ObservableCollection<string> typesOfBalls;

        public ObservableCollection<string> Throwers
        {
            get => throwers;
            set
            {
                throwers = value;
                OnPropertyRaised();
            }
        }
        private ObservableCollection<string> throwers;

        public int ThrowCount;
        public ObservableCollection<string> Contestants{
            get{return contestants;}
            set
            {
                OnPropertyRaised();
                this.contestants = value;
            }
        }
        private ObservableCollection<string> contestants;

        public RelayCommand CmdRecordResult { get; set; }
        public RelayCommand CmdSelectPaidBy { get; set; }
        public RelayCommand CmdAddContestant { get; set; }
        public RelayCommand CmdAddCategory { get; set; }
        public RelayCommand CmdAddCategoryParam { get; set; }
        public RelayCommand CmdForceSave { get; set; }
        public RelayCommand CmdLeaderboardShow { get; set; }

        public Action CloseWindow;

        ViewFactory VFDialog = new ViewFactory("CategoryParamAddWindow"); //need to use Views to access, but MVVM says i shouldnt use Views in VMs. UGH
        ViewFactory VFLeaderboard = new ViewFactory("LeaderboardWindow");
        
        public bool IsUsingIO { get { return isUsingIO; }
            set
            {
                isUsingIO = value;
                OnPropertyRaised("IsUsingIO");
            } 
        }
        private bool isUsingIO;
        public VMThrow()
        {
            //TODO replace event listeners with actions?
            //My understanding of async/await would tell me that async delegates should contain async methods. async really spreads fast
            Contestants = new ObservableCollection<string>(Statics.DatabaseModel.PlayerList);
                Contestants.CollectionChanged += async delegate (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs args)
                {
                    await Task.Run(() => AddToObservableCollectionAsync(sender, args, "Contestants")); 
                };
                
            TypesOfBalls = new ObservableCollection<string>(Statics.DatabaseModel.TypeList);
                TypesOfBalls.CollectionChanged += async delegate (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs args)
                {
                    await Task.Run(() => AddToObservableCollectionAsync(sender, args, "Types"));
                };
                
            Throwers = new ObservableCollection<string>(Statics.DatabaseModel.ThrowerList);
                Throwers.CollectionChanged += async delegate (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs args)
                {
                    await Task.Run(() => AddToObservableCollectionAsync(sender, args, "Throwers"));
                };
                
            

            CurrentThrow = new Throw();
            currentThrow.ID = 1;
            RecentThrows = new ObservableCollection<Throw>();
            
            CmdRecordResult = new RelayCommand(o => { _ = RecordResult(o); }, new Func<bool>(() => ShouldCommandsBeActive()) );
            CmdSelectPaidBy = new RelayCommand(o => { SelectPaidBy(o); }); //for a small app like this i know it seems kinda silly to use commands instead of just triggers, but i really need the practice
            CmdAddContestant = new RelayCommand(o => { Contestants.Add(o.ToString()); });
            CmdAddCategory = new RelayCommand((o,o2) => { AddCategoryValue(o, o2); });
            CmdAddCategoryParam = new RelayCommand(o => 
            { 
                VFDialog = new ViewFactory("CategoryParamAddWindow"); 
                VFDialog.ShowDialog();
                if(VFDialog.Value is not null)
                {
                    string str = VFDialog.Value.Remove(0, 38); //remove System.Windows.Controls.ComboboxItem: Data
                    switch (str)
                    {
                        case "Throwers":
                            Throwers.Add(VFDialog.Value2);
                            break;
                        case "Balls":
                            TypesOfBalls.Add(VFDialog.Value2);
                            break;
                        case "Contestants":
                            Contestants.Add(VFDialog.Value2);
                            break;
                    }
                }
            });
            CmdForceSave = new RelayCommand(o => { _ = ForceSaveAsync(); }, new Func<bool>(() => ShouldCommandsBeActive()));
            CmdLeaderboardShow = new RelayCommand(o => VFLeaderboard.Show());

        }
        public async Task ForceSaveAsync()
        {
            IsUsingIO = true;
            await Task.Run(() => Statics.DatabaseModel.ForceDatabaseSave());//this makes SO MUCH SENSE NOW
            IsUsingIO = false;
        }
        public void SelectPaidBy(object stringSource){ CurrentThrow.PaidBy = (string)stringSource;}
        private bool ShouldCommandsBeActive() { return !IsUsingIO; } //reaaaaally just want to bind RelayCommand.CanExecute to a bool, but i dont think thats possible?
        private void AddCategoryValue(string category, string value)
        {
            switch (category)
            {
                case "Contestants":
                    Contestants.Add(value);
                    break;
                case "Types":
                    TypesOfBalls.Add(value);
                    break;
                case "Throwers":
                    Throwers.Add(value);
                    break;
            }
        }
        private async Task RecordResult(object stringSource)
        {
            IsUsingIO = true;
            Throw t = new Throw
                (
                    CurrentThrow.Type,
                    CurrentThrow.For,
                    CurrentThrow.PaidBy,
                    (string)stringSource,
                    CurrentThrow.ID
                ); 
            CurrentThrow.ID += 1;
            RecentThrows.Add(t); //Is there a way to automatically add values to array without a call? Like a binding on the UI?
            await Task.Run(() => Statics.DatabaseModel.AppendDatabase(t));
            Statics.ThrowsCurrentSession.Add(t);
            IsUsingIO =false;
        }
        private async Task AddToObservableCollectionAsync(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs args, string category)
        {
            IsUsingIO = true;
            
            foreach(string s in args.NewItems)
            {
                Statics.DatabaseModel.AppendCategoryList(category, s);
            }
            //I like await task.delay so the UI has a chance to flicker, giving an indication that something has changed behind the scenes
            await Task.Delay(20);
            IsUsingIO = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised([CallerMemberName] string propertyname = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
}
