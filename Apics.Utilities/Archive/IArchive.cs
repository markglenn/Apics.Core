using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Apics.Utilities.Archive
{
    /// <summary>
    /// The IArchive interface defines the capabilities of loading files from
    /// an archive (Zip/Tar/Directory).  The abstraction simplifies loading
    /// from different archives or a combination of them.
    /// </summary>
    public interface IArchive : IDisposable, IEnumerable<string>
    {
        /// <summary>
        /// Gets the name of the originating archive
        /// </summary>
        string ArchiveName { get; }

        /// <summary>
        /// True if the archive is read only
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Opens a file stream from the archive
        /// </summary>
        /// <param name="fileName">Name of the file inside the archive</param>
        /// <returns>Returns a stream to the file or null if it doesn't exist</returns>
        Stream OpenFile( string fileName );

        /// <summary>
        /// Returns whether a file exists in the archive
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        /// <returns>True if the file exists</returns>
        bool Exists( string fileName );

        /// <summary>
        /// Creates a stream to write to if writable
        /// </summary>
        /// <param name="fileName">Name of the file to create</param>
        /// <returns>A stream to the file or null if not writable</returns>
        Stream CreateWritableStream( string fileName );

        /// <summary>
        /// Deletes a file from the archive
        /// </summary>
        /// <param name="fileName">Filename to delete</param>
        /// <returns>True if the file is deleted</returns>
        bool DeleteFile( string fileName );
    }
}