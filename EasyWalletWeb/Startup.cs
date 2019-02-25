using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace EasyWalletWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContextPool<DatabaseContext>(o => o.UseMySql(Configuration.GetConnectionString("WalletDB")));
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o => {
                //o.Cookie.Domain = "wallet.luischicas.com";
                o.ExpireTimeSpan = new TimeSpan(2, 0, 0, 0);
                o.LoginPath = "/login";
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "home",
                    template: "",
                    defaults: new { controller = "Home", action = "Index" });

                routes.MapRoute(
                    name: "login",
                    template: "login",
                    defaults: new { controller = "Auth", action = "Login" });

                routes.MapRoute(
                    name: "signup",
                    template: "signup",
                    defaults: new { controller = "Auth", action = "Signup" });

                routes.MapRoute(
                    name: "wallet",
                    template: "u",
                    defaults: new { controller = "Wallet", action = "Index" });

                routes.MapRoute(
                    name: "new-entry",
                    template: "u/entries/new",
                    defaults: new { controller = "Wallet", action = "Entry" });

                routes.MapRoute(
                    name: "logout",
                    template: "logout",
                    defaults: new { controller = "Auth", action = "Logout" });

                routes.MapRoute(
                    name: "categories",
                    template: "u/categories",
                    defaults: new { controller = "Categories", action = "Index" });

                routes.MapRoute(
                    name: "new-category",
                    template: "u/categories/new",
                    defaults: new { controller = "Categories", action = "New" });

                routes.MapRoute(
                    name: "edit-category",
                    template: "u/categories/edit/{id}",
                    defaults: new { controller = "Categories", action = "Edit" });

                routes.MapRoute(
                    name: "delete-category",
                    template: "u/categories/delete/{id}",
                    defaults: new { controller = "Categories", action = "Delete" });

                routes.MapRoute(
                    name: "history",
                    template: "u/reports/history",
                    defaults: new { controller = "Reports", action = "History" });

                routes.MapRoute(
                    name: "history-delete",
                    template: "u/reports/history/delete/{id}",
                    defaults: new { controller = "Reports", action = "HistoryDelete" });
            });
        }
    }
}
