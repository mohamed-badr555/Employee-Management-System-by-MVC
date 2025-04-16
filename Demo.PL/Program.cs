using System;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Demo.PL.Extensitions;
using Demo.PL.Mapping_Profiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //CreateHostBuilder(args).Build().Run();
            var webApplicationBuilder = WebApplication.CreateBuilder(args);

            #region Configure Services
            webApplicationBuilder.Services.AddControllersWithViews();
            webApplicationBuilder.Services.AddDbContext<AppDbContext>(options =>
                 options.UseLazyLoadingProxies().UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection")));

            webApplicationBuilder.Services.AddApplicationServices();
            webApplicationBuilder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
            webApplicationBuilder.Services.AddAutoMapper(M => M.AddProfile(new UserProfile()));
            webApplicationBuilder.Services.AddAutoMapper(M => M.AddProfile(new RoleProfile()));


            webApplicationBuilder.Services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.Password.RequiredUniqueChars = 3;
                config.Password.RequireDigit = true;
                config.Password.RequireNonAlphanumeric = true;
                config.Password.RequireUppercase = true;
                config.Password.RequireUppercase = true;
                config.Password.RequiredLength = 8;
                //config.SignIn.RequireConfirmedEmail = true;
                config.User.RequireUniqueEmail = true;
                config.Lockout.MaxFailedAccessAttempts = 3;
                config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                config.Lockout.AllowedForNewUsers = true;

            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            webApplicationBuilder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SignIn";
                config.ExpireTimeSpan = TimeSpan.FromHours(4);
            });
            #endregion

            var app = webApplicationBuilder.Build();
            #region Configure Kestrel Middlewares
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            #endregion
            app.Run();
        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });
    }
}
