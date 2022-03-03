using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml.Linq;
using System.Xml.XPath;

namespace EELBALL_TRACKER.Models
{

    internal class DatabaseModel //I really didnt want to implement INotifyPropChanged here but couldnt figure a way to report when application is in the middle of saving so here we are.
    {
        public string FullPath;
        private List<Throw> CacheList;
        private readonly int CacheSize = 5;

        public DatabaseModel()
        {
            FullPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\EelBallData.xml";
            CacheList = new List<Throw>();
            CheckForExistingDB();
        }
        private void CheckForExistingDB()
        {
            if (!File.Exists(FullPath))
            {
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
        }
        public void AppendDatabase(Throw t)
        {
            AppendDatabase(new List<Throw> { t });
        }
        public void AppendDatabase(List<Throw> throws) //If theres enough data, add it to the XML database. Otherwise add it to the cache array. 
        {
            CacheList.AddRange(throws);
            if (CacheList.Count > CacheSize)
            {
                XDocument _doc = XDocument.Load(FullPath);
                foreach (Throw t in CacheList)
                {
                    _doc.XPathSelectElement("EelBall/Throws").Add
                        (
                        new XElement("Throw",
                            new XElement("Date",
                                new XElement("Month", t.ThrowTime.Month),
                                new XElement("Day", t.ThrowTime.Day),
                                new XElement("Time", t.ThrowTime.TimeOfDay.ToString())),
                            new XElement("Thrower", t.Thrower),
                            new XElement("Type", t.Type),
                            new XElement("For", t.For),
                            new XElement("PaidBy", t.PaidBy),
                            new XElement("Result", t.Result)
                            , new XAttribute("ID", t.ID))
                        );
                }
                _doc.Save(FullPath);
                Thread.Sleep(100);
            }
            Thread.Sleep(20);
        }
    }
}
