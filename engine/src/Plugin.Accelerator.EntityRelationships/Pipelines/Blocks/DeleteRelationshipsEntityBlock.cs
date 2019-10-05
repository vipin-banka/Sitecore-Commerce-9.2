using Microsoft.Extensions.Logging;
using Plugin.Accelerator.EntityRelationships.Entity;
using Plugin.Accelerator.EntityRelationships.Extensions;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;

namespace Plugin.Accelerator.EntityRelationships.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.DeleteRelationshipsEntityBlock)]
    public class DeleteRelationshipsEntityBlock : PipelineBlock<DeleteEntityArgument, DeleteEntityArgument, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public DeleteRelationshipsEntityBlock(CommerceCommander commerceCommander)
        {
            this._commerceCommander = commerceCommander;
        }

        public override async Task<DeleteEntityArgument> Run(DeleteEntityArgument arg, CommercePipelineExecutionContext context)
        {
            if (arg == null || arg.EntityToDelete == null && string.IsNullOrEmpty(arg.EntityToDelete?.Id))
                return arg;

            if (arg.EntityToDelete is RelationshipsEntity)
                return arg;

            string id = arg.EntityToDelete.Id.ToRelationshipsEntityId();
            if (string.IsNullOrEmpty(id))
                return arg;
            try
            {
                if (!await this._commerceCommander.DeleteEntity(context.CommerceContext, id).ConfigureAwait(false))
                    context.Logger.LogWarning("Association entity '" + id + "' for entity '" + arg.EntityToDelete.Id + "' was not deleted.");
            }
            catch
            {
                context.Logger.LogWarning("Association entity '" + id + "' for entity '" + arg.EntityToDelete.Id + "' was not deleted.");
            }
            return arg;
        }
    }
}