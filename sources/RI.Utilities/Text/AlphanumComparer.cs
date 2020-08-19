using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

using RI.Utilities.Exceptions;
using RI.Utilities.ObjectModel;




namespace RI.Utilities.Text
{
    /// <summary>
    ///     Implements a comparer which uses natural alphanumeric string sorting.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Natural alphanumeric sorting is a more sophisticated string sorting algorithm which respects the natural way humans look at strings, especially if they contain numbers.
    ///     </para>
    ///     <para>
    ///         The following list of strings is used as an example: Test100xyz, Test200abc, Test99abc.
    ///         Traditional sorting would sort this list as shown, putting Test99abc at the end (because the first four characters are the same and then 9 is greater than 2).
    ///         With natural alphanumeric sorting, the list is sorted as: Test99abc, Test100xyz, Test200abc (because the algorithm builds chunks and compares those instead of character-by-character).
    ///     </para>
    ///     <para>
    ///         Because this kind of sorting depends on the used culture, the used culture must be specified.
    ///     </para>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    /// TODO: Code example
    public sealed class AlphanumComparer : IComparer<string>, IComparer, IEqualityComparer<string>, IEqualityComparer, ICloneable<AlphanumComparer>, ICloneable
    {
        #region Static Methods

        /// <summary>
        ///     Creates a natural alphanumeric comparer for the current thread culture (case matters; strings are not trimmed before comparison; only pure numbers).
        /// </summary>
        /// <returns>
        ///     The comparer.
        /// </returns>
        public static AlphanumComparer CurrentCulture ()
        {
            return new AlphanumComparer(CultureInfo.CurrentCulture, AlphanumComparerFlags.PureNumbers);
        }

        /// <summary>
        ///     Creates a natural alphanumeric comparer for the current thread culture (case is ignored; strings are not trimmed before comparison; only pure numbers).
        /// </summary>
        /// <returns>
        ///     The comparer.
        /// </returns>
        public static AlphanumComparer CurrentCultureIgnoreCase ()
        {
            return new AlphanumComparer(CultureInfo.CurrentCulture, AlphanumComparerFlags.PureNumbers | AlphanumComparerFlags.IgnoreCase);
        }

        /// <summary>
        ///     Creates a natural alphanumeric comparer for the invariant culture (case matters; strings are not trimmed before comparison; only pure numbers).
        /// </summary>
        /// <returns>
        ///     The comparer.
        /// </returns>
        public static AlphanumComparer InvariantCulture ()
        {
            return new AlphanumComparer(CultureInfo.InvariantCulture, AlphanumComparerFlags.PureNumbers);
        }

        /// <summary>
        ///     Creates a natural alphanumeric comparer for the invariant culture (case is ignored; strings are not trimmed before comparison; only pure numbers).
        /// </summary>
        /// <returns>
        ///     The comparer.
        /// </returns>
        public static AlphanumComparer InvariantCultureIgnoreCase ()
        {
            return new AlphanumComparer(CultureInfo.InvariantCulture, AlphanumComparerFlags.PureNumbers | AlphanumComparerFlags.IgnoreCase);
        }

        /// <summary>
        ///     Creates a natural alphanumeric comparer for the current thread culture (case matters; strings are trimmed before comparison; only pure numbers).
        /// </summary>
        /// <returns>
        ///     The comparer.
        /// </returns>
        public static AlphanumComparer TrimmedCurrentCulture ()
        {
            return new AlphanumComparer(CultureInfo.CurrentCulture, AlphanumComparerFlags.PureNumbers | AlphanumComparerFlags.Trimmed);
        }

        /// <summary>
        ///     Creates a natural alphanumeric comparer for the current thread culture (case is ignored; strings are trimmed before comparison; only pure numbers).
        /// </summary>
        /// <returns>
        ///     The comparer.
        /// </returns>
        public static AlphanumComparer TrimmedCurrentCultureIgnoreCase ()
        {
            return new AlphanumComparer(CultureInfo.CurrentCulture, AlphanumComparerFlags.PureNumbers | AlphanumComparerFlags.Trimmed | AlphanumComparerFlags.IgnoreCase);
        }

        /// <summary>
        ///     Creates a natural alphanumeric comparer for the invariant culture (case matters; strings are trimmed before comparison; only pure numbers).
        /// </summary>
        /// <returns>
        ///     The comparer.
        /// </returns>
        public static AlphanumComparer TrimmedInvariantCulture ()
        {
            return new AlphanumComparer(CultureInfo.InvariantCulture, AlphanumComparerFlags.PureNumbers | AlphanumComparerFlags.Trimmed);
        }

        /// <summary>
        ///     Creates a natural alphanumeric comparer for the invariant culture (case is ignored; strings are trimmed before comparison; only pure numbers).
        /// </summary>
        /// <returns>
        ///     The comparer.
        /// </returns>
        public static AlphanumComparer TrimmedInvariantCultureIgnoreCase ()
        {
            return new AlphanumComparer(CultureInfo.InvariantCulture, AlphanumComparerFlags.PureNumbers | AlphanumComparerFlags.Trimmed | AlphanumComparerFlags.IgnoreCase);
        }

        #endregion




        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="AlphanumComparer" />.
        /// </summary>
        /// <param name="culture"> The used culture. </param>
        /// <param name="options"> The used comparison options. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="culture" /> is null. </exception>
        public AlphanumComparer (CultureInfo culture, AlphanumComparerFlags options)
        {
            if (culture == null)
            {
                throw new ArgumentNullException(nameof(culture));
            }

            this.Culture = culture;
            this.Options = options;

            this.NumberCharacters = new HashSet<string>(StringComparer.Ordinal);

            if ((options & AlphanumComparerFlags.NumberDecimalSeparator) == AlphanumComparerFlags.NumberDecimalSeparator)
            {
                this.NumberCharacters.Add(this.Culture.NumberFormat.NumberDecimalSeparator);
            }

            if ((options & AlphanumComparerFlags.PositiveSign) == AlphanumComparerFlags.PositiveSign)
            {
                this.NumberCharacters.Add(this.Culture.NumberFormat.PositiveSign);
            }

            if ((options & AlphanumComparerFlags.NegativeSign) == AlphanumComparerFlags.NegativeSign)
            {
                this.NumberCharacters.Add(this.Culture.NumberFormat.NegativeSign);
            }

            if ((options & AlphanumComparerFlags.NumberGroupSeparator) == AlphanumComparerFlags.NumberGroupSeparator)
            {
                this.NumberCharacters.Add(this.Culture.NumberFormat.NumberGroupSeparator);
            }
        }

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets the used culture.
        /// </summary>
        /// <value>
        ///     The used culture.
        /// </value>
        public CultureInfo Culture { get; }

        /// <summary>
        ///     Gets whether comparison is performed case-insensitive.
        /// </summary>
        /// <value>
        ///     true if the case is ignored, false otherwise.
        /// </value>
        public bool IgnoreCase => (this.Options & AlphanumComparerFlags.IgnoreCase) == AlphanumComparerFlags.IgnoreCase;

        /// <summary>
        ///     Gets the used comparison options.
        /// </summary>
        /// <value>
        ///     The used comparison options.
        /// </value>
        public AlphanumComparerFlags Options { get; }

        /// <summary>
        ///     Gets whether comparison is performed trimmed.
        /// </summary>
        /// <value>
        ///     true if the values are trimmed of whitespaces before being compared.
        /// </value>
        public bool Trimmed => (this.Options & AlphanumComparerFlags.Trimmed) == AlphanumComparerFlags.Trimmed;

        private HashSet<string> NumberCharacters { get; }

        #endregion




        #region Instance Methods

        private int CompareChunks (string x, string y)
        {
            NumberStyles numberCompareStyles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowLeadingWhite | NumberStyles.AllowThousands | NumberStyles.AllowTrailingWhite;

            double xValue;
            double yValue;

            if (double.TryParse(x, numberCompareStyles, this.Culture, out xValue) && double.TryParse(y, numberCompareStyles, this.Culture, out yValue))
            {
                return xValue.CompareTo(yValue);
            }

            CompareInfo comparer = this.Culture.CompareInfo;
            int result = comparer.Compare(x, y, this.IgnoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
            return result;
        }

        private bool IsDigit (char chr)
        {
            return char.IsDigit(chr) || this.NumberCharacters.Contains(new string(chr, 1));
        }

        private int ReadWhileSameType (string str, int startIndex, StringBuilder sb)
        {
            bool isDigit = false;
            int readCount;

            for (readCount = startIndex; readCount < str.Length; readCount++)
            {
                char chr = str[readCount];

                if (readCount == 0)
                {
                    isDigit = this.IsDigit(chr);
                    sb.Append(chr);
                }
                else
                {
                    if (isDigit != this.IsDigit(chr))
                    {
                        break;
                    }

                    sb.Append(chr);
                }
            }

            return readCount;
        }

        #endregion




        #region Interface: ICloneable<AlphanumComparer>

        /// <inheritdoc />
        public AlphanumComparer Clone ()
        {
            CultureInfo culture = (CultureInfo)this.Culture.Clone();
            AlphanumComparer clone = new AlphanumComparer(culture, this.Options);
            return clone;
        }

        /// <inheritdoc />
        object ICloneable.Clone ()
        {
            return this.Clone();
        }

        #endregion




        #region Interface: IComparer

        /// <inheritdoc />
        /// <exception cref="InvalidTypeArgumentException"> <paramref name="x" /> or <paramref name="y" /> is not null and not of type <see cref="string" />. </exception>
        int IComparer.Compare (object x, object y)
        {
            if ((x != null) && !(x is string))
            {
                throw new InvalidTypeArgumentException(nameof(x));
            }

            if ((x != null) && !(y is string))
            {
                throw new InvalidTypeArgumentException(nameof(x));
            }

            return this.Compare((string)x, (string)y);
        }

        #endregion




        #region Interface: IComparer<string>

        /// <inheritdoc />
        public int Compare (string x, string y)
        {
            if ((x == null) && (y == null))
            {
                return 0;
            }

            if (x == null)
            {
                return -1;
            }

            if (y == null)
            {
                return 1;
            }

            if (this.Trimmed)
            {
                x = x.Trim();
                y = y.Trim();
            }

            if ((x.Length == 0) && (y.Length == 0))
            {
                return 0;
            }

            if (x.Length == 0)
            {
                return -1;
            }

            if (y.Length == 0)
            {
                return 1;
            }

            int xIndex = 0;
            int yIndex = 0;

            StringBuilder xChunk = new StringBuilder();
            StringBuilder yChunk = new StringBuilder();

            while ((xIndex < x.Length) && (yIndex < y.Length))
            {
                xChunk.Remove(0, xChunk.Length);
                yChunk.Remove(0, xChunk.Length);

                int xRead = this.ReadWhileSameType(x, xIndex, xChunk);
                int yRead = this.ReadWhileSameType(y, yIndex, yChunk);

                int result = this.CompareChunks(xChunk.ToString(), yChunk.ToString());

                if (result != 0)
                {
                    return result;
                }

                xIndex += xRead;
                yIndex += yRead;
            }

            while (xIndex < x.Length)
            {
                xChunk.Append(x[xIndex]);
                xIndex++;
            }

            while (yIndex < y.Length)
            {
                yChunk.Append(y[yIndex]);
                yIndex++;
            }

            return this.CompareChunks(xChunk.ToString(), yChunk.ToString());
        }

        #endregion




        #region Interface: IEqualityComparer

        /// <inheritdoc />
        /// <exception cref="InvalidTypeArgumentException"> <paramref name="x" /> or <paramref name="y" /> is not null and not of type <see cref="string" />. </exception>
        bool IEqualityComparer.Equals (object x, object y)
        {
            if ((x != null) && !(x is string))
            {
                throw new InvalidTypeArgumentException(nameof(x));
            }

            if ((x != null) && !(y is string))
            {
                throw new InvalidTypeArgumentException(nameof(x));
            }

            return this.Equals((string)x, (string)y);
        }

        /// <inheritdoc />
        /// <exception cref="InvalidTypeArgumentException"> <paramref name="obj" /> is not null and not of type <see cref="string" />. </exception>
        int IEqualityComparer.GetHashCode (object obj)
        {
            if (obj == null)
            {
                return 0;
            }

            if ((obj != null) && !(obj is string))
            {
                throw new InvalidTypeArgumentException(nameof(obj));
            }

            return this.GetHashCode(obj as string);
        }

        #endregion




        #region Interface: IEqualityComparer<string>

        /// <inheritdoc />
        public bool Equals (string x, string y)
        {
            return this.Compare(x, y) == 0;
        }

        /// <inheritdoc />
        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse"),]
        [SuppressMessage("ReSharper", "HeuristicUnreachableCode"),]
        public int GetHashCode (string obj)
        {
            if (obj == null)
            {
                return 0;
            }

            CompareInfo comparer = this.Culture.CompareInfo;
            int hashCode = comparer.GetHashCode(obj, this.IgnoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
            return hashCode;
        }

        #endregion
    }
}
