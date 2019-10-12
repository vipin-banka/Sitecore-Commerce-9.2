using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.ResolveEntityLocalizationMapperBlock)]
    public class ResolveEntityLocalizationMapperBlock : PipelineBlock<ResolveEntityLocalizationMapperArgument, IEntityLocalizationMapper, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public ResolveEntityLocalizationMapperBlock(
            CommerceCommander commerceCommander)
        {
            this._commerceCommander = commerceCommander;
        }

        public override Task<IEntityLocalizationMapper> Run(ResolveEntityLocalizationMapperArgument arg, CommercePipelineExecutionContext context)
        {
            var mapper = arg.ImportEntityArgument.CatalogImportPolicy.Mappings.GetEntityLocalizationMapper(
                arg.LanguageEntity,
                arg.ImportEntityArgument,
                _commerceCommander,
                context);

            return Task.FromResult(mapper);
        }
    }
}