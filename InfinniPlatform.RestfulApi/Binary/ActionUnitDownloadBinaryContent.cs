﻿using System;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using System.Linq;
using System.Collections.Generic;
using InfinniPlatform.Api.SearchOptions.Builders;

namespace InfinniPlatform.RestfulApi.Binary
{
	/// <summary>
    ///     Модуль загрузки двоичного контекта
	/// </summary>
	public sealed class ActionUnitDownloadBinaryContent
	{
		public void Action(IUrlEncodedDataContext target)
		{
		    if (target.FormData.ContentId != null)
		    {
		        target.Result = LoadBlobData(target.Context.GetComponent<IBlobStorageComponent>(target.Version), target.FormData.ContentId);
		    }
		    else
		    {
                target.Result = FillContentByDocumentId(target.Context.GetComponent<IBlobStorageComponent>(target.Version), target.FormData);
		    }
		}

	    private dynamic LoadBlobData(IBlobStorageComponent blobStorageComponent, string contentId)
	    {
            var blobStorage = blobStorageComponent.GetBlobStorage();
            var blobData = blobStorage.GetBlobData(Guid.Parse(contentId));
            return blobData;	        
	    }

	    private dynamic FillContentByDocumentId(IBlobStorageComponent blobStorageComponent, dynamic formData)
	    {
            Action<FilterBuilder> builder = f => f.AddCriteria(cr => cr.Property("Id").IsEquals(formData.DocumentId));
            IEnumerable<dynamic> documents = new DocumentApi(null).GetDocument(formData.Configuration, formData.Metadata, builder, 0, 1);
            dynamic document = documents.FirstOrDefault();

			if (document != null)
			{
                var linkValue = ObjectHelper.GetProperty(document, formData.FieldName);
				if (linkValue != null)
				{
                    return LoadBlobData(blobStorageComponent, linkValue.Info.ContentId);
			}
                return null;
			}
	        return null;
		}
	}
}
