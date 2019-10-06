using Plugin.Accelerator.CatalogImport.Framework.Model;
using Plugin.Accelerator.CatalogImport.Framework.Policy;
using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments;
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
                    entityLocalizableProperties = catalogImportPolicy
                        .Mappings
                        .GetEntityLocalizableProperties(
                            language.GetEntity(),
                            arg.CommerceEntity.GetType(),
                            language.Language,
                            entityLocalizableProperties,
                            context);

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

                        IList<Component> componentsList = new List<Component> { itemVariationComponent };

                        if (itemVariationComponent.ChildComponents != null ||
                            itemVariationComponent.ChildComponents.Any())
                        {
                            itemVariationComponent.ChildComponents.Select(x =>
                            {
                                componentsList.Add(x);
                                return x;
                            }).ToList();
                        }

                        foreach (var component in componentsList)
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

                arg.Properties = entityLocalizableProperties;
                arg.ComponentsProperties = componentsPropertiesList;
            }

            return await Task.FromResult(arg);
        }
    }
}