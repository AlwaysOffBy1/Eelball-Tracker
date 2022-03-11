using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace EELBALL_TRACKER
{
    internal class Throw : INotifyPropertyChanged
    {
        //hate how bulky setting properties can be. Can i update the object in the VM so this can thin out? google didnt give a real answer
        public string Thrower
        {
            get => _thrower;
            set
            {
                _thrower = value;
                OnPropertyRaised("Thrower");
            }
        }
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
        public DateTime ThrowTime
        {
            get => _throwTime;
            set
            {
                _throwTime = value;
                OnPropertyRaised("ThrowTime");
            }
        }
        private DateTime _throwTime;
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

        public Throw(int id)
        {
            Thrower = "EelGuyLIVE";
            Type = "EELBALL";
            Result = "Miss";
            For = "EelGuyLIVE";
            PaidBy = "EelGuyLIVE";
            ThrowTime = DateTime.Now;
            ID = id;
        }
        public Throw(string thrower, string type, string fr, string paidBy, string result, int id)
        {
            Thrower = thrower;
            Type = type;
            For = fr;
            PaidBy = paidBy;
            Result = result;
            ThrowTime = DateTime.Now;
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
