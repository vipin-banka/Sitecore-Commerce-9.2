using Plugin.Accelerator.CatalogImport.Sample.Entity;
using Sitecore.Commerce.Core;
using System.Collections.Generic;

namespace Plugin.Accelerator.CatalogImport.Sample.Pipelines.Arguments
{
    public class AssociateParentsArgument : PipelineArgument
    {
        public AssociateParentsArgument(Product product, string entityId)
        {
            this.Product = product;
            this.EntityId = entityId;
        }

        public Product Product { get; set; }

        public string EntityId { get; set; }

        public IList<string> EntityIds { get; set; }
    }
}