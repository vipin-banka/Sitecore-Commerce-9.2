using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Sitecore.Commerce.Core;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments
{
    public class ResolveComponentLocalizationMapperArgument : PipelineArgument
    {
        public ResolveComponentLocalizationMapperArgument(ImportEntityArgument importEntityArgument,
            CommerceEntity commerceEntity, Component component, ILanguageEntity languageEntity)
        : this(importEntityArgument, commerceEntity, component, languageEntity, null)
        {
        }
        public ResolveComponentLocalizationMapperArgument(ImportEntityArgument importEntityArgument, CommerceEntity commerceEntity, Component component, ILanguageEntity languageEntity, IEntity sourceComponent)
        {
            this.ImportEntityArgument = importEntityArgument;
            this.LanguageEntity = languageEntity;
            this.CommerceEntity = commerceEntity;
            this.Component = component;
            this.SourceComponent = sourceComponent;
        }

        public ImportEntityArgument ImportEntityArgument { get; set; }

        public ILanguageEntity LanguageEntity { get; set; }

        public CommerceEntity CommerceEntity { get; set; }

        public Component Component { get; set; }

        public IEntity SourceComponent { get; set; }
    }
}