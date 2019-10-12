using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Plugin.Accelerator.CatalogImport.Framework.Model;
using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Plugin.Accelerator.CatalogImport.Framework.Extensions;

namespace Plugin.Accelerator.CatalogImport.Framework.Policy
{
    public class Mappings
    {
        public IList<EntityMapperType> EntityMappings { get; set; }

        public IList<MapperType> EntityComponentMappings { get; set; }

        public IList<MapperType> ItemVariantionMappings { get; set; }

        public IList<MapperType> ItemVariantionComponentMappings { get; set; }

        public IEntityImportHandler GetImportHandler(ImportEntityArgument importEntityArgument, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            var handlerType = this
                .EntityMappings
                .FirstOrDefault(x => x.Key.Equals(importEntityArgument.SourceEntityDetail.EntityType, StringComparison.OrdinalIgnoreCase));

            if (handlerType != null)
            {
                var t = Type.GetType(handlerType.ImportHandlerTypeName);

                if (t != null)
                {
                    if (Activator.CreateInstance(t, importEntityArgument.SourceEntityDetail.SerializedEntity,
                            commerceCommander, context) is
                        IEntityImportHandler handler)
                    {
                        return handler;
                    }
                }
            }

            return null;
        }

        public IEntityMapper GetEntityMapper(CommerceEntity targetEntity, ImportEntityArgument importEntityArgument, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            var mapperType = this
                .EntityMappings
                .FirstOrDefault(x => x.Key.Equals(importEntityArgument.SourceEntityDetail.EntityType, StringComparison.OrdinalIgnoreCase));

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.FullTypeName ?? mapperType.ImportHandlerTypeName);

                if (t != null)
                {
                    if (Activator.CreateInstance(t, importEntityArgument.SourceEntity, targetEntity, commerceCommander,
                        context) is IEntityMapper mapper)
                    {
                        return mapper;
                    }
                }
            }

            return null;
        }

        public IEntityLocalizationMapper GetEntityLocalizationMapper(ILanguageEntity languageEntity, ImportEntityArgument importEntityArgument, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            var mapperType = this
                .EntityMappings
                .FirstOrDefault(x => x.Key.Equals(importEntityArgument.SourceEntityDetail.EntityType, StringComparison.OrdinalIgnoreCase));

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.ImportHandlerTypeName ?? mapperType.FullTypeName ?? mapperType.LocalizationFullTypeName);

                if (t != null)
                {
                    if (Activator.CreateInstance(t, languageEntity.GetEntity(), commerceCommander, context) is
                        IEntityLocalizationMapper mapper)
                    {
                        return mapper;
                    }
                }
            }

            return null;
        }

        public IComponentMapper GetEntityComponentMapper(CommerceEntity targetEntity, ImportEntityArgument importEntityArgument, string componentMappingKey, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            var mapperType = this
                .EntityComponentMappings
                .FirstOrDefault(x => x.Key.Equals(componentMappingKey, StringComparison.OrdinalIgnoreCase));

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.FullTypeName);

                if (t != null)
                {
                    if (Activator.CreateInstance(t, importEntityArgument.SourceEntity, targetEntity, commerceCommander, context) is
                        IComponentMapper mapper)
                    {
                        return mapper;
                    }
                }
            }

            return null;
        }

        public IComponentLocalizationMapper GetEntityComponentLocalizationMapper(CommerceEntity targetEntity, Component component,  ILanguageEntity languageEntity, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            var metadataPolicy = component.GetComponentMetadataPolicy();
            var mapperType = this
                .EntityComponentMappings
                .FirstOrDefault(x => 
                    (metadataPolicy != null && !string.IsNullOrEmpty(metadataPolicy.MapperKey) && x.Key.Equals(metadataPolicy.MapperKey, StringComparison.OrdinalIgnoreCase))
                    || x.Type.Equals(component.GetType().FullName, StringComparison.OrdinalIgnoreCase));

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.LocalizationFullTypeName ?? mapperType.FullTypeName);

                if (t != null)
                {
                    if (Activator.CreateInstance(t, languageEntity.GetEntity(), targetEntity, commerceCommander, context) is
                        IComponentLocalizationMapper mapper)
                    {
                        return mapper;
                    }
                }
            }

            return null;
        }

        public IComponentMapper GetItemVariationComponentMapper(CommerceEntity targetEntity, Component parentComponent, ImportEntityArgument importEntityArgument, object sourceComponent, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            var mapperType = this
                .ItemVariantionMappings
                .FirstOrDefault();

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.FullTypeName);

                if (t != null)
                {
                    if (Activator.CreateInstance(t, importEntityArgument.SourceEntity, sourceComponent, targetEntity, parentComponent,
                        commerceCommander, context) is IComponentMapper mapper)
                    {
                        return mapper;
                    }
                }
            }

            return null;
        }

        public IComponentLocalizationMapper GetItemVariantComponentLocalizationMapper(CommerceEntity targetEntity, Component component, ILanguageEntity languageEntity, object sourceVariant, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            var mapperType = this
                .ItemVariantionMappings
                .FirstOrDefault();

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.LocalizationFullTypeName ?? mapperType.FullTypeName);

                if (t != null)
                {
                    if (Activator.CreateInstance(t, languageEntity.GetEntity(), sourceVariant, targetEntity, component, commerceCommander, context) is IComponentLocalizationMapper mapper)
                    {
                        return mapper;
                    }
                }
            }

            return null;
        }

        public IComponentMapper GetComponentChildComponentMapper(CommerceEntity targetEntity, Component parentComponent, ImportEntityArgument importEntityArgument, object sourceComponent, string childComponentMappingKey, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            var mapperType = this
                .ItemVariantionComponentMappings
                .FirstOrDefault(x => x.Key.Equals(childComponentMappingKey, StringComparison.OrdinalIgnoreCase));

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.FullTypeName);

                if (t != null)
                {
                    if (Activator.CreateInstance(t, importEntityArgument.SourceEntity, sourceComponent, targetEntity,
                        parentComponent, commerceCommander, context) is IComponentMapper mapper)
                    {
                        return mapper;
                    }
                }
            }

            return null;
        }

        public IComponentLocalizationMapper GetComponentChildComponentLocalizationMapper(CommerceEntity targetEntity, Component component, ILanguageEntity languageEntity, object sourceVariant, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            var metadataPolicy = component.GetComponentMetadataPolicy();
            var mapperType = this
                .ItemVariantionComponentMappings
                .FirstOrDefault(
                    x => (metadataPolicy != null && !string.IsNullOrEmpty(metadataPolicy.MapperKey) && x.Key.Equals(metadataPolicy.MapperKey, StringComparison.OrdinalIgnoreCase))
                        || x.Type.Equals(component.GetType().FullName, StringComparison.OrdinalIgnoreCase));

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.LocalizationFullTypeName ?? mapperType.FullTypeName);

                if (t != null)
                {
                    if (Activator.CreateInstance(t, languageEntity.GetEntity(), sourceVariant, targetEntity, component,
                        commerceCommander, context) is IComponentLocalizationMapper mapper)
                    {
                        return mapper;
                    }
                }
            }

            return null;
        }
    }
}