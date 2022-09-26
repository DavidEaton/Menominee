namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemTire : InstallablePart
    {
        public string Type { get; private set; }
        public double Width { get; private set; }
        public double AspectRatio { get; private set; }
        public double Diameter { get; private set; }
        public int LoadIndex { get; private set; }
        public string SpeedRating { get; private set; }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemTire() { }

        #endregion
    }
}
