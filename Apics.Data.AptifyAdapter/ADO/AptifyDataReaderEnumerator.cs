using System;
using System.Collections;
using System.Data;
using System.Linq;

namespace Apics.Data.AptifyAdapter.ADO
{
    internal class AptifyDataReaderEnumerator : IEnumerator
    {
        private readonly IDataReader reader;

        public AptifyDataReaderEnumerator( IDataReader reader )
        {
            this.reader = reader;
        }

        #region IEnumerator Members

        public object Current
        {
            get { return this.reader; }
        }

        public bool MoveNext( )
        {
            return this.reader.NextResult( );
        }

        public void Reset( )
        {
            throw new NotSupportedException( );
        }

        #endregion
    }
}