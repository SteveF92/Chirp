using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using Chirp.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.Logging;
using Microsoft.AspNet.Diagnostics;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using Chirp.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authentication.Cookies;
using System.Net;
using Chirp.Models;

namespace Chirp
{
    public class Startup
    {
        public static IConfigurationRoot Configuration;

        public Startup(IApplicationEnvironment appEnv)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(config =>
            {
//#if !DEBUG
//                config.Filters.Add(new RequireHttpsAttribute());
//#endif
            })
            .AddJsonOptions(opt =>
            {
                opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddSignalR();

            services.AddIdentity<ChirpUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-._";

                config.Password.RequiredLength = 8;
                config.Password.RequireLowercase = false;
                config.Password.RequireNonLetterOrDigit = false;
                config.Password.RequireDigit = false;
                config.Password.RequireUppercase = false;

                config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
                config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == (int) HttpStatusCode.OK)
                        {
                            ctx.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                        }
                        else
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }   
                        return Task.FromResult(0);
                    }
                };
            })
            .AddEntityFrameworkStores<ChirpContext>();

            services.AddLogging();

            services.AddEntityFramework()
                .AddNpgsql()
                .AddDbContext<ChirpContext>();

            services.AddTransient<ChirpContextSeedData>();
            services.AddScoped<IChirpRepository, ChirpRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, ChirpContextSeedData seeder, ILoggerFactory loggerFactory, IHostingEnvironment env)
        {
            if (string.Equals(env.EnvironmentName, "Development", StringComparison.OrdinalIgnoreCase))
            {
                app.UseDeveloperExceptionPage();

                app.UseRuntimeInfoPage(); // default path is /runtimeinfo
            }

            loggerFactory.AddDebug(LogLevel.Information);

            app.UseStaticFiles();

            app.UseIdentity();

            Mapper.Initialize(config =>
            {
                config.CreateMap<ChirpPost, ChirpPostViewModel>().ReverseMap();
                config.CreateMap<ChirpUser, ChirpUserViewModel>().ReverseMap();
            });

            app.UseSignalR();

            app.UseMvc(Configure =>
            {
                Configure.MapRoute(
                    name: "Api",
                    template: "api/{action}/{id?}"
                );
                Configure.MapRoute(
                    name: "Default",
                    template: "{action}/{id?}",
                    defaults: new { controller = "App", action = "Index" }
                );
            });

            await seeder.EnsureSeedDataAsync();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
