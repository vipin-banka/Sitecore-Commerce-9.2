using Sitecore.Commerce.Core;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments
{
    public class ResolveEntityMapperArgument : PipelineArgument
    {
        public ResolveEntityMapperArgument(ImportEntityArgument importEntityArgument, CommerceEntity commerceEntity)
        {
            this.ImportEntityArgument = importEntityArgument;
            this.CommerceEntity = commerceEntity;
        }

        public ImportEntityArgument ImportEntityArgument { get; set; }

        public CommerceEntity CommerceEntity { get; set; }
    }
}