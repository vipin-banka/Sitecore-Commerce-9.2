using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.ValidateSourceEntityBlock)]
    public class ValidateSourceEntityBlock : PipelineBlock<ImportEntityArgument, ImportEntityArgument, CommercePipelineExecutionContext>
    {
        public override async Task<ImportEntityArgument> Run(ImportEntityArgument arg, CommercePipelineExecutionContext context)
        {
            if (!arg.ImportHandler.Validate())
            {
                context.Abort(await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().Error, "EntityValidationFailed", null, $"Entity validation failed, it cannot be imported. entityType={arg.SourceEntityDetail.EntityType}"), context);
            }

            return arg;
        }
    }
}