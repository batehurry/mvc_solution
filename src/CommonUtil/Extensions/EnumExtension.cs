using System;
using System.Reflection;

namespace CommonUtil.Extensions
{
    public static class EnumExtension
    {
        public static string GetEnumRemark(this Enum _enum)
        {
            Type type = _enum.GetType();
            //获得对元数据的访问
            FieldInfo fd = type.GetField(_enum.ToString());
            if (fd == null) return string.Empty;
            object[] attrs = fd.GetCustomAttributes(typeof(RemarkAttribute), false);
            string name = string.Empty;
            foreach (RemarkAttribute attr in attrs)
            {
                name = attr.Remark;
            }
            return name;
        }
    }
}
