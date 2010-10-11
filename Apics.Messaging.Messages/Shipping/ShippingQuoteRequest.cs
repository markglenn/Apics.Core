using System;
using System.Linq;

namespace Apics.Messaging.Messages.Shipping
{
    public class ShippingQuoteRequest
    {
        public ShippingAddress Address { get; set; }
        public decimal Weight { get; set; }
    }

    public class ShippingAddress
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }

        public string City { get; set; }
        public string StateProvince { get; set; }

        public string CountryIsoCode { get; set; }
        public string PostalCode { get; set; }
    }
}
