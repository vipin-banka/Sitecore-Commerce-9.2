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

        protected override string Id => this.SourceEntity.Id;

        protected override void Initialize()
        {
            this.Name = this.SourceEntity.Name;
            this.DisplayName = this.SourceEntity.DisplayName;
            this.Description = this.SourceEntity.Description;
        }

        public override IList<string> GetParentList()
        {
            return this.SourceEntity.Parents;
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