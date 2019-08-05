using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace EasyWalletWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var culture = new CultureInfo(Thread.CurrentThread.CurrentCulture.Name);
            culture.NumberFormat.CurrencyNegativePattern = 1;
            Thread.CurrentThread.CurrentCulture = culture;
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
                o.ExpireTimeSpan = new TimeSpan(2, 0, 0, 0);
                o.LoginPath = "/login";
            });

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
                .AddViewLocalization(opts => opts.ResourcesPath = "Resources")
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<RequestLocalizationOptions>(opts =>
            {
                var english = new CultureInfo("en");
                var spanish = new CultureInfo("es-US");
                english.NumberFormat.CurrencyNegativePattern = 1;
                spanish.NumberFormat.CurrencyNegativePattern = 1;

                var supportedCultures = new List<CultureInfo>
                {
                    english,
                    spanish
                };

                opts.DefaultRequestCulture = new RequestCulture(english);
                opts.SupportedCultures = supportedCultures;
                opts.SupportedUICultures = supportedCultures;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

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
                    name: "language",
                    template: "lang",
                    defaults: new { controller = "Home", action = "SetLanguage" });

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

                routes.MapRoute(
                    name: "monthly",
                    template: "u/reports/monthly",
                    defaults: new { controller = "Reports", action = "Monthly" });

                routes.MapRoute(
                    name: "balance",
                    template: "u/reports/balance",
                    defaults: new { controller = "Reports", action = "Balance" });
            });
        }
    }
}
