using Bank.Data.Impl;
using Bank.Data.Interface;
using Bank.Domain.Impl;
using Bank.Domain.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System.IO;

namespace Bank
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddTransient<IAccountManager, AccountManager>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<ITransactionManager, TransactionManager>();
            services.AddTransient<ITransactionLogRepository, TransactionLogRepository>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(coookie =>
            {
                coookie.LoginPath = new PathString("/Account/Login/");
                coookie.LogoutPath = new PathString("/Account/Login/");
                coookie.AccessDeniedPath = new PathString("/Account/Forbidden/");
                coookie.ExpireTimeSpan = new System.TimeSpan(0, 20, 0);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseExceptionHandler("/Error/Error");
            }
            else
            {
                app.UseExceptionHandler("/Error/Error");
            }
            
            loggerFactory.AddSerilog();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "login",
                    template: "{controller=Account}/{action=Login}/{id?}");

                routes.MapRoute(
                    name: "register",
                    template: "{controller=Account}/{action=Register}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "transactionHistory",
                    template: "{controller=Transaction}/{action=TransactionHistory}/{id?}");

                routes.MapRoute(
                    name: "transfer",
                    template: "{controller=Transaction}/{ action=Transact }/{id?}");

                routes.MapRoute(
                    name: "transaction",
                    template: "{controller=Transaction}/{ action=Index }/{id?}");

                routes.MapRoute(
                    name: "logout",
                    template: "{controller=Account}/{action=Logout}/{id?}");

                routes.MapRoute(
                    name: "error",
                    template: "{controller=Error}/{action=Error}/{id?}");
            });
        }
    }
}
