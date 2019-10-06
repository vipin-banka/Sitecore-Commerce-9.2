using System.Collections.Generic;

namespace Plugin.Accelerator.CatalogImport.Framework.Model
{
    public class SourceEntityDetail
    {
        public SourceEntityDetail()
        {
            this.Components = new List<string>();
            this.VariantComponents = new List<string>();
        }

        public string SerializedEntity { get; set; }

        public IList<string> Components { get; set; }

        public IList<string> VariantComponents { get; set; }

        public string EntityType { get; set; }
    }
}