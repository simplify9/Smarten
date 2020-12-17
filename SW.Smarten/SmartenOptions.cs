using Marten;
using Microsoft.Extensions.DependencyInjection;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;

namespace SW.Smarten
{
    public class SmartenOptions
    {
        public const string ConfigurationSection = "Smarten";
        private readonly IServiceCollection services;

        public SmartenOptions(IServiceCollection services)
        {
            this.services = services;
        }

        public string ConnectionStringName { get; set; }
        public string MaintenanceDatabase { get; set; }
        public string DatabaseSchema { get; set; }
        public bool EnableLogging { get; set; }
        public StoreOptions StoreOptions { get; set; } = new StoreOptions();

        public IDictionary<string, Type> EntityDictionary { get; set; } = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        public void AddEntity<TEntity, TPrimaryKey>(string url)
            where TEntity : class, IEntity<TPrimaryKey> 
        {
            EntityDictionary.Add(url.ToLower(), typeof(EntityAdapter<TEntity, TPrimaryKey>));
            services.AddScoped<EntityAdapter<TEntity, TPrimaryKey>>();
        }

        public void AddEntity<TEntity, TPrimaryKey, TTranslation, TModel>(string url)
            where TEntity : class, IMultiLingualEntity<TTranslation>, IEntity<TPrimaryKey> 
            where TTranslation : class, IEntityTranslation
        {
            EntityDictionary.Add(url.ToLower(), typeof(EntityAdapter<TEntity, TPrimaryKey, TTranslation, TModel>));
            services.AddScoped<EntityAdapter<TEntity, TPrimaryKey, TTranslation, TModel>>();
        }
    }
}
