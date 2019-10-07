using Sitecore.Commerce.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.Accelerator.CatalogImport.Framework.Abstractions
{
    public interface IEntityImportHandler
    {
        string EntityId { get; }

        IDictionary<string, IList<string>> ParentEntityIds { get; set; }

        Task<CommerceEntity> Create(IServiceProvider serviceProvider, CommercePipelineExecutionContext context);

        IEntity GetSourceEntity();

        CommerceEntity GetCommerceEntity();

        void SetCommerceEntity(CommerceEntity commerceEntity);

        IList<string> GetParentList();

        bool HasVariants();

        IList<IEntity> GetVariants();

        bool HasLanguages();

        IList<ILanguageEntity> GetLanguages();

        bool HasVariants(ILanguageEntity languageEntity);

        IList<IEntity> GetVariants(ILanguageEntity languageEntity);
    }

    public interface IEntityImportHandler<TCommerceEntity>
    where TCommerceEntity : CommerceEntity
    {
        TCommerceEntity CommerceEntity { get; set; }
    }
}