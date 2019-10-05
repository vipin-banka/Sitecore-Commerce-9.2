namespace Plugin.Accelerator.EntityRelationships.Pipelines.Blocks
{
    using Plugin.Accelerator.EntityRelationships.Entity;
    using Plugin.Accelerator.EntityRelationships.Extensions;
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Plugin.Catalog;
    using Sitecore.Framework.Conditions;
    using Sitecore.Framework.Pipelines;
    using System.Threading.Tasks;

    [PipelineDisplayName(Constants.RelationshipsEntityBlock)]
    public class RelationshipsEntityBlock : PipelineBlock<PersistEntityArgument, PersistEntityArgument, CommercePipelineExecutionContext>
    {
        private readonly IPersistEntityPipeline _persistEntityPipeline;
        private readonly IDoesEntityExistPipeline _doesEntityExistPipeline;

        public RelationshipsEntityBlock(IPersistEntityPipeline persistEntityPipeline,
            IDoesEntityExistPipeline doesEntityExistPipeline)
        {
            this._persistEntityPipeline = persistEntityPipeline;
            this._doesEntityExistPipeline = doesEntityExistPipeline;
        }

        public override async Task<PersistEntityArgument> Run(PersistEntityArgument arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull(this.Name + ": argument cannot be null");

            CommerceEntity entity = arg.Entity;
            if (entity == null || entity is RelationshipsEntity || (entity.GetType() != typeof(SellableItem) && entity.GetType() != typeof(Category)))
            {
                return arg;
            }

            var associationEntityId = arg.Entity.Id.ToRelationshipsEntityId(); ;

            var entityFound = await this._doesEntityExistPipeline.Run(new FindEntityArgument(typeof(RelationshipsEntity), associationEntityId, arg.Entity.EntityVersion, false), context).ConfigureAwait(false);

            if (!entityFound)
            {
                var associationEntity = new RelationshipsEntity();
                associationEntity.Id = associationEntityId;
                associationEntity.EntityVersion = arg.Entity.EntityVersion;
                await this._persistEntityPipeline.Run(new PersistEntityArgument(associationEntity), context).ConfigureAwait(false);
            }

            return arg;
        }
    }
}