using System;
using System.Linq;
using Castle.ActiveRecord;

namespace Apics.Data.Database
{
    public static class Generate
    {
        public static void Schema( )
        {
            ActiveRecordStarter.CreateSchema( );
        }
    }
}
