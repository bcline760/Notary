using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notary.Contract;
using Notary.Interface.Service;

namespace Notary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificatesController : NotaryDataController<Certificate>
    {
        public CertificatesController(ICertificateService certService, ILog log) : base(certService, log)
        {
        }

        [HttpPost, Route("{slug}/download"), Authorize(Roles = "Admin,CertificateAdmin,User")]
        public async Task<IActionResult> DownloadCertificateAsync(string slug, CertificateFormat format, string pwd = null)
        {
            if (format == CertificateFormat.Pkcs12 && string.IsNullOrEmpty(pwd))
                return BadRequest("Please supply a private key password for PKCS #12 certificates");

            var service = (ICertificateService)Service;

            byte[] certBinary = await service.DownloadCertificateAsync(slug, format, pwd);

            if (certBinary == null)
                return NotFound();

            return Ok(certBinary);
        }

        [HttpPost, Route("issue")]
        public async Task<IActionResult> IssueCertificateAsync([FromBody]CertificateRequest request)
        {
            if (request == null)
                return BadRequest();

            var service = (ICertificateService)Service;

            var result = await ExecuteServiceMethod(service.IssueCertificateAsync, request, System.Net.HttpStatusCode.OK);

            return result;
        }

        [HttpDelete, Route("{slug}"), Authorize(Roles = "Admin,CertificateAdmin")]
        public override Task<IActionResult> DeleteAsync(string slug)
        {
            return base.DeleteAsync(slug);
        }
    }
}