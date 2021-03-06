﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using log4net;

using Notary.Contract;
using Notary.Interface.Repository;
using Notary.Interface.Service;

namespace Notary.Service
{
    public class TokenService : EntityService<AuthenticatedUser>, ITokenService
    {
        public TokenService(ITokenRepository repo, ILog log):base(repo, log)
        {
        }

        public async Task<List<AuthenticatedUser>> GetAccountTokens(string slug)
        {
            var repo = (ITokenRepository)Repository;

            return await repo.GetAccountTokens(slug);
        }
    }
}
