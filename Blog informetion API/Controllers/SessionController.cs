using Blog_informetion_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blog_informetion_API.Controllers
{
    [ApiController]
    public class SessionController:ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SessionController> _logger;
        public SessionController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IConfiguration configuration, ILogger<SessionController> logger)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._configuration = configuration;
            this._logger = logger;
        }
        [HttpPost("/information/publicist")]
        public async Task<IActionResult> RegisterPublicist([FromBody] RegisterModel model)
        {
            try
            {
                var verify = await _userManager.FindByEmailAsync(model.Email!);
                if (verify != null)
                {
                    return BadRequest(new { message = $"El correo: {model.Email} ya esta registrado" });
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);


                if (result.Succeeded)
                {
                    await this._userManager.AddToRoleAsync(user, "User");
                    return Ok(new { message = "Usuario registrado correctamente" });
                }

                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error al registrar al usuario");
                return StatusCode(500, "Error interno del servidor.");

            }

        }

        [HttpPost("/information/publicists")]
        public async Task<IActionResult> SignPublicist([FromBody] LoginModel model)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    var token = GenerateJwtToken(user!);
                    return Ok(new { token });
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error al iniciar sesion.");
                return StatusCode(500, "Error interno del servidor.");
            }

        }
        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            try
            {
                var roles = await this._userManager.GetRolesAsync(user);

                var claims = new List<Claim>
            {
                 new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error al general token");
                throw;
            }

        }
    }
}
