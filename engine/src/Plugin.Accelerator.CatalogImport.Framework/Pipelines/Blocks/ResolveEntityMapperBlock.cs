using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.ResolveEntityMapperBlock)]
    public class ResolveEntityMapperBlock : PipelineBlock<ResolveEntityMapperArgument, IEntityMapper, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public ResolveEntityMapperBlock(
            CommerceCommander commerceCommander)
        {
            this._commerceCommander = commerceCommander;
        }

        public override Task<IEntityMapper> Run(ResolveEntityMapperArgument arg, CommercePipelineExecutionContext context)
        {
            var mapper = arg.ImportEntityArgument.CatalogImportPolicy.Mappings.GetEntityMapper(arg.CommerceEntity, arg.ImportEntityArgument, _commerceCommander, context);

            return Task.FromResult(mapper);
        }
    }
}