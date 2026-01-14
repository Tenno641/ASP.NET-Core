using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.Controllers;

[Route("api/{version:apiVersion}/[controller]")]
[ApiController]
public class CustomControllerBase : ControllerBase { }
