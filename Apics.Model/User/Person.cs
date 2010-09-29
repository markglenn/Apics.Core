using System;
using System.Collections.Generic;
using System.Linq;
using Castle.ActiveRecord;

namespace Apics.Model.User
{
    [ActiveRecord( Lazy = true )]
    public class Person
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [Property]
        public virtual string Prefix { get; set; }

        [Property]
        public virtual string FirstName { get; set; }

        [Property]
        public virtual string MiddleName { get; set; }

        [Property]
        public virtual string LastName { get; set; }

        [HasMany]
        public virtual IList<PersonAddress> Addresses { get; set; }

        [BelongsTo( "PhoneID" )]
        public virtual PhoneNumber Phone { get; set; }

        [Property]
        public virtual string Email1 { get; set; }

        [Property]
        public virtual string WhoCreated { get; set; }

        [Property]
        public virtual string WhoUpdated { get; set; }

        [Property]
        public virtual DateTime DateCreated { get; set; }

        [Property]
        public virtual DateTime DateUpdated { get; set; }
    }
}
