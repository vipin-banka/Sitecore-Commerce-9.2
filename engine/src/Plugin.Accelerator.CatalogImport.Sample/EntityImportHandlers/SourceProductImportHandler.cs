using Plugin.Accelerator.CatalogImport.Framework.ImportHandlers;
using Plugin.Accelerator.CatalogImport.Sample.Entity;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;

namespace Plugin.Accelerator.CatalogImport.Sample.EntityImportHandlers
{
    public class SourceProductImportHandler : SellableItemImportHandler<SourceProduct>
    {
        public SourceProductImportHandler(string sourceProduct, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
            : base(sourceProduct, commerceCommander, context)
        {
        }

        protected override void Initialize()
        {
            this.ProductId = this.SourceEntity.Id;
            this.Name = this.SourceEntity.Name;
            this.DisplayName = this.SourceEntity.DisplayName;
            this.Description = this.SourceEntity.Description;
            this.Brand = string.Empty;
            this.Manufacturer = string.Empty;
            this.TypeOfGood = string.Empty;
        }
        
        public override void Map()
        {
            this.CommerceEntity.Name = this.SourceEntity.Name;
            this.CommerceEntity.DisplayName = this.SourceEntity.DisplayName;
            this.CommerceEntity.Description = this.SourceEntity.Description;
        }

        protected override void MapLocalizeValues(SourceProduct localizedSourceEntity, SellableItem localizedTargetEntity)
        {
            localizedTargetEntity.DisplayName = localizedSourceEntity.DisplayName;
            localizedTargetEntity.Description = localizedSourceEntity.Description;
        }
    }
}