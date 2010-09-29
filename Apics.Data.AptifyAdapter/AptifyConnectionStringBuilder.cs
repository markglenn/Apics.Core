using System;
using System.Linq;
using System.Text.RegularExpressions;
using Aptify.Framework.DataServices;

namespace Apics.Data.AptifyAdapter
{
    public class AptifyConnectionStringBuilder
    {
        private static readonly Regex ConnectionStringRegex = new Regex( @"([^;=]+)=([^;]+)" );

        #region [ Public Properties ]

        public string ConnectionString
        {
            get { return this.connectionString; }
        }

        internal UserCredentials Credentials
        {
            get { return this.credentials; }
        }

        public string DataSource
        {
            get { return this.dataSource; }
        }

        public string InitialCatalog
        {
            get { return this.initialCatalog; }
        }

        public string UserId
        {
            get { return this.userId; }
        }

        public string Password
        {
            get { return this.password; }
        }

        public bool TrustedConnection
        {
            get { return this.trustedConnection; }
        }

        #endregion [ Public Properties ]

        private readonly string connectionString;
        private readonly UserCredentials credentials;
        private readonly string dataSource;
        private readonly string initialCatalog;
        private readonly string password;
        private readonly bool trustedConnection;
        private readonly string userId;

        public AptifyConnectionStringBuilder( string connectionString )
        {
            this.connectionString = connectionString;

            // Clean up the connection string
            connectionString = connectionString.Replace( '_', ' ' );

            MatchCollection matches = ConnectionStringRegex.Matches( connectionString );

            // Go through each section of the connection string
            foreach( Match match in matches )
            {
                string key = match.Groups[ 1 ].Value.ToLower( );
                string value = match.Groups[ 2 ].Value;

                switch( key )
                {
                    case "data source":
                    case "server":
                        this.dataSource = value;
                        break;

                    case "initial catalog":
                    case "database":
                        this.initialCatalog = value;
                        break;

                    case "user id":
                        this.userId = value;
                        break;

                    case "password":
                        this.password = value;
                        break;

                    case "trusted connection":
                        this.trustedConnection = Boolean.Parse( value );
                        break;
                }
            }

            // Define the Aptify credentials
            this.credentials = new UserCredentials(
                this.dataSource, this.initialCatalog, this.initialCatalog,
                this.trustedConnection, -1, this.userId, this.password,
                null, false, -1, true );
        }

        #region [ Object Overrides ]

        public override string ToString( )
        {
            return this.connectionString;
        }

        #endregion [ Object Overrides ]
    }
}