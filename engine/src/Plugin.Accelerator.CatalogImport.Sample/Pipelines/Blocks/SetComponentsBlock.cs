using Plugin.Accelerator.CatalogImport.Framework.Policy;
using Plugin.Accelerator.CatalogImport.Sample.Pipelines.Arguments;
using Plugin.Accelerator.CatalogImport.Sample.VariantComponentMappers;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Accelerator.CatalogImport.Sample.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.SetComponentsBlock)]
    public class SetComponentsBlocks : PipelineBlock<SellableItem, SellableItem, CommercePipelineExecutionContext>
    {
        public override Task<SellableItem> Run(SellableItem arg, CommercePipelineExecutionContext context)
        {
            var importProductArgument = context.CommerceContext.GetObject<ImportProductArgument>();
            if (importProductArgument != null && importProductArgument.Product != null)
            {
                var catalogImportPolicy = context.CommerceContext.GetPolicy<CatalogImportPolicy>();
                this.SetSellableItemDetails(arg, importProductArgument, catalogImportPolicy, context);
                this.SetSellableItemComponents(arg, importProductArgument, catalogImportPolicy, context);
                this.SetSellableItemVariantsComponents(arg, importProductArgument, catalogImportPolicy, context);
            }

            return Task.FromResult(arg);
        }

        private void SetSellableItemDetails(SellableItem sellableItem, ImportProductArgument importProductArgument, CatalogImportPolicy catalogImportPolicy, CommercePipelineExecutionContext context)
        {
            catalogImportPolicy.Mappings.MapEntity(
                sellableItem, 
                importProductArgument.Product, 
                context);
        }

        private void SetSellableItemComponents(SellableItem sellableItem, ImportProductArgument importProductArgument, CatalogImportPolicy catalogImportPolicy, CommercePipelineExecutionContext context)
        {
            if (importProductArgument.Product.Components != null && importProductArgument.Product.Components.Any())
            {
                foreach (var productLevel in importProductArgument.Product.Components)
                {
                    catalogImportPolicy.Mappings.MapEntityChildComponent(
                        sellableItem, 
                        importProductArgument.Product, 
                        productLevel, 
                        context);
                }
            }
        }

        private void SetSellableItemVariantsComponents(SellableItem sellableItem, ImportProductArgument importProductArgument, CatalogImportPolicy catalogImportPolicy, CommercePipelineExecutionContext context)
        {
            if (importProductArgument.Product.Variants != null && importProductArgument.Product.Variants.Any())
            {
                foreach (var productVariant in importProductArgument.Product.Variants)
                {
                    var variantMapper = new ItemVariantionComponentMapper(
                        importProductArgument.Product,
                        productVariant,
                        sellableItem,
                        sellableItem.GetComponent<ItemVariationsComponent>(),
                        context);
                    var itemVariationComponent = variantMapper.Map();

                    if (importProductArgument.Product.VariantComponents != null && importProductArgument.Product.VariantComponents.Any())
                    {
                        foreach (var variantComponentName in importProductArgument.Product.VariantComponents)
                        {
                            catalogImportPolicy.Mappings.MapComponentChildComponent(
                                sellableItem,
                                itemVariationComponent,
                                importProductArgument.Product, 
                                productVariant, 
                                variantComponentName,
                                context);
                        }
                    }
                }


                var itemVariationsComponent = sellableItem.GetComponent<ItemVariationsComponent>();

                var orphanVariants = (from n in itemVariationsComponent.Variations
                    join o in importProductArgument.Product.Variants on n.Id equals o.Id into p
                    where !p.Any()
                    select n).ToList();


                foreach (var orphanVariant in orphanVariants)
                {
                    if (catalogImportPolicy.DeleteOrphanVariant)
                    {
                        itemVariationsComponent.ChildComponents.Remove(orphanVariant);
                    }
                    else
                    {
                        orphanVariant.Disabled = true;
                    }
                }
            }
        }
    }
}