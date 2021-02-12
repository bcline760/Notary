using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Notary.Logging;

namespace Notary.Api.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public abstract class NotaryController : ControllerBase
    {
        protected ILog Log { get; }
        protected NotaryController(ILog log)
        {
            Log = log;
        }

        /// <summary>
        /// Execute a service method
        /// </summary>
        /// <typeparam name="TOut">The service method return type</typeparam>
        /// <param name="serviceMethod">A delegate for the service method to be executed</param>
        /// <param name="successCode">The HTTP status code used to return on success</param>
        /// <param name="failCode">The HTTP status code used to return on failure</param>
        /// <returns>An HTTP action result with message and status code</returns>
        protected async Task<IActionResult> ExecuteServiceMethod<TOut>(
            Func<Task<TOut>> serviceMethod,
            HttpStatusCode successCode,
            HttpStatusCode failCode
        ) where TOut : class
        {
            IActionResult result = null;

            try
            {
                var response = await serviceMethod();

                result = response == null ? GetHttpResponseCode(failCode, response) : GetHttpResponseCode(successCode, response);
            }
            catch (Exception ex)
            {
                ex.IfNotLoggedThenLog(Log);
                result = StatusCode((int)HttpStatusCode.InternalServerError);
            }

            return result;
        }

        /// <summary>
        /// Execute a service method
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut">The service method return type</typeparam>
        /// <param name="serviceMethod">A delegate for the service method to be executed</param>
        /// <param name="svcParam">The parameter to be passed into the service</param>
        /// <param name="successCode"></param>
        /// <param name="failCode"></param>
        /// <returns>An HTTP action result with message and status code</returns>
        protected async Task<IActionResult> ExecuteServiceMethod<TIn, TOut>(
            Func<TIn, Task<TOut>> serviceMethod,
            TIn svcParam,
            HttpStatusCode successCode,
            HttpStatusCode failCode) where TOut: class
        {
            IActionResult result = null;

            try
            {
                var response = await serviceMethod(svcParam);

                result = response == null ? GetHttpResponseCode(failCode, response) : GetHttpResponseCode(successCode, response);
            }
            catch (Exception ex)
            {
                ex.IfNotLoggedThenLog(Log);
                result = StatusCode((int)HttpStatusCode.InternalServerError);
            }

            return result;
        }

        /// <summary>
        /// Execute a service method
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <param name="serviceMethod">A delegate for the service method to be executed</param>
        /// <param name="successCode"></param>
        /// <returns>An HTTP action result with message and status code</returns>
        protected async Task<IActionResult> ExecuteServiceMethod<TIn>(
            Func<TIn, Task> serviceMethod,
            TIn svcParam,
            HttpStatusCode successCode)
        {
            IActionResult result = null;

            try
            {
                await serviceMethod(svcParam);

                return GetHttpResponseCode<object>(successCode, null);
            }
            catch (Exception ex)
            {
                ex.IfNotLoggedThenLog(Log);
                result = StatusCode((int)HttpStatusCode.InternalServerError);
            }

            return result;
        }

        /// <summary>
        /// Execute a service method
        /// </summary>
        /// <typeparam name="TIn">First argument type</typeparam>
        /// <typeparam name="TIn2">Second argument type</typeparam>
        /// <param name="serviceMethod">A delegate for the service method to be executed</param>
        /// <param name="param1">First argument of method</param>
        /// <param name="param2">Second argument of method</param>
        /// <param name="code">The HTTP status code to return upon completion</param>
        /// <returns></returns>
        protected async Task<IActionResult> ExecuteServiceMethod<TIn, TIn2>(
            Func<TIn, TIn2, Task> serviceMethod,
            TIn param1,
            TIn2 param2,
            HttpStatusCode code)
        {
            IActionResult result = null;

            try
            {
                await serviceMethod(param1, param2);

                return GetHttpResponseCode<object>(code, null);
            }
            catch (Exception ex)
            {
                ex.IfNotLoggedThenLog(Log);
                result = StatusCode((int)HttpStatusCode.InternalServerError);
            }

            return result;
        }

        /// <summary>
        /// Execute a service method
        /// </summary>
        /// <typeparam name="TIn">The type used by the first service method parameter</typeparam>
        /// <typeparam name="TIn2">The type used by the second service method parameter</typeparam>
        /// <typeparam name="TOut">The service method return type</typeparam>
        /// <param name="serviceMethod">A delegate for the service method to be executed</param>
        /// <param name="svcParam">The first service method parameter</param>
        /// <param name="svcParam2">The second service method parameter</param>
        /// <param name="successCode">The HTTP status code used to return on success</param>
        /// <param name="failCode">The HTTP status code used to return on failure</param>
        /// <returns>An HTTP action result with message and status code</returns>
        protected async Task<IActionResult> ExecuteServiceMethod<TIn, TIn2, TOut>(
            Func<TIn, TIn2, Task<TOut>> serviceMethod,
            TIn svcParam,
            TIn2 svcParam2,
            HttpStatusCode successCode,
            HttpStatusCode failCode) where TOut : class
        {
            IActionResult result = null;

            try
            {
                var response = await serviceMethod(svcParam, svcParam2);
                result = response == null ? GetHttpResponseCode(failCode, response) : GetHttpResponseCode(successCode, response);
            }
            catch (Exception ex)
            {
                ex.IfNotLoggedThenLog(Log);
                result = StatusCode((int)HttpStatusCode.InternalServerError);
            }

            return result;
        }

        /// <summary>
        /// Get an action result based on desired HTTP status code
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="code">The kind of HTTP status code</param>
        /// <param name="returnObject">The object to return if any. Set to object default if not desired</param>
        /// <param name="context">Additional context such as a URL</param>
        /// <returns>An HTTP action result with message and status code</returns>
        private IActionResult GetHttpResponseCode<TOut>(HttpStatusCode code, TOut returnObject) where TOut : class
        {
            switch (code)
            {
                case HttpStatusCode.OK:
                    return Ok(returnObject);
                case HttpStatusCode.Created:
                    return CreatedAtAction(Request.Path, returnObject);
                case HttpStatusCode.Accepted:
                    return Accepted(returnObject);
                case HttpStatusCode.NoContent:
                    return NoContent();
                case HttpStatusCode.BadRequest:
                    return BadRequest();
                case HttpStatusCode.Unauthorized:
                    return Unauthorized();
                case HttpStatusCode.Forbidden:
                    return Forbid();
                case HttpStatusCode.NotFound:
                    return NotFound();
                case HttpStatusCode.Conflict:
                    return Conflict(returnObject);
                default:
                    return StatusCode((int)code);
            }
        }
    }
}
