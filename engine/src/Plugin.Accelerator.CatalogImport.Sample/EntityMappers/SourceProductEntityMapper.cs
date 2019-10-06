using Plugin.Accelerator.CatalogImport.Framework.Mappers;
using Plugin.Accelerator.CatalogImport.Sample.Entity;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;

namespace Plugin.Accelerator.CatalogImport.Sample.EntityMappers
{
    public class SourceProductEntityMapper : BaseEntityMapper<SourceProduct, SellableItem>
    {
        public SourceProductEntityMapper(SourceProduct sourceProduct, SellableItem sellableItem, CommercePipelineExecutionContext context)
            : base(sourceProduct, sellableItem, context)
        {
        }

        public override void Map()
        {
            this.CommerceEntity.Name = this.SourceEntity.Name;
            this.CommerceEntity.DisplayName = this.SourceEntity.DisplayName;
            this.CommerceEntity.Description = this.SourceEntity.Description;
        }

        protected override void MapLocalizeValues(SellableItem sellableItem)
        {
            sellableItem.DisplayName = this.SourceEntity.DisplayName;
            sellableItem.Description = this.SourceEntity.Description;
        }
    }
}