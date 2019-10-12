using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Linq;
using System.Threading.Tasks;

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

        public override Task<CommerceEntity> Run(CommerceEntity arg, CommercePipelineExecutionContext context)
        {
            var importEntityArgument = context.CommerceContext.GetObject<ImportEntityArgument>();
            if (importEntityArgument?.SourceEntity != null)
            {
                this.SetCommerceEntityComponents(arg, importEntityArgument, context);
            }

            return Task.FromResult(arg);
        }

        private void SetCommerceEntityComponents(CommerceEntity commerceEntity, ImportEntityArgument importEntityArgument,  CommercePipelineExecutionContext context)
        {
            if (importEntityArgument.SourceEntityDetail.Components != null && importEntityArgument.SourceEntityDetail.Components.Any())
            {
                foreach (var componentName in importEntityArgument.SourceEntityDetail.Components)
                {
                    importEntityArgument.CatalogImportPolicy.Mappings.MapEntityChildComponent(
                        commerceEntity,
                        importEntityArgument.SourceEntity,
                        componentName,
                        _commerceCommander,
                        context);
                }
            }
        }
    }
}