﻿#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Semantic Logging Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Sinks
{
    /// <summary>
    /// Purges archive files generated by the <see cref="RollingFlatFileSink"/>.
    /// </summary>
    public class RollingFlatFilePurger
    {
        private readonly string directory;
        private readonly string baseFileName;
        private readonly int cap;

        /// <summary>
        /// Initializes a new instance of the <see cref="RollingFlatFilePurger"/> class.
        /// </summary>
        /// <param name="directory">The folder where archive files are kept.</param>
        /// <param name="baseFileName">The base name for archive files.</param>
        /// <param name="cap">The number of archive files to keep.</param>
        public RollingFlatFilePurger(string directory, string baseFileName, int cap)
        {
            if (directory == null)
            {
                throw new ArgumentNullException("directory");
            }

            if (baseFileName == null)
            {
                throw new ArgumentNullException("baseFileName");
            }

            if (cap < 1)
            {
                throw new ArgumentOutOfRangeException("cap");
            }

            this.directory = directory;
            this.baseFileName = baseFileName;
            this.cap = cap;
        }

        /// <summary>
        /// Extracts the sequence number from an archive file name.
        /// </summary>
        /// <param name="fileName">The archive file name.</param>
        /// <returns>The sequence part of the file name.</returns>
        public static string GetSequence(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            int extensionDotIndex = fileName.LastIndexOf('.');

            if (extensionDotIndex <= 0)
            {
                // no dots - can't extract sequence
                return string.Empty;
            }

            int sequenceDotIndex = fileName.LastIndexOf('.', extensionDotIndex - 1);

            if (sequenceDotIndex < 0)
            {
                // single dot - can't extract sequence
                return string.Empty;
            }

            return fileName.Substring(sequenceDotIndex + 1, extensionDotIndex - sequenceDotIndex - 1);
        }

        /// <summary>
        /// Purges archive files.
        /// </summary>
        public void Purge()
        {
            var extension = Path.GetExtension(this.baseFileName);
            var searchPattern = Path.GetFileNameWithoutExtension(this.baseFileName) + ".*" + extension;

            string[] matchingFiles = this.TryGetMatchingFiles(searchPattern);

            if (matchingFiles.Length <= this.cap)
            {
                // bail out early if possible
                return;
            }

            // sort the archive files in descending order by creation date and sequence number
            var sortedArchiveFiles =
                matchingFiles
                    .Select(matchingFile => new ArchiveFile(matchingFile))
                    .OrderByDescending(archiveFile => archiveFile);

            using (var enumerator = sortedArchiveFiles.GetEnumerator())
            {
                // skip the most recent files
                for (int i = 0; i < this.cap; i++)
                {
                    if (!enumerator.MoveNext())
                    {
                        // should not happen
                        return;
                    }
                }

                // delete the older files
                while (enumerator.MoveNext())
                {
                    TryDelete(enumerator.Current.Path);
                }
            }
        }

        private static void TryDelete(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (UnauthorizedAccessException)
            {
                // cannot delete the file because of a permissions issue - just skip it
            }
            catch (IOException)
            {
                // cannot delete the file, most likely because it is already opened - just skip it
            }
        }

        private static DateTime GetCreationTime(string path)
        {
            try
            {
                return File.GetCreationTimeUtc(path);
            }
            catch (UnauthorizedAccessException)
            {
                // will cause file be among the first files when sorting, 
                // and its deletion will likely fail causing it to be skipped
                return DateTime.MinValue;
            }
        }

        private string[] TryGetMatchingFiles(string searchPattern)
        {
            try
            {
                return Directory.GetFiles(this.directory, searchPattern, SearchOption.TopDirectoryOnly);
            }
            catch (DirectoryNotFoundException)
            {
            }
            catch (IOException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }

            return new string[0];
        }

        internal class ArchiveFile : IComparable<ArchiveFile>
        {
            private readonly string path;
            private readonly DateTime creationTime;
            private readonly string fileName;
            private string sequenceString;
            private int? sequence;

            public ArchiveFile(string path)
            {
                this.path = path;
                this.fileName = System.IO.Path.GetFileName(path);
                this.creationTime = GetCreationTime(path);
            }

            public string Path
            {
                get { return this.path; }
            }

            public DateTime CreationTime
            {
                get { return this.creationTime; }
            }

            public string SequenceString
            {
                get
                {
                    if (this.sequenceString == null)
                    {
                        this.sequenceString = GetSequence(this.fileName);
                    }

                    return this.sequenceString;
                }
            }

            public int Sequence
            {
                get
                {
                    if (!this.sequence.HasValue)
                    {
                        int theSequence;
                        if (int.TryParse(this.SequenceString, NumberStyles.None, CultureInfo.InvariantCulture, out theSequence))
                        {
                            this.sequence = theSequence;
                        }
                        else
                        {
                            this.sequence = 0;
                        }
                    }

                    return this.sequence.Value;
                }
            }

            public int CompareTo(ArchiveFile other)
            {
                Guard.ArgumentNotNull(other, "other");

                var creationDateComparison = this.CreationTime.CompareTo(other.CreationTime);
                if (creationDateComparison != 0)
                {
                    return creationDateComparison;
                }

                if (this.Sequence != 0 && other.Sequence != 0)
                {
                    // both archive files have proper sequences - use them
                    return this.Sequence.CompareTo(other.Sequence);
                }
                else
                {
                    // compare the sequence part of the file name as plain strings
                    return string.Compare(this.SequenceString, other.SequenceString, StringComparison.Ordinal);
                }
            }
        }
    }
}
