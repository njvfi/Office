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
            List<Events> events = GetAllEvents();
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
        public Events GetEvent(int id)
        {
            Events events = _eventsContext.Events.FirstOrDefault(e => e.Id == id);
            return events;
        }

        public List<Events> GetAllEvents()
        {
            return _eventsContext.Events.ToList();
        }

        public void CreateEvent(Events events)
        {
            _eventsContext.Add(events);
            _eventsContext.SaveChanges();
        }
    }
}
