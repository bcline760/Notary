using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using log4net;
using Notary.Contract;
using Notary.Interface.Service;

namespace Notary.Api.Controllers
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

        [AllowAnonymous]
        [HttpPost, Route("signin")]
        public async Task<IActionResult> SignInAsync([FromBody]BasicCredentials credentials)
        {
            if (credentials == null)
                return BadRequest();

            var token = await ExecuteServiceMethod(sessionService.SignInAsync, credentials, HttpStatusCode.OK, HttpStatusCode.Unauthorized);

            return token;
        }

        [HttpPost, Route("signout")]
        public async Task<IActionResult> SignOutAsync()
        {
            if (!User.Identity.IsAuthenticated)
                return BadRequest();

            if (User.HasClaim(c => c.Type == "slug"))
            {
                string slug = User.Claims.First(s => s.Type == "slug").Value;

                return await ExecuteServiceMethod(sessionService.SignoutAsync, slug, HttpStatusCode.OK);
            }

            return NoContent();
        }

        private readonly ISessionService sessionService;
        private readonly IAccountService accountService;
    }
}
