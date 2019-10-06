using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines
{
    [PipelineDisplayName(Constants.SetComponentsPipeline)]
    public interface ISetComponentsPipeline : IPipeline<CommerceEntity, CommerceEntity, CommercePipelineExecutionContext>
    {
    }
}