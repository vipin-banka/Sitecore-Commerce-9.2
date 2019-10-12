﻿using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.ResolveImportHandlerInstanceBlock)]
    public class ResolveImportHandlerInstanceBlock : PipelineBlock<ImportEntityArgument, ImportEntityArgument, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public ResolveImportHandlerInstanceBlock(
            CommerceCommander commerceCommander)
        {
            this._commerceCommander = commerceCommander;
        }

        public override async Task<ImportEntityArgument> Run(ImportEntityArgument arg, CommercePipelineExecutionContext context)
        {
            var importHandler = await this._commerceCommander.Pipeline<IResolveEntityImportHandlerPipeline>()
                .Run(new ResolveEntityImportHandlerArgument(arg), context)
                .ConfigureAwait(false);

            if (importHandler == null)
            {
                context.Abort("Import handler instance not resolved.", context);
            }

            arg.ImportHandler = importHandler;
            return arg;
        }
    }
}