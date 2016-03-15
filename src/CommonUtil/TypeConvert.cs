using System;
using Newtonsoft.Json;
using CommonUtil.Extensions;

namespace CommonUtil
{
    public static class TypeConvert
    {
        /// <summary>
        /// 指定类型转换
        /// </summary>
        /// <typeparam name="T">要转换的类型</typeparam>
        /// <param name="obj">要转换的值</param>
        /// <param name="defobj">为空时默认值</param>
        /// <param name="diffCase">是否区分大小写，对于特殊类型的转换，如枚举</param>
        /// <returns></returns>
        public static T To<T>(object obj, object defobj = null, bool diffCase = true) => (T)To(typeof(T), obj, defobj);

        /// <summary>
        /// 转换类型
        /// </summary>
        /// <param name="type">要转换的类型</param>
        /// <param name="obj">要转换的值</param>
        /// <param name="defobj">默认值</param>
        /// <param name="diffCase">是否区分大小写，对于特殊类型的转换，如枚举</param>
        /// <returns></returns>
        public static object To(Type type, object obj, object defobj = null, bool diffCase = true)
        {
            var isNull = obj == null || obj is DBNull;
            if (isNull && defobj != null && defobj != DBNull.Value)
            {
                if (defobj.GetType() == type) return defobj;//返回默认值
                obj = defobj;
                isNull = false;
            }

            //如果转换为object类型，就直接返回
            if (type == typeof(object)) return obj;

            //常用类型转换
            if (isNull)
            {
                //枚举特殊处理，默认第一项
                if (type.IsEnum)
                {
                    string[] eNames = Enum.GetNames(type);
                    if (eNames.Length > 0) return Enum.Parse(type, eNames[0]);
                }
                if (type.IsValueType) return Activator.CreateInstance(type);
                return null;
            }

            var oType = obj.GetType();
            if (type == oType) return obj;
            var cType = type.GenericTypeArguments.Length > 0 ? type.GenericTypeArguments[0] : type;
            if (cType == oType) return obj;
            if (cType.IsEnum) return Enum.Parse(cType, obj.ToString().Trim(), !diffCase);
            if (cType == typeof(string))
            {
                if (oType.IsClass && oType != typeof(string)) return obj.ToJson();
                return obj.ToString();
            }
            if (cType.IsClass)
            {
                if (oType == typeof(string)) return string.IsNullOrEmpty(obj.ToString()) ? null : JsonConvert.DeserializeObject(obj.ToString(), cType);
            }
            else
            {
                if (cType == typeof(int)) return Convert.ToInt32(obj);
                if (cType == typeof(bool)) return Convert.ToBoolean(obj);
                if (cType == typeof(DateTime)) return Convert.ToDateTime(obj);
                if (cType == typeof(float)) return (float)Convert.ToDouble(obj);
                if (cType == typeof(decimal)) return Convert.ToDecimal(obj);
                if (cType == typeof(double)) return Convert.ToDouble(obj);
                if (cType == typeof(short)) return Convert.ToInt16(obj);
                if (cType == typeof(long)) return Convert.ToInt64(obj);
                if (cType == typeof(uint)) return Convert.ToUInt32(obj);
                if (cType == typeof(ulong)) return Convert.ToUInt64(obj);
                if (cType == typeof(ushort)) return Convert.ToUInt16(obj);
                if (cType == typeof(char)) return Convert.ToChar(obj);
            }

            return obj;
        }
    }
}
