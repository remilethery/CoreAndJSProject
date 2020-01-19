using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SpyStore.Dal.Repos.Interfaces;
using SpyStore.Models.Entities;

namespace SpyStore.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController: ControllerBase
    {
        private readonly IProductRepo _repo;
        public SearchController(IProductRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("{searchString", Name = "SearchProducts")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<IList<Product>> Search(string searchString)
        {
            var results = _repo.Search(searchString).ToList();
            if (results.Count == 0)
            {
                return NotFound();
            }
            return results;
        }
    }

}