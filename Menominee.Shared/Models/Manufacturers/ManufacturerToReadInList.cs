namespace Menominee.Shared.Models.Manufacturers
{
    public class ManufacturerToReadInList
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Prefix { get; set; }
        public string Name { get; set; }
        public string DisplayText
        {
            get
            {
                return Prefix + " - " + Name;
            }
        }
        //public xxx Country { get; set; }
        //public xxx Franchise { get; set; }
    }
}
