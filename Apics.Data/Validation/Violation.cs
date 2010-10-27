using System;
using System.Collections.Generic;

namespace Apics.Data.Validation
{
    /// <summary>
    /// Defines an instance of a violation of an object
    /// </summary>
    public class Violation
    {
        #region [ Private Members ]

        private readonly string name; 
        private readonly string message;
        private readonly object instance;

        #endregion [ Private Members ]

        #region [ Public Properties ]

        /// <summary>
        /// Name of the property or process that caused the violation
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Violation message
        /// </summary>
        public string Message
        {
            get { return this.message; }
        }

        /// <summary>
        /// Instance of the object that caused the violation
        /// </summary>
        public object Instance
        {
            get { return this.instance; }
        }

        public static IEnumerable<Violation> NoViolations = new Violation[ ] { };

        #endregion [ Public Properties ]
        
        /// <summary>
        /// Creates a violation message
        /// </summary>
        /// <param name="name">Name of the property or other descriptor</param>
        /// <param name="message">Message of the violation</param>
        /// <param name="instance">Instance that contained the violation</param>
        public Violation( string name, string message, object instance )
        {
            this.name = name;
            this.message = message;
            this.instance = instance;
        }

        /// <summary>
        /// Gets the friendly name of the violation for end users
        /// </summary>
        /// <returns>Friendly name</returns>
        public string FriendlyName( )
        {
            if ( !String.IsNullOrEmpty( this.name ) )
                return string.Format( "Violation: {0} - {1}", this.name, this.message );

            return string.Format( "Violation: {0}", this.message );
        }
    }
}