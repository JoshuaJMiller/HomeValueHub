using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HomeValueHub.Shared.Models
{
    public class HomeValueEstimate
    {
        [JsonPropertyName("salePrice")]
        public decimal SalePrice { get; set; }
    }
}
