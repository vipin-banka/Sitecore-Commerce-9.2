using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.SetComponentsOnCatalogEntityBlock)]
    public class SetComponentsOnCatalogEntityBlock : PipelineBlock<CatalogContentArgument, CatalogContentArgument, CommercePipelineExecutionContext>
    {
        private ISetComponentsPipeline _setComponentsPipeline;
        public SetComponentsOnCatalogEntityBlock(ISetComponentsPipeline setComponentsPipeline)
        {
            this._setComponentsPipeline = setComponentsPipeline;
        }

        public override async Task<CatalogContentArgument> Run(CatalogContentArgument arg, CommercePipelineExecutionContext context)
        {
            if (arg != null && arg.Catalog != null)
            {
                await this._setComponentsPipeline.Run(arg.Catalog, context).ConfigureAwait(false);
            }

            if (arg != null && arg.Categories != null && arg.Categories.Any())
            {
                foreach (var category in arg.Categories)
                {
                    await this._setComponentsPipeline.Run(category, context).ConfigureAwait(false);
                }
            }

            if (arg != null && arg.SellableItems != null && arg.SellableItems.Any())
            {
                foreach(var sellableItem in arg.SellableItems)
                {
                    await this._setComponentsPipeline.Run(sellableItem, context).ConfigureAwait(false);
                }
            }

            return await Task.FromResult(arg);
        }
    }
}