namespace TestingHelperLibrary
{
    public static class VehicleTestHelper
    {
        public static readonly Dictionary<int, string> Makers = new()
        {
            {1, "Toyota"},
            {2, "Volkswagen"},
            {3, "General Motors"},
            {4, "Hyundai"},
            {5, "Ford"},
            {6, "Honda"},
            {7, "Stellantis"},
            {8, "BMW"},
            {9, "Mercedes-Benz"},
            {10, "Nissan"},
            {11, "Renault"},
            {12, "Tesla"},
            {13, "Subaru"},
            {14, "Suzuki"},
            {15, "Audi"},
            {16, "Volvo"},
            {17, "Mazda"},
            {18, "Porsche"},
            {19, "Land Rover"},
            {20, "Mitsubishi"}
        };

        public static readonly Dictionary<int, List<string>> Models = new()
        {
            {1, new List<string> {"Camry", "Corolla", "Prius"}},
            {2, new List<string> {"Golf", "Passat", "Tiguan"}},
            {3, new List<string> {"Silverado", "Equinox", "Malibu"}},
            {4, new List<string> {"Elantra", "Sonata", "Tucson"}},
            {5, new List<string> {"F-150", "Explorer", "Mustang"}},
            {6, new List<string> {"Accord", "Civic", "CR-V"}},
            {7, new List<string> {"Jeep Wrangler", "RAM 1500", "Chrysler 300"}},
            {8, new List<string> {"3 Series", "5 Series", "X5"}},
            {9, new List<string> {"C-Class", "E-Class", "S-Class"}},
            {10, new List<string> {"Altima", "Rogue", "Sentra"}},
            {11, new List<string> {"Clio", "Captur", "Megane"}},
            {12, new List<string> {"Model 3", "Model S", "Model X"}},
            {13, new List<string> {"Outback", "Forester", "Impreza"}},
            {14, new List<string> {"Swift", "Vitara", "Baleno"}},
            {15, new List<string> {"A4", "A6", "Q5"}},
            {16, new List<string> {"XC90", "S60", "V60"}},
            {17, new List<string> {"Mazda3", "Mazda6", "CX-5"}},
            {18, new List<string> {"911", "Cayenne", "Panamera"}},
            {19, new List<string> {"Range Rover", "Discovery", "Evoque"}},
            {20, new List<string> {"Outlander", "Eclipse Cross", "Lancer"}}
        };
    }
}
