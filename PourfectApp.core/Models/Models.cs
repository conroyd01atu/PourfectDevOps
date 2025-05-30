using SQLite;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace PourfectApp.Core.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique]
        public string Username { get; set; }

        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastLoginDate { get; set; }

        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public bool VerifyPassword(string password)
        {
            return HashPassword(password) == PasswordHash;
        }
    }

    public class Brew
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime BrewDate { get; set; }
        public string CoffeeName { get; set; }
        public string Roaster { get; set; }
        public DateTime RoastDate { get; set; }
        public double CoffeeWeight { get; set; }
        public double WaterWeight { get; set; }
        public string GrindSize { get; set; }
        public int WaterTemperature { get; set; }
        public string BrewTime { get; set; }
        public string Dripper { get; set; }
        public string Notes { get; set; }
        public double Rating { get; set; }

        [Ignore]
        public string Ratio => CoffeeWeight > 0 ? $"1:{WaterWeight / CoffeeWeight:F1}" : "N/A";
    }

    public class Recipe
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Method { get; set; }
        public double CoffeeWeight { get; set; }
        public double WaterWeight { get; set; }
        public int Temperature { get; set; }
        public string TotalTime { get; set; }
        public string GrindSize { get; set; }
        public bool IsFavorite { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string StepsJson { get; set; }
        public string ParametersJson { get; set; }

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
        public string TimeRange { get; set; }
        public string Description { get; set; }
        public double WaterAmount { get; set; }
    }

    public class RecipeParameters
    {
        public string BloomTime { get; set; }
        public double BloomWater { get; set; }
        public string PourTechnique { get; set; }
        public int NumberOfPours { get; set; }
        public string FilterType { get; set; }
        public string Notes { get; set; }
    }
}