using AutoMapper;
using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;

namespace SW.Smarten
{
    public static class IServiceCollectionExtensions
    {

        public static IServiceCollection AddSmarten(this IServiceCollection services, Action<SmartenOptions> configure)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            
            var smartenOptions = new SmartenOptions(services);

            if (configure != null) configure.Invoke(smartenOptions);

            configuration.GetSection(SmartenOptions.ConfigurationSection).Bind(smartenOptions);
            services.AddSingleton(smartenOptions);


            var connectionString = configuration.GetConnectionString(smartenOptions.ConnectionStringName);

            var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString)
            {
                Database = smartenOptions.MaintenanceDatabase
            };

            var maintenanceDatabaseConnectionString = connectionStringBuilder.ConnectionString;

            if (smartenOptions.EnableLogging)
            {
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                smartenOptions.StoreOptions.Logger(new MartenLogger(loggerFactory));

            }

            smartenOptions.StoreOptions.Connection(connectionString);
            smartenOptions.StoreOptions.DatabaseSchemaName = smartenOptions.DatabaseSchema;
            smartenOptions.StoreOptions.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
            smartenOptions.StoreOptions.CreateDatabasesForTenants(c =>
            {

                // Specify a db to which to connect in case database needs to be created.
                // If not specified, defaults to 'postgres' on the connection for a tenant.
                c.MaintenanceDatabase(maintenanceDatabaseConnectionString);

                c.ForTenant()
                    .CheckAgainstPgDatabase()
                    .WithOwner("doadmin")
                    .WithEncoding("UTF-8")
                    .ConnectionLimit(-1);
                    //.OnDatabaseCreated(_ =>
                    //{
                    //    //dbCreated = true;
                    //});
            });

            services.AddMarten(smartenOptions.StoreOptions).InitializeStore();

            return services;
        }
    }
}
