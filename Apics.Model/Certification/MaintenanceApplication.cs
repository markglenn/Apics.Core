using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using System.Diagnostics;
using Apics.Model.Fulfillment;
using Apics.Data;

namespace Apics.Model.Certification
{
    [ActiveRecord( "APICSCertificationMaintenanceApplication", Lazy = true )]
    [DebuggerDisplay( "Maintenance Application: {Id}" )]
    public class MaintenanceApplication
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [BelongsTo( "OrderID", NotNull = true, Lazy = FetchWhen.OnInvoke, Cascade = CascadeEnum.None )]
        public virtual Order Order { get; set; }

        [Property( Length = 50 )]
        public virtual string CertificationType { get; set; }

        [Property]
        public virtual DateTime? DeadlineDate { get; set; }

        [Property]
        public virtual bool IsLocked { get; set; }

        [Property]
        public virtual DateTime ApplicationDate { get; set; }

        [Property]
        public virtual DateTime? DateUpdated { get; set; }

        [HasMany( Lazy = true, Cascade = ManyRelationCascadeEnum.Delete )]
        public virtual IList<MaintenanceApplicationActivity> Activites { get; set; }

        public virtual void Save( IDataStore store )
        {
            var applications = store.Repository<MaintenanceApplication>( );
            var orders = store.Repository<Order>( );
            var activities = store.Repository<MaintenanceApplicationActivity>( );
            var points = store.Repository<MaintenanceActivityPoint>( );

            orders.Insert( this.Order );
            applications.InsertOrUpdate( this );

            foreach ( var activity in this.Activites.Where( a => a.Points.Any( p => p.Points != 0.0M ) ) )
            {
                activity.Application = this;
                activities.InsertOrUpdate( activity );

                foreach ( var point in activity.Points.Where( p => p.Points != 0.0M ) )
                {
                    point.Activity = activity;
                    points.InsertOrUpdate( point );
                }
            }
        }
    }

}
