using AutoMapper.Configuration.Annotations;
using ECommerce.API.Errors;
using ECommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ECommerce.API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext _context;

        public BuggyController(StoreContext context)
        {
            _context = context;
        }
        [HttpGet("testauth")]
        [Authorize]
        public ActionResult<string> GetSecretText()
        {
            return Ok("Secret stuff");
        }

        [HttpGet("not-found")]
        public ActionResult GetNotFoundRequest()
        {
            var thing = new Nullable<int>();// _context.Products.Find(42);
            if (thing == null)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok();
        }
        [HttpGet("server-error")]
        public ActionResult GetServerError()
        {
            var thing = _context.Products.Find(42);
            var thingToReturn = thing.ToString();
            return Ok(new ApiResponse(500));
        }
        [HttpGet("bad-request")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }
        [HttpGet("bad-request/{id}")]
        public ActionResult GetNotFoundRequest(int id)
        {
            return Ok(new ApiResponse(400));
        }

    }
}