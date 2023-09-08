using System.Collections.Generic;
using System.Linq;

namespace Menominee.Shared.Models.Pagination;

public class PagedList<T>
{
    private PagedList(List<T> items, int page, int pageSize, int totalPages)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalPages = totalPages;
    }

    public List<T> Items { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;

    public static PagedList<T> Create(IQueryable<T> query, int page, int pageSize)
    {
        var defaultPage = 1;
        var defaultPageSize = 10;

        page = page <= 0 ? defaultPage : page;
        pageSize = pageSize <= 0 ? defaultPageSize : pageSize;

        var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        var totalCount = query.Count();
        var totalPages = (int)System.Math.Ceiling(totalCount / (double)pageSize);

        return new PagedList<T>(items, page, pageSize, totalPages);
    }

    public PagedList()
    {
        Items = new List<T>();
        Page = 1;
        PageSize = 10;
        TotalPages = 1;
    }
}
