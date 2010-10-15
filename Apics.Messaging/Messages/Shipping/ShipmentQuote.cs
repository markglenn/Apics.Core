using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;

namespace Apics.Messaging.Messages.Shipping
{
    [DataContract]
    public enum NotificationType
    {
        [EnumMember] Success,
        [EnumMember] Message,
        [EnumMember] Warning,
        [EnumMember] Error,
        [EnumMember] Fatal
    }

    [DebuggerDisplay( "{Name} - {Price} - {Message}" )]
    [DataContract]
    public class ShipmentQuote
    {
        #region [ Public Properties ]

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public int? Days { get; set; }

        [DataMember]
        public NotificationType NotificationType { get; set; }

        #endregion [ Public Properties ]

        public ShipmentQuote( string shipmentName, decimal price, int? days, string message = null )
        {
            if ( String.IsNullOrEmpty( shipmentName ) )
                throw new ArgumentException( "Cannot be null or empty", "shipmentName" );

            this.Name = shipmentName;
            this.Price = price;
            this.Message = message;
            this.Days = days;
            this.NotificationType = NotificationType.Success;
        }

        public ShipmentQuote( string shipmentName, NotificationType notificationType, string message )
        {
            if ( String.IsNullOrEmpty( shipmentName ) )
                throw new ArgumentException( "shipmentName" );
            if ( message == null )
                throw new ArgumentNullException( "shipmentName" );

            this.Name = shipmentName;
            this.Message = message;
            this.NotificationType = notificationType;
        }

        public ShipmentQuote( )
        {
        }
    }
}
