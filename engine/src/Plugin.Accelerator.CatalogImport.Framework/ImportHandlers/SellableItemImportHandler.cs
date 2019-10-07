using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}