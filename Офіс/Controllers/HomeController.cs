using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Офіс.DAL.Contexts;
using Офіс.DAL.Entities;
using Офіс.DAL.Repositories;
using Офіс.ViewModels;

namespace Офіс.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly EventsRepository _eventsRepository;
        private readonly EventsContext _eventsContext;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment, EventsRepository eventsRepository)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _eventsRepository = eventsRepository;
        }
        #region Navigation & Constructors 
        public IActionResult Index()
        {
            List<Events> events = _eventsRepository.GetEvents();
            /*Events event1 = new Events{Name = "Коментатори", Description = "Коміки смішно коментують відео", 
                                       Place = "UNICA", Cost = "Безкоштовно", RegistrationLink = "Типу посилання", 
                                       ImageName = "Commentators.jpg"};
            events.Add(event1);*/
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

        public IActionResult About_Us()
        {
            return View();
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
