using System.ComponentModel.DataAnnotations;

namespace HomeValueHub.Shared.Models
{
    public class HomeDetail
    {
        public HomeDetail()
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
        [Range(1, double.MaxValue, ErrorMessage = "Finished Square Feet must be greater that 0.")]
        public decimal FinishedSquareFeet { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Total Rooms must be greater than 0.")]
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
