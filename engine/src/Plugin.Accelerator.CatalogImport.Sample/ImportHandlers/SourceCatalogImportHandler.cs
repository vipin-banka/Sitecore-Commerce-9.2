using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Plugin.Accelerator.CatalogImport.Framework.ImportHandlers;
using Plugin.Accelerator.CatalogImport.Sample.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Plugin.Accelerator.CatalogImport.Sample.ImportHandlers
{
    public class SourceCatalogImportHandler : CatalogImportHandler<SourceCatalog>
    {
        public SourceCatalogImportHandler(string sourceCatalog)
            : base(sourceCatalog)
        {
        }

        protected override void Initialize()
        {
            this.Name = this.SourceEntity.Name;
            this.DisplayName = this.SourceEntity.DisplayName;
        }
    }
}