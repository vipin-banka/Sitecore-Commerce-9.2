using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines
{
    [PipelineDisplayName(Constants.ResolveEntityMapperPipeline)]
    public interface IResolveEntityMapperPipeline : IPipeline<ResolveEntityMapperArgument, IEntityMapper, CommercePipelineExecutionContext>
    {
    }
}