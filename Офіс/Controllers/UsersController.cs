using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Офіс.DAL.Entities;
using Офіс.DAL.Repositories;
using Офіс.Models;
using Офіс.Services;

namespace Офіс.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UsersRepository _usersRepository;
        private readonly IConfiguration _configuration;


        public UsersController(UsersRepository usersRepository, IConfiguration configuration)
        {
            _usersRepository = usersRepository;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Login

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginModel login)
        {
            try
            {
                if (string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
                    return BadRequest("Username and/or Password not specified");
                if (_usersRepository.Login(login))
                {
                    var secretKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
                    var signinCredentials = new SigningCredentials
                   (secretKey, SecurityAlgorithms.HmacSha256);
                    var jwtSecurityToken = new JwtSecurityToken(
                        issuer: _configuration["Jwt:ValidIssuer"],
                        audience: _configuration["Jwt:ValidAudience"],
                        claims: new List<Claim>(),
                        expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryInMinutes"])),
                        signingCredentials: signinCredentials
                    );
                    var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                    Response.Cookies.Append("X-Access-Token", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
                    Response.Cookies.Append("UserId", login.Id.ToString());
                    var user = _usersRepository.GetUser(login.Username);
                    AssignRole(user);
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                return BadRequest("An error occurred in generating the token");
            }
            return Unauthorized();
        }
        #endregion

        #region Register
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(RegisterModel user)
        {
            if (!ModelState.IsValid)
                return View(user);
            switch (_usersRepository.UserCheck(user))
            {
                case "email": return BadRequest("Користувач з таким Email вже існує");
                case "username": return BadRequest("Цей Юзернейм вже зайнятий");
                case "password": return BadRequest("Паролі не співпадають");
                case "empty": return View(user);
                case "false": _usersRepository.Register(user); break;
            }
            LoginModel model = new LoginModel
            {
                Username = user.Username,
                Password = user.Password,
            };
            _usersRepository.Login(model);
            return RedirectToAction("Index");
        }
        #endregion

        #region Roles
        public async Task AssignRole(Users user)
        {
            string role = user.Role switch
            {
                Role.Member => "Member",
                Role.User => "User"
            };
            await _usersRepository.AssignRole(user.Id, role);
        }
        public async Task UpgradeRole(int userId)
        {
            await _usersRepository.AssignRole(userId,"Member");
        }

        public async Task<IActionResult> ChangeRole(int userId, Role _role)
        {
            string role = _role switch
            {
                Role.Member => "Member",
                Role.User => "User"
            };
            await _usersRepository.AssignRole(userId, role);
            return RedirectToAction("Index", "Home");
        }
        public async Task DowngradeRole(int userId)
        {
            await _usersRepository.AssignRole(userId, "User");
        }
        #endregion

        public IActionResult Logout()
        {
            Response.Cookies.Delete("X-Access-Token");

            return RedirectToAction("Index", "Home");
        }
    }
}
