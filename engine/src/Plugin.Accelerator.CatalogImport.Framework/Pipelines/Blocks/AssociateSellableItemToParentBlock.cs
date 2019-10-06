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
            string entityId = arg.CommerceEntity.Id;

            if (arg.Parents == null
                || !arg.Parents.Any()
                || !(arg.CommerceEntity is SellableItem))
            {
                return await Task.FromResult(arg);
            }

            foreach (var catalog in arg.Parents)
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