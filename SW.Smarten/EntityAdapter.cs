using AutoMapper;
using Marten;
using Newtonsoft.Json;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Smarten
{

    class EntityAdapter<TEntity, TPrimaryKey> : IEntityAdapter
        where TEntity : IEntity<TPrimaryKey>
    {
        private readonly IQuerySession querySession;
        private readonly IDocumentSession documentSession;

        public EntityAdapter(IQuerySession querySession, IDocumentSession documentSession)
        {
            this.querySession = querySession;
            this.documentSession = documentSession;
        }

        public virtual async Task BulkSave(string bulkData)
        {
            var entities = JsonConvert.DeserializeObject<IEnumerable<TEntity>>(bulkData);

            documentSession.Store(entities);
            await documentSession.SaveChangesAsync();
        }

        public virtual async Task<object> GetById(string id)
        {
            if (typeof(IEntity<int>).IsAssignableFrom(typeof(TEntity)))
                return await querySession.LoadAsync<TEntity>(int.Parse(id));
            else if (typeof(IEntity<string>).IsAssignableFrom(typeof(TEntity)))
                return await querySession.Query<TEntity>().Where(i => string.Equals(i.Id.ToString(), id, StringComparison.OrdinalIgnoreCase)).SingleOrDefaultAsync();
            else
                throw new NotImplementedException();
        }

        async public virtual Task<object> Search(string search)
        {
            return await querySession.Query<TEntity>().ToListAsync();
        }
    }

    class EntityAdapter<TEntity, TPrimaryKey, TTranslation, TModel> : EntityAdapter<TEntity, TPrimaryKey>
        where TEntity : class, IMultiLingualEntity<TTranslation>, IEntity<TPrimaryKey>
        where TTranslation : class, IEntityTranslation

    {
        private readonly RequestContext requestContext;
        private static MapperConfiguration mapperConfig;

        public EntityAdapter(IQuerySession querySession, IDocumentSession documentSession, RequestContext requestContext) : base(querySession, documentSession)
        {
            this.requestContext = requestContext;
            Configure();

        }

        private void Configure()
        {
            if (mapperConfig == null)
                mapperConfig = new MapperConfiguration(configure =>
                    configure.CreateMultiLingualMap<TEntity, TTranslation, TModel>());
        }

        async public override Task<object> Search(string search)
        {
            var entities = (IEnumerable<TEntity>)await base.Search(search);
            var mapper = mapperConfig.CreateMapper();
            var result = entities.Select(i => mapper.Map<TModel>(i, options =>
            {

                options.Items["locale"] = requestContext.Locale;
            }));

            return result;
        }

        public override async Task<object> GetById(string id)
        {
            var entity = await base.GetById(id);
            var mapper = mapperConfig.CreateMapper();
            var model = mapper.Map<TModel>(entity, options =>
            {

                options.Items["locale"] = requestContext.Locale;
            });


            return model;
        }

        public override Task BulkSave(string bulkData)
        {
            return base.BulkSave(bulkData);
        }
    }

}
