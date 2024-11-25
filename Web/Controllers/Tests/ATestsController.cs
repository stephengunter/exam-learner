using ApplicationCore.DataAccess;
using ApplicationCore.Models;
using Ardalis.Specification;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Web.Controllers.Tests;

public class ATestsController : BaseTestController
{
   private readonly DefaultContext _defaultContext;
   public ATestsController(DefaultContext defaultContext)
   {
      _defaultContext = defaultContext;
   }
   [HttpGet]
   public async Task<ActionResult> Index()
   {
      return Ok();
   }


   [HttpGet("ex")]
   public ActionResult Ex()
   {
      throw new Exception("Test Throw Exception");
   }
}
