using FIFA_API.Models.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace FIFA_API.Controllers
{
    public partial class ProduitsController
    {
        [HttpGet("Search")]
        [ActionName("Search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Produit>>> SearchProducts([FromQuery] string q)
        {
            var products = await _manager.SearchProductsAsync(q);
            return Ok(products);
        }
    }
}
