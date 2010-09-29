using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Net.Mail;
using System.Linq;

namespace Apics.Utilities.Messaging
{
    [DataContract]
    public class EmailMessage : IMessage, IEquatable<EmailMessage>
    {
        #region [ Private Members ]

        private static readonly string EmailMessageId = typeof( EmailMessage ).ToString( );

        private EmailAddressCollection toAddress = new EmailAddressCollection( );
        private EmailAddressCollection bccAddresses = new EmailAddressCollection( );
        private EmailAddressCollection ccAddresses = new EmailAddressCollection( );
        private Dictionary<string,string> parameters = new Dictionary<string, string>( );
        
        #endregion [ Private Members ]
        
        #region [ Public Properties ]

        /// <summary>
        /// Message ID
        /// </summary>
        [DataMember]
        public string MessageId
        {
            get { return EmailMessageId;  }
            set
            {
                if ( value != EmailMessageId )
                    throw new InvalidOperationException( "Can't set invalid ID for this message" );
            }
        }

        /// <summary>
        /// From whom the email is sent
        /// </summary>
        [DataMember]
        public EmailAddress FromAddress { get; set; }

        /// <summary>
        /// Template used to render the message
        /// </summary>
        [DataMember]
        public string Template { get; set; }

        /// <summary>
        /// To whom the email is being sent
        /// </summary>
        [DataMember]
        public EmailAddressCollection ToAddress
        {
            get { return this.toAddress; }
            set { this.toAddress = value; }
        }

        /// <summary>
        /// To whom the email is CCed
        /// </summary>
        [DataMember]
        public EmailAddressCollection CcAddresses
        {
            get { return this.ccAddresses; }
            set { this.ccAddresses = value; }
        }

        /// <summary>
        /// To whom the email is BCCed
        /// </summary>
        [DataMember]
        public EmailAddressCollection BccAddresses
        {
            get { return this.bccAddresses; }
            set { this.bccAddresses = value; }
        }

        /// <summary>
        /// Mail subject
        /// </summary>
        [DataMember]
        public string Subject
        {
            get; set;
        }

        /// <summary>
        /// Message parameters used to generate the email
        /// </summary>
        [DataMember]
        public Dictionary<string, string> Parameters
        {
            get { return this.parameters; }
            set { this.parameters = value; }
        }
        
        #endregion [ Public Properties ]

        #region [ IEquatable Methods ]

        public override bool Equals( object obj )
        {
            return Equals( obj as EmailMessage );
        }

        public bool Equals( EmailMessage other )
        {
            if ( other == null )
                return false; 

            if( ReferenceEquals( this, other ) )
                return true;

            if ( !this.toAddress.Equals( other.toAddress) )
                return false;

            if ( !this.bccAddresses.Equals( other.bccAddresses) )
                return false;

            if ( this.FromAddress == other.FromAddress )
                return false;

            if ( this.Template != other.Template ) 
                return false;

            return this.Subject == other.Subject;
        }

        public override int GetHashCode( )
        {
            unchecked
            {
                int result = ( this.toAddress != null ? this.toAddress.GetHashCode( ) : 0 );
                result = ( result * 397 ) ^ ( this.bccAddresses != null ? this.bccAddresses.GetHashCode( ) : 0 );
                result = ( result * 397 ) ^ ( this.ccAddresses != null ? this.ccAddresses.GetHashCode( ) : 0 );
                result = ( result * 397 ) ^ ( this.FromAddress != null ? this.FromAddress.GetHashCode( ) : 0 );
                result = ( result * 397 ) ^ ( this.Template != null ? this.Template.GetHashCode( ) : 0 );
                result = ( result * 397 ) ^ ( this.Subject != null ? this.Subject.GetHashCode( ) : 0 );
                return result;
            }
        }
        
        #endregion [ IEquatable Methods ]
    }
   
    [CollectionDataContract]
    public class EmailAddressCollection : List<EmailAddress>, IEquatable<EmailAddressCollection>
    {
        public static implicit operator MailAddressCollection( EmailAddressCollection addresses )
        {
            var collection = new MailAddressCollection( );

            addresses.ForEach( address => collection.Add( address ) );

            return collection;
        }

        public override bool Equals( object obj )
        {
            return this.Equals( obj as EmailAddressCollection );
        }
       
        public bool Equals( EmailAddressCollection other )
        {
            if ( other == null )
                return false;

            return other.SequenceEqual( this );
        }

        public override int GetHashCode( )
        {
            return base.GetHashCode( );
        }
    }

    [DataContract]
    public class EmailAddress : IEquatable<EmailAddress>
    {
        #region [ Private Members ]

        [DataMember]
        private string address;

        [DataMember]
        private string displayName;

        #endregion [ Private Members ]

        #region [ Public Properties ]

        public string Address
        {
            get { return this.address; }
        }

        public string DisplayName
        {
            get { return this.displayName; }
        }

        #endregion [ Public Properties ]

        public EmailAddress( string address )
            : this( address, String.Empty )
        {
        }

        public EmailAddress( string address, string displayName )
        {
            if( address == null ) 
                throw new ArgumentNullException( "address" );

            this.address = address;
            this.displayName = displayName;
        }

        public static implicit operator MailAddress( EmailAddress address )
        {
            if ( String.IsNullOrEmpty( address.DisplayName ) )
                return new MailAddress( address.Address );

            return new MailAddress( address.Address, address.DisplayName );
        }

        #region [ IEquatable<EmailAddress> Members ]

        public override bool Equals( object obj )
        {
            return Equals( obj as EmailAddress );
        }
        
        public bool Equals( EmailAddress other )
        {
            if( ReferenceEquals( null, other ) ) 
                return false;
            if( ReferenceEquals( this, other ) ) 
                return true;

            return Equals( other.address, this.address ) && Equals( other.displayName, this.displayName );
        }

        #endregion [ IEquatable<EmailAddress> Members ]

        public override int GetHashCode( )
        {
            unchecked
            {
                return ( ( this.address != null ? this.address.GetHashCode( ) : 0 ) * 397 ) ^ 
                    ( this.displayName != null ? this.displayName.GetHashCode( ) : 0 );
            }
        }
    }
}