using Plugin.Accelerator.CatalogImport.Framework.Mappers;
using Plugin.Accelerator.CatalogImport.Sample.Entity;
using Sitecore.Commerce.Core;

namespace Plugin.Accelerator.CatalogImport.Sample.EntityMappers
{
    public class SourceCatalogEntityMapper : BaseEntityMapper<SourceCatalog, Sitecore.Commerce.Plugin.Catalog.Catalog>
    {
        public SourceCatalogEntityMapper(SourceCatalog sourceCatalog, Sitecore.Commerce.Plugin.Catalog.Catalog catalog, CommercePipelineExecutionContext context)
            : base(sourceCatalog, catalog, context)
        {
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