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

    internal struct DatabaseModel
    {
        private string FullPath;
        public List<string> ThrowerList;
        public List<string> TypeList;
        public List<string> PlayerList;
        public int ThrowCount;
        private List<Throw> CacheList;
        private bool isForceSave;
        private DatabaseThrows dbThrows;

        private readonly int CacheSize = 5;
        private XDocument Doc;

        public DatabaseModel()
        {
            FullPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\EelBallData.xml";
            ThrowerList = new List<string>();
            TypeList = new List<string>();
            PlayerList = new List<string>();
            ThrowCount = 0;
            CacheList = new List<Throw>();
            isForceSave = false;
            Doc = new XDocument(); //hmm
            dbThrows = new DatabaseThrows(new Throw[1]);
            Doc = CheckForExistingDB();
            
            GetData();

        }
        private void GetData() //get all the throwers, ball types, and players from the XML
        {
            if(Doc != null)
            {
                //holy dang LINQ is cool/hard
                //Probably a better way to do this. like make the whole xml file up to throws an ienumerable, then run each querey on that ienumerable instead of the xdocument.
                //but you know what? this list is never gonna be more than like 50 so i guess its ok to waste some millis. gonna add TODO anyway for the OCD

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

                PlayerList = Doc.Descendants("Contestants")
                    .Where(i =>
                    {
                        string c = (string)i.Value;
                        return c != null;
                    })
                    .Descendants("Contestant")
                    .Select(a => a.Value)
                    .ToList();

                dbThrows = new DatabaseThrows(Doc.Descendants("Throw")
                    .Select(a => new Throw()
                    {
                        ID = Int32.Parse(a.Attribute("ID").Value),
                        ThrowTime = new DateTime(
                            Int32.Parse(a.Element("Date").Element("Year").Value),
                            Int32.Parse(a.Element("Date").Element("Month").Value), 
                            Int32.Parse(a.Element("Date").Element("Day").Value),
                            Int32.Parse(a.Element("Date").Element("Time").Value.Split(':')[0]), //time format = hh:mm:ss.mm
                            Int32.Parse(a.Element("Date").Element("Time").Value.Split(':')[1]), //minute
                            Int32.Parse(a.Element("Date").Element("Time").Value.Split(':')[2].Split('.')[0])  //second
                        ),
                        Thrower = a.Element("Thrower").Value,
                        Type = a.Element("Type").Value,
                        For = a.Element("For").Value,
                        PaidBy = a.Element("PaidBy").Value,
                        Result = a.Element("Result").Value
                    })
                    .ToArray());

                    

                ThrowCount = Int32.Parse(Doc.Descendants("TotalThrows").First().Value);
                Statics.ThrowsFromDB = dbThrows;
                Statics.Contestants = ThrowerList;
            }   
        }
        private void StaticDBSet()
        {

        }
        public void AppendCategoryList(string category, string value)
        {
            string subCategory = category.Substring(0, category.Length - 1); //This is really lazy. Throws => Throw. Throwers => Thrower. Players => Player. Maybe I should learn about custom references? Serialization?
            Doc.XPathSelectElement("EelBall/" + category).Add
                (
                    new XElement(subCategory, value)
                );
            Doc.Save(FullPath);
        }
        private XDocument CheckForExistingDB()
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
            return XDocument.Load(FullPath);
        }
        public void AppendDatabase(Throw t)
        {
            AppendDatabase(new List<Throw> { t });
        }
        public void AppendDatabase(List<Throw> throws) //If theres enough data, add it to the XML database. Otherwise add it to the cache list. 
        {
            CacheList.AddRange(throws);
            AppendDatabase();
        }
        private void AppendDatabase()
        {
            //if the cachelist is long, empty it. if forcing save, dont force if cache is empty
            if (CacheList.Count > CacheSize || (isForceSave && CacheList.Count > 0))
            {
                foreach (Throw t in CacheList)
                {
                    Doc.XPathSelectElement("EelBall/Throws").Add
                        (
                        new XElement("Throw",
                            new XElement("Date",
                                new XElement("Month", t.ThrowTime.Month),
                                new XElement("Day", t.ThrowTime.Day),
                                new XElement("Year", t.ThrowTime.Year),
                                new XElement("Time", t.ThrowTime.TimeOfDay.ToString())),
                            new XElement("Thrower", t.Thrower),
                            new XElement("Type", t.Type),
                            new XElement("For", t.For),
                            new XElement("PaidBy", t.PaidBy),
                            new XElement("Result", t.Result)
                            , new XAttribute("ID", t.ID))
                        );
                    ThrowCount++;
                    Doc.XPathSelectElement("EelBall/TotalThrows").Value = ThrowCount.ToString();
                    t.IsHasBeenRecorded = true;
                }
                CacheList = new List<Throw>(); //Reset CacheList since its values have been dumped into DB
                Doc.Save(FullPath); //XDocument does NOT implement IDisposable and uses xmlreader so simply saving without disposal is ok
                Thread.Sleep(1000); //yea you thought it took that long to add to an XML. TODO make a function that decreases the sleep amount as the DB gets bigger
            }
            Thread.Sleep(20);
        }
        public void ForceDatabaseSave()
        {
            //TODO Is there a more elegant way to do this? Perhaps edit the "set" of the bool to auto append data? Is that good practice for a set to run a function?
            isForceSave = true;
            if(CacheList.Count > 0)
            {
                AppendDatabase();
            }
            isForceSave = false;
        }
    }
}

