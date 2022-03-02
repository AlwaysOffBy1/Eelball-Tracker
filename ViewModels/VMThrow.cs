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
        public Throw CurrentThrow { get;set; }
        private ObservableCollection<Throw> outputThrows;
        public ObservableCollection<Throw> OutputThrows {
            get
            {
                return this.outputThrows;
            }
            set
            {
                OnPropertyRaised();
                this.outputThrows = value;
            } 
        }
        private ObservableCollection<string> contestants { get; set; }
        public ObservableCollection<string> Contestants
        {
            get
            {
                return contestants;
            }
            set
            {
                OnPropertyRaised();
                this.contestants = value;
            }
        }
        public RelayCommand cmdRecordResult { get; set; }
        public DatabaseModel DatabaseModel { get; set; }

        public VMThrow()
        {
            CurrentThrow = new Throw("z");
            OutputThrows = new ObservableCollection<Throw>();
            Contestants = new ObservableCollection<string>();
            DatabaseModel = new DatabaseModel(); 
            cmdRecordResult = new RelayCommand(o => { RecordResult(o); });
            DatabaseModel.AppendDatabase(
                new List<Throw> 
                {
                    new Throw("2"), 
                    new Throw("3"), 
                    new Throw("4"), 
                    new Throw("5")
                }
            );

        }
        public async void RecordResult(object stringSource)
        {
            await Task.Run(() => 
            {
                DatabaseModel.AppendDatabase(new Throw("a")); 
            });
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
