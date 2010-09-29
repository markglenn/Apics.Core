using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Apics.Utilities.Network
{
    /// <summary>
    /// Identifies entry as either File or Directory
    /// </summary>
    public enum DirectoryEntryType
    {
        File,
        Directory
    }

    /// <summary>
    /// Stores extended info about FTP file
    /// </summary>
    public class FtpFileInformation
    {
        #region [ Private Members ]

        private readonly string fileName;

        private readonly string permission;
        private readonly string filePath;
        private readonly long size;
        private readonly DirectoryEntryType fileType;
        private readonly DateTime fileDateTime;

        #endregion [ Private Members ]

		#region [ Public Properties ]

        /// <summary>
        /// Full path name
        /// </summary>
		public string FullPath
		{
			get { return Path.Combine( FilePath, FileName ); }
		}

        /// <summary>
        /// File name
        /// </summary>
        public string FileName
        {
            get { return this.fileName; }
        }

        /// <summary>
        /// Path to file
        /// </summary>
        public string FilePath
        {
            get { return this.filePath; }
        }

        /// <summary>
        /// Type of the file ( File or Directory )
        /// </summary>
        public DirectoryEntryType FileType
        {
            get { return this.fileType; }
        }

        /// <summary>
        /// Size of the file
        /// </summary>
        public long Size
        {
            get { return this.size; }
        }

        /// <summary>
        /// Date of the file
        /// </summary>
        public DateTime FileDateTime
        {
            get { return this.fileDateTime; }
        }

        /// <summary>
        /// Permissions on file
        /// </summary>
        public string Permission
        {
            get { return this.permission; }
        }

        /// <summary>
        /// Extension of the file
        /// </summary>
        public string Extension
		{
			get { return Path.GetExtension( FileName ); }
		}

        /// <summary>
        /// Name of the file
        /// </summary>
		public string NameOnly
		{
			get { return Path.GetFileNameWithoutExtension( this.FileName ); }
		}

		#endregion [ Public Properties ]

		/// <summary>
		/// Constructor taking a directory listing line and path
		/// </summary>
		/// <param name="line">The line returned from the detailed directory list</param>
		/// <param name="path">Path of the directory</param>
		/// <remarks></remarks>
		public FtpFileInformation ( string line, string path )
		{
			//parse line
			Match m = GetMatchingRegex ( line );
			if ( m == null )
				throw ( new FormatException ( "Unable to parse line: " + line ) );

            if ( !String.IsNullOrEmpty( m.Groups[ "size" ].Value ) )
            {
                if( !Int64.TryParse( m.Groups[ "size" ].Value, out this.size ) )
                    throw new FormatException( "Unable to parse line.  Size is invalid: " + line );
            }

		    this.fileName = m.Groups[ "name" ].Value;
			this.filePath = path;
			this.permission = m.Groups[ "permission" ].Value;

			// Determine file type
			this.fileType = DirectoryEntryType.Directory;

			string dir = m.Groups[ "dir" ].Value;
			if ( String.IsNullOrEmpty( dir ) || dir == "-" )
				this.fileType = DirectoryEntryType.File;

            if ( !DateTime.TryParse( m.Groups[ "timestamp" ].Value, out this.fileDateTime ) )
                this.fileDateTime = DateTime.MinValue;
		}

		private static Match GetMatchingRegex ( string line )
		{
		    return Parsers.Select( parser => parser.Match( line ) )
                .FirstOrDefault( m => m.Success );
		}

        #region [ Regular expressions for parsing LIST results ]

		/// <summary>
		/// List of REGEX formats for different FTP server listing formats
		/// </summary>
		/// <remarks>
		/// The first three are various UNIX/LINUX formats, fourth is for MS FTP
		/// in detailed mode and the last for MS FTP in 'DOS' mode.
		/// I wish VB.NET had support for Const arrays like C# but there you go
		/// </remarks>
		private static readonly Regex[ ] Parsers = new[ ] { 
            new Regex ( "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d+\\s+\\w+\\s+\\w+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{4})\\s+(?<name>.+)" ),
            new Regex ( "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d+\\s+\\d+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{4})\\s+(?<name>.+)" ), 
            new Regex ( "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d+\\s+\\d+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{1,2}:\\d{2})\\s+(?<name>.+)" ), 
            new Regex ( "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d+\\s+\\w+\\s+\\w+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{1,2}:\\d{2})\\s+(?<name>.+)" ), 
            new Regex ( "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})(\\s+)(?<size>(\\d+))(\\s+)(?<ctbit>(\\w+\\s\\w+))(\\s+)(?<size2>(\\d+))\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{2}:\\d{2})\\s+(?<name>.+)") ,  
            new Regex ( "(?<timestamp>\\d{2}\\-\\d{2}\\-\\d{2}\\s+\\d{2}:\\d{2}[Aa|Pp][mM])\\s+(?<dir>\\<\\w+\\>){0,1}(?<size>\\d+){0,1}\\s+(?<name>.+)" ) };

		#endregion [ Regular expressions for parsing LIST results ]
    }
}
