using Plugin.Accelerator.CatalogImport.Sample.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.Accelerator.CatalogImport.Sample.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.ImportProductBlock)]
    public class ImportProductBlock : PipelineBlock<ImportProductArgument, SellableItem, CommercePipelineExecutionContext>
    {
        private readonly FindEntityCommand _findEntityCommand;
        private readonly CreateSellableItemCommand _createSellableItemCommand;
        private readonly ISetComponentsPipeline _setComponentsPipeline;
        private readonly IPersistEntityPipeline _persistEntityPipeline;

        public ImportProductBlock(
            FindEntityCommand findEntityCommand,
            CreateSellableItemCommand createSellableItemCommand,
            ISetComponentsPipeline setComponentsPipeline,
            IPersistEntityPipeline persistEntityPipeline)
        {
            this._findEntityCommand = findEntityCommand;
            this._createSellableItemCommand = createSellableItemCommand;
            this._setComponentsPipeline = setComponentsPipeline;
            this._persistEntityPipeline = persistEntityPipeline;
        }

        public override async Task<SellableItem> Run(ImportProductArgument arg, CommercePipelineExecutionContext context)
        {
            context.CommerceContext.AddUniqueObject(arg);

            string sellableItemId = CommerceEntity.IdPrefix<SellableItem>() + arg.Product.Id;

            var sellableItem = await this._findEntityCommand.Process(context.CommerceContext, typeof(SellableItem), sellableItemId).ConfigureAwait(false) as SellableItem;

            if (sellableItem == null)
            {
                sellableItem = await this._createSellableItemCommand.Process(context.CommerceContext,
                        arg.Product.Id,
                        arg.Product.Name,
                        arg.Product.DisplayName,
                        arg.Product.Description,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        new List<string>().ToArray())
                    .ConfigureAwait(false);

                /*if (sellableItem != null)
                {
                    string catalogId = "Habitat_Master".ToEntityId<Sitecore.Commerce.Plugin.Catalog.Catalog>();
                    string categoryId = "Imported".ToCategoryId(catalogId.SimplifyEntityName());

                    //string catalogId = "Entity-Catalog-Habitat_Master";
                    //string entityId = "Entity-Category-Habitat_Master-Imported";

                    CatalogReferenceArgument referenceArgument = await this._associateSellableItemToParent
                        .Process(context.CommerceContext, catalogId, categoryId, sellableItem.Id).ConfigureAwait(false);
                }*/
            }
            else
            {
                await this._setComponentsPipeline.Run(sellableItem as SellableItem, context)
                    .ConfigureAwait(false);

                await this._persistEntityPipeline.Run(new PersistEntityArgument(sellableItem), context)
                    .ConfigureAwait(false);
            }

            return await Task.FromResult(sellableItem as SellableItem);
        }
    }
}