﻿using Autofac;

namespace InfinniPlatform.IoC
{
    public sealed class AutofacContainerModule : Module
    {
        public AutofacContainerModule(IContainerModule containerModule)
        {
            _containerModule = containerModule;
        }


        private readonly IContainerModule _containerModule;


        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            _containerModule.Load(new AutofacContainerBuilder(builder));
        }
    }
}