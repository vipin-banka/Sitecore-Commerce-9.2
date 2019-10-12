using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;
using Sitecore.Commerce.Plugin.Catalog;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.ResolveComponentMapperBlock)]
    public class ResolveComponentMapperBlock : PipelineBlock<ResolveComponentMapperArgument, IComponentMapper, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public ResolveComponentMapperBlock(CommerceCommander commerceCommander)
        {
            this._commerceCommander = commerceCommander;
        }

        public override Task<IComponentMapper> Run(ResolveComponentMapperArgument arg, CommercePipelineExecutionContext context)
        {
            IComponentMapper mapper = null;
            if (arg.ParenComponent == null)
            {
                mapper = arg.ImportEntityArgument.CatalogImportPolicy.Mappings.GetEntityComponentMapper(
                    arg.CommerceEntity,
                    arg.ImportEntityArgument,
                    arg.ComponentName,
                    _commerceCommander,
                    context);
            }
            else if (arg.ParenComponent.GetType() == typeof(ItemVariationsComponent))
            {
                mapper = arg.ImportEntityArgument.CatalogImportPolicy.Mappings.GetItemVariationComponentMapper(
                    arg.CommerceEntity,
                    arg.ParenComponent,
                    arg.ImportEntityArgument,
                    arg.SourceComponent,
                    _commerceCommander,
                    context);
            }
            else
            {
                mapper = arg.ImportEntityArgument.CatalogImportPolicy.Mappings.GetComponentChildComponentMapper(
                    arg.CommerceEntity,
                    arg.ParenComponent,
                    arg.ImportEntityArgument,
                    arg.SourceComponent,
                    arg.ComponentName,
                    _commerceCommander,
                    context);
            }

            return Task.FromResult(mapper);
        }
    }
}