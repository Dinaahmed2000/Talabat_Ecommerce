using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Repository.Data;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext _dbcontext;

        public BuggyController(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        [HttpGet("notfound")]  //api/buggy/notfound
        public ActionResult getNotFoundRequest()
        {
            var product = _dbcontext.Products.Find(100);
            if (product is null)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(product);
        }
        [HttpGet("servererror")]
        public ActionResult getServerError()
        {
            var product = _dbcontext.Products.Find(100);
            var productToReturn = product.ToString();
            return Ok(productToReturn);
        }
        [HttpGet("badrequest")]
        public ActionResult getBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }
        [HttpGet("badrequest/{id}")]  //api/buggy/badrequest/five
        public ActionResult getBadRequest(int id)  //validation error
        {
            return Ok();
        }


    }
}
