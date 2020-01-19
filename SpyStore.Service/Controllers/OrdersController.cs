using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SpyStore.Dal.Repos.Interfaces;
using SpyStore.Models.Entities;

namespace SpyStore.Service.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepo _repo;

        public OrdersController(IOrderRepo _repo)
        {
            _repo = _repo;
        }

        [HttpGet("{OrderId}", Name = "GetOrderHistory")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetOrderHistory(int customerId)
        {
            _repo.Context.CustomerId = customerId;

            IList<Order> orderWithTotals = _repo.GetOrderHistory();
            return orderWithTotals == null ? (IActionResult)NotFound() : new ObjectResult(orderWithTotals);
        }
    }
}