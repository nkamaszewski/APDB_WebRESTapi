using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using APDB_WebRESTapi.DAL;
using APDB_WebRESTapi.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace APDB_WebRESTapi.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ILoginDBService _loginDBService;
    
        public LoginController(ILoginDBService loginDBService)
        {
            _loginDBService = loginDBService;
      
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(LoginRequest loginRequest)
        {
            var token = _loginDBService.GetToken(loginRequest);
        
            if(token == null)
            {
                return Unauthorized("Wrong login or password");
            }

            return Ok(token);
        }

        [HttpPost("refresh-token/{refToken}")]
        public IActionResult RefreshToken(string refToken)
        {

            var token = _loginDBService.RefreshToken(refToken);

            if (token == null)
            {
                return BadRequest("Wrong refresh token");
            }

            return Ok(token);
        }
    }
}