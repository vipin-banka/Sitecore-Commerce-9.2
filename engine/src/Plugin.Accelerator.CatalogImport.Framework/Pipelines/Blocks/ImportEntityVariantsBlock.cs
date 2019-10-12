using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.ImportEntityVariantsBlock)]
    public class ImportEntityVariantsBlock : PipelineBlock<CommerceEntity, CommerceEntity, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public ImportEntityVariantsBlock(CommerceCommander commerceCommander)
        {
            this._commerceCommander = commerceCommander;
        }

        public override Task<CommerceEntity> Run(CommerceEntity arg, CommercePipelineExecutionContext context)
        {
            var importEntityArgument = context.CommerceContext.GetObject<ImportEntityArgument>();
            if (importEntityArgument?.SourceEntity != null)
            {
                this.ImportVariants(arg, importEntityArgument, context);
            }

            return Task.FromResult(arg);
        }

        private void ImportVariants(CommerceEntity commerceEntity, ImportEntityArgument importEntityArgument,  CommercePipelineExecutionContext context)
        {
            var orphanVariants = new List<ItemVariationComponent>();
            ItemVariationsComponent itemVariationsComponent = null;
            var sourceEntityHasVariants = importEntityArgument.ImportHandler.HasVariants();
            if (!sourceEntityHasVariants
                && commerceEntity.HasComponent<ItemVariationsComponent>())
            {
                itemVariationsComponent = commerceEntity.GetComponent<ItemVariationsComponent>();
                if (itemVariationsComponent.Variations != null
                    && itemVariationsComponent.Variations.Any())
                {
                    orphanVariants = itemVariationsComponent.Variations;
                }
            }

            if (sourceEntityHasVariants)
            {
                itemVariationsComponent =
                    itemVariationsComponent ?? commerceEntity.GetComponent<ItemVariationsComponent>();

                var variants = importEntityArgument.ImportHandler.GetVariants();

                foreach (var variant in variants)
                {
                    var itemVariationComponent = importEntityArgument.CatalogImportPolicy.Mappings.MapItemVariationComponent(
                        commerceEntity,
                        itemVariationsComponent,
                        importEntityArgument.SourceEntity,
                        variant,
                        _commerceCommander,
                        context);

                    if (importEntityArgument.SourceEntityDetail.VariantComponents != null
                        && importEntityArgument.SourceEntityDetail.VariantComponents.Any())
                    {
                        foreach (var variantComponentName in importEntityArgument.SourceEntityDetail.VariantComponents)
                        {
                            importEntityArgument.CatalogImportPolicy.Mappings.MapComponentChildComponent(
                                commerceEntity,
                                itemVariationComponent,
                                importEntityArgument.SourceEntity,
                                variant,
                                variantComponentName,
                                _commerceCommander,
                                context);
                        }
                    }
                }

                orphanVariants = (from n in itemVariationsComponent.Variations
                                  join o in variants on n.Id equals o.Id into p
                                  where !p.Any()
                                  select n).ToList();
            }

            if (orphanVariants != null
            && orphanVariants.Any())
            {
                foreach (var orphanVariant in orphanVariants)
                {
                    if (importEntityArgument.CatalogImportPolicy.DeleteOrphanVariant)
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