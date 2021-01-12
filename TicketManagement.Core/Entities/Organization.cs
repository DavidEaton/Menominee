namespace TicketManagement.Core.Entities
{
    public class Organization
    {
        public string Name { get; set; }
        public Person Contact { get; set; }
        // Flatten value object
        //public Address Address { get; set; }

        #region ORM

        // EF requires an empty constructor
        protected Organization() { }

        #endregion

    }
}
