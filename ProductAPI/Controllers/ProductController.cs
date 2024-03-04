using Microsoft.AspNetCore.Mvc;
using ProductAPI.Helpers;
using ProductAPI.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [Route("products")]
        [SwaggerResponse(StatusCodes.Status200OK, "Get Products details", typeof(List<Product>))]
        public IActionResult GetAllProducts()
        {
            var products = CommonHelper.GetMockModel<List<Product>>();
            return Ok(new { success = true, products = products });
        }
    }
}
