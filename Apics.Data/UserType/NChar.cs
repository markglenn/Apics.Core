using System;
using System.Data;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace Apics.Data.UserType
{
    [Serializable]
    public class NChar : IUserType
    {
        #region [ Implementation of IUserType ]

        public new bool Equals( object x, object y )
        {
            return x.Equals( y );
        }

        public int GetHashCode( object x )
        {
            return x.GetHashCode( );
        }

        public object NullSafeGet( IDataReader rs, string[ ] names, object owner )
        {
            var name = NHibernateUtil.String.NullSafeGet( rs, names[ 0 ] ) as string;
            return name == null ? null : name.Trim( );
        }

        public void NullSafeSet( IDbCommand cmd, object value, int index )
        {
            NHibernateUtil.String.NullSafeSet( cmd, value, index );
        }

        public object DeepCopy( object value )
        {
            return value;
        }

        public object Replace( object original, object target, object owner )
        {
            return original;
        }

        public object Assemble( object cached, object owner )
        {
            return cached;
        }

        public object Disassemble( object value )
        {
            return value;
        }

        public SqlType[ ] SqlTypes
        {
            get { return new[ ] { new SqlType( DbType.StringFixedLength ) }; }
        }

        public Type ReturnedType
        {
            get { return typeof( NChar ); }
        }

        public bool IsMutable
        {
            get { return false; }
        }

        #endregion [ Implementation of IUserType ]
    }
}
