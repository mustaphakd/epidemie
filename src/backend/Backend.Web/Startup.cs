using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Backend.Web
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
/*
            this.loggerFactory = new LoggerFactory();
            services.AddSingleton<ILoggerFactory>((srvProvider) => this.loggerFactory);
            services.AddLogging();*/
            /*
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowAnyOrigin();
                    builder.AllowCredentials();
                });
            });*/
            services.AddControllers();
            //services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                logger.LogInformation("In Development environment");
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                logger.LogInformation("In Production environment");
                //app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            /**
             * services.AddSingleton<IMyService>((container) =>
    {
        var logger = container.GetRequiredService<ILogger<MyService>>();
        return new MyService() { Logger = logger };
    });
            */
            //skipping Csp

            // cors
            app.UseCors();

            // app.ConfigureFeedServices(env);

            //app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSpaStaticFiles(
                new StaticFileOptions(
                    new Microsoft.AspNetCore.StaticFiles.Infrastructure.SharedOptions
                    {
                        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/dist"))
                    }));
           // app.UseCookiePolicy();
            //https://github.com/aspnet/Security/issues/1310
           // app.UseAuthentication();
            //app.UseMvcWithDefaultRoute();


            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                /*
                  endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                 */
            });
        }
    }
}
