using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Plugin.Accelerator.CatalogImport.Framework.Extensions;
using Plugin.Accelerator.CatalogImport.Framework.Model;
using Sitecore.Commerce.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plugin.Accelerator.CatalogImport.Framework.Mappers
{
    public abstract class BaseEntityMapper<TSourceEntity, TCommerceEntity> : IEntityMapper, IEntityLocalizationMapper
        where TSourceEntity : IEntity
        where TCommerceEntity : CommerceEntity, new()
    {
        public TSourceEntity SourceEntity { get; }

        public TCommerceEntity CommerceEntity { get; }

        public CommercePipelineExecutionContext Context { get; }

        protected BaseEntityMapper(TSourceEntity sourceEntity, TCommerceEntity commerceEntity, CommercePipelineExecutionContext context)
        {
            this.SourceEntity = sourceEntity;
            this.CommerceEntity = commerceEntity;
            this.Context = context;
        }

        public virtual void Map()
        {
        }

        public virtual IList<LocalizablePropertyValues> Map(ILanguageEntity languageEntity, IList<LocalizablePropertyValues> entityLocalizableProperties)
        {
            Type t = typeof(TCommerceEntity);
            if (t == null)
                throw new InvalidOperationException("Type cannot be null.");

            var commerceEntity = Activator.CreateInstance(t) as TCommerceEntity;

            var l = languageEntity as ILanguageEntity<TSourceEntity>;
            if (l == null)
                throw new InvalidOperationException("Language entity cannot be null.");

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

        protected virtual void MapLocalizeValues(TSourceEntity sourceEntity, TCommerceEntity targetEntity)
        {
        }
    }
}