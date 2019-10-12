using Plugin.Accelerator.CatalogImport.Framework.Model;
using Sitecore.Commerce.Core;
using System.Collections.Generic;
using System.Linq;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments
{
    public class ImportLocalizeContentArgument : PipelineArgument
    {
        public ImportLocalizeContentArgument(CommerceEntity commerceEntity, ImportEntityArgument importEntityArgument)
        {
            this.CommerceEntity = commerceEntity;
            this.ImportEntityArgument = importEntityArgument;
        }

        public CommerceEntity CommerceEntity { get; set; }

        public LocalizationEntity LocalizationEntity { get; set; }

        public IList<LocalizablePropertyValues> Properties { get; set; }

        public IList<LocalizableComponentPropertiesValues> ComponentsProperties { get; set; }

        public ImportEntityArgument ImportEntityArgument { get; set; }

        public bool HasLocalizationContent => (this.Properties != null && this.Properties.Any()) 
        || (this.ComponentsProperties != null && this.ComponentsProperties.Any());
    }
}