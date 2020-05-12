
using Backend.DataTransferObjects;
using Backend.Model;
using Backend.Repository;
using Backend.Security;
using Backend.Services.Configurations;
using Backend.Services.Core;
using Backend.Services.Features.ContactTracking;
using Backend.Services.Features.Security;
using Backend.Services.GraphQL.Types;
using Backend.Services.Security;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Voyager;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Backend.Services.DependencyInjection
{
    public static class BackendServiceCollectionExtensions
    {
        public static SecurityBuildersAccessor AddBackendServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddBackendRepositories(configuration); // <--> identityBUilder.AddEntityFrameworkStores
            services.AddGrapQlTypes(configuration);

            var identityBuilder = services
                .AddIdentity<User, IdentityRole>(options =>
                {
                    //not best practice but suitable for RAD
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 4;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredUniqueChars = 0;

                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(1);
                    options.Lockout.MaxFailedAccessAttempts = 20;
                    options.Lockout.AllowedForNewUsers = true;

                    options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;

                    options.User.RequireUniqueEmail = true;
                })
                //.AddFeedContextUserStore(services)
                .AddUserStore<UserStore>()
                .AddRoleStore<RoleStore>()
                //.AddUserManager<AspNetUserManager<User>>()
                .AddSignInManager<SignInManager<User>>();
                //.AddEntityFrameworkStores<FeedDbContext>();*/

            services.AddScoped<SignInHandler>();
            //services.Add(new ServiceDescriptor(typeof(UserManager<User>), typeof(AspNetUserManager<User>), ServiceLifetime.Scoped));


            services.AddScoped<IUserAccessor, DefaultUserAccessor>();
            services.AddScoped<IAuthenticationService, Security.AuthenticationService>();

            var authenticationBuilder = services.AddAuthentication(authenticationOptions =>
            {
                //CookieAuthenticationDefaults.AuthenticationScheme; throws InvalidOperationException: No authentication handler is registered for the scheme 'Identity.Application'. The registered schemes are: Cookies. Did you forget to call AddAuthentication().Add[SomeAuthHandler]("Identity.Application",...)?
                // IdentityConstants.ApplicationScheme;

                authenticationOptions.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                authenticationOptions.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
                authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authenticationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                authenticationOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                authenticationOptions.DefaultForbidScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(bearerOptions =>
            {

                bearerOptions.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = Constant.TokenSecurity.jwtAuthenticationIssuerSigningKey,
                    ValidIssuer = Constant.TokenSecurity.JwtAuthenticationAuthority,
                    ValidAudience = Constant.TokenSecurity.JwtAuthenticationAudience,
                    ClockSkew = TimeSpan.FromSeconds(19)
                };
            });

            //services.AddScoped<UserRegistrationManager>();
            services.AddScoped<SecurityService>();
            services.AddScoped<ContactTrackingService>();

            services.AddControllers(options =>
                options.Filters.Add(new HttpResponseExceptionFilter()));
            //identityBuilder
            var securityBuilderAccessor = new SecurityBuildersAccessor(null, authenticationBuilder);
            return securityBuilderAccessor;

        }

        public static IApplicationBuilder ConfigureBackendServices(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            var serviceProvider = app.ApplicationServices;
            using (var scope = serviceProvider.CreateScope())
            {
               // var dbContext = scope.ServiceProvider.GetService<xxxDbContext>();
                //dbContext.Seed(scope.ServiceProvider);
            }
            return app;
        }
        public static IApplicationBuilder ConfigurRemainingeBackendServices(this IApplicationBuilder app, IWebHostEnvironment env)
        {

            app
                .UseWebSockets()
                .UseGraphQL("/graphql")
                .UsePlayground("/graphql")
                .UseVoyager("/graphql");
            return app;
        }

        public static IServiceCollection AddGrapQlTypes(this IServiceCollection services, IConfiguration configuration)
        {
            // Add GraphQL Services
            services.AddGraphQL(sp => SchemaBuilder.New()
                .AddServices(sp)
                .AddQueryType<QueryType>()
                .AddMutationType<MutationType>()
                //.AddSubscriptionType(d => d.Name("Subscription"))
                //.AddType<CharacterQueries>()
                //.AddType<ReviewQueries>()
                .AddType<PhysicalContactType>()
                //.AddType<ReviewSubscriptions>()
                .AddType<SynchronizationToken>()
                .Create());

            return services;
        }


        public static IServiceCollection AddAuthenticationHandlers(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization(options =>
            {
                options.InvokeHandlersAfterFailure = false;
            });

            //services.AddScoped<IAuthorizationHandler, BackendServiceAuthorizationHandler>();
            return services;
        }
    }
}
