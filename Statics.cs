using System;
using System.Collections.Generic;
using EELBALL_TRACKER.Models;
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
        public static List<string> Contestants;
        public static DatabaseModel DatabaseModel;
        public static List<Throw> ThrowsCurrentSession;
    }
}
