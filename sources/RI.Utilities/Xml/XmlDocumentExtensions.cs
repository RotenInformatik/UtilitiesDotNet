using System;
using System.Xml;
using System.Xml.Linq;




namespace RI.Utilities.Xml
{
    /// <summary>
    ///     Provides utility/extension methods for the <see cref="XmlDocument" /> type.
    /// </summary>
    /// <threadsafety static="false" instance="false" />
    public static class XmlDocumentExtensions
    {
        #region Static Methods

        /// <summary>
        ///     Converts a <see cref="XmlDocument" /> to a <see cref="XDocument" />.
        /// </summary>
        /// <param name="xmlDocument"> The <see cref="XmlDocument" /> to convert. </param>
        /// <returns> The converted <see cref="XDocument" />. </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="xmlDocument" /> is null. </exception>
        /// TODO: Add overload with LoadOptions
        public static XDocument ToXDocument (this XmlDocument xmlDocument)
        {
            if (xmlDocument == null)
            {
                throw new ArgumentNullException(nameof(xmlDocument));
            }

            using (XmlNodeReader nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }

        #endregion
    }
}
