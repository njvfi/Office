using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Plugins;
using System.Diagnostics;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Офіс.DAL.Entities;
using Офіс.DAL.Repositories;
using Офіс.Models;
using Офіс.ViewModels;

namespace Офіс.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly IConfiguration _configuration;

        private readonly EventsRepository _eventsRepository;

        private readonly UsersRepository _usersRepository;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment, EventsRepository eventsRepository, UsersRepository usersRepository, IConfiguration configuration)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _eventsRepository = eventsRepository;
            _usersRepository = usersRepository;
            _configuration = configuration;
        }
        #region Navigation & Constructors 

        [AllowAnonymous]
        public IActionResult Index()
        {
            List<Events> events = _eventsRepository.GetAllEvents();
            EventListViewModel model = new EventListViewModel
            {
                Events = events
            };
            return View(model);
        }
        public IActionResult Videos()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult About_Us()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Event(int id)
        {
            if (id == 0) 
            return RedirectToAction("Index");
            Events events = _eventsRepository.GetEvent(id);
            return View(events);
        }
        public IActionResult Create()
        {
            return View(new CreateViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
			if (model.Image == null || model.Image.Length <= 0)
			{
				return View(model);
			}
			Events events = new();
            events.Name = model.Name;
            events.Description = model.Description;
            events.Place = model.Place;
            events.DateTime = model.DateTime;
            events.Cost = model.Cost;
            events.RegistrationLink = model.RegistrationLink;
			var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","EventPics");

			// Проверка существования папки
			if (!Directory.Exists(uploadDir))
			{
				Directory.CreateDirectory(uploadDir); // Создание папки, если она не существует
			}

			var filePath = Path.Combine(uploadDir, model.Image.FileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await model.Image.CopyToAsync(stream);
			}

			//model.Image.Save($"EventPics/{model.Name}.jpg");
			events.ImageName = model.Image.FileName;
            _eventsRepository.CreateEvent(events);
            return RedirectToAction("Index");
        }

        /*
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        { 
            return View(); 
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        */
        #endregion
        /*
        #region User
        
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
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return BadRequest("An error occurred in generating the token");
            }
            return Unauthorized();
        }
        
        public IActionResult AssignRole(int UserId, Role role)
        {
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(RegisterModel user)
        {
            if (!ModelState.IsValid)
                return View(user);
            switch(_usersRepository.UserCheck(user))
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

        public IActionResult Logout()
        {
            Response.Cookies.Delete("X-Access-Token");

            return RedirectToAction("Index", "Home");
        }
        #endregion
        */

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
