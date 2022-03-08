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
        private XDocument Doc;



        public DatabaseModel()
        {
            FullPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\EelBallData.xml";
            CacheList = new List<Throw>();
            CheckForExistingDB();
            GetUILists();
        }
        private void GetUILists() //get all the throwers, ball types, and players from the XML
        {
            if(Doc != null)
            {
                //holy dang LINQ is cool/hard
                //Probably a better way to do this. like make the whole xml file up to throws an ienumerable, then run each querey on that ienumerable instead of the xdocument.
                //but you know what? this list is never gonna be more than like 50 so i guess its ok to waste some millis
                ThrowerList = Doc.Descendants("Throwers")
                    .Where(i =>
                    {
                        string c = (string)i.Value;
                        return c != null;
                    })
                    .Descendants("Thrower")
                    .Select(a => a.Value)
                    .ToList();

                TypeList = Doc.Descendants("Types")
                    .Where(i =>
                    {
                        string c = (string)i.Value;
                        return c != null;
                    })
                    .Descendants("Type")
                    .Select(a => a.Value)
                    .ToList();

                PlayerList = Doc.Descendants("Players")
                    .Where(i =>
                    {
                        string c = (string)i.Value;
                        return c != null;
                    })
                    .Descendants("Player")
                    .Select(a => a.Value)
                    .ToList();

                ThrowCount = Int32.Parse(Doc.Descendants("TotalThrows").First().Value);
            }   
        }
        private void SetUIList(string category, string value)
        {
            string subCategory = category.Substring(0, category.Length - 1); //This is really lazy. Throws => Throw. Throwers => Thrower. Players => Player. Maybe I should learn about custom references? 
            Doc.XPathSelectElement("EelBall/" + category).Add
                (
                    new XElement(subCategory, value)
                );
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
            Doc = XDocument.Load(FullPath);
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
                foreach (Throw t in CacheList)
                {
                    Doc.XPathSelectElement("EelBall/Throws").Add
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
                //XDocument does NOT implement IDisposable and uses xmlreader so simply saving without disposal is ok
                Doc.Save(FullPath);
                Thread.Sleep(100);
            }
            Thread.Sleep(20);
        }
    }
}
