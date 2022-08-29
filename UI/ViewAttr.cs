using System.Collections;
using System.Collections.Generic;
using System;

namespace Mao
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ViewAttribute : Attribute 
    {
        //包名
        public string PkgName{ get; private set;}

        //组件名
        public string ResName{ get; private set;}


        public string Url{
            get {
                string url = $"ui://{PkgName}/{ResName}";
                return url;
            }
        }
        public ViewAttribute(string pkgName,string resName)
        {
            PkgName = pkgName;
            ResName = resName;
        }
    }
}