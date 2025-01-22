using Офіс.DAL.Contexts;
using Офіс.DAL.Entities;
using Офіс.Models;

namespace Офіс.DAL.Repositories
{
    public class UsersRepository
    {
        private readonly UsersContext _context;

        public UsersRepository(UsersContext context)
        {
            _context = context;
        }

        public bool EmailCheck(string Email)
        {
            var result = _context.Users.FirstOrDefault(x => x.Email == Email);
            return result == null;
        }

        public string UserCheck(RegisterModel model)
        {
            string error;
            var username = _context.Users.FirstOrDefault(u => u.Username == model.Username);
            var email = _context.Users.FirstOrDefault(u =>u.Email == model.Email);
            var password = model.Password != model.Repeat;
            var empty = string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Username) || 
                        string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.Repeat);
            if (username != null) error = "username";
            else if (email != null) error = "email";
            else if (password) error = "password";
            else if (empty) error = "empty";
            else error = "false";
            return error;
        }
        
        public bool Login(LoginModel user)
        {
            var result = _context.Users.FirstOrDefault(u => (u.Username == user.Username || u.Email == user.Username) && u.Password == user.Password);
            user.Id = result.Id;
            return result != null;
        }
        public bool Register(RegisterModel model)
        {
            var result = _context.Users.FirstOrDefault(u => u.Email == model.Email);
            if (result == null)
            {
                Users user = new Users 
                { 
                    Email = model.Email,
                    Password = model.Password,
                    Username = model.Username,
                    Role = Role.User
                } ;
                _context.Users.Add(user);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
