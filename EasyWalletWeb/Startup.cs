using EasyWallet.Business.Abstractions;
using EasyWallet.Business.Services;
using EasyWallet.Data;
using EasyWallet.Data.Abstractions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

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

            services.AddDbContextPool<DatabaseContext>(o => o.UseMySql(GetConnectionString()));
            services.AddDbContextPool<EasyWalletContext>(o => o.UseMySql(GetConnectionString()));

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

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IEntryService, EntryService>();

            services.Configure<RequestLocalizationOptions>(opts =>
            {
                var english = new CultureInfo("en-US");
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
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

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
                routes.MapRoute("home", "", new { controller = "Home", action = "Index" });
                routes.MapRoute("language", "lang", new { controller = "Home", action = "SetLanguage" });
                routes.MapRoute("login", "login", new { controller = "Auth", action = "Login" });
                routes.MapRoute("signup", "signup", new { controller = "Auth", action = "Signup" });
                routes.MapRoute("logout", "logout", new { controller = "Auth", action = "Logout" });

                routes.MapRoute("wallet", "u", new { controller = "Wallet", action = "Index" });
                routes.MapRoute("new-entry", "u/entries/new", new { controller = "Wallet", action = "Entry" });

                routes.MapRoute("categories", "u/categories", new { controller = "Categories", action = "Index" });
                routes.MapRoute("new-category", "u/categories/new", new { controller = "Categories", action = "New" });
                routes.MapRoute("edit-category", "u/categories/edit/{id}", new { controller = "Categories", action = "Edit" });
                routes.MapRoute("delete-category", "u/categories/delete/{id}", new { controller = "Categories", action = "Delete" });

                routes.MapRoute("history", "u/reports/history", new { controller = "Reports", action = "History" });
                routes.MapRoute("history-delete", "u/reports/history/delete/{id}", new { controller = "Reports", action = "HistoryDelete" });
                routes.MapRoute("monthly", "u/reports/monthly", new { controller = "Reports", action = "Monthly" });
                routes.MapRoute("balance", "u/reports/balance", new { controller = "Reports", action = "Balance" });
            });
        }

        private string GetConnectionString()
        {
            string host = Configuration["RDS_HOSTNAME"];
            string dbName = Configuration["RDS_DB_NAME"];
            string username = Configuration["RDS_USERNAME"];
            string password = Configuration["RDS_PASSWORD"];
            return $"Data Source={host};Initial Catalog={dbName};User ID={username};Password={password};";
        }
    }
}
