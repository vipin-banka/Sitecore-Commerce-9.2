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

        protected override string Id => this.SourceEntity.Id;

        protected override void Initialize()
        {
            this.Name = this.SourceEntity.Name;
            this.DisplayName = this.SourceEntity.DisplayName;
        }

        public override bool HasLanguages()
        {
            return this.SourceEntity.Languages != null
                   && this.SourceEntity.Languages.Any();
        }

        public override IList<ILanguageEntity> GetLanguages()
        {
            return this.SourceEntity.Languages.Select(x => x as ILanguageEntity).ToList();
        }
    }
}