﻿using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.UpdateEntityBlock)]
    public class UpdateEntityBlock : PipelineBlock<CommerceEntity, CommerceEntity, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public UpdateEntityBlock(CommerceCommander commerceCommander)
        {
            this._commerceCommander = commerceCommander;
        }

        public override async Task<CommerceEntity> Run(CommerceEntity arg, CommercePipelineExecutionContext context)
        {
            var importEntityArgument = context.CommerceContext.GetObject<ImportEntityArgument>();
            if (importEntityArgument?.SourceEntity != null)
            {
                await this.SetCommerceEntityDetails(arg, importEntityArgument, context).ConfigureAwait(false);
            }

            return arg;
        }

        private async Task SetCommerceEntityDetails(CommerceEntity commerceEntity, ImportEntityArgument importEntityArgument, CommercePipelineExecutionContext context)
        {
            if (!importEntityArgument.IsNew)
            {
                if (importEntityArgument.ImportHandler is IEntityMapper mapper)
                {
                    mapper.Map();
                }
                else
                {
                    mapper = await this._commerceCommander.Pipeline<IResolveEntityMapperPipeline>()
                        .Run(new ResolveEntityMapperArgument(importEntityArgument, commerceEntity), context)
                        .ConfigureAwait(false);

                    if (mapper == null)
                    {
                        await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().Warning, "EntityMapperMissing", null, $"Entity mapper instance for entityType={importEntityArgument.SourceEntityDetail.EntityType} not resolved.");
                    }
                    else
                    {
                        mapper.Map();
                    }
                }
            }
        }
    }
}