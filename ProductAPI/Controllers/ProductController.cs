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
        private readonly IConfiguration _configuration;
        private readonly bool _isPaginationEnabled;
        private readonly int _pageSize;

        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _isPaginationEnabled = Convert.ToBoolean(_configuration["IsPaginnationEnabled"]);
            _pageSize = Convert.ToInt32(_configuration["PageSize"]);
        }

        [HttpGet]
        [Route("products")]
        [SwaggerResponse(StatusCodes.Status200OK, "Get Products details", typeof(List<Product>))]
        public IActionResult GetAllProducts()
        {
            var products = CommonHelper.GetMockModel<List<Product>>();

            if (_isPaginationEnabled) 
            {
                products = products.Take(_pageSize).ToList();
            }

            return Ok(new { success = true, products = products });
        }
    }
}
