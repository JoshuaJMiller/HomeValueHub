using System.ComponentModel.DataAnnotations;

namespace HomeValueHub.Shared.Models
{
    public class HomeFeatures
    {
        [Required]
        public int Bedrooms { get; set; }

        [Required]
        public int Bathrooms { get; set; }

        [Required]
        public decimal FinishedSquareFeet { get; set; }

        public decimal LastSoldPrice { get; set; }
        
        public int TotalRooms { get; set; }
    }
}
