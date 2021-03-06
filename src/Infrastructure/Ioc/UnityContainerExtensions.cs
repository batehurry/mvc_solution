﻿using System;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;

namespace Infrastructure
{
    public static class UnityContainerExtensions
    {
        public static void RegisterInheritedTypes(this IUnityContainer container, Assembly assembly, Type baseType)
        {
            var allTypes = assembly.GetTypes();
            var baseInterfaces = baseType.GetInterfaces();
            foreach (var type in allTypes)
            {
                if (type.BaseType != null && type.BaseType.GenericEq(baseType))
                {
                    var typeInterface = type.GetInterfaces().Where(x => !baseInterfaces.Any(bi => bi.GenericEq(x)));
                    foreach (var itype in typeInterface)
                    {
                        if (itype == null)
                        {
                            continue;
                        }
                        container.RegisterType(itype, type);
                    }
                }
            }
        }
    }
}
