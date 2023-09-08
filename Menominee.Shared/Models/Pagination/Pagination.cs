using Menominee.Common.Enums;

namespace Menominee.Shared.Models.Pagination;

public class Pagination
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public SortOrder SortOrder { get; set; } = SortOrder.Asc;
    public SortColumn SortColumn { get; set; } = SortColumn.Id;

    public Pagination()
    {
    }
}
