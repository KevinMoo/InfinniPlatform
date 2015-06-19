﻿using InfinniPlatform.Api.Actions;
using InfinniPlatform.Api.Factories;

namespace InfinniPlatform.Metadata.StateMachine.ActionUnits.ActionOperatorBuilders
{
    public sealed class ActionOperatorBuilderScript : IActionOperatorBuilder
    {
        private readonly IScriptProcessor _scriptProcessor;
        private readonly string _unitIdentifier;

        public ActionOperatorBuilderScript(IScriptProcessor scriptProcessor, string unitIdentifier)
        {
            _scriptProcessor = scriptProcessor;
            _unitIdentifier = unitIdentifier;
        }

        public IActionOperator BuildActionOperator()
        {
            return new ActionOperator(_unitIdentifier,
                context => _scriptProcessor.InvokeScript(_unitIdentifier, context));
        }
    }
}