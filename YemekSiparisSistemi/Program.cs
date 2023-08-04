using YemekSiparisSistemi.Models;
using Microsoft.EntityFrameworkCore;

namespace YemekSiparisSistemi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //Add DBContext services to the container
            builder.Services.AddDbContext<FoodOrderSystemDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));

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
                name:"Default",
                pattern: "{controller=Home}/{action=Index}/{id:int?}");

            app.Run();
        }
    }
}