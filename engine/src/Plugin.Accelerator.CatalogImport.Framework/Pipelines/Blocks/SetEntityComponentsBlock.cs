using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Accelerator.CatalogImport.Framework.Extensions;
using Plugin.Accelerator.CatalogImport.Framework.Model;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.SetEntityComponentsBlock)]
    public class SetEntityComponentsBlock : PipelineBlock<CommerceEntity, CommerceEntity, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public SetEntityComponentsBlock(CommerceCommander commerceCommander)
        {
            this._commerceCommander = commerceCommander;
        }

        public override async Task<CommerceEntity> Run(CommerceEntity arg, CommercePipelineExecutionContext context)
        {
            var importEntityArgument = context.CommerceContext.GetObject<ImportEntityArgument>();
            if (importEntityArgument?.SourceEntity != null)
            {
                await this.SetCommerceEntityComponents(arg, importEntityArgument, context).ConfigureAwait(false);
            }

            return arg;
        }

        private async Task SetCommerceEntityComponents(CommerceEntity commerceEntity, ImportEntityArgument importEntityArgument,  CommercePipelineExecutionContext context)
        {
            if (importEntityArgument.SourceEntityDetail.Components != null && importEntityArgument.SourceEntityDetail.Components.Any())
            {
                foreach (var componentName in importEntityArgument.SourceEntityDetail.Components)
                {
                    var mapper = await this._commerceCommander.Pipeline<IResolveComponentMapperPipeline>()
                        .Run(new ResolveComponentMapperArgument(importEntityArgument, commerceEntity, componentName), context)
                        .ConfigureAwait(false);

                    if (mapper == null)
                    {
                        var message = new CommandMessage
                        {
                            Text =
                                $"Component mapper not found for {componentName}."
                        };
                        context.CommerceContext.AddMessage(message);
                    }
                    else
                    {
                        var component =  mapper.Execute(null);
                        component.SetComponentMetadataPolicy(componentName);
                    }
                }
            }
        }
    }
}