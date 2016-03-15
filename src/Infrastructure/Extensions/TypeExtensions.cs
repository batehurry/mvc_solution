using System;

namespace Infrastructure
{
    public static class TypeExtensions
    {
        /// <summary>
        /// 类型相同
        /// </summary>
        /// <param name="type"></param>
        /// <param name="toCompare"></param>
        /// <returns></returns>
        public static bool GenericEq(this Type type, Type toCompare)
        {
            return type.Namespace == toCompare.Namespace && type.Name == toCompare.Name;
        }
    }
}
