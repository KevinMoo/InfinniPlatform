﻿using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Tests.TestData
{
    public sealed class ActionUnitWrapGeneratorTest
    {
        public void Action(IApplyContext target)
        {
            target.Result = new DynamicWrapper();
            target.Result.TestValue = "Test";
        }
    }
}