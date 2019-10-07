using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Accelerator.CatalogImport.Framework.Extensions;
using Plugin.Accelerator.CatalogImport.Framework.Metadata;

namespace Plugin.Accelerator.CatalogImport.Framework.ImportHandlers
{
    public abstract class SellableItemImportHandler<TSourceEntity> : BaseEntityImportHandler<TSourceEntity, SellableItem>
        where TSourceEntity : IEntity
    {
        protected string ProductId { get; set; }

        protected string Name { get; set; }

        protected string DisplayName { get; set; }

        protected string Description { get; set; }

        protected string Brand { get; set; }

        protected string Manufacturer { get; set; }

        protected string TypeOfGood { get; set; }

        protected IList<string> Tags { get; set; }

        public SellableItemImportHandler(string sourceEntity, CommercePipelineExecutionContext context)
            : base(sourceEntity, context)
        {
            this.Tags = new List<string>();
        }

        public override async Task<CommerceEntity> Create(IServiceProvider serviceProvider, IDictionary<string, IList<string>> parents, CommercePipelineExecutionContext context)
        {
            this.Initialize();
            var command = serviceProvider.GetService(typeof(CreateSellableItemCommand)) as CreateSellableItemCommand;
            if (command == null)
                throw new InvalidOperationException("SellableItem cannot be created, CreateSellableItemCommand not found.");
            this.CommerceEntity = await command.Process(context.CommerceContext, ProductId, Name, DisplayName, Description, Brand, Manufacturer, TypeOfGood, Tags.ToArray());
            return this.CommerceEntity;
        }

        public override bool HasVariants()
        {
            var variants = typeof(TSourceEntity).GetPropertyValueWithAttribute<VariantsAttribute, IEnumerable>(this.SourceEntity);
            return variants != null && variants.GetEnumerator().MoveNext();
        }

        public override IList<IEntity> GetVariants()
        {
            return this.GetVariants(this.SourceEntity);
        }

        public override bool HasVariants(ILanguageEntity languageEntity)
        {
            var languages = this.GetLanguages();
            if (languages != null
                && languages.Any())
            {
                var matchedLanguage = languages.FirstOrDefault(x =>
                    x.Language.Equals(languageEntity.Language, StringComparison.OrdinalIgnoreCase));
                if (matchedLanguage != null)
                {
                    var variants = this.GetVariants(languageEntity.GetEntity());
                    return variants != null && variants.GetEnumerator().MoveNext();
                }
            }

            return false;
        }

        public override IList<IEntity> GetVariants(ILanguageEntity languageEntity)
        {
            var languages = this.GetLanguages();
            if (languages != null
                && languages.Any())
            {
                var matchedLanguage = languages.FirstOrDefault(x =>
                    x.Language.Equals(languageEntity.Language, StringComparison.OrdinalIgnoreCase));
                if (matchedLanguage != null)
                {
                    return this.GetVariants(languageEntity.GetEntity());
                }
            }

            return new List<IEntity>();
        }
    }
}