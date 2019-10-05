using Plugin.Accelerator.CatalogImport.Sample.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;

namespace Plugin.Accelerator.CatalogImport.Sample.Pipelines
{
    [PipelineDisplayName(Constants.ImportProductPipeline)]
    public interface IImportProductPipeline: IPipeline<ImportProductArgument, SellableItem, CommercePipelineExecutionContext>
    {
    }
}