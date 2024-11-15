using System.Drawing;

namespace Офіс.ViewModels
{
	public class CreateViewModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public string? Place {  get; set; }
		public DateTime? DateTime { get; set; }
		public string?  Cost { get; set; }
		public string? RegistrationLink { get; set; }
		public IFormFile Image { get; set; }
	}
}
