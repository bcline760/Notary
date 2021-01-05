using System;
using System.Collections.Generic;
using System.Text;

namespace Notary
{
    public static class Constants
    {
        /// <summary>
        /// A key size of 1,024 bits
        /// </summary>
        public static readonly int KeySizeSmall = 1024;

        /// <summary>
        /// A key size of 2,048 bits
        /// </summary>
        public static readonly int KeySizeMedium = 2048;

        /// <summary>
        /// A key size of 4,096 bits
        /// </summary>
        public static readonly int KeySizeLarge = 4096;

        /// <summary>
        /// The number of iterations to use for password hashing. More = secure, but more = slower
        /// </summary>
        public static readonly int PasswordHashIterations = 4096;

        /// <summary>
        /// Number of bytes to use for password hashes
        /// </summary>
        public static readonly int PasswordHashLength = 16;

        /// <summary>
        /// The salt used for password hashing.
        /// </summary>
        public static string PasswordHashSalt = "jCRB2r#r5-K@#%PqBHnL73mRP?^eGb*cT8=d7-bAMD4z8%Fb";

        public static string SigningCertificateName = "notary.pem";
    }
}
