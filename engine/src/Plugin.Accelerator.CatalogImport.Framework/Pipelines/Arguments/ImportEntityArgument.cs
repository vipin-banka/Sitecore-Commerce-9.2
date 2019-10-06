using Plugin.Accelerator.CatalogImport.Framework.ImportHandlers;
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
            this.Parents = new Dictionary<string, IList<string>>();
        }

        public SourceEntityDetail SourceEntityDetail { get; set; }

        public object SourceEntity { get; set; }

        public IDictionary<string, IList<string>> Parents { get; set; }

        public bool IsNew { get; set; }

        public IImportHandler ImportHandler { get; set; }

        public CommerceEntity CommerceEntity { get; set; }
    }
}