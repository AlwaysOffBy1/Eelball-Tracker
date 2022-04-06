using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace EELBALL_TRACKER
{
    public class Throw : INotifyPropertyChanged
    {
        //hate how bulky setting properties can be. Can i update the object in the VM so this can thin out? google didnt give a real answer
        private string _thrower;
        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyRaised("Type");
            }
        }
        private string _type;
        public string For
        {
            get => _for;
            set
            {
                _for = value;
                OnPropertyRaised("For");
            }
        }
        private string _for;
        public string PaidBy
        {
            get => _paidBy;
            set
            {
                _paidBy = value;
                OnPropertyRaised("PaidBy");
            }
        }
        private string _paidBy;
        public string Result
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyRaised("Result");
            }
        }
        private string _result;
        public TimeOnly ThrowTime
        {
            get => _throwTime;
            set
            {
                _throwTime = value;
                OnPropertyRaised("ThrowTime");
            }
        }
        private TimeOnly _throwTime;
        public int ID
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyRaised("ID");
            }
        }
        private int _id;
        public bool IsHasBeenRecorded //Like to use "is" as the first word for all bools. kinda funny here
        {
            get => _isHasBeenRecorded;
            set
            {
                _isHasBeenRecorded = value;
                OnPropertyRaised("IsHasBeenRecorded");
            }
        }
        private bool _isHasBeenRecorded;
        public Throw()
        {
            Type = "EELBALL";
            Result = "Miss";
            For = "EelGuyLIVE";
            PaidBy = "EelGuyLIVE";
        }
        public Throw(int id)
        {
            Type = "EELBALL";
            Result = "Miss";
            For = "EelGuyLIVE";
            PaidBy = "EelGuyLIVE";
            ThrowTime = TimeOnly.FromDateTime(DateTime.Now);
            ID = id;
            IsHasBeenRecorded = false;
        }
        public Throw(string type, string fr, string paidBy, string result, int id)
        {
            Type = type;
            For = fr;
            PaidBy = paidBy;
            Result = result;
            ThrowTime = TimeOnly.FromDateTime(DateTime.Now);
            ID = id;
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
