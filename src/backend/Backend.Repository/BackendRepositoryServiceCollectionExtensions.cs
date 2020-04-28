using Backend.Repository.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Repository
{
    public static class BackendRepositoryServiceCollectionExtensions
    {
        public static void AddBackendRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            //"RepoOptions:PgsqlPassword"
            services.Configure<RepoOptions>(option => {
                option.PgsqlPassword = configuration.GetSection("RepoOptions:PgsqlPassword").Value;
            });

            services.AddSingleton<SecurityRepository>();
            services.AddSingleton<CommonCitizenRepository>();
        }
    }
}
