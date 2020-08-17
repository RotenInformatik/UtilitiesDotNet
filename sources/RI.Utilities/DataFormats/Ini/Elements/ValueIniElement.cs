using System;

using RI.Utilities.Exceptions;
using RI.Utilities.Text;




namespace RI.Utilities.DataFormats.Ini.Elements
{
    /// <summary>
    ///     Represents a name-value-pair in INI data.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         See <see cref="IniDocument" /> for more general and detailed information about working with INI data.
    ///     </para>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    public sealed class ValueIniElement : IniElement
    {
        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="ValueIniElement" />.
        /// </summary>
        /// <param name="name"> The name. </param>
        /// <param name="value"> The value. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="name" /> is null. </exception>
        /// <exception cref="EmptyStringArgumentException"> <paramref name="name" /> is an empty string. </exception>
        public ValueIniElement (string name, string value)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (name.IsEmptyOrWhitespace())
            {
                throw new EmptyStringArgumentException(nameof(name));
            }

            this.Name = name;
            this.Value = value;
        }

        #endregion




        #region Instance Fields

        private string _name;

        private string _value;

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        /// <exception cref="ArgumentNullException"> <paramref name="value" /> is null. </exception>
        /// <exception cref="EmptyStringArgumentException"> <paramref name="value" /> is an empty string. </exception>
        public string Name
        {
            get => this._name;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (value.IsEmptyOrWhitespace())
                {
                    throw new EmptyStringArgumentException(nameof(value));
                }

                this._name = value;
            }
        }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        /// <value>
        ///     The value.
        /// </value>
        /// <remarks>
        ///     <note type="note">
        ///         The value returned by this property is never null.
        ///         If null is set, it is replaced with <see cref="string.Empty" />.
        ///     </note>
        /// </remarks>
        public string Value
        {
            get => this._value;
            set => this._value = value ?? string.Empty;
        }

        #endregion




        #region Overrides

        /// <inheritdoc />
        public override string ToString ()
        {
            return this.Name + IniSettings.DefaultNameValueSeparator + this.Value;
        }

        #endregion
    }
}
