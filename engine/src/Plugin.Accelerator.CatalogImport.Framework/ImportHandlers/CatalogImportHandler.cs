using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.Accelerator.CatalogImport.Framework.ImportHandlers
{
    public abstract class CatalogImportHandler<TSourceEntity> : BaseEntityImportHandler<TSourceEntity, Catalog>
        where TSourceEntity : IEntity
    {
        protected string Name { get; set; }

        protected string DisplayName { get; set; }

        public CatalogImportHandler(string sourceEntity, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        :base(sourceEntity, commerceCommander, context)
        {
        }

        public override async Task<CommerceEntity> Create()
        {
            this.Initialize();
            var command  = this.CommerceCommander.Command<CreateCatalogCommand>();
            this.CommerceEntity = await command.Process(this.Context.CommerceContext, Name, DisplayName);
            return this.CommerceEntity;
        }

        public override IList<string> GetParentList()
        {
            return new List<string>();
        }
    }
}