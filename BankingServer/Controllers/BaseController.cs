using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BankingServer.Controllers
{
    [Route("[controller]")]
    public class BaseController : Controller
    {
        [HttpOptions]
        public IActionResult Options()
        {
            return Ok();
        }
        internal OkObjectResult Ok(string message)
        {
            return Ok(new List<string> { message});
        }

        internal BadRequestObjectResult BadRequest(IEnumerable<string> messages)
        {
            var responseObject = BuildErrorResponseWithReferenceId(messages);
            return base.BadRequest(responseObject);
        }

        private ErrorResponse BuildErrorResponseWithReferenceId(IEnumerable<string> messages)
        {
            return new ErrorResponse
            {
                Errors = messages
            };

        }
    }

    class ErrorResponse
    {
        public string ReferenceId { get; set; }
        public int Code { get; set; }
        public string ErrorMessage { get; set; }
        public string Stacktrace { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
