using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;

namespace Plugin.Accelerator.CatalogImport.Sample.Pipelines
{
    public class SetComponentsPipeline : CommercePipeline<SellableItem, SellableItem>, ISetComponentsPipeline
    {
        public SetComponentsPipeline(IPipelineConfiguration<ISetComponentsPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}