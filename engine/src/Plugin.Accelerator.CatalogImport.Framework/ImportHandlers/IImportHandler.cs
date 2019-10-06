using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Accelerator.CatalogImport.Framework.Abstractions;

namespace Plugin.Accelerator.CatalogImport.Framework.ImportHandlers
{
    public interface IImportHandler
    {
        string EntityId { get; }

        Task<CommerceEntity> Create(IServiceProvider serviceProvider, IDictionary<string, IList<string>> parents, CommercePipelineExecutionContext context);

        IEntity GetSourceEntity();

        IList<string> GetParentList();

        bool HasVariants();

        IList<IEntity> GetVariants();

        bool HasLanguages();

        IList<ILanguageEntity> GetLanguages();

        bool HasVariants(ILanguageEntity languageEntity);

        IList<IEntity> GetVariants(ILanguageEntity languageEntity);
    }
}