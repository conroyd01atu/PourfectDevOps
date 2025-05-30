using SQLite;
using System.Text.Json;

namespace PourfectApp.Models
{
    public class Recipe
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Method { get; set; } // V60, Chemex, etc.
        public double CoffeeWeight { get; set; }
        public double WaterWeight { get; set; }
        public int Temperature { get; set; }
        public string TotalTime { get; set; }
        public string GrindSize { get; set; }
        public bool IsFavorite { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } // Username

        // Store steps as JSON
        public string StepsJson { get; set; }

        // Store additional parameters as JSON
        public string ParametersJson { get; set; }

        // Calculated properties
        [Ignore]
        public string Ratio => CoffeeWeight > 0 ? $"1:{WaterWeight / CoffeeWeight:F1}" : "N/A";

        [Ignore]
        public string FavoriteIcon => IsFavorite ? "⭐" : "☆";

        [Ignore]
        public List<RecipeStep> Steps
        {
            get => string.IsNullOrEmpty(StepsJson)
                ? new List<RecipeStep>()
                : JsonSerializer.Deserialize<List<RecipeStep>>(StepsJson);
            set => StepsJson = JsonSerializer.Serialize(value);
        }

        [Ignore]
        public RecipeParameters Parameters
        {
            get => string.IsNullOrEmpty(ParametersJson)
                ? new RecipeParameters()
                : JsonSerializer.Deserialize<RecipeParameters>(ParametersJson);
            set => ParametersJson = JsonSerializer.Serialize(value);
        }
    }

    public class RecipeStep
    {
        public int StepNumber { get; set; }
        public string TimeRange { get; set; } // e.g., "0:00-0:30"
        public string Description { get; set; }
        public double WaterAmount { get; set; } // grams of water for this step
    }

    public class RecipeParameters
    {
        public string BloomTime { get; set; }
        public double BloomWater { get; set; }
        public string PourTechnique { get; set; } // "continuous", "pulse", etc.
        public int NumberOfPours { get; set; }
        public string FilterType { get; set; }
        public string Notes { get; set; }
    }
}