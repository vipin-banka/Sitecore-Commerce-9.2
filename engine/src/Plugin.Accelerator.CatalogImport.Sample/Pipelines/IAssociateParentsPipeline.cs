using Plugin.Accelerator.CatalogImport.Sample.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Accelerator.CatalogImport.Sample.Pipelines
{
    [PipelineDisplayName(Constants.AssociateParentsPipeline)]
    public interface IAssociateParentsPipeline : IPipeline<AssociateParentsArgument, AssociateParentsArgument, CommercePipelineExecutionContext>
    {
    }
}