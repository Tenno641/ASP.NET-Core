using Microsoft.AspNetCore.Mvc;

namespace AspControllers.Controllers;

public class BankController : Controller
{
    [Route("/")]
    public IActionResult Index()
    {
        return Ok(new { Message = "Welcome to the bank" });
    }

    [Route("account-details")]
    public IActionResult AccountDetails()
    {
        return Ok(new { AccountNumber = 100, AccountHolderName = "User-Defined", CurrentBalance = 5000 });
    }

    [Route("account-statement")]
    public IActionResult AccountStatement()
    {
        return File("text.pdf", "application/pdf");
    }

    [Route("get-current-balance/{accountNumber:int?}")]
    public IActionResult GetCurrentBalance()
    {
        if (!Request.RouteValues.ContainsKey("accountNumber")) return NotFound("Account number should be supplided");
        int accountNumber = Convert.ToInt32(Request.RouteValues["accountNumber"]);
        if (accountNumber == 1001) return Ok(5000);
        else return BadRequest("Account number should be 1001");
    }
}
