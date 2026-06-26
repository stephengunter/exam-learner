using ApplicationCore.Authorization;
using ApplicationCore.Consts;
using ApplicationCore.Exceptions;
using ApplicationCore.Exceptions.Identity;
using ApplicationCore.Helpers;
using ApplicationCore.Models;
using ApplicationCore.Models.Auth;
using ApplicationCore.Models.Identity;
using ApplicationCore.Services;
using ApplicationCore.Services.Auth;
using ApplicationCore.Settings;
using ApplicationCore.Web.Controllers;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Web.Models;
using LoginRequest = Web.Models.LoginRequest;

namespace Web.Controllers;

[EnableCors("Global")]
public class AuthController : BaseController
{
   private readonly IUsersService _usersService;
   private readonly IJwtTokenService _jwtTokenService;

   public AuthController(IUsersService usersService, IJwtTokenService jwtTokenService)
   {
      _usersService = usersService;
      _jwtTokenService = jwtTokenService;

   }

   [HttpPost]
   public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
   {
      ValidateRequest(request);
      if (!ModelState.IsValid) return BadRequest(ModelState);

      var user = await _usersService.FindByUsernameAsync(request.Username);
      if (user is null)
      {
         ModelState.AddModelError("", "身分驗證失敗, 請重新登入.");
         return BadRequest(ModelState);
      }

      bool isValid = await _usersService.CheckPasswordAsync(user, request.Password);
      if (!isValid)
      {
         ModelState.AddModelError("", "身分驗證失敗, 請重新登入.");
         return BadRequest(ModelState);
      }

      var roles = await _usersService.GetRolesAsync(user);

      var accessToken = await _jwtTokenService.CreateAccessTokenAsync(RemoteIpAddress, user, roles);
      string refreshToken = await _jwtTokenService.CreateRefreshTokenAsync(RemoteIpAddress, user);

      return new AuthResponse(accessToken.Token, accessToken.ExpiresIn, refreshToken);
   }


   [HttpPut("{id}")]
   public async Task<ActionResult<AuthResponse>> RefreshToken(string id, [FromBody] RefreshTokenRequest request)
   {
      var user = await _usersService.FindByIdAsync(id);
      if (user is null) return NotFound();

      var cp = _jwtTokenService.ResolveClaimsFromToken(request.AccessToken);
      if (cp is null) throw new TokenResolveFailedException();
      if (cp.Claims.IsNullOrEmpty()) throw new TokenResolveFailedException("Claims IsNullOrEmpty!");
      if (cp.Id() != id) throw new RefreshTokenFailedException($"User Id Not Equals To Put Id: {id}");

      await ValidateRequestAsync(request, user);
      if (!ModelState.IsValid) return BadRequest(ModelState);

      var roles = await _usersService.GetRolesAsync(user);
      var accessToken = await _jwtTokenService.CreateAccessTokenAsync(RemoteIpAddress, user, roles, null);
      string refreshToken = await _jwtTokenService.CreateRefreshTokenAsync(RemoteIpAddress, user);

      return new AuthResponse(accessToken.Token, accessToken.ExpiresIn, refreshToken);

   }

   async Task ValidateRequestAsync(RefreshTokenRequest request, User user)
   {
      bool isValid = await _jwtTokenService.IsValidRefreshTokenAsync(request.RefreshToken, user);
      if (!isValid) ModelState.AddModelError("token", "身分驗證失敗. 請重新登入");
   }
   void ValidateRequest(LoginRequest request)
   {
      if (String.IsNullOrEmpty(request.Username)) ModelState.AddModelError("name", ValidationMessages.Required("name"));
      if (String.IsNullOrEmpty(request.Password)) ModelState.AddModelError("password", ValidationMessages.Required("password"));
   }



}

