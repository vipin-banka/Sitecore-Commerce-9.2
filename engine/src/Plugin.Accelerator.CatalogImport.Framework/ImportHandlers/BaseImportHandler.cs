using Newtonsoft.Json;
using Sitecore.Commerce.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.Accelerator.CatalogImport.Framework.Abstractions;

namespace Plugin.Accelerator.CatalogImport.Framework.ImportHandlers
{
    public abstract class BaseImportHandler<TSourceEntity, TCommerceEntity> : IImportHandler
    where TSourceEntity : class
    where TCommerceEntity : CommerceEntity
    {
        public TSourceEntity SourceEntity { get; }

        public BaseImportHandler(string sourceEntity)
        {
            this.SourceEntity = JsonConvert.DeserializeObject<TSourceEntity>(sourceEntity);
        }

        public string EntityId
        {
            get { return IdWithPrefix(); }
        }

        public object GetSourceEntity()
        {
            return this.SourceEntity;
        }

        protected abstract string Id { get; }

        protected string IdWithPrefix()
        {
            var prefix = CommerceEntity.IdPrefix<TCommerceEntity>();
            var id = this.Id;
            if (!id.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("{0}{1}", prefix, id);
            }

            return id;
        }

        protected virtual void Initialize()
        {
        }

        public abstract Task<CommerceEntity> Create(IServiceProvider serviceProvider, IDictionary<string, IList<string>> parents, CommercePipelineExecutionContext context);

        public abstract IList<string> GetParentList();

        public abstract bool HasVariants();

        public abstract IList<IEntity> GetVariants();

        public virtual bool HasLanguages()
        {
            return false;
        }

        public virtual IList<ILanguageEntity> GetLanguages()
        {
            return new List<ILanguageEntity>();
        }

        public virtual bool HasVariants(ILanguageEntity languageEntity)
        {
            return false;
        }

        public virtual IList<IEntity> GetVariants(ILanguageEntity languageEntity)
        {
            return new List<IEntity>();
        }
    }
}