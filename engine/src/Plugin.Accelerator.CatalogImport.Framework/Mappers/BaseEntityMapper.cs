using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Plugin.Accelerator.CatalogImport.Framework.Extensions;
using Plugin.Accelerator.CatalogImport.Framework.Model;
using Sitecore.Commerce.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plugin.Accelerator.CatalogImport.Framework.Mappers
{
    public abstract class BaseEntityMapper<T> : IEntityMapper, IEntityLocalizationMapper
        where T : CommerceEntity, new()
    {
        public T CommerceEntity { get; }

        public CommercePipelineExecutionContext Context { get; }

        protected BaseEntityMapper(CommercePipelineExecutionContext context)
        : this(null, context)
        {
        }

        protected BaseEntityMapper(T commerceEntity, CommercePipelineExecutionContext context)
        {
            this.CommerceEntity = commerceEntity;
            this.Context = context;
        }

        public virtual void Map()
        {
        }

        public virtual IList<LocalizablePropertyValues> Map(string language, IList<LocalizablePropertyValues> entityLocalizableProperties)
        {
            Type t = typeof(T);
            if (t == null)
                throw new InvalidOperationException("Type cannot be null.");

            var entity = Activator.CreateInstance(t) as T;

            this.MapLocalizeValues(entity);

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
                        var propertyValue = propertyInfo.GetValue(entity);
                        var parameter = localizablePropertyValues.Parameters.FirstOrDefault(x =>
                            x.Key.Equals(language, StringComparison.OrdinalIgnoreCase));

                        if (parameter == null)
                        {
                            parameter = new Parameter { Key = language, Value = null };
                            localizablePropertyValues.Parameters.Add(parameter);
                        }

                        parameter.Value = propertyValue;
                    }
                }
            }

            return entityLocalizableProperties;
        }

        protected virtual void MapLocalizeValues(T entity)
        {
        }
    }
}