using Mapster;
using VTU.Infrastructure.Models;

namespace VTU.Infrastructure.Extension;

/// <summary>
/// 分页查询扩展<br/>
/// 1.未实现KeySet分页<br/>
/// 2.未实现对数据排序<br/>
/// https://learn.microsoft.com/zh-cn/ef/core/querying/pagination
/// </summary>
public static class QueryableExtension
{
    /// <summary>
    /// 读取列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query">查询表单式</param>
    /// <param name="pInfo">分页参数</param>
    /// <returns></returns>
    public static PagedInfo<T> ToPage<T>(this IQueryable<T> query, PagerInfo pInfo)
    {
        var total = query.Count();
        var page = new PagedInfo<T>
        {
            PageSize = pInfo.PageSize,
            PageNum = pInfo.PageNum,
            Result = query.Skip((pInfo.PageNum - 1) * pInfo.PageSize).Take(pInfo.PageSize).ToList(),
            TotalNum = total
        };

        return page;
    }

    /// <summary>
    /// 转指定实体类Dto
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="query"></param>
    /// <param name="PInfo"></param>
    /// <returns></returns>
    public static PagedInfo<T2> ToPage<T, T2>(this IQueryable<T> query, PagerInfo PInfo)
    {
        var page = new PagedInfo<T2>();
        var total = query.Count();
        page.PageSize = PInfo.PageSize;
        page.PageNum = PInfo.PageNum;
        page.TotalNum = total;
        page.Result = query.Skip((PInfo.PageNum - 1) * PInfo.PageSize).Take(PInfo.PageSize).ToList().Adapt<List<T2>>();
        return page;
    }
}