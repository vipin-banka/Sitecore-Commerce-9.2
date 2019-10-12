using Microsoft.Extensions.Logging;
using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines
{
    public class ResolveEntityMapperPipeline : CommercePipeline<ResolveEntityMapperArgument, IEntityMapper>, IResolveEntityMapperPipeline
    {
        public ResolveEntityMapperPipeline(IPipelineConfiguration<IResolveEntityMapperPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}