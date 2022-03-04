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
        private ObservableCollection<Throw> throwsInDataGrid;
        public ObservableCollection<Throw> ThrowsInDataGrid {
            get{return this.throwsInDataGrid;}
            set
            {
                OnPropertyRaised("ThrowsInDataGrid");
                this.throwsInDataGrid = value;
            } 
        }
        private ObservableCollection<string> contestants;
        public ObservableCollection<string> Contestants{
            get{return contestants;}
            set
            {
                OnPropertyRaised("Contestants");
                this.contestants = value;
            }
        }
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
            CurrentThrow = new Throw("Test Thrower", "SUBBALL", "From", "By", "MISS", "a");
            ThrowsInDataGrid = new ObservableCollection<Throw>();
            Contestants = new ObservableCollection<string>();
            DatabaseModel = new DatabaseModel();
            CmdRecordResult = new RelayCommand(o => { RecordResult(o); }, new Func<bool>(() => ShouldCommandsBeActive()) );

        }
        private bool ShouldCommandsBeActive() { return !IsUsingIO; }
        public async void RecordResult(object stringSource)
        {
            /*
             * Ok I'm trying to use a different thread for IO, then bind a bool to that thread's completion
             * When the IO thread has  started saving the file, the property IsUsingIO should be set to true
             * When the IO thread has finished saving the file, the property IsUsingIO should be set to false
             */
            IsUsingIO = true;
            Throw t = CurrentThrow;
            t.Result = (string)stringSource;
            ThrowsInDataGrid.Add(t);
            IsUsingIO = !await RecordResultAsync(t);
            CurrentThrow = new Throw("b");
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
