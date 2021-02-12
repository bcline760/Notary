using System;
using System.Collections.Generic;
using System.Text;

namespace Notary.Data.Model
{
    public class SanModel
    {
        /// <summary>
        /// The kind of SAN
        /// </summary>
        public string Kind
        {
            get => default;
            set
            {
            }
        }

        /// <summary>
        /// Subject Alternative Name. Can be a DNS or e-mail
        /// </summary>
        public string Name
        {
            get => default;
            set
            {
            }
        }
    }
}
