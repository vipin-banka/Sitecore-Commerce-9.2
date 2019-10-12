using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;
using Plugin.Accelerator.CatalogImport.Framework.Abstractions;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.ResolveEntityImportHandlerBlock)]
    public class ResolveEntityImportHandlerBlock : PipelineBlock<ResolveEntityImportHandlerArgument, IEntityImportHandler, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public ResolveEntityImportHandlerBlock(
            CommerceCommander commerceCommander)
        {
            this._commerceCommander = commerceCommander;
        }

        public override Task<IEntityImportHandler> Run(ResolveEntityImportHandlerArgument arg, CommercePipelineExecutionContext context)
        {
            var importHandler = arg.ImportEntityArgument.CatalogImportPolicy.Mappings.GetImportHandler(arg.ImportEntityArgument, _commerceCommander, context);

            return Task.FromResult(importHandler);
        }
    }
}