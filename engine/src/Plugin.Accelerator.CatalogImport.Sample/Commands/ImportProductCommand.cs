using Plugin.Accelerator.CatalogImport.Sample.Entity;
using Plugin.Accelerator.CatalogImport.Sample.Pipelines;
using Plugin.Accelerator.CatalogImport.Sample.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.Plugin.Catalog;
using System;
using System.Threading.Tasks;

namespace Plugin.Accelerator.CatalogImport.Sample.Commands
{
    public class ImportProductCommand : CommerceCommand
    {
        public ImportProductCommand(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public virtual async Task<SellableItem> Process(
            CommerceContext commerceContext,
            Product product)
        {
            SellableItem result = null;
            using (CommandActivity.Start(commerceContext, this))
            {
                // Create/Update sellable item, sellable item components, manage versions and workflow.
                await this.PerformTransaction(commerceContext, async () => result = await this.Pipeline<IImportProductPipeline>().Run(new ImportProductArgument(product), commerceContext.PipelineContextOptions));

                // Manage association of sellable item with catalog and categories.
                await this.PerformTransaction(commerceContext, async () => await this.Pipeline<IAssociateParentsPipeline>().Run(new AssociateParentsArgument(product, result.Id), commerceContext.PipelineContextOptions));

                // Manage localization values for sellable item
                await this.PerformTransaction(commerceContext, async () => await this.Pipeline<IImportLocalizeContentPipeline>().Run(new ImportLocalizeContentArgument(result.Id, product, result), commerceContext.PipelineContextOptions));
            }
            return result;
        }
    }
}