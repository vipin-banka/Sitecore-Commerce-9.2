using Plugin.Accelerator.CatalogImport.Framework.Model;
using Plugin.Accelerator.CatalogImport.Framework.Policy;
using Plugin.Accelerator.CatalogImport.Sample.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Accelerator.CatalogImport.Sample.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.GetLocalizePropertiesBlock)]
    public class GetLocalizePropertiesBlock : PipelineBlock<ImportLocalizeContentArgument, ImportLocalizeContentArgument, CommercePipelineExecutionContext>
    {
        public override async Task<ImportLocalizeContentArgument> Run(ImportLocalizeContentArgument arg, CommercePipelineExecutionContext context)
        {
            if (arg.Product.Languages != null
                && arg.Product.Languages.Any())
            {
                var catalogImportPolicy = context.CommerceContext.GetPolicy<CatalogImportPolicy>();

                IList<LocalizablePropertyValues> entityLocalizableProperties = null;

                IList<LocalizableComponentPropertiesValues> componentsPropertiesList = new List<LocalizableComponentPropertiesValues>();

                foreach (var productLanguage in arg.Product.Languages)
                {
                    entityLocalizableProperties = catalogImportPolicy
                        .Mappings
                        .GetEntityLocalizableProperties(
                            productLanguage.Entity,
                            typeof(SellableItem),
                            productLanguage.Language,
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
                                productLanguage.Entity,
                                productLanguage.Language,
                                componentsPropertiesList,
                                context);
                    }

                    if (productLanguage.Entity.Variants == null || !productLanguage.Entity.Variants.Any())
                    {
                        continue;
                    }

                    foreach (var productVariant in productLanguage.Entity.Variants)
                    {
                        var itemVariationComponent = (arg.CommerceEntity as SellableItem).GetVariation(productVariant.Id);

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
                                    productLanguage.Entity,
                                    productVariant, 
                                    productLanguage.Language,
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