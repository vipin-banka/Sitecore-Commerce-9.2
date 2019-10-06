using Plugin.Accelerator.CatalogImport.Framework.Model;
using Sitecore.Commerce.Core;
using System.Collections.Generic;
using System.Linq;
using Plugin.Accelerator.CatalogImport.Framework.ImportHandlers;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments
{
    public class ImportLocalizeContentArgument : PipelineArgument
    {
        public ImportLocalizeContentArgument(CommerceEntity commerceEntity, IImportHandler importHandler)
        {
            this.CommerceEntity = commerceEntity;
            this.ImportHandler = importHandler;
        }

        public CommerceEntity CommerceEntity { get; set; }

        public LocalizationEntity LocalizationEntity { get; set; }

        public IList<LocalizablePropertyValues> Properties { get; set; }

        public IList<LocalizableComponentPropertiesValues> ComponentsProperties { get; set; }

        public IImportHandler ImportHandler { get; set; }

        public bool HasLocalizationContent => (this.Properties != null && this.Properties.Any()) 
        || (this.ComponentsProperties != null && this.ComponentsProperties.Any());
    }
}