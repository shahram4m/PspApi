using Sample.Application.Services;
using Sample.Application.ViewModels.Terminal;
using Sample.Core.Api;
using Sample.Core.Api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sample.WebApi.Controllers
{
    [RoutePrefix("api/v1/dictionaries")]
    public class DictionariesController : BaseServiceController
    {
        public DictionariesService ServiceForDictionaries { get; set; }

        [Route("keys")]
        public async Task<IHttpActionResult> GetKeys(ApiParameters apiParameters)
        {
            var serviceResult = await ServiceForDictionaries.GetDictionaryKeys(apiParameters);

            return CreateServiceResult(serviceResult);
        }

        [Route("{key}/value")]
        public async Task<IHttpActionResult> GetValue(string key)
        {
            var serviceResult = await ServiceForDictionaries.GetDictionaryValue(key);

            return CreateServiceResult(serviceResult);
        }

        [Authorize]
        [HttpPost]
        [Route("GetName")]
        public async Task<IHttpActionResult> GetName()
        {
            var identity = User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                var name = claims.Where(p => p.Type == "userid").FirstOrDefault()?.Value;
                string result = name;
                return this.Json(result);
                //return new
                //{
                //    data = name
                //};
            }
            return null;
        }

        [Authorize]
        [HttpPost]
        [Route("GetTerminalData")]
        public async Task<IHttpActionResult> GetTerminalData([FromBody] TerminalViewModel terminalModel)
        {
            return this.Json(terminalModel);
        }

    }
}
