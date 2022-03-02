﻿using EELBALL_TRACKER.Models;
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
            get{return this.outputThrows;}
            set
            {
                OnPropertyRaised("OutputThrows");
                this.outputThrows = value;
            } 
        }
        private ObservableCollection<string> contestants { get; set; }
        public ObservableCollection<string> Contestants{
            get{return contestants;}
            set
            {
                OnPropertyRaised("Contestants");
                this.contestants = value;
            }
        }
        public RelayCommand cmdRecordResult { get; set; }
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
            CurrentThrow = new Throw("z");
            OutputThrows = new ObservableCollection<Throw>();
            Contestants = new ObservableCollection<string>();
            DatabaseModel = new DatabaseModel();
            cmdRecordResult = new RelayCommand(o => { RecordResult(o); }, new Func<bool>(() => ShouldCommandsBeActive()) );

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
            await RecordResultAsync(t);
            CurrentThrow = new Throw("b");

            IsUsingIO = false;
            
        }
        private async Task RecordResultAsync(Throw t)
        {
            await Task.Run(() => DatabaseModel.AppendDatabase(t));
        }
        private void idk() { }
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
