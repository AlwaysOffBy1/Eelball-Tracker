using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml.Linq;
using System.Xml.XPath;

namespace EELBALL_TRACKER.Models
{

    internal class DatabaseModel //I really didnt want to implement INotifyPropChanged here but couldnt figure a way to report when application is in the middle of saving so here we are.
    {
        public string FullPath;
        public List<string> ThrowerList;
        public List<string> TypeList;
        public List<string> PlayerList;
        public int ThrowCount;
        private List<Throw> CacheList;

        private readonly int CacheSize = 5;




        public DatabaseModel()
        {
            FullPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\EelBallData.xml";
            CacheList = new List<Throw>();
            CheckForExistingDB();
            GetUILists();
        }
        private void GetUILists() //get all the throwers, ball types, and players from the XML
        {
            XDocument _doc = XDocument.Load(FullPath);
            if( _doc != null)
            {
                //holy dang LINQ is cool/hard
                //Probably a better way to do this. like make the whole xml file up to throws an ienumerable, then run each querey on that ienumerable instead of the xdocument.
                //but you know what? this list is never gonna be more than like 50 so i guess its ok to waste some millis
                ThrowerList = _doc.Descendants("Throwers")
                    .Where(i =>
                    {
                        string c = (string)i.Value;
                        return c != null;
                    })
                    .Descendants("Thrower")
                    .Select(a => a.Value)
                    .ToList();

                TypeList = _doc.Descendants("Types")
                    .Where(i =>
                    {
                        string c = (string)i.Value;
                        return c != null;
                    })
                    .Descendants("Type")
                    .Select(a => a.Value)
                    .ToList();

                PlayerList = _doc.Descendants("Players")
                    .Where(i =>
                    {
                        string c = (string)i.Value;
                        return c != null;
                    })
                    .Descendants("Player")
                    .Select(a => a.Value)
                    .ToList();

                ThrowCount = Int32.Parse(_doc.Descendants("TotalThrows").First().Value);
            }   
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
