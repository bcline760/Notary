using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;

using log4net;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Notary.Contract;
using Notary.Interface.Service;
using Notary.Logging;

namespace Notary.Web.Controllers
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
            var sessions = await ExecuteServiceMethod(SessionService.GetSessions, slug, activeOnly, "GetAllSessionsAsync", DesiredStatusCode.OK);

            return sessions;
        }

        [HttpPut, Route("")]
        public async Task<IActionResult> UpdateAsync(Account newAccount)
        {
            var user = Request.HttpContext.User;
            var claim = user.FindFirst("slug");

            ApiResponse<string> apiResponse = new ApiResponse<string>
            {
                Success = false
            };

            try
            {
                await AccountService.SaveAsync(newAccount, claim.Value);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = $"Failed to execute service operation SaveAsync.";
                ex.IfNotLoggedThenLog(Log);
                return BadRequest(apiResponse);
            }
        }

        [HttpDelete, Route("{slug}")]
        public async Task<IActionResult> DeleteAsync(string slug)
        {
            var user = Request.HttpContext.User;
            var claim = user.FindFirst("slug");

            ApiResponse<string> apiResponse = new ApiResponse<string>
            {
                Success = false
            };

            try
            {
                await AccountService.DeleteAsync(slug, claim.Value);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = $"Failed to execute service operation DeleteAsync.";
                ex.IfNotLoggedThenLog(Log);
                return BadRequest(apiResponse);
            }
        }

        protected IAccountService AccountService { get; }

        protected ISessionService SessionService { get; }
    }
}
