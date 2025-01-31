using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.Text;
using Îô³ñ.DAL.Contexts;
using Îô³ñ.DAL.Entities;
using Îô³ñ.DAL.Repositories;
using Îô³ñ.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        string connection = "Server = (localdb)\\mssqllocaldb;Database = Officedb;Trusted_Connection=true";

        builder.Services.AddScoped<EventsRepository>();

        builder.Services.AddDbContext<EventsContext>(options => options.UseSqlServer(connection));

        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<UsersContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddScoped<UsersRepository>();

        builder.Services.AddDbContext<UsersContext>(options => options.UseSqlServer(connection));

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
       .AddJwtBearer(options =>
       {
           options.SaveToken = true;
           options.RequireHttpsMetadata = false;
           options.TokenValidationParameters = new TokenValidationParameters()
           {
               ValidateIssuer = true,
               ValidateAudience = true,
               ValidateIssuerSigningKey = true,
               ValidateLifetime = true,
               ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
               ValidAudience = builder.Configuration["Jwt:ValidAudience"],
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])),
               ClockSkew = TimeSpan.Zero
           };
           options.Events = new JwtBearerEvents
           {
               OnMessageReceived = context =>
               {

                   if (context.Request.Cookies.ContainsKey("X-Access-Token"))
                   {
                       context.Token = context.Request.Cookies["X-Access-Token"];
                   }

                   return Task.CompletedTask;
               }
           };
       });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();
        app.UseAuthentication();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}