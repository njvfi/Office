using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace Офіс.DAL.Entities
{
    public class Events
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Place { get; set; }
        public DateTime? DateTime { get; set; }
        public string Cost { get; set; }
        public string RegistrationLink { get; set; }
        public string ImageName { get; set; }
    }
}
