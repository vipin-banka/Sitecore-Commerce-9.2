using Plugin.Accelerator.EntityRelationships.Entity;
using Plugin.Accelerator.EntityRelationships.Extensions;
using Plugin.Accelerator.EntityViews.Entity;
using Sitecore.Commerce.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Accelerator.EntityRelationships.EntityView
{
    public class RelationshipsEntityView : View<CommerceEntity>
    {
        private readonly IDoesEntityExistPipeline _doesEntityExistPipeline;
        private readonly IFindEntityPipeline _findEntityPipeline;

        public RelationshipsEntityView(IDoesEntityExistPipeline doesEntityExistPipeline,
            IFindEntityPipeline findEntityPipeline)
        {
            this._doesEntityExistPipeline = doesEntityExistPipeline;
            this._findEntityPipeline = findEntityPipeline;
        }

        public override string ViewName => "CommerceEntityRelationships";

        public override string ViewDisplayName => "Entity Relationships";

        public override async Task ModifyView(CommercePipelineExecutionContext context,
            Sitecore.Commerce.EntityViews.EntityView entityView,
            CommerceEntity entity)
        {
            var associatedEntityId = entityView.EntityId.ToRelationshipsEntityId();
            var associatedEntity = await this._findEntityPipeline.Run(new FindEntityArgument(typeof(RelationshipsEntity), associatedEntityId, entityView.EntityVersion, false), context).ConfigureAwait(false) as RelationshipsEntity;

            if (associatedEntity != null
                && associatedEntity.Associations != null
                && associatedEntity.Associations.Any())
            {
                var view = new Sitecore.Commerce.EntityViews.EntityView
                {
                    Name = this.ViewName,
                    DisplayName = this.ViewDisplayName,
                    EntityId = entityView.EntityId,
                    EntityVersion = entityView.EntityVersion,
                    ItemId = associatedEntity.Id,
                    VersionedItemId = associatedEntity.Id + "-" + associatedEntity.EntityVersion,
                    UiHint = "Table"
                };


                foreach (var association in associatedEntity.Associations)
                {
                    var associatedItemEntityView = new Sitecore.Commerce.EntityViews.EntityView()
                    {
                        Name = "Details",
                        DisplayName = "Details",
                        EntityId = entityView.EntityId,
                        EntityVersion = entityView.EntityVersion,
                        ItemId = association.EntityTarget,
                        VersionedItemId = association.EntityTarget + "-" + association.EntityVersion
                    };

                    associatedItemEntityView.Properties.Add(
                        new Sitecore.Commerce.EntityViews.ViewProperty
                        {
                            Name = "Entity Name",
                            DisplayName = "Entity Name",
                            Value = association.Name,
                            UiType = "EntityLink",
                            IsReadOnly = true,
                            IsRequired = true
                        });

                    associatedItemEntityView.Properties.Add(
                    new Sitecore.Commerce.EntityViews.ViewProperty
                    {
                        Name = "Entity Version",
                        RawValue = association.EntityVersion,
                        IsReadOnly = true,
                        IsRequired = false
                    });

                    associatedItemEntityView.Properties.Add(
                    new Sitecore.Commerce.EntityViews.ViewProperty
                    {
                        Name = "Relationship Type",
                        RawValue = association.RelationshipType,
                        IsReadOnly = true,
                        IsRequired = false
                    });

                    view.ChildViews.Add(associatedItemEntityView);

                }

                entityView.ChildViews.Add(view);
            }
        }

        public override async Task<bool> ShouldViewApply(CommercePipelineExecutionContext context, Sitecore.Commerce.EntityViews.EntityView entityView, CommerceEntity entity)
        {
            var isMasterView = entityView.Name.Equals("Master", StringComparison.OrdinalIgnoreCase);
            if (!isMasterView)
                return false;

            var id = entityView.EntityId.ToRelationshipsEntityId();

            var associatedEntityExists = await this._doesEntityExistPipeline.Run(new FindEntityArgument(typeof(RelationshipsEntity), id, entityView.EntityVersion, false), context).ConfigureAwait(false);

            return associatedEntityExists;
        }
    }
}