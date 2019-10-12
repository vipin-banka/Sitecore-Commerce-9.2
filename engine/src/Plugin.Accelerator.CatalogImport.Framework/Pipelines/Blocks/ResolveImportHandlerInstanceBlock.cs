using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.ResolveImportHandlerInstanceBlock)]
    public class ResolveImportHandlerInstanceBlock : PipelineBlock<ImportEntityArgument, ImportEntityArgument, CommercePipelineExecutionContext>
    {
        private CommerceCommander _commerceCommander;

        public ResolveImportHandlerInstanceBlock(
            CommerceCommander commerceCommander)
        {
            this._commerceCommander = commerceCommander;
        }

        public override Task<ImportEntityArgument> Run(ImportEntityArgument arg, CommercePipelineExecutionContext context)
        {
            var importHandler = arg.CatalogImportPolicy.Mappings.GetImportHandlerInstance(arg, _commerceCommander, context);

            if (importHandler == null)
            {
                context.Abort("Import handler instance not resolved.", context);
            }

            arg.ImportHandler = importHandler;
            return Task.FromResult(arg);
        }
    }
}