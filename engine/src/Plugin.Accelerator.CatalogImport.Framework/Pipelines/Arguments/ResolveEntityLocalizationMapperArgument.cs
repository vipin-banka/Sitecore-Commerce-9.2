using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Sitecore.Commerce.Core;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments
{
    public class ResolveEntityLocalizationMapperArgument : PipelineArgument
    {
        public ResolveEntityLocalizationMapperArgument(ImportEntityArgument importEntityArgument, ILanguageEntity languageEntity)
        {
            this.ImportEntityArgument = importEntityArgument;
            this.LanguageEntity = languageEntity;
        }

        public ImportEntityArgument ImportEntityArgument { get; set; }

        public ILanguageEntity LanguageEntity { get; set; }
    }
}