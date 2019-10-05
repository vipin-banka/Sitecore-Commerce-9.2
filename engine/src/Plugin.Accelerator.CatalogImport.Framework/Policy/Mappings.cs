using System;
using System.Collections.Generic;
using System.Linq;
using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Plugin.Accelerator.CatalogImport.Framework.Model;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;

namespace Plugin.Accelerator.CatalogImport.Framework.Policy
{
    public class Mappings
    {
        public IList<MapperType> EntityMappings { get; set; }

        public IList<MapperType> EntityComponentMappings { get; set; }

        public IList<MapperType> ItemVariantComponentMappings { get; set; }

        public void MapEntity(CommerceEntity targetEntity, object sourceEntity, CommercePipelineExecutionContext context)
        {
            var mapperType = this
                .EntityMappings
                .FirstOrDefault(x => x.Type.Equals(targetEntity.GetType().FullName, StringComparison.OrdinalIgnoreCase));

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.FullTypeName);

                if (t == null)
                {
                    throw new InvalidOperationException("Entity mapper type cannot be null.");
                }

                if (Activator.CreateInstance(t, sourceEntity, targetEntity, context) is IEntityMapper mapper)
                {
                    mapper.Map();
                }
            }
        }

        public IList<LocalizablePropertyValues> GetEntityLocalizableProperties(object sourceEntity, Type entityType, string language, IList<LocalizablePropertyValues> entityLocalizableProperties, CommercePipelineExecutionContext context)
        {
            var mapperType = this
                .EntityMappings
                .FirstOrDefault(x => x.Type.Equals(entityType.FullName, StringComparison.OrdinalIgnoreCase));

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.LocalizationFullTypeName ?? mapperType.FullTypeName);

                if (t == null)
                {
                    throw new InvalidOperationException("Entity localization mapper type cannot be null.");
                }

                if (Activator.CreateInstance(t, sourceEntity, context) is IEntityLocalizationMapper mapper)
                {
                   return mapper.Map(language, entityLocalizableProperties);
                }
            }

            return new List<LocalizablePropertyValues>();
        }

        public Component MapEntityChildComponent(CommerceEntity targetEntity, object sourceEntity, string componentMappingKey,
            CommercePipelineExecutionContext context)
        {
            var mapperType = this
                .EntityComponentMappings
                .FirstOrDefault(x => x.Key.Equals(componentMappingKey, StringComparison.OrdinalIgnoreCase));

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.FullTypeName);

                if (t == null)
                {
                    throw new InvalidOperationException("Component mapper type cannot be null.");
                }

                if (Activator.CreateInstance(t, sourceEntity, targetEntity, context) is IComponentMapper mapper)
                {
                    return mapper.Map();
                }
            }

            return null;
        }

        public LocalizableComponentPropertiesValues GetEntityComponentLocalizableProperties(CommerceEntity targetEntity, Component component, object sourceEntity, string language, IList<LocalizableComponentPropertiesValues> componentPropertiesList, CommercePipelineExecutionContext context)
        {
            var mapperType = this
                .EntityComponentMappings
                .FirstOrDefault(x => x.Type.Equals(component.GetType().FullName, StringComparison.OrdinalIgnoreCase));

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.LocalizationFullTypeName ?? mapperType.FullTypeName);

                if (t == null)
                {
                    throw new InvalidOperationException("Component localization mapper type cannot be null.");
                }

                if (Activator.CreateInstance(t, sourceEntity, targetEntity, context) is IComponentLocalizationMapper mapper)
                {
                    var componentProperties = componentPropertiesList.FirstOrDefault(x =>
                        component.Id.Equals(x.Id, StringComparison.OrdinalIgnoreCase));

                    componentProperties = mapper.Map(language, componentProperties);
                    if (componentProperties != null)
                    {
                        componentProperties.Id = component.Id;
                        if (!componentPropertiesList.Any(x =>
                            component.Id.Equals(x.Id, StringComparison.OrdinalIgnoreCase)))
                        {
                            componentPropertiesList.Add(componentProperties);
                        }
                    }
                }
            }

            return null;
        }

        public Component MapComponentChildComponent(CommerceEntity targetEntity, Component parentComponent, object sourceEntity, object sourceComponent, string childComponentMappingKey, CommercePipelineExecutionContext context)
        {
            var mapperType = this
                .ItemVariantComponentMappings
                .FirstOrDefault(x => x.Key.Equals(childComponentMappingKey, StringComparison.OrdinalIgnoreCase));

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.FullTypeName);

                if (t == null)
                {
                    throw new InvalidOperationException("Component mapper type cannot be null.");
                }

                if (Activator.CreateInstance(t, sourceEntity, sourceComponent, targetEntity, parentComponent, context) is IComponentMapper mapper)
                {
                    return mapper.Map();
                }
            }

            return null;
        }

        public LocalizableComponentPropertiesValues GetVariantComponentsLocalizableProperties(CommerceEntity targetEntity, Component component, object sourceEntity, object sourceVariant, string language, IList<LocalizableComponentPropertiesValues> componentPropertiesList, CommercePipelineExecutionContext context)
        {
            var mapperType = this
                .ItemVariantComponentMappings
                .FirstOrDefault(x => x.Type.Equals(component.GetType().FullName, StringComparison.OrdinalIgnoreCase));

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.LocalizationFullTypeName ?? mapperType.FullTypeName);

                if (t == null)
                {
                    throw new InvalidOperationException("Component localization mapper type cannot be null.");
                }

                if (Activator.CreateInstance(t, sourceEntity, sourceVariant, targetEntity, component, context) is IComponentLocalizationMapper mapper)
                {
                    var componentProperties = componentPropertiesList.FirstOrDefault(x =>
                        component.Id.Equals(x.Id, StringComparison.OrdinalIgnoreCase));

                    componentProperties = mapper.Map(language, componentProperties);
                    if (componentProperties != null)
                    {
                        componentProperties.Id = component.Id;
                        if (!componentPropertiesList.Any(x =>
                            component.Id.Equals(x.Id, StringComparison.OrdinalIgnoreCase)))
                        {
                            componentPropertiesList.Add(componentProperties);
                        }
                    }
                }
            }

            return null;
        }
    }
}