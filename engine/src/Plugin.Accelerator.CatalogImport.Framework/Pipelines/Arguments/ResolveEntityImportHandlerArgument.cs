using Sitecore.Commerce.Core;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments
{
    public class ResolveEntityImportHandlerArgument : PipelineArgument
    {
        public ResolveEntityImportHandlerArgument(ImportEntityArgument importEntityArgument)
        {
            this.ImportEntityArgument = importEntityArgument;
        }

        public ImportEntityArgument ImportEntityArgument { get; set; }
    }
}