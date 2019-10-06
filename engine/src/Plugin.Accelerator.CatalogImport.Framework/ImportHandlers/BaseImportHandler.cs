using Newtonsoft.Json;
using Sitecore.Commerce.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Plugin.Accelerator.CatalogImport.Framework.Extensions;
using Plugin.Accelerator.CatalogImport.Framework.Metadata;

namespace Plugin.Accelerator.CatalogImport.Framework.ImportHandlers
{
    public abstract class BaseImportHandler<TSourceEntity, TCommerceEntity> : IImportHandler
    where TSourceEntity : IEntity
    where TCommerceEntity : CommerceEntity
    {
        public TSourceEntity SourceEntity { get; }

        public BaseImportHandler(string sourceEntity)
        {
            this.SourceEntity = JsonConvert.DeserializeObject<TSourceEntity>(sourceEntity);
        }

        public string EntityId => IdWithPrefix();

        public IEntity GetSourceEntity()
        {
            return this.SourceEntity;
        }

        protected virtual string Id => typeof(TSourceEntity).GetPropertyValueWithAttribute<EntityIdAttribute, string>(this.SourceEntity);

        protected string IdWithPrefix()
        {
            var prefix = CommerceEntity.IdPrefix<TCommerceEntity>();
            var id = this.Id;
            if (!id.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                return $"{prefix}{id}";
            }

            return id;
        }

        protected virtual void Initialize()
        {
        }

        public abstract Task<CommerceEntity> Create(IServiceProvider serviceProvider, IDictionary<string, IList<string>> parents, CommercePipelineExecutionContext context);

        public virtual IList<string> GetParentList()
        {
            return typeof(TSourceEntity).GetPropertyValueWithAttribute<ParentsAttribute, IList<string>>(this.SourceEntity);
        }

        public virtual bool HasVariants()
        {
            var variants = typeof(TSourceEntity).GetPropertyValueWithAttribute<VariantsAttribute, IEnumerable>(this.SourceEntity);
            return variants != null && variants.GetEnumerator().MoveNext();
        }

        public virtual IList<IEntity> GetVariants()
        {
            var variants = typeof(TSourceEntity).GetPropertyValueWithAttribute<VariantsAttribute, IEnumerable>(this.SourceEntity);

            if (variants != null)
            {
                return variants.Cast<IEntity>().ToList();
            }

            return new List<IEntity>();
        }

        public virtual bool HasLanguages()
        {
            var languages = typeof(TSourceEntity).GetPropertyValueWithAttribute<LanguagesAttribute, IEnumerable>(this.SourceEntity);
            return languages != null && languages.GetEnumerator().MoveNext();
        }

        public virtual IList<ILanguageEntity> GetLanguages()
        {
            var languages = typeof(TSourceEntity).GetPropertyValueWithAttribute<LanguagesAttribute, IEnumerable>(this.SourceEntity);
            if (languages != null)
            {
                return languages.Cast<ILanguageEntity>().ToList();
            }

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