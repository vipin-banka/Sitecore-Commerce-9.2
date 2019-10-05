using Plugin.Accelerator.CatalogImport.Sample.Entity;
using Sitecore.Commerce.Core;

namespace Plugin.Accelerator.CatalogImport.Sample.Pipelines.Arguments
{
    public class ImportProductArgument : PipelineArgument
    {
        public ImportProductArgument(Product product)
        {
            this.Product = product;
        }

        public Product Product { get; set; }
    }
}