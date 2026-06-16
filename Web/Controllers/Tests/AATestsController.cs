using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Web.Controllers;
using ApplicationCore.Helpers;

namespace Web.Controllers.Tests;

public class AATestsController : BaseTestController
{

   public AATestsController()
   {
      
   }
   [HttpGet]
   public async Task<ActionResult> Index(string input)
   {
      return Ok();
   }
}
