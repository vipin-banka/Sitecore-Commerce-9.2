using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;

namespace Plugin.Accelerator.CatalogImport.Sample.Pipelines
{
    [PipelineDisplayName(Constants.SetComponentsPipeline)]
    public interface ISetComponentsPipeline : IPipeline<SellableItem, SellableItem, CommercePipelineExecutionContext>
    {
    }
}
