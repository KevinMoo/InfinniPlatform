﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Sdk.Metadata
{
    public sealed class MenuMetadata
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Caption { get; set; }

        public dynamic Items { get; set; }
    }
}
