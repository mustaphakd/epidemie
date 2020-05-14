using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Backend.Services.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
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
            services.AddBackendServices(Configuration);

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo{ Title = "Mokolo Feed Api", Version = "V1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                logger.LogInformation("In Development environment");
                //app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
                app.UseExceptionHandler("/Error");
            }
            else
            {
                logger.LogInformation("In Production environment");
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            //skipping Csp

            //app.UseHttpsRedirection();
            // cors
            app.UseCors();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Makolo apis V1");
            });

            app.ConfigureBackendServices(env);

            app.UseDefaultFiles(); // easy access to index.html
            app.UseStaticFiles();
            app.UseSpaStaticFiles(
                new StaticFileOptions(
                    new Microsoft.AspNetCore.StaticFiles.Infrastructure.SharedOptions
                    {
                        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/dist"))
                    }));

            //https://github.com/aspnet/Security/issues/1310
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //https://stackoverflow.com/questions/59387914/allow-anonymouos-access-to-healthcheck-endpoint-when-authentication-fallback-pol
                //https://github.com/ChilliCream/hotchocolate/issues/1189
                //endpoints.MapHealthChecks("/graphql").WithMetadata(new AllowAnonymousAttribute());
            });


            app.ConfigurRemainingeBackendServices(env);

            logger.LogInformation("Configuration complete");
        }
    }
}
