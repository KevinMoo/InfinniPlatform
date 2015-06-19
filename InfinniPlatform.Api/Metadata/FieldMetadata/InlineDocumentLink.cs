﻿namespace InfinniPlatform.Api.Metadata.FieldMetadata
{
    public sealed class InlineDocumentLink
    {
        private readonly dynamic _childDocument;
        private readonly string _configId;
        private readonly string _documentId;
        private readonly dynamic _parentDocument;
        private readonly string _propertyName;

        public InlineDocumentLink(string propertyName, string configId, string documentId, dynamic parentDocument,
            dynamic childDocument)
        {
            _propertyName = propertyName;
            _configId = configId;
            _documentId = documentId;
            _parentDocument = parentDocument;
            _childDocument = childDocument;
        }

        public string ConfigId
        {
            get { return _configId; }
        }

        public string DocumentId
        {
            get { return _documentId; }
        }

        public dynamic ParentDocument
        {
            get { return _parentDocument; }
        }

        public dynamic ChildDocument
        {
            get { return _childDocument; }
        }

        public void Resolve(dynamic resolvedDocument)
        {
            _parentDocument[_propertyName] = resolvedDocument;
        }
    }
}