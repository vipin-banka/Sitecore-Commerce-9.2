using System;
using Plugin.Accelerator.CatalogImport.Framework.ImportHandlers;
using Plugin.Accelerator.CatalogImport.Sample.Entity;
using System.Collections.Generic;
using System.Linq;
using Plugin.Accelerator.CatalogImport.Framework.Abstractions;

namespace Plugin.Accelerator.CatalogImport.Sample.ImportHandlers
{
    public class SourceProductImportHandler : SellableItemImportHandler<SourceProduct>
    {
        public SourceProductImportHandler(string sourceProduct)
            : base(sourceProduct)
        {
        }

        protected override string Id => this.SourceEntity.Id;

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

        public override IList<string> GetParentList()
        {
            return this.SourceEntity.Parents;
        }

        public override bool HasVariants()
        {
            return this.SourceEntity.Variants != null && this.SourceEntity.Variants.Any();
        }

        public override IList<IEntity> GetVariants()
        {
            return this.SourceEntity.Variants.Select(x=>x as IEntity).ToList();
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

        public override bool HasVariants(ILanguageEntity languageEntity)
        {
            var matchedLanguage = this.SourceEntity.Languages.FirstOrDefault(x =>
                x.Language.Equals(languageEntity.Language, StringComparison.OrdinalIgnoreCase));
            if (matchedLanguage != null)
            {
                return matchedLanguage.Entity.Variants != null 
                       && !matchedLanguage.Entity.Variants.Any();
            }

            return false;
        }

        public override IList<IEntity> GetVariants(ILanguageEntity languageEntity)
        {
            var matchedLanguage = this.SourceEntity.Languages.FirstOrDefault(x =>
                x.Language.Equals(languageEntity.Language, StringComparison.OrdinalIgnoreCase));
            if (matchedLanguage != null)
            {
                return matchedLanguage.Entity.Variants.Select(x => x as IEntity).ToList();
            }

            return new List<IEntity>();
        }
    }
}