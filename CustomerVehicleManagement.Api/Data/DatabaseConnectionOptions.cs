namespace CustomerVehicleManagement.Api.Data
{
    internal class DatabaseConnectionOptions
    {
        public string Server { get; set; }
        public string DatabaseName { get; set; }
        public bool IntegratedSecurity { get; set; }
        public string Password { get; set; }
        public string UserId { get; set; }
        public bool PersistSecurityInfo = false;
        public bool MultipleActiveResultSets = false;
        public bool Encrypt = true;
        public bool TrustServerCertificate { get; set; }
        public int ConnectTimeout = 30;
    }
}
