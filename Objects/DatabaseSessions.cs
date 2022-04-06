using System;
using System.Collections;
using EELBALL_TRACKER.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EELBALL_TRACKER
{
    //https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerable?view=net-6.0

    public class DatabaseSessions : IEnumerable
    {
        private Session[] _dbSessions;
        public DatabaseSessions(Session[] sessionsArr)
        {
            _dbSessions = new Session[sessionsArr.Length];
            for (int i = 0; i < sessionsArr.Length; i++)
            {
                _dbSessions[i] = sessionsArr[i];
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        public DatabaseSessionsEnum GetEnumerator()
        {
            return new DatabaseSessionsEnum(_dbSessions);
        }

    }
    public class DatabaseSessionsEnum : IEnumerator
    {
        public Session[] _dbSessions;
        int position = -1;

        public DatabaseSessionsEnum(Session[] list)
        {
            _dbSessions = list;
        }

        object IEnumerator.Current
        {
            get => Current;
        }
        public Session Current
        {
            get
            {
                try
                {
                    return _dbSessions[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
        


        public bool MoveNext()
        {
            position++;
            return position < _dbSessions.Length;
        }

        public void Reset()
        {
            position = -1;
        }
    }
}
