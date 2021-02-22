using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using log4net;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Notary.Logging;
using Notary.Contract;
using Notary.Interface.Service;

namespace Notary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncryptionController : NotaryController
    {
        public EncryptionController(IAccountService acctSvc, IEncryptionService encSvc, ILog log) : base(log)
        {
            AccountService = acctSvc;
            EncryptionService = encSvc;
        }

        [HttpPost, Route("encrypt")]
        public async Task<IActionResult> EncryptAsync()
        {
            var postedFile = Request.Form.Files[0];
            var slugClaim = User.Claims.First(c => c.Type == "slug");
            try
            {
                var account = await AccountService.GetAsync(slugClaim.Value);
                if (account != null && account.Active)
                {
                    using (var stream = postedFile.OpenReadStream())
                    {
                        byte[] unencrypted = new byte[stream.Length];
                        stream.Read(unencrypted);

                        byte[] encryptedData = EncryptionService.Encrypt(unencrypted, account.Slug);
                        return Ok(encryptedData);
                    }
                }

                return Forbid();
            }
            catch (Exception ex)
            {
                ex.IfNotLoggedThenLog(Log);
                return StatusCode(500);
            }
        }

        [HttpPost, Route("decrypt")]
        public async Task<IActionResult> DecryptAsync()
        {
            var slugClaim = User.Claims.First(c => c.Type == "slug");
            var postedFile = Request.Form.Files[0];
            try
            {
                var account = await AccountService.GetAsync(slugClaim.Value);
                if (account != null && account.Active)
                {
                    using (var stream = postedFile.OpenReadStream())
                    {
                        byte[] encrypted = new byte[stream.Length];
                        stream.Read(encrypted);

                        byte[] decryptedData = EncryptionService.Decrypt(encrypted, account.Slug);
                        return Ok(decryptedData);
                    }
                }
                return Forbid();
            }
            catch (Exception ex)
            {
                ex.IfNotLoggedThenLog(Log);
                return StatusCode(500);
            }
        }

        private IAccountService AccountService { get; }

        private IEncryptionService EncryptionService { get; }
    }
}
