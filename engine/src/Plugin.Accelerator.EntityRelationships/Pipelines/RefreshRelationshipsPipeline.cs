using Microsoft.Extensions.Logging;
using Plugin.Accelerator.EntityRelationships.Pipelines;
using Plugin.Accelerator.EntityRelationships.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Accelerator.EntityRelationships.Pipelines
{
    public class RefreshRelationshipsPipeline : CommercePipeline<RefreshRelationshipsArgument, RefreshRelationshipsArgument>, IRefreshRelationshipsPipeline
    {
        public RefreshRelationshipsPipeline(IPipelineConfiguration<IRefreshRelationshipsPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}