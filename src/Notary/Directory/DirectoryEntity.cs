using System;
using System.Collections.Generic;
using System.Text;

namespace Notary.Directory
{
    public class DirectoryEntity
    {
        public string AccountName { get; set; }

        public string CommonName { get; set; }

        public string EmailAddress { get; set; }

        public string Name { get; set; }

        public string ObjectCategory { get; set; }

        public string ObjectGuid { get; set; }

        public string ObjectSid { get; set; }

    }
}
