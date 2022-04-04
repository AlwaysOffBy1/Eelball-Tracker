using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EELBALL_TRACKER
{
    //https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerable?view=net-6.0

    public class DatabaseThrows : IEnumerable
    {
        private Throw[] _dbThrows;
        public DatabaseThrows(Throw[] throwsArr)
        {
            _dbThrows = new Throw[throwsArr.Length];
            for (int i = 0; i < throwsArr.Length; i++)
            {
                _dbThrows[i] = throwsArr[i];
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        public DatabaseThrowsEnum GetEnumerator()
        {
            return new DatabaseThrowsEnum(_dbThrows);
        }

    }
    public class DatabaseThrowsEnum : IEnumerator
    {
        public Throw[] _dbThrows;
        int position = -1;

        public DatabaseThrowsEnum(Throw[] list)
        {
            _dbThrows= list;
        }

        object IEnumerator.Current
        {
            get => Current;
        }
        public Throw Current
        {
            get
            {
                try
                {
                    return _dbThrows[position];
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
            return position < _dbThrows.Length;
        }

        public void Reset()
        {
            position = -1;
        }
    }
}
