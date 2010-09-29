using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace Apics.Data.AptifyAdapter.ADO
{

    public class AptifyParameterCollection : DbParameterCollection
    {
        private readonly List<DbParameter> parameters = new List<DbParameter>( );

        public DbParameter[ ] ToArray( )
        {
            return this.parameters.ToArray( );
        }

        public static implicit operator DbParameter[ ]( AptifyParameterCollection collection )
        {
            return collection.ToArray( );
        }

        #region [ DbParameterCollection Overrides ]

        public override int Count
        {
            get { return this.parameters.Count; }
        }

        public override bool IsFixedSize
        {
            get { return false; }
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override bool IsSynchronized
        {
            get { return false; }
        }

        public override object SyncRoot
        {
            get { return this; }
        }

        public override int Add( object value )
        {
            this.parameters.Add( ( DbParameter )value );
            return this.parameters.Count - 1;
        }

        public override void AddRange( Array values )
        {
            this.parameters.AddRange( values.Cast<DbParameter>( ) );
        }

        public override void Clear( )
        {
            this.parameters.Clear( );
        }

        public override bool Contains( string value )
        {
            return this.parameters.Any( p => p.ParameterName == value );
        }

        public override bool Contains( object value )
        {
            return this.parameters.Any( p => p == value );
        }

        public override void CopyTo( Array array, int index )
        {
            if( array == null )
                throw new ArgumentNullException( "array" );

            Array.Copy( this.parameters.ToArray( ), 0, array, index, this.parameters.Count );
        }

        public override IEnumerator GetEnumerator( )
        {
            return this.parameters.GetEnumerator( );
        }

        protected override DbParameter GetParameter( string parameterName )
        {
            return this.parameters.First( p => p.ParameterName == parameterName );
        }

        protected override DbParameter GetParameter( int index )
        {
            return this.parameters[ index ];
        }

        public override int IndexOf( string parameterName )
        {
            return this.parameters.IndexOf( GetParameter( parameterName ) );
        }

        public override int IndexOf( object value )
        {
            return this.parameters.IndexOf( ( DbParameter )value );
        }

        public override void Insert( int index, object value )
        {
            this.parameters.Insert( index, ( DbParameter )value );
        }

        public override void Remove( object value )
        {
            this.parameters.Remove( ( DbParameter )value );
        }

        public override void RemoveAt( string parameterName )
        {
            this.parameters.RemoveAll( p => p.ParameterName == parameterName );
        }

        public override void RemoveAt( int index )
        {
            this.parameters.RemoveAt( index );
        }

        protected override void SetParameter( string parameterName, DbParameter value )
        {
            this.parameters.Add( new SqlParameter( parameterName, value ) );
        }

        protected override void SetParameter( int index, DbParameter value )
        {
            if( index >= this.parameters.Count )
                this.parameters.Insert( index, value );
            else
                this.parameters[ index ] = value;
        }

        #endregion [ DbParameterCollection Overrides ]
    }
}
