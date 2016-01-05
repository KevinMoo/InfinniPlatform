﻿using System;

using InfinniPlatform.Core.ContextComponents;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.SystemConfig.Utils;

namespace InfinniPlatform.SystemConfig.ActionUnits
{
	public sealed class ActionUnitGetDocument
	{
	    private readonly IConfigurationMediatorComponent _configurationMediatorComponent;
	    private readonly IMetadataComponent _metadataComponent;
	    private readonly InprocessDocumentComponent _inprocessDocumentComponent;
	    private readonly IReferenceResolver _referenceResolver;

	    public ActionUnitGetDocument(IConfigurationMediatorComponent configurationMediatorComponent,
	                                 IMetadataComponent metadataComponent,
	                                 InprocessDocumentComponent inprocessDocumentComponent,
	                                 IReferenceResolver referenceResolver)
	    {
	        _configurationMediatorComponent = configurationMediatorComponent;
	        _metadataComponent = metadataComponent;
	        _inprocessDocumentComponent = inprocessDocumentComponent;
	        _referenceResolver = referenceResolver;
	    }

	    public void Action(IApplyContext target)
		{
	        var executor = new DocumentExecutor(_configurationMediatorComponent,
	                                            _metadataComponent,
	                                            _inprocessDocumentComponent,
	                                            _referenceResolver);

	        target.Result = executor.GetCompleteDocuments(target.Item.Configuration,
	                                                      target.Item.Metadata,
	                                                      Convert.ToInt32(target.Item.PageNumber),
	                                                      Convert.ToInt32(target.Item.PageSize),
	                                                      target.Item.Filter,
	                                                      target.Item.Sorting,
	                                                      target.Item.IgnoreResolve);
		}
	}
}