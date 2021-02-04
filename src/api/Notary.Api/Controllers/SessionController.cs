using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using log4net;
using NC=Notary.Contract;
using Notary.Interface.Service;

namespace Notary.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : NotaryController
    {
        public SessionController(ISessionService sessionSvc, IAccountService accountSvc, ILog log)
            : base(log)
        {
            sessionService = sessionSvc;
            accountService = accountSvc;
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignInAsync(NC.ICredentials credentials)
        {
            if (credentials == null)
                return BadRequest();

            var token = await ExecuteServiceMethod(sessionService.SignInAsync, credentials, HttpStatusCode.OK, HttpStatusCode.Unauthorized);

            return token;
        }

        private readonly ISessionService sessionService;
        private readonly IAccountService accountService;
    }
}
