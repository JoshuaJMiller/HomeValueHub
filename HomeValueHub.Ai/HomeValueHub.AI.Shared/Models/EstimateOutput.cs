using Microsoft.ML.Data;

namespace HomeValueHub.AI.Shared.Models
{
    public class EstimateOutput
    {
        [ColumnName("Score")]
        public float Price;
    }
}