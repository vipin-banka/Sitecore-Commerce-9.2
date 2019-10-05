using Plugin.Accelerator.EntityRelationships.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Accelerator.EntityRelationships.Pipelines
{
    [PipelineDisplayName(Constants.RefreshRelationshipsPipeline)]
    public interface IRefreshRelationshipsPipeline : IPipeline<RefreshRelationshipsArgument, RefreshRelationshipsArgument, CommercePipelineExecutionContext>
    {
    }
}