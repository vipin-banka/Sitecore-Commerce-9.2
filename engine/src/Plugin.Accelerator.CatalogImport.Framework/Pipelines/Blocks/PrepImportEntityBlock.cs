using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments;
using Plugin.Accelerator.CatalogImport.Framework.Policy;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.PrepImportEntityBlock)]
    public class PrepImportEntityBlock : PipelineBlock<ImportEntityArgument, ImportEntityArgument, CommercePipelineExecutionContext>
    {
        public override Task<ImportEntityArgument> Run(ImportEntityArgument arg, CommercePipelineExecutionContext context)
        {
            context.CommerceContext.AddUniqueObject(arg);

            var catalogImportPolicy = context.CommerceContext.GetPolicy<CatalogImportPolicy>();

            if (catalogImportPolicy == null)
            {
                context.Abort("Catalog import policy not found.", context);
            }

            arg.CatalogImportPolicy = catalogImportPolicy;

            return Task.FromResult(arg);
        }
    }
}