using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PopApis.ApiControllers;
using PopApis.Data;
using PopApis.Models;
using PopLibrary;
using PopLibrary.Helpers;
using PopLibrary.Stripe;
using PopLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PopApis
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var config = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            Configuration = config;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(logger => logger.AddConsole());
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddAuthentication("BasicAuthentication").
                AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services.AddControllersWithViews();
            services.AddRazorPages();            

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, Role.Admin));
                options.AddPolicy("User", policy => policy.RequireClaim(ClaimTypes.Role, Role.Admin, Role.User));
            });

            services.Configure<Users>(Configuration.GetSection("Users"));
            services.Configure<SqlSettings>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<FinalizeOptions>(Configuration.GetSection("FinalizeOptions"));
            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
            services.AddSingleton(sp => sp.GetService<IOptions<Users>>().Value);
            services.AddSingleton(sp => sp.GetService<IOptions<SqlSettings>>().Value);
            services.AddSingleton(sp => sp.GetService<IOptions<FinalizeOptions>>().Value);
            services.AddSingleton(sp => sp.GetService<IOptions<StripeSettings>>().Value);
            services.AddScoped<SqlAdapter>();
            services.AddScoped<AuctionData>();
            services.AddScoped<EventData>();
            services.AddScoped<AuctionController>();
            services.AddScoped<AccountingController>();
            services.AddScoped<FinalizeHelper>();
            services.AddHttpClient();
            services.AddSingleton<StripeAdapter>();
            services.AddScoped<PopLibrary.IAuthenticationService, PopLibrary.AuthenticationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
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
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
