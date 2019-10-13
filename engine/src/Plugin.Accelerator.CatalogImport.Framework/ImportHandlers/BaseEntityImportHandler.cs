﻿using Newtonsoft.Json;
using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Plugin.Accelerator.CatalogImport.Framework.Extensions;
using Plugin.Accelerator.CatalogImport.Framework.Metadata;
using Plugin.Accelerator.CatalogImport.Framework.Model;
using Sitecore.Commerce.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Accelerator.CatalogImport.Framework.ImportHandlers
{
    public abstract class BaseEntityImportHandler<TSourceEntity, TCommerceEntity> : IEntityImportHandler, IEntityImportHandler<TCommerceEntity>, IEntityMapper, IEntityLocalizationMapper
    where TSourceEntity : IEntity
    where TCommerceEntity : CommerceEntity
    {
        public TSourceEntity SourceEntity { get; }

        public TCommerceEntity CommerceEntity { get; set; }

        public CommerceCommander CommerceCommander { get; }

        public CommercePipelineExecutionContext Context { get; }

        public IDictionary<string, IList<string>> ParentEntityIds { get; set; }

        public BaseEntityImportHandler(string sourceEntity, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            this.SourceEntity = JsonConvert.DeserializeObject<TSourceEntity>(sourceEntity);
            this.CommerceCommander = commerceCommander;
            this.Context = context;
        }

        public string EntityId => IdWithPrefix();

        public IEntity GetSourceEntity()
        {
            return this.SourceEntity;
        }

        public virtual bool Validate()
        {
            return true;
        }

        public CommerceEntity GetCommerceEntity()
        {
            return this.CommerceEntity;
        }

        public void SetCommerceEntity(CommerceEntity commerceEntity)
        {
            this.CommerceEntity = commerceEntity as TCommerceEntity;
        }

        protected virtual string Id => typeof(TSourceEntity).GetPropertyValueWithAttribute<EntityIdAttribute, string>(this.SourceEntity);

        protected string IdWithPrefix()
        {
            var prefix = Sitecore.Commerce.Core.CommerceEntity.IdPrefix<TCommerceEntity>();
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

        public abstract Task<CommerceEntity> Create();

        public virtual IList<string> GetParentList()
        {
            return typeof(TSourceEntity).GetPropertyValueWithAttribute<ParentsAttribute, IList<string>>(this.SourceEntity);
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

        public virtual bool HasVariants()
        {
            return false;
        }

        public virtual IList<IEntity> GetVariants()
        {
            return new List<IEntity>();
        }

        protected IList<IEntity> GetVariants(object instance)
        {
            var variants = typeof(TSourceEntity).GetPropertyValueWithAttribute<VariantsAttribute, IEnumerable>(instance);

            if (variants != null)
            {
                return variants.Cast<IEntity>().ToList();
            }

            return new List<IEntity>();
        }

        public virtual bool HasVariants(ILanguageEntity languageEntity)
        {
            return false;
        }

        public virtual IList<IEntity> GetVariants(ILanguageEntity languageEntity)
        {
            return new List<IEntity>();
        }

        public virtual void Map()
        {
        }

        public virtual IList<LocalizablePropertyValues> Map(ILanguageEntity languageEntity, IList<LocalizablePropertyValues> entityLocalizableProperties)
        {
            Type t = typeof(TCommerceEntity);

            var commerceEntity = Activator.CreateInstance(t) as TCommerceEntity;

            ILanguageEntity<TSourceEntity> l = languageEntity as ILanguageEntity<TSourceEntity>;
            if (l == null)
            {
                this.Context.Abort(Context.CommerceContext.AddMessage(Context.GetPolicy<KnownResultCodes>().Error, "LanguageEntityMissing", null, "Language entity cannot be null").Result, this.Context);
            }

            this.MapLocalizeValues(l.Entity, commerceEntity);

            if (entityLocalizableProperties == null)
            {
                entityLocalizableProperties = LocalizablePropertyListManager.GetEntityProperties(t, this.Context);
                if (entityLocalizableProperties != null)
                {
                    entityLocalizableProperties = entityLocalizableProperties.Clone();
                }
            }

            if (entityLocalizableProperties == null || !entityLocalizableProperties.Any())
            {
                return new List<LocalizablePropertyValues>();
            }

            var properties = TypePropertyListManager.GetProperties(t);
            foreach (var localizablePropertyValues in entityLocalizableProperties)
            {
                if (!string.IsNullOrEmpty(localizablePropertyValues.PropertyName))
                {
                    var propertyInfo = properties.FirstOrDefault(x =>
                        x.Name.Equals(localizablePropertyValues.PropertyName, StringComparison.OrdinalIgnoreCase));
                    if (propertyInfo != null)
                    {
                        var propertyValue = propertyInfo.GetValue(commerceEntity);
                        var parameter = localizablePropertyValues.Parameters.FirstOrDefault(x =>
                            x.Key.Equals(languageEntity.Language, StringComparison.OrdinalIgnoreCase));

                        if (parameter == null)
                        {
                            parameter = new Parameter { Key = languageEntity.Language, Value = null };
                            localizablePropertyValues.Parameters.Add(parameter);
                        }

                        parameter.Value = propertyValue;
                    }
                }
            }

            return entityLocalizableProperties;
        }

        protected virtual void MapLocalizeValues(TSourceEntity localizedSourceEntity, TCommerceEntity localizedTargetEntity)
        {
        }
    }
}