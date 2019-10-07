﻿using Microsoft.AspNetCore.OData.Builder;
using Plugin.Accelerator.CatalogImport.Framework.Model;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;

namespace Plugin.Accelerator.CatalogImport.Framework
{
    [PipelineDisplayName(Constants.ConfigureServiceApiBlock)]
    public class ConfigureServiceApiBlock: PipelineBlock<ODataConventionModelBuilder, ODataConventionModelBuilder, CommercePipelineExecutionContext>
    {       
        public override Task<ODataConventionModelBuilder> Run(ODataConventionModelBuilder modelBuilder,
            CommercePipelineExecutionContext context)
        {
            string entitySetName = "Commands";

            ActionConfiguration actionConfiguration = modelBuilder.Action("ImportEntity");
            actionConfiguration.Parameter<SourceEntityDetail>("sourceEntity");
            actionConfiguration.ReturnsFromEntitySet<CommerceCommand>(entitySetName);

            return Task.FromResult(modelBuilder);
        }
    }
}