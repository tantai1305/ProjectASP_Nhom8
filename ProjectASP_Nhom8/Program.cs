using Microsoft.EntityFrameworkCore;
using ProjectASP_Nhom8.Models;

namespace ProjectASP_Nhom8
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(option =>
            {
                option.IdleTimeout = TimeSpan.FromMinutes(30);
                option.Cookie.HttpOnly = true;
                option.Cookie.IsEssential = true;
            });
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddDbContext<QLThamQuanDNContext>(option => option.UseSqlServer(
            builder.Configuration.GetConnectionString("conn")));

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
            app.UseSession();
            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}