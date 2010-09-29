using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;

namespace Apics.Model.User
{
	[ActiveRecord( Lazy = true )]
	public class PersonAddress
	{
		[PrimaryKey]
		public virtual int Id { get; set; }

		[BelongsTo( Column = "PersonID" )]
		public virtual Person Person { get; set; }

		[Property]
		public virtual int Sequence { get; set; }

		[Property]
		public virtual string Name { get; set; }
	}
}
