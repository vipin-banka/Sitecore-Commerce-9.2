using Plugin.Accelerator.CatalogImport.Framework.ImportHandlers;
using Plugin.Accelerator.CatalogImport.Sample.Entity;
using Sitecore.Commerce.Core;

namespace Plugin.Accelerator.CatalogImport.Sample.EntityImportHandlers
{
    public class SourceCatalogImportHandler : CatalogImportHandler<SourceCatalog>
    {
        public SourceCatalogImportHandler(string sourceCatalog, CommercePipelineExecutionContext context)
            : base(sourceCatalog, context)
        {
        }

        protected override void Initialize()
        {
            this.Name = this.SourceEntity.Name;
            this.DisplayName = this.SourceEntity.DisplayName;
        }

        public override void Map()
        {
            this.CommerceEntity.Name = this.SourceEntity.Name;
            this.CommerceEntity.DisplayName = this.SourceEntity.DisplayName;
        }

        protected override void MapLocalizeValues(Sitecore.Commerce.Plugin.Catalog.Catalog catalog)
        {
            catalog.DisplayName = this.SourceEntity.DisplayName;
        }
    }
}