using Plugin.Accelerator.CatalogImport.Framework.Mappers;
using Plugin.Accelerator.CatalogImport.Sample.Entity;
using Sitecore.Commerce.Core;

namespace Plugin.Accelerator.CatalogImport.Sample.EntityMappers
{
    public class SourceCategoryEntityMapper : BaseEntityMapper<SourceCategory, Sitecore.Commerce.Plugin.Catalog.Category>
    {
        public SourceCategoryEntityMapper(SourceCategory sourceCategory, Sitecore.Commerce.Plugin.Catalog.Category category, CommercePipelineExecutionContext context)
            : base(sourceCategory, category, context)
        {
        }

        public override void Map()
        {
            this.CommerceEntity.Name = this.SourceEntity.Name;
            this.CommerceEntity.DisplayName = this.SourceEntity.DisplayName;
            this.CommerceEntity.Description = this.SourceEntity.Description;
        }

        protected override void MapLocalizeValues(Sitecore.Commerce.Plugin.Catalog.Category category)
        {
            category.DisplayName = this.SourceEntity.DisplayName;
            category.Description = this.SourceEntity.Description;
        }
    }
}