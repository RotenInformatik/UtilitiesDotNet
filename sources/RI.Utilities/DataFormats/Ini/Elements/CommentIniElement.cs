namespace RI.Utilities.DataFormats.Ini.Elements
{
    /// <summary>
    ///     Represents a comment in INI data.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         See <see cref="IniDocument" /> for more general and detailed information about working with INI data.
    ///     </para>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    /// TODO: Implement ICopyable
    /// TODO: Implement ICloneable
    public sealed class CommentIniElement : IniElement
    {
        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="CommentIniElement" />.
        /// </summary>
        /// <param name="comment"> The comment. </param>
        public CommentIniElement (string comment)
        {
            this.Comment = comment;
        }

        #endregion




        #region Instance Fields

        private string _comment;

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets or sets the comment.
        /// </summary>
        /// <value>
        ///     The comment.
        /// </value>
        /// <remarks>
        ///     <note type="note">
        ///         The value returned by this property is never null.
        ///         If null is set, it is replaced with <see cref="string.Empty" />.
        ///     </note>
        /// </remarks>
        public string Comment
        {
            get => this._comment;
            set => this._comment = value ?? string.Empty;
        }

        #endregion




        #region Overrides

        /// <inheritdoc />
        public override string ToString ()
        {
            return IniSettings.DefaultCommentStart + this.Comment;
        }

        #endregion
    }
}
