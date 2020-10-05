using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Notary.Logging;
using Notary.Contract;
using Notary.Interface.Service;

using log4net;
using System.Net;

namespace Notary.Web.Controllers
{
    public abstract class NotaryDataController<T> : NotaryController
        where T : Entity
    {
        protected IEntityService<T> Service { get; private set; }
        protected NotaryDataController(IEntityService<T> service, ILog log) : base(log)
        {
            Service = service;
        }

        [HttpGet, Route("")]
        public virtual async Task<IActionResult> GetAllAsync()
        {
            var result = await ExecuteServiceMethod(Service.GetAllAsync, "GetAllAsync", DesiredStatusCode.OK);

            return result;
        }

        [HttpGet, Route("{slug}")]
        public virtual async Task<IActionResult> GetAsync(string slug)
        {
            var result = await ExecuteServiceMethod(Service.GetAsync, slug, "GetAsync", DesiredStatusCode.OK);

            return result;
        }

        [HttpPost, Route("")]
        public virtual async Task<IActionResult> PostAsync(T contract)
        {
            try
            {
                var slugClaim = User.Claims.FirstOrDefault(c => c.Type == "slug");

                await Service.SaveAsync(contract, slugClaim.Value);
                return Created(Request.Path.ToUriComponent(), contract);
            }
            catch (Exception ex)
            {
                ex.IfNotLoggedThenLog(Log);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete, Route("")]
        public virtual async Task<IActionResult> DeleteAsync(string slug)
        {
            var slugClaim = User.Claims.FirstOrDefault(c => c.Type == "slug");

            try
            {
                var contract = await Service.GetAsync(slug);
                if (contract != null)
                {
                    contract.Active = false;
                    contract.Updated = DateTime.Now;

                    await Service.SaveAsync(contract, slugClaim.Value);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                ex.IfNotLoggedThenLog(Log);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
