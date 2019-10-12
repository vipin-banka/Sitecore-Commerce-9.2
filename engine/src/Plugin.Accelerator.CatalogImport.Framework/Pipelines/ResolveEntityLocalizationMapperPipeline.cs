using Microsoft.Extensions.Logging;
using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines
{
    public class ResolveEntityLocalizationMapperPipeline : CommercePipeline<ResolveEntityLocalizationMapperArgument, IEntityLocalizationMapper>, IResolveEntityLocalizationMapperPipeline
    {
        public ResolveEntityLocalizationMapperPipeline(IPipelineConfiguration<IResolveEntityLocalizationMapperPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}