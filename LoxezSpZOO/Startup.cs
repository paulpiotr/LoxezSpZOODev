using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
//using System.Web.Mvc;
//using LoxezSpZOO.Data;

namespace LoxezSpZOO
{
    public class Startup
    {
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ///ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            string LoxezSpZOOContext = Configuration.GetConnectionString("LoxezSpZOOContext");
            string rsaFileContent = EnryptDecrypt.EnryptDecrypt.GetRsaFileContent();
            LoxezSpZOOContext = EnryptDecrypt.EnryptDecrypt.DecryptString(LoxezSpZOOContext, rsaFileContent);
            if (!string.IsNullOrWhiteSpace(LoxezSpZOOContext))
            {
                services.AddDbContext<LoxezSpZOOContext.Data.LoxezSpZOODataContext>(options => options.UseSqlServer(LoxezSpZOOContext));
            }
            string GtContext = Configuration.GetConnectionString("GtContext");
            GtContext = EnryptDecrypt.EnryptDecrypt.DecryptString(GtContext, rsaFileContent);
            if (!string.IsNullOrWhiteSpace(LoxezSpZOOContext))
            {
                services.AddDbContext<GtContext.Data.GtDataContext>(options => options.UseSqlServer(GtContext));
            }
            services.AddSession(options =>
            {
                options.Cookie.Name = "LoxezSpZOO";
                options.IdleTimeout = TimeSpan.FromDays(365);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    List<CultureInfo> supportedCultures = new List<CultureInfo>
                        {
                            new CultureInfo("pl-PL"),
                        };
                    options.DefaultRequestCulture = new RequestCulture(culture: "pl-PL", uiCulture: "pl-PL");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                });
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("pl-PL");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("pl-PL");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLog4Net();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                    );
                endpoints.MapControllers();
            });
        }
    }
}
