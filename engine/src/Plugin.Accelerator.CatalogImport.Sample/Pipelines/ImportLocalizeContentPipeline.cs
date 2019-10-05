using Microsoft.Extensions.Logging;
using Plugin.Accelerator.CatalogImport.Sample.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Accelerator.CatalogImport.Sample.Pipelines
{
    public class ImportLocalizeContentPipeline : CommercePipeline<ImportLocalizeContentArgument, ImportLocalizeContentArgument>, IImportLocalizeContentPipeline
    {
        public ImportLocalizeContentPipeline(IPipelineConfiguration<IImportLocalizeContentPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}