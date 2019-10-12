using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
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

        public override Task<CommerceEntity> Run(CommerceEntity arg, CommercePipelineExecutionContext context)
        {
            var importEntityArgument = context.CommerceContext.GetObject<ImportEntityArgument>();
            if (importEntityArgument?.SourceEntity != null)
            {
                this.SetCommerceEntityDetails(arg, importEntityArgument, context);
            }

            return Task.FromResult(arg);
        }

        private void SetCommerceEntityDetails(CommerceEntity commerceEntity, ImportEntityArgument importEntityArgument, CommercePipelineExecutionContext context)
        {
            if (!importEntityArgument.IsNew)
            {
                if (importEntityArgument.ImportHandler is IEntityMapper mapper)
                {
                    mapper.Map();
                }
                else
                {
                    importEntityArgument.CatalogImportPolicy.Mappings.MapEntity(
                        commerceEntity,
                        importEntityArgument.SourceEntity,
                        _commerceCommander,
                        context);
                }
            }
        }
    }
}