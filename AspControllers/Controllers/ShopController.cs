using AspControllers.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspControllers.Controllers;

public class ShopController : Controller
{
    [Route("order")]
    public IActionResult Order(Order order)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        order.OrderNo = Random.Shared.Next(1, 100000);
        return Ok(order);
    }
}
