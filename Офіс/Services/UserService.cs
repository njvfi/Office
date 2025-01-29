using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Офіс.DAL.Contexts;
using Офіс.DAL.Entities;
using Офіс.DAL.Repositories;

namespace Офіс.Services
{
    public class UserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly UsersRepository _usersRepository;

        public UserService(UserManager<IdentityUser> userManager, UsersRepository usersRepository, UsersContext usersContext)
        {
            _userManager = userManager;
            _usersRepository = usersRepository;
            Initialize(usersContext);
        }

        public static async void Initialize(UsersContext context)
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
            if (! await roleManager.RoleExistsAsync("Member"))
            {
                var role = new IdentityRole("Member");
                await roleManager.CreateAsync(role);
            }

            if (! await roleManager.RoleExistsAsync("User"))
            {
                var role = new IdentityRole("User");
                await roleManager.CreateAsync(role);
            }
        }

        public async Task AssignRole(int id, string role)
        {
            //var user = _usersRepository.GetUser(id);
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) throw new Exception("User not found");

            var roleExists = await _userManager.GetRolesAsync(user);

            if(!roleExists.Contains(role))
            { 
                var result = await _userManager.AddToRoleAsync(user, role);
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to assign role " + string.Join(",",result.Errors));
                }
            }
        }
    }
}
