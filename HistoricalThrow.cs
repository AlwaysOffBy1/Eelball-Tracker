using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EELBALL_TRACKER
{
    internal class HistoricalThrow
    {
        public string Thrower { get;set; }
        public string Type { get;set; } 
        public string For { get;set; }
        public string PaidBy { get;set; }
        public string Result { get;set; } 
        public DateTime ThrowTime { get;set; }

        public HistoricalThrow(string thrower, string type, string fr, string paidBy, string result)
        {
            Thrower = thrower;
            Type = type;
            For = fr;
            PaidBy = paidBy;
            Result = result;
            ThrowTime = DateTime.Now;
        }
    }
}
