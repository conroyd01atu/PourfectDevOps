using SQLite;

namespace PourfectApp.Models
{
    public class Brew
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime BrewDate { get; set; }

        // Coffee Details
        public string CoffeeName { get; set; }
        public string Roaster { get; set; }
        public DateTime RoastDate { get; set; }

        // Brew Parameters
        public double CoffeeWeight { get; set; }
        public double WaterWeight { get; set; }
        public string GrindSize { get; set; }
        public int WaterTemperature { get; set; }
        public string BrewTime { get; set; }
        public string Dripper { get; set; }

        // Evaluation
        public string Notes { get; set; }
        public double Rating { get; set; }

        // Calculated property
        public string Ratio => CoffeeWeight > 0 ? $"1:{WaterWeight / CoffeeWeight:F1}" : "N/A";
    }
}