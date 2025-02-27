using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Diagnostics;
using System.Drawing;
using Офіс.DAL.Entities;
using Офіс.DAL.Repositories;
using Офіс.ViewModels;

namespace Офіс.Controllers
{
    public class HomeController : Controller
    {
        //TODO About us +
        //TODO Photos +
        //TODO Where to watch? +
        //TODO Contacts +

        private readonly ILogger<HomeController> _logger;

        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly EventsRepository _eventsRepository;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment, EventsRepository eventsRepository)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _eventsRepository = eventsRepository;
        }
        #region Navigation & Constructors 
        public IActionResult Index()
        {
            return View();
        }

       /* public IActionResult Events()
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
        }*/

        public IActionResult About_Us()
        {
            return View();
        }

        /*public IActionResult Event(int id)
        {
            if (id == 0) 
            return RedirectToAction("Index");
            Events events = _eventsRepository.GetEvent(id);
            return View(events);
        }*/

        public IActionResult Create()
        {
            return View(new CreateViewModel());
        }

        public IActionResult Photos()
        {
            return View();
        }
        public IActionResult Where_to_watch()
        {
            return View();
        }
        public IActionResult Contacts()
        {
            return View();
        }
        #endregion
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
            var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "EventPics");

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
