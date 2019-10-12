using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines
{
    [PipelineDisplayName(Constants.ResolveEntityLocalizationMapperPipeline)]
    public interface IResolveEntityLocalizationMapperPipeline : IPipeline<ResolveEntityLocalizationMapperArgument, IEntityLocalizationMapper, CommercePipelineExecutionContext>
    {
    }
}