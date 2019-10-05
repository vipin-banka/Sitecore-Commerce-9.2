﻿using Microsoft.Extensions.Logging;
using Plugin.Accelerator.CatalogImport.Sample.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Accelerator.CatalogImport.Sample.Pipelines
{
    public class AssociateParentsPipeline : CommercePipeline<AssociateParentsArgument, AssociateParentsArgument>, IAssociateParentsPipeline
    {
        public AssociateParentsPipeline(IPipelineConfiguration<IAssociateParentsPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}