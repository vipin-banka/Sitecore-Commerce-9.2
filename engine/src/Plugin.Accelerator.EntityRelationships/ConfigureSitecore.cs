namespace Plugin.Accelerator.EntityRelationships
{
    using Microsoft.Extensions.DependencyInjection;
    using Plugin.Accelerator.EntityRelationships.EntityView;
    using Plugin.Accelerator.EntityRelationships.Pipelines;
    using Plugin.Accelerator.EntityRelationships.Pipelines.Blocks;
    using Plugin.Accelerator.EntityViews.Pipelines.Blocks;
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.EntityViews;
    using Sitecore.Commerce.Plugin.Catalog;
    using Sitecore.Commerce.Plugin.EntityVersions;
    using Sitecore.Framework.Configuration;
    using Sitecore.Framework.Pipelines.Definitions.Extensions;
    using System.Reflection;

    public class ConfigureSitecore : IConfigureSitecore
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);

            services.AddTransient<RelationshipsEntityView>();

            services.Sitecore().Pipelines(config => config

                .ConfigurePipeline<IGetEntityViewPipeline>(c =>
                {
                    c.Add<GetViewBlock<RelationshipsEntityView, CommerceEntity>>().After<GetSellableItemDetailsViewBlock>();
                })

                .ConfigurePipeline<IRunningPluginsPipeline>(c =>
                {
                    c.Add<Pipelines.Blocks.RegisteredPluginBlock>().After<RunningPluginsBlock>();
                })
                .ConfigurePipeline<IPersistEntityPipeline>(c =>
                {
                    c.Add<RelationshipsEntityBlock>().After<LocalizeEntityBlock>();
                })
                .ConfigurePipeline<ICreateRelationshipPipeline>(c =>
                {
                    c.Add<AddRelationshipsToReferenceEntityBlock>().After<CreateRelationshipBlock>();
                })
                .ConfigurePipeline<IDeleteRelationshipPipeline>(c =>
                {
                    c.Add<RemoveRelationshipsFromReferenceEntityBlock>().After<DeleteRelationshipBlock>();
                })
                .ConfigurePipeline<IAddEntityVersionPipeline>(c =>
                {
                    c.Add<CloneRelationshipsEntityBlock>().Before<Sitecore.Commerce.Core.CloneEntityBlock>();
                })
                .ConfigurePipeline<IDeleteEntityPipeline>(c =>
                {
                    c.Add<DeleteRelationshipsEntityBlock>().After<IndexDeletedSitecoreItemBlock>();
                })
                .AddPipeline<IRefreshRelationshipsPipeline, RefreshRelationshipsPipeline>(c =>
                {
                    c.Add<RefreshRelationshipsBlock>();
                }));

            services.RegisterAllCommands(assembly);
        }
    }
}