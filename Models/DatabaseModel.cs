using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace EELBALL_TRACKER.Models
{

    internal class DatabaseModel: INotifyPropertyChanged //I really didnt want to implement INotifyPropChanged here but couldnt figure a way to report when application is in the middle of saving so here we are.
    {
        public string FullPath { get; private set; }
        private bool isupdating { get; set; }
        public bool IsUpdating { 
            get 
            {
                return isupdating;
            }
            private set
            { 
                isupdating = value;
                OnPropertyRaised("IsUpdating");
            }
        } //is there a better way to do this? (show that a file is being saved) Probably. 

        public DatabaseModel()
        {
            FullPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\EelBallData.xml";
            CheckForExistingDB();
        }
        private void CheckForExistingDB()
        {
            if (!File.Exists(FullPath))
            {
                IsUpdating = true;
                using (FileStream fs = File.Create(FullPath))
                {
                    try
                    {
                        Assembly.GetExecutingAssembly().GetManifestResourceStream("EELBALL_TRACKER.Assets.BlankData.xml").CopyTo(fs);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            IsUpdating = false;
        }
        public async void AppendDatabase(List<Throw> throws) //Add data to the database. 
        {
            IsUpdating = true;
            
            var task = Task.Run(() => {
                XDocument _doc = XDocument.Load(FullPath);
                foreach (Throw t in throws)
                {
                    _doc.XPathSelectElement("EelBall/Throws").Add
                        (
                        new XElement("Throw",
                            new XElement("Date",
                                new XElement("Month", t.ThrowTime.Month),
                                new XElement("Day", t.ThrowTime.Day),
                                new XElement("Time", t.ThrowTime.TimeOfDay)),
                            new XElement("Thrower", t.Thrower),
                            new XElement("Type", t.Type),
                            new XElement("For", t.For),
                            new XElement("PaidBy", t.PaidBy),
                            new XElement("Result", t.Result)
                            , new XAttribute("ID", t.ID))
                        );
                    _doc.Save(FullPath);
                    Thread.Sleep(300);
                }
            });
            await task.ContinueWith(t =>
            {
                IsUpdating = false;
                //If i want to report something finished
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
