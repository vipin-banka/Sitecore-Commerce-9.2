﻿using System.Collections.Generic;
using Plugin.Accelerator.CatalogImport.Framework.Entity;

namespace Plugin.Accelerator.CatalogImport.Sample.Entity
{
    public class SourceProduct : BaseEntity
    {
        public SourceProduct()
        {
            this.Languages = new List<LanguageEntity<SourceProduct>>();
            this.Variants = new List<SourceProductVariant>();
        }

        public string PartNumber { get; set; }

        public string Weight { get; set; }

        public string Accessories { get; set; }

        public string Dimensions { get; set; }

        public IList<LanguageEntity<SourceProduct>> Languages { get; set; }

        public IList<string> VariantComponents { get; set; }

        public IList<SourceProductVariant> Variants { get; set; }
    }
}