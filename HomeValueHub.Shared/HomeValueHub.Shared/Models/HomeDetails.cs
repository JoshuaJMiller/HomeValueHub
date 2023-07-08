using System.ComponentModel.DataAnnotations;

namespace HomeValueHub.Shared.Models
{
    public class HomeDetails
    {
        public HomeDetails()
        {
            Features = new HomeFeatures();
            Location = new HomeLocation();
            History = new HomeHistory();
        }

        public HomeFeatures Features { get; set; }
        public HomeLocation Location { get; set; }
        public HomeHistory History { get; set; }
    }

    public class HomeFeatures
    {
        public HomeFeatures()
        {
            // default to studio
            Bedrooms = 0;
            Bathrooms = 1;
        }

        [Required]
        public int Bedrooms { get; set; }

        [Required]
        public int Bathrooms { get; set; }

        [Required]
        public decimal FinishedSquareFeet { get; set; }

        [Required]
        public int TotalRooms { get; set; }
    }

    public class HomeLocation
    {
        [Required]
        public string ZipCode { get; set; }
    }

    public class HomeHistory
    {
        public decimal LastSoldPrice { get; set; }
    }
}
