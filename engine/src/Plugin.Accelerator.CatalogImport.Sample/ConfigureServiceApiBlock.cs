using Microsoft.AspNetCore.OData.Builder;
using Plugin.Accelerator.CatalogImport.Sample.Entity;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;

namespace Plugin.Accelerator.CatalogImport.Sample
{
    [PipelineDisplayName(Constants.ConfigureServiceApiBlock)]
    public class ConfigureServiceApiBlock: PipelineBlock<ODataConventionModelBuilder, ODataConventionModelBuilder, CommercePipelineExecutionContext>
    {       
        public override Task<ODataConventionModelBuilder> Run(ODataConventionModelBuilder modelBuilder,
            CommercePipelineExecutionContext context)
        {
            string entitySetName = "Commands";
            modelBuilder.AddComplexType(typeof(BaseEntity));
            modelBuilder.AddComplexType(typeof(Product));

            ActionConfiguration actionConfiguration = modelBuilder.Action("ImportProduct");
            actionConfiguration.Parameter<Product>("product");
            actionConfiguration.ReturnsFromEntitySet<CommerceCommand>(entitySetName);

            return Task.FromResult(modelBuilder);
        }
    }
}