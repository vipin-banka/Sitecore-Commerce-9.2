using Plugin.Accelerator.CatalogImport.Framework.Extensions;
using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.GetLocalizationEntityBlock)]
    public class GetLocalizationEntityBlock : PipelineBlock<ImportLocalizeContentArgument, ImportLocalizeContentArgument, CommercePipelineExecutionContext>
    {
        private readonly IFindEntityPipeline _findEntityPipeline;
        public GetLocalizationEntityBlock(IFindEntityPipeline findEntityPipeline)
        {
            this._findEntityPipeline = findEntityPipeline;
        }

        public override async Task<ImportLocalizeContentArgument> Run(ImportLocalizeContentArgument arg, CommercePipelineExecutionContext context)
        {
            if (arg.CommerceEntity != null 
                && arg.HasLocalizationContent)
            {
                var localizationEntityId = arg.CommerceEntity.Id.ToLocalizationEntityId();
                var localizationEntity = await this._findEntityPipeline.Run(new FindEntityArgument(typeof(LocalizationEntity), localizationEntityId, arg.CommerceEntity.EntityVersion, false), context).ConfigureAwait(false);

                arg.LocalizationEntity = localizationEntity as LocalizationEntity;

            }

            return await Task.FromResult(arg);
        }
    }
}