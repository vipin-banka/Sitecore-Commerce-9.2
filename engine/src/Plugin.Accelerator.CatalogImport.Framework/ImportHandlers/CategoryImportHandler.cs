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

        public CategoryImportHandler(string sourceEntity, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
            : base(sourceEntity, commerceCommander, context)
        {
        }

        protected override string Id
        {
            get
            {
                var firstParent = this.ParentEntityIds.FirstOrDefault();
                string catalogId = firstParent.Key.SimplifyEntityName();
                return this.SourceEntity.Id.ToCategoryId(catalogId);
            }
        }

        public override async Task<CommerceEntity> Create()
        {
            if (this.ParentEntityIds == null || !this.ParentEntityIds.Any())
            {
                this.Context.Abort(await Context.CommerceContext.AddMessage(Context.GetPolicy<KnownResultCodes>().Error, "CategoryCatelogNotDefined", null, "Catalog must be defined to create a new category."), this.Context);
                return this.CommerceEntity;
            }

            var firstParent = this.ParentEntityIds.FirstOrDefault();
            this.CatalogId = firstParent.Key;

            this.Initialize();
            var command = this.CommerceCommander.Command<CreateCategoryCommand>();
            this.CommerceEntity = await command.Process(this.Context.CommerceContext, CatalogId, Name, DisplayName, Description);
            return this.CommerceEntity;
        }
    }
}