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

    public class SessionsList : IEnumerable
    {
        private Session[] _dbSessionsList;
        public SessionsList(Session[] SessionsListArr)
        {
            _dbSessionsList = new Session[SessionsListArr.Length];
            for (int i = 0; i < SessionsListArr.Length; i++)
            {
                _dbSessionsList[i] = SessionsListArr[i];
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        public SessionsListEnum GetEnumerator()
        {
            return new SessionsListEnum(_dbSessionsList);
        }

    }
    public class SessionsListEnum : IEnumerator
    {
        public Session[] _dbSessionsList;
        int position = -1;

        public SessionsListEnum(Session[] list)
        {
            _dbSessionsList = list;
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
                    return _dbSessionsList[position];
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
            return position < _dbSessionsList.Length;
        }

        public void Reset()
        {
            position = -1;
        }
    }
}
