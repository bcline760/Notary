using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using log4net;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Notary.Contract;
using Notary.Interface.Service;

namespace Notary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : NotaryController
    {
        public AccountController(
            ILog log,
            IAccountService service,
            ISessionService sessionService) : base(log)
        {
            AccountService = service;
            SessionService = sessionService;
        }

        [HttpPost, Route("")]
        public async Task<IActionResult> CreateAsync(Account newAccount)
        {
            var result = await ExecuteServiceMethod(AccountService.RegisterAccountAsync, newAccount, "CreateAsync", DesiredStatusCode.Created);

            return result;
        }

        [HttpGet, Route("{slug}")]
        public async Task<IActionResult> GetAsync(string slug)
        {
            var result = await ExecuteServiceMethod(AccountService.GetAsync, slug, "GetAsync", DesiredStatusCode.OK);

            return result;
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await ExecuteServiceMethod(AccountService.GetAllAsync, "GetAllAsync", DesiredStatusCode.OK);

            return result;
        }

        [HttpGet, Route("{slug}/sessions/{activeOnly}")]
        public async Task<IActionResult> GetAllSessionsAsync(string slug, bool activeOnly)
        {
            //var sessions = await ExecuteServiceMethod(SessionService.)
        }

        [HttpPut, Route("")]
        public async Task<IActionResult> UpdateAsync(Account newAccount)
        {

        }

        [HttpDelete, Route("{slug}")]
        public async Task<IActionResult> DeleteAsync(string slug)
        {

        }

        protected IAccountService AccountService { get; }

        protected ISessionService SessionService { get; }
    }
}
