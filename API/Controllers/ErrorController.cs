using ECommerce.API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("errors/{code}")]
    [ApiExplorerSettings(IgnoreApi =true)]
    public class ErrorController:BaseApiController
    {
        public IActionResult Errors(int code)
        {
            return new ObjectResult(new ApiResponse(code));
        }
    }
}