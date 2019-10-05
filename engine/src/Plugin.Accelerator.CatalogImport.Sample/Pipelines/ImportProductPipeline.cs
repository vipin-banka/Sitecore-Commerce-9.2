using Microsoft.Extensions.Logging;
using Plugin.Accelerator.CatalogImport.Sample.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;

namespace Plugin.Accelerator.CatalogImport.Sample.Pipelines
{
    public class ImportProductPipeline : CommercePipeline<ImportProductArgument, SellableItem>, IImportProductPipeline
    {
        public ImportProductPipeline(IPipelineConfiguration<IImportProductPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}