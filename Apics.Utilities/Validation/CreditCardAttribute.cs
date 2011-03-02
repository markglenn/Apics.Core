using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Apics.Utilities.Validation
{
    public enum CardType
    {
        Unknown = 1,
        Visa = 2,
        MasterCard = 4,
        Amex = 8,
        Diners = 16,
        Discover = 32,

        All = CardType.Visa | CardType.MasterCard | CardType.Amex | CardType.Discover,
        AllOrUnknown = CardType.Unknown | CardType.Visa | CardType.MasterCard | CardType.Amex | CardType.Diners
    }

    public class CreditCardAttribute : ValidationAttribute
    {
        #region [ Private Members ]

        private CardType cardType = CardType.All;

        #endregion [ Private Members ]

        public CreditCardAttribute( )
        {

        }

        public CreditCardAttribute( CardType acceptedCardTypes )
        {
            this.cardType = acceptedCardTypes;
        }

        public override bool IsValid( object value )
        {
            var number = Convert.ToString( value );

            // Nothing entered in the field
            if ( String.IsNullOrEmpty( number ) )
                return false;

            return IsValidType( number, this.cardType ) && IsValidNumber( number );
        }

        public override string FormatErrorMessage( string name )
        {
            return String.Format( "The {0} field contains an invalid credit card number", name );
        }

        #region [ Private Methods ]

        private bool IsValidType( string cardNumber, CardType cardType )
        {
            // Visa
            if ( Regex.IsMatch( cardNumber, "^(4)" )
                && ( ( cardType & CardType.Visa ) != 0 ) )
                return cardNumber.Length == 13 || cardNumber.Length == 16;

            // MasterCard
            if ( Regex.IsMatch( cardNumber, "^(51|52|53|54|55)" )
                && ( ( cardType & CardType.MasterCard ) != 0 ) )
                return cardNumber.Length == 16;

            // Amex
            if ( Regex.IsMatch( cardNumber, "^(34|37)" )
                && ( ( cardType & CardType.Amex ) != 0 ) )
                return cardNumber.Length == 15;

            // Discover
            if ( ( cardType & CardType.Discover ) != 0 && Regex.IsMatch( cardNumber, "^(6011)" ) )
                return cardNumber.Length == 16;

            // Diners
            if ( ( cardType & CardType.Diners ) != 0  && Regex.IsMatch( cardNumber, "^(300|301|302|303|304|305|36|38)" ) )
                return cardNumber.Length == 14;

            //Unknown
            if ( ( cardType & CardType.Unknown ) != 0 )
                return true;

            return false;
        }

        private bool IsValidNumber( string number )
        {
            int[ ] deltas = new int[ ] { 0, 1, 2, 3, 4, -4, -3, -2, -1, 0 };
            int checksum = 0;
            char[ ] chars = number.ToCharArray( );
            for ( int i = chars.Length - 1; i > -1; i-- )
            {
                int j = ( ( int )chars[ i ] ) - 48;
                checksum += j;
                if ( ( ( i - chars.Length ) % 2 ) == 0 )
                    checksum += deltas[ j ];
            }

            return ( ( checksum % 10 ) == 0 );
        }

        #endregion [ Private Methods ]
    }
}
