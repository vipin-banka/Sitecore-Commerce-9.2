using Plugin.Accelerator.CatalogImport.Framework.ImportHandlers;
using Plugin.Accelerator.CatalogImport.Sample.Entity;
using Sitecore.Commerce.Core;

namespace Plugin.Accelerator.CatalogImport.Sample.EntityImportHandlers
{
    public class SourceCategoryImportHandler : CategoryImportHandler<SourceCategory>
    {
        public SourceCategoryImportHandler(string sourceCategory, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
            : base(sourceCategory, commerceCommander, context)
        {
        }

        protected override void Initialize()
        {
            this.Name = this.SourceEntity.Name;
            this.DisplayName = this.SourceEntity.DisplayName;
            this.Description = this.SourceEntity.Description;
        }

        public override void Map()
        {
            this.CommerceEntity.Name = this.SourceEntity.Name;
            this.CommerceEntity.DisplayName = this.SourceEntity.DisplayName;
            this.CommerceEntity.Description = this.SourceEntity.Description;
        }

        protected override void MapLocalizeValues(SourceCategory localizedSourceEntity, Sitecore.Commerce.Plugin.Catalog.Category localizedTargetEntity)
        {
            localizedTargetEntity.DisplayName = localizedSourceEntity.DisplayName;
            localizedTargetEntity.Description = localizedSourceEntity.Description;
        }
    }
}