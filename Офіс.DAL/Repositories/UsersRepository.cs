using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Офіс.DAL.Contexts;
using Офіс.DAL.Entities;
using Офіс.Models;

namespace Офіс.DAL.Repositories
{
    public class UsersRepository
    {
        private readonly UsersContext _context;
        private readonly UserManager<IdentityUser> _userManager;


        public UsersRepository(UsersContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            Initialize(context);
        }

        public static async Task Initialize(UsersContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleValidators = new List<IRoleValidator<IdentityRole>>();
            var normalizer = new UpperInvariantLookupNormalizer();
            var errorDescriber = new IdentityErrorDescriber();
            var logger = NullLogger<RoleManager<IdentityRole>>.Instance;

            var roleManager = new RoleManager<IdentityRole>(
                roleStore,
                roleValidators,
                normalizer,
                errorDescriber,
                logger
            );

            // Create roles if they don't exist
            if (!await roleManager.RoleExistsAsync("Member"))
            {
                var role = new IdentityRole("Member");
                await roleManager.CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                var role = new IdentityRole("User");
                await roleManager.CreateAsync(role);
            }
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
            var allUsers = _context.Users.ToList();
            var result = _context.Users.FirstOrDefault(u => (u.Username == user.Username /*|| u.Email == user.Username*/) && u.Password == user.Password);
            var debug = 1;
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

        public Users GetUser(int Id)
        {
            return _context.Users.FirstOrDefault(u =>u.Id == Id);
        }

        public Users GetUser(string username)
        { 
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                user = _context.Users.FirstOrDefault(u => u.Email == username);
            }
            return user; 
        }

        public async Task AssignRole(int id, string role)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) throw new Exception("User not found");

            var roleExists = await _userManager.GetRolesAsync(user);

            if (!roleExists.Contains(role))
            {
                var result = await _userManager.AddToRoleAsync(user, role);
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to assign role " + string.Join(",", result.Errors));
                }
            }
        }
    }
}
