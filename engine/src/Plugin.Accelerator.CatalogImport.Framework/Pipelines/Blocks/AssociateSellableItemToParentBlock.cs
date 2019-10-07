using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.AssociateSellableItemToParentBlock)]
    public class AssociateSellableItemToParentBlock : PipelineBlock<ImportEntityArgument, ImportEntityArgument, CommercePipelineExecutionContext>
    {
        private readonly AssociateSellableItemToParentCommand _associateSellableItemToParent;

        public AssociateSellableItemToParentBlock(AssociateSellableItemToParentCommand associateSellableItemToParent)
        {
            this._associateSellableItemToParent = associateSellableItemToParent;
        }

        public override async Task<ImportEntityArgument> Run(ImportEntityArgument arg, CommercePipelineExecutionContext context)
        {
            var commerceEntity = arg.ImportHandler.GetCommerceEntity();
            string entityId = commerceEntity.Id;

            if (arg.ImportHandler.ParentEntityIds == null
                || !arg.ImportHandler.ParentEntityIds.Any()
                || !(commerceEntity is SellableItem))
            {
                return await Task.FromResult(arg);
            }

            foreach (var catalog in arg.ImportHandler.ParentEntityIds)
            {
                if (catalog.Value == null || !catalog.Value.Any())
                {
                    var r = await this._associateSellableItemToParent
                    .Process(context.CommerceContext, catalog.Key, catalog.Key, entityId).ConfigureAwait(false);
                }
                else
                {
                    foreach (var parentId in catalog.Value)
                    {
                        var r = await this._associateSellableItemToParent
                        .Process(context.CommerceContext, catalog.Key, parentId, entityId).ConfigureAwait(false);
                    }
                }
            }

            return await Task.FromResult(arg);
        }
    }
}