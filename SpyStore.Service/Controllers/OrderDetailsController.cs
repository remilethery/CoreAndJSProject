using Microsoft.AspNetCore.Mvc;
using SpyStore.Dal.Repos.Interfaces;
using SpyStore.Models.ViewModels;

namespace SpyStore.Service.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderRepo _repo;

        public OrderDetailsController(IOrderRepo _repo)
        {
            _repo = _repo;
        }

        [HttpGet("{OrderId}", Name = "GetOrderDetails")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetOrderWithDetailsForCustomer(int orderId)
        {
            OrderWithDetailsAndProductInfo orderWithDetails = _repo.GetOneWithDetails(orderId);
            return orderWithDetails == null ? (IActionResult) NotFound() : new ObjectResult(orderWithDetails);
        }
    }

}