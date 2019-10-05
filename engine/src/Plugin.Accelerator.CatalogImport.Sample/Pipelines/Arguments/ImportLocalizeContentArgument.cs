using Plugin.Accelerator.CatalogImport.Framework.Model;
using Plugin.Accelerator.CatalogImport.Sample.Entity;
using Sitecore.Commerce.Core;
using System.Collections.Generic;

namespace Plugin.Accelerator.CatalogImport.Sample.Pipelines.Arguments
{
    public class ImportLocalizeContentArgument : PipelineArgument
    {
        public ImportLocalizeContentArgument(string entityId, Product product, CommerceEntity commerceEntity)
        {
            this.Product = product;
            this.EntityId = entityId;
            this.CommerceEntity = commerceEntity;
        }

        public string EntityId { get; set; }

        public Product Product { get; set; }

        public CommerceEntity CommerceEntity { get; set; }

        public LocalizationEntity LocalizationEntity { get; set; }

        public IList<LocalizablePropertyValues> Properties { get; set; }

        public IList<LocalizableComponentPropertiesValues> ComponentsProperties { get; set; }
    }
}