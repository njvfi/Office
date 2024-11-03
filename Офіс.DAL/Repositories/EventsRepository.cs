using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Офіс.DAL.Contexts;
using Офіс.DAL.Entities;

namespace Офіс.DAL.Repositories
{
    public class EventsRepository
    {
        private readonly EventsContext _eventsContext;

        public EventsRepository(EventsContext eventsContext)
        {
            _eventsContext = eventsContext;
            Init();
        }

        public void Init()
        {
            List<Events> events = GetEvents();
            if (events.Count == 0)
            {
                Events event1 = new Events
                {
                    Name = "Коментатори",
                    Description = "Коміки смішно коментують відео",
                    Place = "UNICA",
                    Cost = "Безкоштовно",
                    RegistrationLink = "Типу посилання",
                    ImageName = "Commentators.jpg"
                };
                _eventsContext.Events.Add(event1);
                _eventsContext.SaveChanges();
            }
        }

        public List<Events> GetEvents()
        {
            return _eventsContext.Events.ToList();
        }
    }
}
