using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;




namespace RI.Utilities.Binary
{
    /// <summary>
    ///     Provides utility/extension methods for the <see cref="byte" /> array type.
    /// </summary>
    /// <threadsafety static="false" instance="false" />
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Encodes a byte array as Base64 string.
        /// </summary>
        /// <param name="data">The byte array.</param>
        /// <param name="options">The Base64 formatting options. Default value is <see cref="Base64FormattingOptions.None"/>.</param>
        /// <returns>
        /// The Base64 string or an empty string if the byte array is empty.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="data" /> is null. </exception>
        public static string EncodeBase64(this byte[] data, Base64FormattingOptions options = Base64FormattingOptions.None)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Length == 0)
            {
                return string.Empty;
            }

            return Convert.ToBase64String(data, options);
        }

        /// <summary>
        /// Encodes a byte array as a hexadecimal string.
        /// </summary>
        /// <param name="data">The byte array.</param>
        /// <returns>
        /// The hexadecimal string or an empty string if the byte array is empty.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="data" /> is null. </exception>
        public static string EncodeHex (this byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Length == 0)
            {
                return string.Empty;
            }

            StringBuilder str = new StringBuilder(data.Length * 2, data.Length * 2);

            for (int i1 = 0; i1 < data.Length; i1++)
            {
                str.Append(data[i1].ToString("x2", CultureInfo.InvariantCulture));
            }

            return str.ToString();
        }

        /// <summary>
        /// Computes a GUID of a byte array.
        /// </summary>
        /// <param name="data">The byte array.</param>
        /// <returns>
        /// The GUID.
        /// </returns>
        /// <remarks>
        /// <note type="note">
        /// <see cref="ComputeGuid"/> first reduces the byte array to a MD5 hash and uses this to construct a GUID.
        /// </note>
        ///<note type="security">
        /// Do not use <see cref="ComputeGuid"/> for security relevant operations as it uses MD5 internally.
        /// </note>
        /// </remarks>
        public static Guid ComputeGuid (this byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Length == 0)
            {
                return Guid.Empty;
            }

            byte[] bytes = data.ComputeMd5();
            return new Guid(bytes);
        }

        /// <summary>
        /// Computes the MD5 hash of a byte array.
        /// </summary>
        /// <param name="data">The byte array.</param>
        /// <returns>
        /// The MD5 hash.
        /// </returns>
        /// <remarks>
        ///<note type="security">
        /// Do not use <see cref="ComputeMd5"/> for security relevant operations.
        /// </note>
        /// </remarks>
        public static byte[] ComputeMd5 (this byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Length == 0)
            {
                return new byte[16];
            }

            using (MD5 algorithm = MD5.Create())
            {
                return algorithm.ComputeHash(data);
            }
        }
    }
}
