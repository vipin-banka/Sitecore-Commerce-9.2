using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Plugin.Accelerator.CatalogImport.Framework.Extensions;
using Plugin.Accelerator.CatalogImport.Framework.Model;
using Sitecore.Commerce.Core;
using System;
using System.Linq;

namespace Plugin.Accelerator.CatalogImport.Framework.Mappers
{
    public abstract class BaseComponentMapper<T> : IComponentMapper, IComponentLocalizationMapper
    where T : Component, new()
    {
        public IComponentHandler ComponentHandler { get; }

        public CommerceCommander CommerceCommander { get; }

        public CommercePipelineExecutionContext Context { get; }

        protected BaseComponentMapper(IComponentHandler componentHandler, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            this.ComponentHandler = componentHandler;
            this.CommerceCommander = commerceCommander;
            this.Context = context;
        }

        public virtual Component Map()
        {
            Type t = typeof(T);

            T component;

            if (!this.AllowMultipleComponents)
            {
                component = this.ComponentHandler.GetComponent(t) as T;
                if (component == null)
                {
                    component = Activator.CreateInstance(t) as T;
                    this.ComponentHandler.SetComponent(component);
                }
            }
            else
            {
                component = this.ComponentHandler.GetComponent(t, this.ComponentId) as T;
                if (component == null)
                {
                    component = Activator.CreateInstance(t) as T;
                    component = this.ComponentHandler.AddComponent(component) as T;
                }
            }

            this.Map(component);
            return component;
        }

        public virtual LocalizableComponentPropertiesValues Map(ILanguageEntity languageEntity, LocalizableComponentPropertiesValues localizableComponentPropertiesValues)
        {
            Type t = typeof(T);

            var component = Activator.CreateInstance(t) as T;

            this.MapLocalizeValues(component);

            if (localizableComponentPropertiesValues == null)
            {
                var entityComponentsLocalizableProperties =
                    LocalizablePropertyListManager.GetEntityComponentProperties(this.ComponentHandler.GetEntityType(),
                        this.Context);

                if (entityComponentsLocalizableProperties == null || !entityComponentsLocalizableProperties.Any())
                    return null;

                var path = this.GetLocalizableComponentPath(component);

                var componentProperties =
                    entityComponentsLocalizableProperties.FirstOrDefault(x =>
                        x.Path.Equals(path, StringComparison.OrdinalIgnoreCase));

                if (componentProperties == null || !componentProperties.PropertyValues.Any())
                    return null;

                localizableComponentPropertiesValues = componentProperties.Clone();
            }


            var properties = TypePropertyListManager.GetProperties(t);
            
            foreach (var localizablePropertyValues in localizableComponentPropertiesValues.PropertyValues)
            {
                if (!string.IsNullOrEmpty(localizablePropertyValues.PropertyName))
                {
                    var propertyInfo = properties.FirstOrDefault(x =>
                        x.Name.Equals(localizablePropertyValues.PropertyName, StringComparison.OrdinalIgnoreCase));
                    if (propertyInfo != null)
                    {
                        var propertyValue = propertyInfo.GetValue(component);
                        var parameter = localizablePropertyValues.Parameters.FirstOrDefault(x =>
                            x.Key.Equals(languageEntity.Language, StringComparison.OrdinalIgnoreCase));

                        if (parameter == null)
                        {
                            parameter = new Parameter {Key = languageEntity.Language, Value = null};
                            localizablePropertyValues.Parameters.Add(parameter);
                        }

                        parameter.Value = propertyValue;
                    }
                }
            }

            return localizableComponentPropertiesValues;
        }

        protected virtual void Map(T component)
        {
            return;
        }

        protected virtual void MapLocalizeValues(T component)
        {
            return;
        }

        protected virtual string GetLocalizableComponentPath(T component)
        {
            return component.GetType().Name;
        }

        protected virtual bool AllowMultipleComponents
        {
            get { return false; }
        }

        protected virtual string ComponentId
        {
            get { return string.Empty; }
        }

        public virtual ComponentAction GetComponentAction()
        {
            return ComponentAction.Map;
        }

        public virtual Component Remove()
        {
            return this.ComponentHandler.RemoveComponent<T>();
        }
    }
}