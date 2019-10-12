using Sitecore.Commerce.Core;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments
{
    public class ResolveComponentMapperArgument : PipelineArgument
    {
        public ResolveComponentMapperArgument(ImportEntityArgument importEntityArgument, CommerceEntity commerceEntity, string componentName)
            : this(importEntityArgument, commerceEntity, null, null, componentName)
        {
        }

        public ResolveComponentMapperArgument(ImportEntityArgument importEntityArgument, CommerceEntity commerceEntity, Component parentComponent, object sourceComponent, string componentName)
        {
            this.ImportEntityArgument = importEntityArgument;
            this.CommerceEntity = commerceEntity;
            this.ComponentName = componentName;
            this.ParenComponent = parentComponent;
            this.SourceComponent = sourceComponent;
        }

        public ImportEntityArgument ImportEntityArgument { get; set; }

        public CommerceEntity CommerceEntity { get; set; }

        public string ComponentName { get; set; }

        public Component ParenComponent { get; set; }

        public object SourceComponent { get; set; }
    }
}