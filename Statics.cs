using System;
using System.Collections.Generic;
using EELBALL_TRACKER.Models;
using EELBALL_TRACKER.Objects;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EELBALL_TRACKER
{
    internal static class Statics
    {
        /// <summary>
        /// Contestant list needs to be used by multiple VMS
        /// Didn't want to do subscriptions since this project is already getting out of hand
        
        /// </summary>
        
        //Contestants should be shared amongst Models/VMs
        public static List<string> Contestants = new List<string>();
        //
        public static DatabaseModel DatabaseModel = new DatabaseModel();

        /*
         * ThrowsFromDB is IEnumerable and gotten at runtime from the database
         * ThrowsCurrentSession will be added to after each throw.
         * LINQ will run on ThrowsFromDB AND ThrowsCurrentSession, then be combined for results
         */
        public static DatabaseSessions ThrowsFromDB = new DatabaseSessions(new Session[1]);
        public static List<Throw> ThrowsCurrentSession = new List<Throw>();
    }
}
