using FinalProject.Data;
using FinalProject.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(convertUrlConnectionString(Configuration["DATABASE_URL"])));
            services.AddDatabaseDeveloperPageExceptionFilter();

            //made login easier code from class video
            services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 3;

            }).AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllers();
            services.AddRazorPages();

            services.AddTransient<IUserService, DefaultUserService>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthPolicies.IsAdmin, (policyBuilder) =>
                    policyBuilder.RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", Configuration["adminEmail"]));
            });
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
                app.UseExceptionHandler("/Error");
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
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });

        }
        private static string convertUrlConnectionString(string url)
        {
            if (url is null)
                throw new ArgumentNullException("It appears you're missing the DATABASE_URL configuration value...");
            if (!url.Contains("//"))
                return url;
            var uri = new Uri(url);
            var host = uri.Host;
            var port = uri.Port;
            var database = uri.Segments.Last();
            var parts = uri.AbsoluteUri.Split(':', '/', '@');
            var user = parts[3];
            var password = parts[4];

            return $"host={host}; port={port}; database={database}; username={user}; password={password}; SSL Mode=Prefer; Trust Server Certificate=true";
        }


    }
}
