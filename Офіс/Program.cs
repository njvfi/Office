using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Îô³ñ.DAL.Contexts;
using Îô³ñ.DAL.Repositories;

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

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}