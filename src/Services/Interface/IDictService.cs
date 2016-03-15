using Entity;
using System;
using System.Collections.Generic;

namespace Services.Interface
{
    public interface IDictService
    {
        bool Create(sys_dict model);
        bool Update(sys_dict model);
        bool Delete(int[] dictid);
        /// <summary>
        /// 获取所有字典
        /// </summary>
        /// <returns></returns>
        List<sys_dict> Dicts();
        /// <summary>
        /// 字典分页
        /// </summary>
        /// <param name="predicate">搜索条件</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="total">总记录书</param>
        /// <returns></returns>
        List<sys_dict> Dicts(System.Linq.Expressions.Expression<Func<sys_dict, bool>> predicate, int pageSize, int pageIndex, out int total);
        /// <summary>
        /// 获取指定类型字典
        /// </summary>
        /// <param name="dictclass"></param>
        /// <returns></returns>
        List<sys_dict> DictByClass(int dictclass);
    }
}
