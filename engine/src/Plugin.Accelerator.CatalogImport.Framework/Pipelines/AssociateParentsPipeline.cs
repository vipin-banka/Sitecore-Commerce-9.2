using Microsoft.Extensions.Logging;
using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines
{
    public class AssociateParentsPipeline : CommercePipeline<ImportEntityArgument, ImportEntityArgument>, IAssociateParentsPipeline
    {
        public AssociateParentsPipeline(IPipelineConfiguration<IAssociateParentsPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}