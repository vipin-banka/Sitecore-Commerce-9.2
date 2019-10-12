using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Accelerator.CatalogImport.Framework.Extensions;
using Plugin.Accelerator.CatalogImport.Framework.Model;

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

        public override async Task<CommerceEntity> Run(CommerceEntity arg, CommercePipelineExecutionContext context)
        {
            var importEntityArgument = context.CommerceContext.GetObject<ImportEntityArgument>();
            if (importEntityArgument?.SourceEntity != null)
            {
                await this.ImportVariants(arg, importEntityArgument, context)
                    .ConfigureAwait(false);
            }

            return arg;
        }

        private async Task ImportVariants(CommerceEntity commerceEntity, ImportEntityArgument importEntityArgument,  CommercePipelineExecutionContext context)
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
                    var itemVariantMapper = await this._commerceCommander.Pipeline<IResolveComponentMapperPipeline>()
                        .Run(
                            new ResolveComponentMapperArgument(importEntityArgument, commerceEntity,
                                itemVariationsComponent, variant, string.Empty), context).ConfigureAwait(false);

                                        var action = itemVariantMapper.GetComponentAction();
                    Component itemVariationComponent = itemVariantMapper.Execute(action);

                    if (action != ComponentAction.Remove
                        && importEntityArgument.SourceEntityDetail.VariantComponents != null
                        && importEntityArgument.SourceEntityDetail.VariantComponents.Any())
                    {
                        foreach (var variantComponentName in importEntityArgument.SourceEntityDetail.VariantComponents)
                        {
                            var itemVariantChildComponentMapper = await this._commerceCommander
                                .Pipeline<IResolveComponentMapperPipeline>().Run(new
                                    ResolveComponentMapperArgument(importEntityArgument, commerceEntity,
                                        itemVariationComponent, variant, variantComponentName), context)
                                .ConfigureAwait(false);

                            var childComponent = itemVariantChildComponentMapper.Execute();
                            childComponent.SetComponentMetadataPolicy(variantComponentName);
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