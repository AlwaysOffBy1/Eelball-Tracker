using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EELBALL_TRACKER
{
    internal class Throw
    {
        public string Thrower { get; set; }
        public string Type { get; set; }
        public string For { get; set; }
        public string PaidBy { get; set; }
        public string Result { get; set; }
        public DateTime ThrowTime { get; set; }
        public string ID { get; set; }

        public Throw(string id)
        {
            Thrower = "EelGuyLIVE";
            Type = "EelBall";
            Result = "Miss";
            For = "Dr_Squekers";
            PaidBy = "republicofcongo";
            ThrowTime = DateTime.Now;
            ID = id;
        }
        public Throw(string thrower, string type, string fr, string paidBy, string result, string id)
        {
            Thrower = thrower;
            Type = type;
            For = fr;
            PaidBy = paidBy;
            Result = result;
            ThrowTime = DateTime.Now;
            ID = id;
        }
    }
}
