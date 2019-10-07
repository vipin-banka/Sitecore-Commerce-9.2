using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Plugin.Accelerator.CatalogImport.Framework.Model;
using Sitecore.Commerce.Core;
using System.Collections.Generic;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments
{
    public class ImportEntityArgument : PipelineArgument
    {
        public ImportEntityArgument(SourceEntityDetail sourceEntityDetail)
        {
            this.SourceEntityDetail = sourceEntityDetail;
        }

        public SourceEntityDetail SourceEntityDetail { get; set; }

        public object SourceEntity { get; set; }

        public bool IsNew { get; set; }

        public IEntityImportHandler ImportHandler { get; set; }
    }
}