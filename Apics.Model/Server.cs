using System;
using System.Linq;
using Castle.ActiveRecord;

namespace Apics.Model
{
    [ActiveRecord]
    public class Server
    {
        [PrimaryKey]
        public int Id { get; set; }

        [Property( "ServerName", NotNull = true, Length = 100 )]
        public string Name { get; set; }

        [Property( Length = 50 )]
        public string Description { get; set; }

        [BelongsTo( "ServerTypeID", NotNull = true, Cascade = CascadeEnum.SaveUpdate )]
        public ServerType Type { get; set; }
    }
}