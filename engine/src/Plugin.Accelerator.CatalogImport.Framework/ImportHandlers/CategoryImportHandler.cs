using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Accelerator.CatalogImport.Framework.ImportHandlers
{
    public abstract class CategoryImportHandler<TSourceEntity> : BaseEntityImportHandler<TSourceEntity, Category>
        where TSourceEntity : IEntity
    {
        protected string CatalogId { get; set; }

        protected string Name { get; set; }

        protected string DisplayName { get; set; }

        protected string Description { get; set; }

        public CategoryImportHandler(string sourceEntity, CommercePipelineExecutionContext context)
            : base(sourceEntity, context)
        {
        }

        protected override string Id
        {
            get
            {
                var firstParent = this.parentEntityIds.FirstOrDefault();
                string catalogId = firstParent.Key.SimplifyEntityName();
                return this.SourceEntity.Id.ToCategoryId(catalogId);
            }
        }

        public override async Task<CommerceEntity> Create(IServiceProvider serviceProvider, CommercePipelineExecutionContext context)
        {
            if (this.parentEntityIds == null || !this.parentEntityIds.Any())
            {
                throw new InvalidOperationException("Catalog must exist to create a new category.");
            }

            var firstParent = this.parentEntityIds.FirstOrDefault();
            this.CatalogId = firstParent.Key;

            this.Initialize();
            var command = serviceProvider.GetService(typeof(CreateCategoryCommand)) as CreateCategoryCommand;
            if (command == null)
                throw new InvalidOperationException("Category cannot be created, CreateCategoryCommand not found.");
            this.CommerceEntity = await command.Process(context.CommerceContext, CatalogId, Name, DisplayName, Description);
            return this.CommerceEntity;
        }
    }
}