using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Apics.Data.AptifyAdapter.ADO
{
    public class AptifyDataReader : DbDataReader
    {
        #region [ Private Properties ]

        private readonly IDataReader reader;

        #endregion [ Private Properties ]

        public AptifyDataReader( IDataReader reader )
        {
            this.reader = reader;
        }

        #region [ DbDataReader Overrides ]

        public override int Depth
        {
            get { return this.reader.Depth; }
        }

        public override int FieldCount
        {
            get { return this.reader.FieldCount; }
        }

        public override bool HasRows
        {
            get { return this.reader.NextResult( ); }
        }

        public override bool IsClosed
        {
            get { return this.reader.IsClosed; }
        }

        public override int RecordsAffected
        {
            get { return this.reader.RecordsAffected; }
        }

        public override object this[ string name ]
        {
            get { return this.reader[ name ]; }
        }

        public override object this[ int ordinal ]
        {
            get { return this.reader[ ordinal ]; }
        }

        public override void Close( )
        {
            this.reader.Close( );
        }

        public override bool GetBoolean( int ordinal )
        {
            return this.reader.GetBoolean( ordinal );
        }

        public override byte GetByte( int ordinal )
        {
            return this.reader.GetByte( ordinal );
        }

        public override long GetBytes( int ordinal, long dataOffset, byte[ ] buffer, int bufferOffset, int length )
        {
            return this.reader.GetBytes( ordinal, dataOffset, buffer, bufferOffset, length );
        }

        public override char GetChar( int ordinal )
        {
            return this.reader.GetChar( ordinal );
        }

        public override long GetChars( int ordinal, long dataOffset, char[ ] buffer, int bufferOffset, int length )
        {
            return this.reader.GetChars( ordinal, dataOffset, buffer, bufferOffset, length );
        }

        public override string GetDataTypeName( int ordinal )
        {
            return this.reader.GetDataTypeName( ordinal );
        }

        public override DateTime GetDateTime( int ordinal )
        {
            return this.reader.GetDateTime( ordinal );
        }

        public override decimal GetDecimal( int ordinal )
        {
            return this.reader.GetDecimal( ordinal );
        }

        public override double GetDouble( int ordinal )
        {
            return this.reader.GetDouble( ordinal );
        }

        public override IEnumerator GetEnumerator( )
        {
            return new AptifyDataReaderEnumerator( this.reader );
        }

        public override Type GetFieldType( int ordinal )
        {
            return this.reader.GetFieldType( ordinal );
        }

        public override float GetFloat( int ordinal )
        {
            return this.reader.GetFloat( ordinal );
        }

        public override Guid GetGuid( int ordinal )
        {
            return this.reader.GetGuid( ordinal );
        }

        public override short GetInt16( int ordinal )
        {
            return this.reader.GetInt16( ordinal );
        }

        public override int GetInt32( int ordinal )
        {
            return this.reader.GetInt32( ordinal );
        }

        public override long GetInt64( int ordinal )
        {
            return this.reader.GetInt64( ordinal );
        }

        public override string GetName( int ordinal )
        {
            return this.reader.GetName( ordinal );
        }

        public override int GetOrdinal( string name )
        {
            return this.reader.GetOrdinal( name );
        }

        public override DataTable GetSchemaTable( )
        {
            return this.reader.GetSchemaTable( );
        }

        public override string GetString( int ordinal )
        {
            return this.reader.GetString( ordinal );
        }

        public override object GetValue( int ordinal )
        {
            return this.reader.GetValue( ordinal );
        }

        public override int GetValues( object[ ] values )
        {
            return this.reader.GetValues( values );
        }

        public override bool IsDBNull( int ordinal )
        {
            return this.reader.IsDBNull( ordinal );
        }

        public override bool NextResult( )
        {
            return this.reader.NextResult( );
        }

        public override bool Read( )
        {
            return this.reader.Read( );
        }

        #endregion [ DbDataReader Overrides ]
    }
}