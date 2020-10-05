
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Notary.Contract;
using Notary.Interface.Service;

namespace Notary.Web.Controllers
{
    [Route("api/[controller]")]
    public class SessionController : NotaryController
    {
        public ISessionService Session { get; }

        public SessionController(ILog log, ISessionService service) : base(log)
        {
            Session = service;
        }

        [AllowAnonymous, Route("signin"), HttpPost]
        public async Task<IActionResult> SignInAsync(string username, string password, bool persist)
        {
            var credentials = new BasicCredentials(username, password, persist);
            var token = await Session.SignInAsync(credentials);

            IActionResult result = null;
            if (token == null)
                result = Unauthorized();
            else
                result = Ok(result);

            return result;
        }

        [Route("signout"), HttpGet]
        public async Task<IActionResult> SignOutAsync(string slug)
        {
            var result = await ExecuteServiceMethod(Session.SignoutAsync, slug, "SignoutAsync", DesiredStatusCode.OK);

            return result;
        }
    }
}
