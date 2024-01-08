using Menominee.Domain.Enums;

namespace Menominee.Shared.Models.Pagination;

public class Pagination
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public SortOrder SortOrder { get; set; } = SortOrder.Asc;
    public VehicleSortColumn SortColumn { get; set; } = VehicleSortColumn.Id;

    public Pagination()
    {
    }
}
