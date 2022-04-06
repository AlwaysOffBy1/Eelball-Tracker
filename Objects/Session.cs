using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EELBALL_TRACKER.Objects
{
    public class Session
    {
        public DateOnly Date { get; set; }
        public List<Throw> Throws { get; set; }

        public Session()
        {
            Throws = new List<Throw>();
            Date = DateOnly.FromDateTime(DateTime.Now);
        }
        public Session(DateOnly date, List<Throw> throws)
        {
            Date = date;
            Throws = throws;
        }
        public Session(DateOnly date)
        {
            Date = date;
        }
    }
}
