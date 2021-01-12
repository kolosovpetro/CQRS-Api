using System;
using CqrsApi.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CqrsApi.Core.Extensions
{
    public static class DataLayerWithPostgreSql
    {
        public static IServiceCollection AddDataLayerWithPostgreSql(this IServiceCollection services,
            IConfiguration configuration)
        {
            var environmentConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            services.AddDbContext<PostgreContext>(options =>
                options.UseNpgsql(
                    // this is commented since it is used in heroku to parse string
                    //StringParser.Convert(environmentConnectionString) ??
                    environmentConnectionString ??
                    configuration.GetConnectionString("LOCAL_POSTGRES_CONNECTION_STRING")));

            services.AddTransient<DbContext, PostgreContext>();
            return services;
        }
    }
}