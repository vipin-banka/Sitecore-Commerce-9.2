using Plugin.Accelerator.CatalogImport.Framework.ImportHandlers;
using Plugin.Accelerator.CatalogImport.Sample.Entity;
using System.Collections.Generic;
using System.Linq;
using Plugin.Accelerator.CatalogImport.Framework.Abstractions;

namespace Plugin.Accelerator.CatalogImport.Sample.ImportHandlers
{
    public class SourceCategoryImportHandler : CategoryImportHandler<SourceCategory>
    {
        public SourceCategoryImportHandler(string sourceCategory)
            : base(sourceCategory)
        {
        }

        protected override void Initialize()
        {
            this.Name = this.SourceEntity.Name;
            this.DisplayName = this.SourceEntity.DisplayName;
            this.Description = this.SourceEntity.Description;
        }
    }
}