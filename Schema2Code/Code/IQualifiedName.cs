﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schema2Code.Code
{
    public interface IQualifiedName : IName
    {
        INamespace NameSpace { get; }
        String FullyQualifiedName { get; }
    }
}
