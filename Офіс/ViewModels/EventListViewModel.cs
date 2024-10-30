using System.Collections;
using Офіс.Controllers;
using Офіс.DAL.Entities;

namespace Офіс.ViewModels
{
	public class EventListViewModel
	{
		public IEnumerable<Events> Events { get; set; } = new List<Events>();
	}
}
