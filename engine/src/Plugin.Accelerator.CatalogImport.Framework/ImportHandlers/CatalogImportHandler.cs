using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.Accelerator.CatalogImport.Framework.ImportHandlers
{
    public abstract class CatalogImportHandler<TSourceEntity> : BaseEntityImportHandler<TSourceEntity, Catalog>
        where TSourceEntity : IEntity
    {
        protected string Name { get; set; }

        protected string DisplayName { get; set; }

        public CatalogImportHandler(string sourceEntity, CommercePipelineExecutionContext context)
        :base(sourceEntity, context)
        {
        }

        public override async Task<CommerceEntity> Create(IServiceProvider serviceProvider, IDictionary<string, IList<string>> parents, CommercePipelineExecutionContext context)
        {
            this.Initialize();
            var command  = serviceProvider.GetService(typeof(CreateCatalogCommand)) as CreateCatalogCommand;
            if (command == null)
                throw new InvalidOperationException("Catalog cannot be created, CreateCatalogCommand not found.");
            this.CommerceEntity = await command.Process(context.CommerceContext, Name, DisplayName);
            return this.CommerceEntity;
        }

        public override IList<string> GetParentList()
        {
            return new List<string>();
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