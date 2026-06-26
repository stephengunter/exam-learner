using ApplicationCore.Helpers;
using ApplicationCore.Models.Identity;
using ApplicationCore.Services;
using ApplicationCore.Services.Auth;
using ApplicationCore.Web.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Tests;

public class AATestsController : BaseTestController
{
   private readonly IUsersService _usersService;
   private readonly IJwtTokenService _jwtTokenService;
   public AATestsController(IUsersService usersService, IJwtTokenService jwtTokenService)
   {
      _usersService = usersService;
      _jwtTokenService = jwtTokenService;

   }
   [HttpGet]
   public async Task<ActionResult> Index(string input)
   {
      
      return Ok("AddPassword");

   }
}
