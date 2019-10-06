using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Accelerator.CatalogImport.Framework.Abstractions;

namespace Plugin.Accelerator.CatalogImport.Framework.ImportHandlers
{
    public abstract class CategoryImportHandler<TSourceEntity> : BaseImportHandler<TSourceEntity, Category>
        where TSourceEntity : IEntity
    {
        protected string CatalogId { get; set; }

        protected string Name { get; set; }

        protected string DisplayName { get; set; }

        protected string Description { get; set; }

        public CategoryImportHandler(string sourceEntity)
            : base(sourceEntity)
        {
        }

        public override async Task<CommerceEntity> Create(IServiceProvider serviceProvider, IDictionary<string, IList<string>> parents, CommercePipelineExecutionContext context)
        {
            if (parents != null && parents.Any())
            {
                var firstParent = parents.FirstOrDefault();
                this.CatalogId = firstParent.Key;
            }

            this.Initialize();
            var command = serviceProvider.GetService(typeof(CreateCategoryCommand)) as CreateCategoryCommand;
            if (command == null)
                throw new InvalidOperationException("Category cannot be created, CreateCategoryCommand not found.");
            return await command.Process(context.CommerceContext, CatalogId, Name, DisplayName, Description);
        }

        public override bool HasVariants()
        {
            return false;
        }

        public override IList<IEntity> GetVariants()
        {
            return new List<IEntity>();
        }
    }
}