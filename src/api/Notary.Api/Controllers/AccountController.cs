using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Net;
using System.Threading.Tasks;

using log4net;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Notary.Contract;
using Notary.Interface.Service;
using Notary.Logging;

namespace Notary.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : NotaryDataController<Account>
    {
        public AccountController(
            ILog log,
            IAccountService service,
            ISessionService sessionService) : base(service, log)
        {
            SessionService = sessionService;
        }

        [HttpGet, Route("{slug}/sessions/{activeOnly}")]
        public async Task<IActionResult> GetAllSessionsAsync(string slug, bool activeOnly)
        {
            var sessions = await ExecuteServiceMethod(SessionService.GetSessions, slug, activeOnly, HttpStatusCode.OK, HttpStatusCode.NotFound);

            return sessions;
        }

        protected ISessionService SessionService { get; }
    }
}
