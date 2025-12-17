using Microsoft.AspNetCore.Mvc;

namespace AspControllers.Controllers;

public class CustomController : Controller
{
    [Route("welcome")]
    public IActionResult Method()
    {
        return new ContentResult()
        {
            Content = """
            {
              "name": "User-Name"
            }
            """,
            ContentType = "application/json"
        };
    }

    [Route("person")]
    public JsonResult Person()
    {
        return new JsonResult(new { Id = Guid.NewGuid(), FirstName = "User-First-Name", LastName = "User-Last-Name", Age = 23 });
        // return Json(person);
    }
}
