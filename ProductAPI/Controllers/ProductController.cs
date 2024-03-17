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
        private readonly ILogger<ProductController> _logger;
        private readonly bool _isPaginationEnabled;
        private readonly int _pageSize;

        public ProductController(IConfiguration configuration, ILogger<ProductController> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _isPaginationEnabled = Convert.ToBoolean(_configuration["IsPaginnationEnabled"]);
            _pageSize = Convert.ToInt32(_configuration["PageSize"]);
        }

        [HttpGet]
        [Route("products")]
        [SwaggerResponse(StatusCodes.Status200OK, "Get Products details", typeof(List<Product>))]
        public IActionResult GetAllProducts()
        {
            var products = CommonHelper.GetMockModel<List<Product>>();

            _logger.LogInformation($"Pagination flag value :: {Environment.GetEnvironmentVariable("IsPaginnationEnabled")}");
            _logger.LogInformation($"Page size value :: {Environment.GetEnvironmentVariable("PageSize")}");

            _logger.LogInformation($"Pagination feature flag value is :: {_isPaginationEnabled} and Page size is :: {_pageSize}");

            if (_isPaginationEnabled) 
            {
                _logger.LogInformation("Applying pagination rules..");
                products = products.Take(_pageSize).ToList();
            }

            return Ok(new { success = true, products = products });
        }
    }
}
