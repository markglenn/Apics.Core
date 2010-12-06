using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apics.Model.Fulfillment;
using Aptify.Framework.BusinessLogic.GenericEntity;
using Aptify.Applications.OrderEntry;

namespace Apics.Data.AptifyAdapter.Extension
{
    public static class OrderMasterExtensions
    {
        public static void UpdateCosts( this Order order )
        {
            order.Saving += ( s, e ) => {
                ( ( dynamic )e.Backing ).CalculateOrderTotals( );
            };
        }
    }
}
