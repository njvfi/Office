using System.Collections;
using Офіс.Controllers;

namespace Офіс.Models
{
	public class EventListViewModel
	{
		public IEnumerable<Events> Events { get; set; } = new List<Events>();
	}
}
