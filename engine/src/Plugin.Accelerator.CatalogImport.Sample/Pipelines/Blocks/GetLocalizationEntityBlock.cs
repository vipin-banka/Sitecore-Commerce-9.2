using Plugin.Accelerator.CatalogImport.Sample.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;

namespace Plugin.Accelerator.CatalogImport.Sample.Pipelines.Blocks
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
            var commerceEntity = await this._findEntityPipeline.Run(new FindEntityArgument(typeof(CommerceEntity), arg.EntityId, false), context).ConfigureAwait(false);

            ////if (commerceEntity != null && commerceEntity.HasComponent<LocalizedEntityComponent>())
            ////{
            ////    var component = commerceEntity.GetComponent<LocalizedEntityComponent>();
            ////    if (component != null && component.Entity != null && !string.IsNullOrEmpty(component.Entity.EntityTarget))
            ////    {
            ////        var localizationEntityId = commerceEntity.GetComponent<LocalizedEntityComponent>().Entity.EntityTarget;
            ////        var localizationEntity = await this._findEntityPipeline.Run(new FindEntityArgument(typeof(LocalizationEntity), localizationEntityId, false), context).ConfigureAwait(false);

            ////        arg.LocalizationEntity = localizationEntity as LocalizationEntity;
            ////    }                
            ////}

            return await Task.FromResult(arg);
        }
    }
}