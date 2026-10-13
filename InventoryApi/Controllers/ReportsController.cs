using InventoryApi.Dtos;
using InventoryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _service;

        public ReportsController(IReportService service) => _service = service;

        [HttpGet("low-stock")]
        public async Task<ActionResult<List<ProductDto>>> LowStock([FromQuery] int threshold = 10) =>
            Ok(await _service.GetLowStockAsync(threshold));

        [HttpGet("top-products")]
        public async Task<ActionResult<List<TopProductDto>>> TopProducts([FromQuery] int count = 5) =>
            Ok(await _service.GetTopOrderedAsync(count));
    }
}
