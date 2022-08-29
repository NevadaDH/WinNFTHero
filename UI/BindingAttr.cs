using System.Collections;
using System.Collections.Generic;
using System;

namespace Mao
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class BindingAttribute: Attribute{
        public string Path{ get; private set;}

        public Type ComponentType{ get; private set;}

        public BindingAttribute(string path)
        {
            Path = path;
        }
    }
}