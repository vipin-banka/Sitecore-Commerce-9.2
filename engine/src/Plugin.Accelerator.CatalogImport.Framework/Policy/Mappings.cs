using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Plugin.Accelerator.CatalogImport.Framework.Model;
using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plugin.Accelerator.CatalogImport.Framework.Policy
{
    public class Mappings
    {
        public IList<EntityMapperType> EntityMappings { get; set; }

        public IList<MapperType> EntityComponentMappings { get; set; }

        public IList<MapperType> ItemVariantionMappings { get; set; }

        public IList<MapperType> ItemVariantionComponentMappings { get; set; }

        public IEntityImportHandler GetImportHandlerInstance(ImportEntityArgument importEntityArgument, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            var handlerType = this
                .EntityMappings
                .FirstOrDefault(x => x.Key.Equals(importEntityArgument.SourceEntityDetail.EntityType, StringComparison.OrdinalIgnoreCase));

            if (handlerType != null)
            {
                var t = Type.GetType(handlerType.ImportHandlerTypeName);

                if (t == null)
                {
                    context.Abort("Entity mapper type cannot be null.", context);
                }

                if (Activator.CreateInstance(t, importEntityArgument.SourceEntityDetail.SerializedEntity, commerceCommander, context) is
                    IEntityImportHandler handler)
                {
                    return handler;
                }
            }

            return null;
        }

        public void MapEntity(CommerceEntity targetEntity, object sourceEntity, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            var mapperType = this
                .EntityMappings
                .FirstOrDefault(x => x.Type.Equals(targetEntity.GetType().FullName, StringComparison.OrdinalIgnoreCase));

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.FullTypeName);

                if (t == null)
                {
                    context.Abort("Entity mapper type cannot be null.", context);
                }

                if (Activator.CreateInstance(t, sourceEntity, targetEntity, commerceCommander, context) is IEntityMapper mapper)
                {
                    mapper.Map();
                }
            }
        }

        public IList<LocalizablePropertyValues> GetEntityLocalizableProperties(object sourceEntity, Type entityType, ILanguageEntity languageEntity, IList<LocalizablePropertyValues> entityLocalizableProperties, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            var mapperType = this
                .EntityMappings
                .FirstOrDefault(x => x.Type.Equals(entityType.FullName, StringComparison.OrdinalIgnoreCase));

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.LocalizationFullTypeName ?? mapperType.FullTypeName);

                if (t == null)
                {
                    context.Abort("Entity localization mapper type cannot be null.", context);
                }

                if (Activator.CreateInstance(t, sourceEntity, commerceCommander, context) is IEntityLocalizationMapper mapper)
                {
                   return mapper.Map(languageEntity, entityLocalizableProperties);
                }
            }

            return new List<LocalizablePropertyValues>();
        }

        public Component MapEntityChildComponent(CommerceEntity targetEntity, object sourceEntity, string componentMappingKey, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            var mapperType = this
                .EntityComponentMappings
                .FirstOrDefault(x => x.Key.Equals(componentMappingKey, StringComparison.OrdinalIgnoreCase));

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.FullTypeName);

                if (t == null)
                {
                    context.Abort("Component mapper type cannot be null.", context);
                }

                if (Activator.CreateInstance(t, sourceEntity, targetEntity, commerceCommander, context) is IComponentMapper mapper)
                {
                    var action = mapper.GetComponentAction();
                    if (action == ComponentAction.Ignore)
                        return null;
                    if (action == ComponentAction.Map)
                    {
                        return mapper.Map();
                    }
                    else
                    {
                        mapper.Remove();
                    }
                }
            }

            return null;
        }

        public LocalizableComponentPropertiesValues GetEntityComponentLocalizableProperties(CommerceEntity targetEntity, Component component, object sourceEntity, ILanguageEntity languageEntity, IList<LocalizableComponentPropertiesValues> componentPropertiesList, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            var mapperType = this
                .EntityComponentMappings
                .FirstOrDefault(x => x.Type.Equals(component.GetType().FullName, StringComparison.OrdinalIgnoreCase));

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.LocalizationFullTypeName ?? mapperType.FullTypeName);

                if (t == null)
                {
                    context.Abort("Component localization mapper type cannot be null.", context);
                }

                if (Activator.CreateInstance(t, sourceEntity, targetEntity, commerceCommander, context) is IComponentLocalizationMapper mapper)
                {
                    var componentProperties = componentPropertiesList.FirstOrDefault(x =>
                        component.Id.Equals(x.Id, StringComparison.OrdinalIgnoreCase));

                    componentProperties = mapper.Map(languageEntity, componentProperties);
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

        public Component MapItemVariationComponent(CommerceEntity targetEntity, Component parentComponent, object sourceEntity, object sourceComponent, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            var mapperType = this
                .ItemVariantionMappings
                .FirstOrDefault();

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.FullTypeName);

                if (t == null)
                {
                    context.Abort("Component mapper type cannot be null.", context);
                }

                if (Activator.CreateInstance(t, sourceEntity, sourceComponent, targetEntity, parentComponent, commerceCommander, context) is IComponentMapper mapper)
                {
                    return mapper.Map();
                }
            }

            return null;
        }

        public LocalizableComponentPropertiesValues GetItemVariantComponentLocalizableProperties(CommerceEntity targetEntity, Component component, object sourceEntity, object sourceVariant, ILanguageEntity languageEntity, IList<LocalizableComponentPropertiesValues> componentPropertiesList, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            var mapperType = this
                .ItemVariantionMappings
                .FirstOrDefault();

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.LocalizationFullTypeName ?? mapperType.FullTypeName);

                if (t == null)
                {
                    context.Abort("Component localization mapper type cannot be null.", context);
                }

                if (Activator.CreateInstance(t, sourceEntity, sourceVariant, targetEntity, component, commerceCommander, context) is IComponentLocalizationMapper mapper)
                {
                    var componentProperties = componentPropertiesList.FirstOrDefault(x =>
                        component.Id.Equals(x.Id, StringComparison.OrdinalIgnoreCase));

                    componentProperties = mapper.Map(languageEntity, componentProperties);
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

        public Component MapComponentChildComponent(CommerceEntity targetEntity, Component parentComponent, object sourceEntity, object sourceComponent, string childComponentMappingKey, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            var mapperType = this
                .ItemVariantionComponentMappings
                .FirstOrDefault(x => x.Key.Equals(childComponentMappingKey, StringComparison.OrdinalIgnoreCase));

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.FullTypeName);

                if (t == null)
                {
                    context.Abort("Component mapper type cannot be null.", context);
                }

                if (Activator.CreateInstance(t, sourceEntity, sourceComponent, targetEntity, parentComponent, commerceCommander, context) is IComponentMapper mapper)
                {
                    return mapper.Map();
                }
            }

            return null;
        }

        public LocalizableComponentPropertiesValues GetVariantComponentsLocalizableProperties(CommerceEntity targetEntity, Component component, object sourceEntity, object sourceVariant, ILanguageEntity languageEntity, IList<LocalizableComponentPropertiesValues> componentPropertiesList, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            var mapperType = this
                .ItemVariantionComponentMappings
                .FirstOrDefault(x => x.Type.Equals(component.GetType().FullName, StringComparison.OrdinalIgnoreCase));

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.LocalizationFullTypeName ?? mapperType.FullTypeName);

                if (t == null)
                {
                    context.Abort("Component localization mapper type cannot be null.", context);
                }

                if (Activator.CreateInstance(t, sourceEntity, sourceVariant, targetEntity, component, commerceCommander, context) is IComponentLocalizationMapper mapper)
                {
                    var componentProperties = componentPropertiesList.FirstOrDefault(x =>
                        component.Id.Equals(x.Id, StringComparison.OrdinalIgnoreCase));

                    componentProperties = mapper.Map(languageEntity, componentProperties);
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