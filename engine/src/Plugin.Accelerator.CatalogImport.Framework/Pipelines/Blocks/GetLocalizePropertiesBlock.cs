using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Plugin.Accelerator.CatalogImport.Framework.Model;
using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments;
using Plugin.Accelerator.CatalogImport.Framework.Policy;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.GetLocalizePropertiesBlock)]
    public class GetLocalizePropertiesBlock : PipelineBlock<ImportLocalizeContentArgument, ImportLocalizeContentArgument, CommercePipelineExecutionContext>
    {
        public override async Task<ImportLocalizeContentArgument> Run(ImportLocalizeContentArgument arg, CommercePipelineExecutionContext context)
        {
            if (arg.ImportHandler.HasLanguages())
            {
                var catalogImportPolicy = context.CommerceContext.GetPolicy<CatalogImportPolicy>();

                IList<LocalizablePropertyValues> entityLocalizableProperties = null;

                IList<LocalizableComponentPropertiesValues> componentsPropertiesList = new List<LocalizableComponentPropertiesValues>();

                foreach (var language in arg.ImportHandler.GetLanguages())
                {
                    if (arg.ImportHandler is IEntityLocalizationMapper mapper)
                    {
                        entityLocalizableProperties = mapper.Map(language.Language, entityLocalizableProperties);
                    }
                    else
                    {
                        entityLocalizableProperties = catalogImportPolicy
                            .Mappings
                            .GetEntityLocalizableProperties(
                                language.GetEntity(),
                                arg.CommerceEntity.GetType(),
                                language.Language,
                                entityLocalizableProperties,
                                context);
                    }

                    if (arg.CommerceEntity.EntityComponents == null || !arg.CommerceEntity.EntityComponents.Any())
                        continue;

                    foreach (var commerceEntityComponent in arg.CommerceEntity.EntityComponents)
                    {
                        catalogImportPolicy
                            .Mappings
                            .GetEntityComponentLocalizableProperties(
                                arg.CommerceEntity,
                                commerceEntityComponent,
                                language.GetEntity(),
                                language.Language,
                                componentsPropertiesList,
                                context);
                    }

                    if (!arg.ImportHandler.HasVariants(language) )
                    {
                        continue;
                    }

                    foreach (var variant in arg.ImportHandler.GetVariants(language))
                    {
                        var itemVariationComponent = (arg.CommerceEntity as SellableItem).GetVariation(variant.Id);

                        if (itemVariationComponent == null)
                            continue;

                        catalogImportPolicy.Mappings.GetItemVariantComponentLocalizableProperties(
                            arg.CommerceEntity,
                            itemVariationComponent,
                            language.GetEntity(),
                            variant,
                            language.Language,
                            componentsPropertiesList,
                            context);

                        if (itemVariationComponent.ChildComponents != null 
                            && itemVariationComponent.ChildComponents.Any())
                        {
                            foreach (var component in itemVariationComponent.ChildComponents)
                            {
                                catalogImportPolicy.Mappings.GetVariantComponentsLocalizableProperties(
                                    arg.CommerceEntity,
                                    component,
                                    language.GetEntity(),
                                    variant,
                                    language.Language,
                                    componentsPropertiesList,
                                    context);
                            }
                        }
                    }
                }

                arg.Properties = entityLocalizableProperties;
                arg.ComponentsProperties = componentsPropertiesList;
            }

            return await Task.FromResult(arg);
        }
    }
}