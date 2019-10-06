using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines
{
    [PipelineDisplayName(Constants.AssociateParentsPipeline)]
    public interface IAssociateParentsPipeline : IPipeline<ImportEntityArgument, ImportEntityArgument, CommercePipelineExecutionContext>
    {
    }
}