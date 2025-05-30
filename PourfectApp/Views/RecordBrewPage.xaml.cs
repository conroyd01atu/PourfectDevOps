using PourfectApp.Models;

namespace PourfectApp.Views
{
    public partial class RecordBrewPage : ContentPage
    {
        public RecordBrewPage()
        {
            InitializeComponent();

            // Set default values
            RoastDatePicker.Date = DateTime.Today;
            DripperPicker.SelectedIndex = 0; // V60

            // Handle rating slider changes
            RatingSlider.ValueChanged += OnRatingChanged;
        }

        private void OnRatingChanged(object sender, ValueChangedEventArgs e)
        {
            RatingLabel.Text = $"{e.NewValue:F1} / 5.0";
        }

        private async void OnSaveBrewClicked(object sender, EventArgs e)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(CoffeeNameEntry.Text))
            {
                await DisplayAlert("Error", "Please enter the coffee name", "OK");
                return;
            }

            try
            {
                // Create brew object
                var brew = new Brew
                {
                    Username = Preferences.Get("username", "Guest"),
                    BrewDate = DateTime.Now,
                    CoffeeName = CoffeeNameEntry.Text,
                    Roaster = RoasterEntry.Text ?? "",
                    RoastDate = RoastDatePicker.Date,
                    CoffeeWeight = double.TryParse(CoffeeWeightEntry.Text, out var cw) ? cw : 0,
                    WaterWeight = double.TryParse(WaterWeightEntry.Text, out var ww) ? ww : 0,
                    GrindSize = GrindSizeEntry.Text ?? "",
                    WaterTemperature = int.TryParse(WaterTempEntry.Text, out var wt) ? wt : 0,
                    BrewTime = BrewTimeEntry.Text ?? "",
                    Dripper = DripperPicker.SelectedItem?.ToString() ?? "V60",
                    Notes = NotesEditor.Text ?? "",
                    Rating = RatingSlider.Value
                };

                // Save to database
                await ServiceHelper.Database.SaveBrewAsync(brew);

                await DisplayAlert("Success", $"Brew recorded for {CoffeeNameEntry.Text}!", "OK");

                // Clear form
                ClearForm();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to save brew: {ex.Message}", "OK");
            }
        }

        private void ClearForm()
        {
            CoffeeNameEntry.Text = "";
            RoasterEntry.Text = "";
            RoastDatePicker.Date = DateTime.Today;
            CoffeeWeightEntry.Text = "";
            WaterWeightEntry.Text = "";
            GrindSizeEntry.Text = "";
            WaterTempEntry.Text = "";
            BrewTimeEntry.Text = "";
            DripperPicker.SelectedIndex = 0;
            NotesEditor.Text = "";
            RatingSlider.Value = 3;
        }
    }
}