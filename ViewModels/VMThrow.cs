using EELBALL_TRACKER.Models;
using EELBALL_TRACKER.ViewModels;
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
        public RelayCommand CmdAddContestant { get; set; }
        public RelayCommand CmdAddCategory { get; set; }
        public RelayCommand CmdAddCategoryParam { get; set; }

        public DatabaseModel DatabaseModel { get; set; }  
        public VMAddCategoryParam CategoryParam { get; set; }
        
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
            //Pull in data from XML file and add action listeners to the .add method.

            DatabaseModel = new DatabaseModel();
            
            Contestants = new ObservableCollection<string>(DatabaseModel.PlayerList);
                Contestants.CollectionChanged += delegate(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs args)
                {
                    AddToObservableCollectionAsync(sender, args, "Contestants"); 
                };
                
            TypesOfBalls = new ObservableCollection<string>(DatabaseModel.TypeList);
                TypesOfBalls.CollectionChanged += delegate (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs args)
                {
                    AddToObservableCollectionAsync(sender, args, "Types");
                };
                
            Throwers = new ObservableCollection<string>(DatabaseModel.ThrowerList);
                Throwers.CollectionChanged += delegate (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs args)
                {
                    AddToObservableCollectionAsync(sender, args, "Throwers");
                };
                
            ThrowCount = DatabaseModel.ThrowCount;
            CategoryParam = new VMAddCategoryParam();

            RecentThrows = new ObservableCollection<Throw>();
            CurrentThrow = new Throw(ThrowCount + 1);
            CmdRecordResult = new RelayCommand(o => { RecordResult(o); }, new Func<bool>(() => ShouldCommandsBeActive()) );
            CmdSelectPaidBy = new RelayCommand(o => { SelectPaidBy(o); }); //for a small app like this i know it seems kinda silly to use commands instead of just triggers, but i really need the practice
            CmdAddContestant = new RelayCommand(o => { Contestants.Add(o.ToString()); });
            CmdAddCategory = new RelayCommand((o,o2) => { AddCategoryValue(o, o2); });
            CmdAddCategoryParam = new RelayCommand(CategoryParam.CmdOpenForm.Execute);

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
        private async void RecordResult(object stringSource)
        //TODO saw online NOT to do async voids, and instead use async Tasks. 
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
            await Task.Run(() => DatabaseModel.AppendDatabase(t));
            IsUsingIO =false;
        }
        private async void AddToObservableCollectionAsync(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs args, string category)
        //"Void-returning async methods have a specific purpose: to make async event handlers" so this one is okay
        {
            IsUsingIO = true;
            
            foreach(string s in args.NewItems)
            {
                DatabaseModel.AppendCategoryList(category, s);
            }
            //I like await task.delay so the UI has a chance to flicker, giving an indication that something has changed behind the scenes
            await Task.Delay(20);
            IsUsingIO = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyRaised(string propertyname = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
}
