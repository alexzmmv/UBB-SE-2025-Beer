﻿using Microsoft.AspNetCore.Mvc;
using DataAccess.Model.Authentication;
using DataAccess.AuthProviders.Facebook;
using DataAccess.AuthProviders.Github;
using DataAccess.AuthProviders.LinkedIn;
using DataAccess.AuthProviders;
using DataAccess.OAuthProviders;
using DataAccess.Service;
using WinUiApp.Data.Data;
using DataAccess.IRepository;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationService authenticationService;

        public AuthenticationController(ISessionRepository sessionRepository, IUserRepository userRepository, ILinkedInLocalOAuthServer linkedinLocalOAuth,
            IGitHubLocalOAuthServer githubLocalOAuth, IFacebookLocalOAuthServer facebookLocalOAuth, IBasicAuthenticationProvider basicAuthProvider)
        {
            authenticationService = new AuthenticationService(sessionRepository, userRepository, linkedinLocalOAuth,
                githubLocalOAuth, facebookLocalOAuth, basicAuthProvider);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponse>> LoginWithCredentials([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Username and password must be provided.");
            }

            AuthenticationResponse response = await this.authenticationService.AuthWithUserPass(request.Username, request.Password);
            if (!response.AuthenticationSuccessful)
            {
                return Unauthorized();
            }

            return Ok(response);
        }

        [HttpPost("oauth")]
        public async Task<ActionResult<AuthenticationResponse>> LoginWithOAuth([FromQuery] OAuthService service)
        {
            object? helper = service switch
            {
                OAuthService.GitHub => HttpContext.RequestServices.GetService(typeof(IGitHubOAuthHelper)),
                OAuthService.Facebook => HttpContext.RequestServices.GetService(typeof(IFacebookOAuthHelper)),
                OAuthService.LinkedIn => HttpContext.RequestServices.GetService(typeof(ILinkedInOAuthHelper)),
                _ => null
            };

            if (helper == null)
            {
                return BadRequest("OAuth rolesService not supported or helper not found.");
            }

            AuthenticationResponse response = await authenticationService.AuthWithOAuth(service, helper);
            if (!response.AuthenticationSuccessful)
            {
                return Unauthorized();
            }

            return Ok(response);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            this.authenticationService.Logout();
            return Ok("Logged out successfully.");
        }

        [HttpGet("user")]
        public async Task<ActionResult<User>> GetCurrentUser([FromQuery] Guid sessionId)
        {
            User? user = await authenticationService.GetUser(sessionId);

            if (user == null)
            {
                return NotFound("User not found for the given session.");
            }
            return Ok(user);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
