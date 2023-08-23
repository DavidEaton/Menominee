

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

    public List<T> Items { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int TotalPages { get; }
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;

    public static PagedList<T> Create(IQueryable<T> query, int page, int pageSize)
    {
        page = page < 0
            ? 1
            : page;

        var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        var totalCount = query.Count();
        var totalPages = (int)System.Math.Ceiling(totalCount / (double)pageSize);

        return new PagedList<T>(items, page, pageSize, totalPages);
    }
}
