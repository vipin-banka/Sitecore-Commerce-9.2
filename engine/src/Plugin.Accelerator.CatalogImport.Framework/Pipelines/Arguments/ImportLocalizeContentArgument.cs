using Plugin.Accelerator.CatalogImport.Framework.Model;
using Sitecore.Commerce.Core;
using System.Collections.Generic;
using Plugin.Accelerator.CatalogImport.Framework.ImportHandlers;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments
{
    public class ImportLocalizeContentArgument : PipelineArgument
    {
        public ImportLocalizeContentArgument(string entityId, CommerceEntity commerceEntity)
        {
            this.EntityId = entityId;
            this.CommerceEntity = commerceEntity;
        }

        public string EntityId { get; set; }

        public CommerceEntity CommerceEntity { get; set; }

        public LocalizationEntity LocalizationEntity { get; set; }

        public IList<LocalizablePropertyValues> Properties { get; set; }

        public IList<LocalizableComponentPropertiesValues> ComponentsProperties { get; set; }

        public IImportHandler ImportHandler { get; set; }
    }
}