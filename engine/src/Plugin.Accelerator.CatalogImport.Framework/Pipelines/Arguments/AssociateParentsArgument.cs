using Sitecore.Commerce.Core;
using System.Collections.Generic;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments
{
    public class AssociateParentsArgument : PipelineArgument
    {
        public AssociateParentsArgument(string entityId)
        {
            this.EntityId = entityId;
        }

        public string EntityId { get; set; }
    }
}