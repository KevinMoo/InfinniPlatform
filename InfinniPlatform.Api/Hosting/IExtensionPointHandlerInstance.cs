﻿namespace InfinniPlatform.Api.Hosting
{
    public interface IExtensionPointHandlerInstance
    {
        string HandlerInstanceName { get; }

        IExtensionPointHandlerInstance RegisterExtensionPoint(string extensionPointTypeName,
            string stateMachineReference);

        ExtensionPointValue GetExtensionPoint(string extensionPointTypeName);
    }
}