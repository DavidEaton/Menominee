namespace Menominee.Idp.Areas.Identity.Data
{
    public interface IConcurrencyAware
    {
        string ConcurrencyStamp { get; set; }
    }
}
