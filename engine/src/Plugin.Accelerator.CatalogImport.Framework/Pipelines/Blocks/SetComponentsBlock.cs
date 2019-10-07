using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments;
using Plugin.Accelerator.CatalogImport.Framework.Policy;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Accelerator.CatalogImport.Framework.Abstractions;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.SetComponentsBlock)]
    public class SetComponentsBlocks : PipelineBlock<CommerceEntity, CommerceEntity, CommercePipelineExecutionContext>
    {
        public override Task<CommerceEntity> Run(CommerceEntity arg, CommercePipelineExecutionContext context)
        {
            var importEntityArgument = context.CommerceContext.GetObject<ImportEntityArgument>();
            if (importEntityArgument?.SourceEntity != null)
            {
                var catalogImportPolicy = context.CommerceContext.GetPolicy<CatalogImportPolicy>();

                this.SetCommerceEntityDetails(arg, importEntityArgument, catalogImportPolicy, context);
                this.SetCommerceEntityComponents(arg, importEntityArgument, catalogImportPolicy, context);
                this.SetSellableItemVariantsComponents(arg, importEntityArgument, catalogImportPolicy, context);
            }

            return Task.FromResult(arg);
        }

        private void SetCommerceEntityDetails(CommerceEntity commerceEntity, ImportEntityArgument importEntityArgument, CatalogImportPolicy catalogImportPolicy, CommercePipelineExecutionContext context)
        {
            if (!importEntityArgument.IsNew)
            {
                if (importEntityArgument.ImportHandler is IEntityMapper mapper)
                {
                    mapper.Map();
                }
                else
                {
                    catalogImportPolicy.Mappings.MapEntity(
                        commerceEntity,
                        importEntityArgument.SourceEntity,
                        context);
                }
            }
        }

        private void SetCommerceEntityComponents(CommerceEntity commerceEntity, ImportEntityArgument importEntityArgument, CatalogImportPolicy catalogImportPolicy, CommercePipelineExecutionContext context)
        {
            if (importEntityArgument.SourceEntityDetail.Components != null && importEntityArgument.SourceEntityDetail.Components.Any())
            {
                foreach (var componentName in importEntityArgument.SourceEntityDetail.Components)
                {
                    catalogImportPolicy.Mappings.MapEntityChildComponent(
                        commerceEntity,
                        importEntityArgument.SourceEntity,
                        componentName,
                        context);
                }
            }
        }

        private void SetSellableItemVariantsComponents(CommerceEntity commerceEntity, ImportEntityArgument importEntityArgument, CatalogImportPolicy catalogImportPolicy, CommercePipelineExecutionContext context)
        {
            if (!(commerceEntity is SellableItem))
                return;

            if (importEntityArgument.ImportHandler.HasVariants())
            {
                var variants = importEntityArgument.ImportHandler.GetVariants();

                foreach (var variant in variants)
                {
                    var itemVariationComponent = catalogImportPolicy.Mappings.MapItemVariationComponent(
                        commerceEntity,
                        commerceEntity.GetComponent<ItemVariationsComponent>(),
                        importEntityArgument.SourceEntity,
                        variant,
                        context);

                    if (importEntityArgument.SourceEntityDetail.VariantComponents != null
                        && importEntityArgument.SourceEntityDetail.VariantComponents.Any())
                    {
                        foreach (var variantComponentName in importEntityArgument.SourceEntityDetail.VariantComponents)
                        {
                            catalogImportPolicy.Mappings.MapComponentChildComponent(
                                commerceEntity,
                                itemVariationComponent,
                                importEntityArgument.SourceEntity,
                                variant,
                                variantComponentName,
                                context);
                        }
                    }
                }

                var itemVariationsComponent = commerceEntity.GetComponent<ItemVariationsComponent>();

                var orphanVariants = (from n in itemVariationsComponent.Variations
                                      join o in variants on n.Id equals o.Id into p
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