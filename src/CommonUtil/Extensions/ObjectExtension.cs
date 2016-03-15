using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtil.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// 指定类型转换
        /// </summary>
        /// <typeparam name="T">指定的类型</typeparam>
        /// <param name="obj">要转换的值</param>
        /// <param name="defobj">为空时的默认值</param>
        /// <param name="diffCase">是否区分大小写，对于特殊类型的转换，如枚举</param>
        /// <returns></returns>
        public static T To<T>(this object obj, object defobj = null, bool diffCase = true) => TypeConvert.To<T>(obj, defobj, diffCase);

        /// <summary>
        /// 指定类型转换
        /// </summary>
        /// <param name="obj">要转换的值</param>
        /// <param name="type">指定的类型</param>
        /// <param name="defobj">为空时的默认值</param>
        /// <param name="diffCase">是否区分大小写，对于特殊类型的转换，如枚举</param>
        /// <returns></returns>
        public static object To(this object obj, Type type, object defobj = null, bool diffCase = true) => TypeConvert.To(type, obj, defobj, diffCase);

        /// <summary>
        /// 转换int类型
        /// </summary>
        /// <param name="obj">要转换的值</param>
        /// <param name="defobj">为空时的默认值</param>
        /// <returns></returns>
        public static int ToInt(this object obj, object defobj = null) => TypeConvert.To<int>(obj, defobj);

        /// <summary>
        /// 转换int?类型
        /// </summary>
        /// <param name="obj">要转换的值</param>
        /// <param name="defobj">为空时的默认值</param>
        /// <returns></returns>
        public static int? ToIntNull(this object obj, object defobj = null) => TypeConvert.To<int?>(obj, defobj);

        /// <summary>
        /// 转换uint类型
        /// </summary>
        /// <param name="obj">要转换的值</param>
        /// <param name="defobj">为空时的默认值</param>
        /// <returns></returns>
        public static uint ToUInt(this object obj, object defobj = null) => TypeConvert.To<uint>(obj, defobj);

        /// <summary>
        /// 转换uint?类型
        /// </summary>
        /// <param name="obj">要转换的值</param>
        /// <param name="defobj">为空时的默认值</param>
        /// <returns></returns>
        public static uint? ToUIntNull(this object obj, object defobj = null) => TypeConvert.To<uint?>(obj, defobj);

        /// <summary>
        /// 转换short类型
        /// </summary>
        /// <param name="obj">要转换的值</param>
        /// <param name="defobj">为空时的默认值</param>
        /// <returns></returns>
        public static short ToShort(this object obj, object defobj = null) => TypeConvert.To<short>(obj, defobj);

        /// <summary>
        /// 转换short?类型
        /// </summary>
        /// <param name="obj">要转换的值</param>
        /// <param name="defobj">为空时的默认值</param>
        /// <returns></returns>
        public static short? ToShortNull(this object obj, object defobj = null) => TypeConvert.To<short?>(obj, defobj);

        /// <summary>
        /// 转换long类型
        /// </summary>
        /// <param name="obj">要转换的值</param>
        /// <param name="defobj">为空时的默认值</param>
        /// <returns></returns>
        public static long ToLong(this object obj, object defobj = null) => TypeConvert.To<long>(obj, defobj);

        /// <summary>
        /// 转换long?类型
        /// </summary>
        /// <param name="obj">要转换的值</param>
        /// <param name="defobj">为空时的默认值</param>
        /// <returns></returns>
        public static long? ToLongNull(this object obj, object defobj = null) => TypeConvert.To<long?>(obj, defobj);

        /// <summary>
        /// 转换ulong类型
        /// </summary>
        /// <param name="obj">要转换的值</param>
        /// <param name="defobj">为空时的默认值</param>
        /// <returns></returns>
        public static ulong ToULong(this object obj, object defobj = null) => TypeConvert.To<ulong>(obj, defobj);

        /// <summary>
        /// 转换ulong?类型
        /// </summary>
        /// <param name="obj">要转换的值</param>
        /// <param name="defobj">为空时的默认值</param>
        /// <returns></returns>
        public static ulong? ToULongNull(this object obj, object defobj = null) => TypeConvert.To<ulong?>(obj, defobj);

        /// <summary>
        /// 转换bool类型
        /// </summary>
        /// <param name="obj">要转换的值</param>
        /// <param name="defobj">为空时的默认值</param>
        /// <returns></returns>
        public static bool ToBool(this object obj, object defobj = null) => TypeConvert.To<bool>(obj, defobj);

        /// <summary>
        /// 转换bool?类型
        /// </summary>
        /// <param name="obj">要转换的值</param>
        /// <param name="defobj">为空时的默认值</param>
        /// <returns></returns>
        public static bool? ToBoolNull(this object obj, object defobj = null) => TypeConvert.To<bool?>(obj, defobj);

        /// <summary>
        /// 转换float类型
        /// </summary>
        /// <param name="obj">要转换的值</param>
        /// <param name="defobj">为空时的默认值</param>
        /// <returns></returns>
        public static float ToFloat(this object obj, object defobj = null) => TypeConvert.To<float>(obj, defobj);

        /// <summary>
        /// 转换float?类型
        /// </summary>
        /// <param name="obj">要转换的值</param>
        /// <param name="defobj">为空时的默认值</param>
        /// <returns></returns>
        public static float? ToFloatNull(this object obj, object defobj = null) => TypeConvert.To<float?>(obj, defobj);

        /// <summary>
        /// 转换double类型
        /// </summary>
        /// <param name="obj">要转换的值</param>
        /// <param name="defobj">为空时的默认值</param>
        /// <returns></returns>
        public static double ToDouble(this object obj, object defobj = null) => TypeConvert.To<double>(obj, defobj);

        /// <summary>
        /// 转换double?类型
        /// </summary>
        /// <param name="obj">要转换的值</param>
        /// <param name="defobj">为空时的默认值</param>
        /// <returns></returns>
        public static double? ToDoubleNull(this object obj, object defobj = null) => TypeConvert.To<double?>(obj, defobj);

        /// <summary>
        /// 转换decimal类型
        /// </summary>
        /// <param name="obj">要转换的值</param>
        /// <param name="defobj">为空时的默认值</param>
        /// <returns></returns>
        public static decimal ToDecimal(this object obj, object defobj = null) => TypeConvert.To<decimal>(obj, defobj);

        /// <summary>
        /// 转换decimal?类型
        /// </summary>
        /// <param name="obj">要转换的值</param>
        /// <param name="defobj">为空时的默认值</param>
        /// <returns></returns>
        public static decimal? ToDecimalNull(this object obj, object defobj = null) => TypeConvert.To<decimal?>(obj, defobj);

        /// <summary>
        /// 转换DateTime类型
        /// </summary>
        /// <param name="obj">要转换的值</param>
        /// <param name="defobj">为空时的默认值</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object obj, object defobj = null) => TypeConvert.To<DateTime>(obj, defobj);

        /// <summary>
        /// 转换DateTime?类型
        /// </summary>
        /// <param name="obj">要转换的值</param>
        /// <param name="defobj">为空时的默认值</param>
        /// <returns></returns>
        public static DateTime? ToDateTimeNull(this object obj, object defobj = null) => TypeConvert.To<DateTime?>(obj, defobj);

        /// <summary>
        /// 转换byte[]类型
        /// </summary>
        /// <param name="obj">要转换的值</param>
        /// <param name="defobj">为空时的默认值</param>
        /// <returns></returns>
        public static byte[] ToByteArray(this object obj, object defobj = null) => TypeConvert.To<byte[]>(obj, defobj);

        /// <summary>
        /// 转换char类型
        /// </summary>
        /// <param name="obj">要转换的值</param>
        /// <param name="defobj">为空时的默认值</param>
        /// <returns></returns>
        public static char ToChar(this object obj, object defobj = null) => TypeConvert.To<char>(obj, defobj);

        /// <summary>
        /// 转换string类型
        /// </summary>
        /// <param name="obj">要转换的值</param>
        /// <param name="defobj">为空时的默认值</param>
        /// <returns></returns>
        public static string ToStringNull(this object obj, object defobj = null) => TypeConvert.To<string>(obj, defobj);

        /// <summary>
        /// 序列化Json字符串（默认Json配置：空值的属性不序列化、Json中存在的属性，实体中不存在的属性不反序列化，避免出错）
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="jsonSettings">指定要序列化的Json配置</param>
        /// <returns></returns>
        public static string ToJson(this object obj, JsonSerializerSettings jsonSettings = null) => obj == null || obj is DBNull ? null : JsonConvert.SerializeObject(obj, jsonSettings ?? DefaultSettings.JsonSerializerSettings);

        /// <summary>
        /// 序列化Json字符串(默认配置)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJsondef(this object obj) => obj == null || obj is DBNull ? null : JsonConvert.SerializeObject(obj);

        /// <summary>
        /// 将对象的公共属性转换为字典
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="isGetNull">是否将NULL值添加到字典</param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDictionary(this object obj, bool isGetNull = false) => obj.ToDictionary<object>(isGetNull);

        /// <summary>
        /// 将对象的公共属性转换为字典
        /// </summary>
        /// <typeparam name="T">字典值类型</typeparam>
        /// <param name="obj">对象</param>
        /// <param name="isGetNull">是否将NULL值添加到字典</param>
        /// <param name="jsonSettings">如果是转换为字符串，对象将会转换为Json串的转换配置</param>
        /// <returns></returns>
        public static Dictionary<string, T> ToDictionary<T>(this object obj, bool isGetNull = false, JsonSerializerSettings jsonSettings = null)
        {
            if (obj == null) return null;
            Dictionary<string, T> dicList = null;
            var type = obj.GetType();
            var piArray = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (piArray.Length > 0)
            {
                for (int i = 0; i < piArray.Length; i++)
                {
                    var pi = piArray[i];
                    if (!pi.CanRead) continue;
                    var value = pi.GetValue(obj);
                    if (value == null && !isGetNull) continue;
                    if (dicList == null) dicList = new Dictionary<string, T>();
                    dicList.Add(pi.Name, value.To<T>());
                }
            }
            var fieldArray = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            if (fieldArray.Length > 0)
            {
                for (int i = 0; i < fieldArray.Length; i++)
                {
                    var field = fieldArray[i];
                    if (field.IsStatic || field.IsPrivate) continue;
                    var value = field.GetValue(obj);
                    if (value == null && !isGetNull) continue;
                    if (dicList == null) dicList = new Dictionary<string, T>();
                    dicList.Add(field.Name, value.To<T>());
                }
            }
            return dicList;
        }

        /// <summary>
        /// 将对象序列化为对象流
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static byte[] ToBuffer(this object obj)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, obj);
            ms.Close();
            return ms.ToArray();
        }

        /// <summary>
        /// 整型判断
        /// </summary>
        /// <param name="obj">整型</param>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <returns></returns>
        public static bool IsMatchedInt(this object obj, int? minValue = null, int? maxValue = null)
        {
            if (!(obj is Int16) && !(obj is Int32) && !(obj is Int64))
                return false;

            if (obj is Int16 && minValue != null && (Int16)obj < minValue)
            {
                return false;
            }

            if (obj is Int16 && maxValue != null && (Int16)obj > maxValue)
            {
                return false;
            }

            if (obj is Int32 && minValue != null && (Int32)obj < minValue)
            {
                return false;
            }

            if (obj is Int32 && maxValue != null && (Int32)obj > maxValue)
            {
                return false;
            }

            if (obj is Int64 && minValue != null && (Int64)obj < minValue)
            {
                return false;
            }

            if (obj is Int64 && maxValue != null && (Int64)obj > maxValue)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 实数判断
        /// </summary>
        /// <param name="obj">实数</param>
        /// <param name="digts">小数位数</param>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <returns></returns>
        public static bool IsMatchedDecimal(this decimal obj, int? digts = null, decimal? minValue = null, decimal? maxValue = null)
        {
            if (digts != null)
            {
                if (obj.ToString().Split('.').Length == 2 && obj.ToString().Split('.')[1].Length > digts)
                    return false;
            }

            if (minValue != null)
            {
                if (obj < minValue)
                    return false;
            }

            if (maxValue != null)
            {
                if (obj > maxValue)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 周期判断
        /// </summary>
        /// <param name="startDate">起始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns></returns>
        public static bool IsMatchedDateTime(this DateTime startDate, DateTime endDate)
        {
            return startDate < endDate;
        }
    }
}
