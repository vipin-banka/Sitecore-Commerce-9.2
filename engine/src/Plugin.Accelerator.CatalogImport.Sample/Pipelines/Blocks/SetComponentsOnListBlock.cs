using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Accelerator.CatalogImport.Sample.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.SetComponentsOnListBlock)]
    public class SetComponentsOnListBlocks : PipelineBlock<CatalogContentArgument, CatalogContentArgument, CommercePipelineExecutionContext>
    {
        private ISetComponentsPipeline _setComponentsPipeline;
        public SetComponentsOnListBlocks(ISetComponentsPipeline setComponentsPipeline)
        {
            this._setComponentsPipeline = setComponentsPipeline;
        }

        public override async Task<CatalogContentArgument> Run(CatalogContentArgument arg, CommercePipelineExecutionContext context)
        {
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