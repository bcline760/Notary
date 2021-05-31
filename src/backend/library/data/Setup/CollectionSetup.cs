using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Notary.Interface;

namespace Notary.Data.Setup
{
    public abstract class CollectionSetup : ICollectionSetup
    {
        public abstract Task Setup();
    }
}
