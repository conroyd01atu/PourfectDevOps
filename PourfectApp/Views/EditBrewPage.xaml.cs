using PourfectApp.Models;

namespace PourfectApp.Views
{
    public partial class EditBrewPage : ContentPage
    {
        private Brew brewToEdit;

        public EditBrewPage(Brew brew)
        {
            InitializeComponent();
            brewToEdit = brew;
            LoadBrewData();

            // Handle rating slider changes
            RatingSlider.ValueChanged += OnRatingChanged;
        }

        private void LoadBrewData()
        {
            // Populate all fields with existing data
            BrewDateLabel.Text = brewToEdit.BrewDate.ToString("MMMM dd, yyyy");

            CoffeeNameEntry.Text = brewToEdit.CoffeeName;
            RoasterEntry.Text = brewToEdit.Roaster;
            RoastDatePicker.Date = brewToEdit.RoastDate;

            CoffeeWeightEntry.Text = brewToEdit.CoffeeWeight.ToString();
            WaterWeightEntry.Text = brewToEdit.WaterWeight.ToString();
            GrindSizeEntry.Text = brewToEdit.GrindSize;
            WaterTempEntry.Text = brewToEdit.WaterTemperature.ToString();
            BrewTimeEntry.Text = brewToEdit.BrewTime;

            // Set dripper picker
            for (int i = 0; i < DripperPicker.Items.Count; i++)
            {
                if (DripperPicker.Items[i] == brewToEdit.Dripper)
                {
                    DripperPicker.SelectedIndex = i;
                    break;
                }
            }

            NotesEditor.Text = brewToEdit.Notes;
            RatingSlider.Value = brewToEdit.Rating;
            RatingLabel.Text = $"{brewToEdit.Rating:F1} / 5.0";
        }

        private void OnRatingChanged(object sender, ValueChangedEventArgs e)
        {
            RatingLabel.Text = $"{e.NewValue:F1} / 5.0";
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(CoffeeNameEntry.Text))
            {
                await DisplayAlert("Error", "Please enter the coffee name", "OK");
                return;
            }

            try
            {
                // Update brew object with new values
                brewToEdit.CoffeeName = CoffeeNameEntry.Text;
                brewToEdit.Roaster = RoasterEntry.Text ?? "";
                brewToEdit.RoastDate = RoastDatePicker.Date;
                brewToEdit.CoffeeWeight = double.TryParse(CoffeeWeightEntry.Text, out var cw) ? cw : 0;
                brewToEdit.WaterWeight = double.TryParse(WaterWeightEntry.Text, out var ww) ? ww : 0;
                brewToEdit.GrindSize = GrindSizeEntry.Text ?? "";
                brewToEdit.WaterTemperature = int.TryParse(WaterTempEntry.Text, out var wt) ? wt : 0;
                brewToEdit.BrewTime = BrewTimeEntry.Text ?? "";
                brewToEdit.Dripper = DripperPicker.SelectedItem?.ToString() ?? "V60";
                brewToEdit.Notes = NotesEditor.Text ?? "";
                brewToEdit.Rating = RatingSlider.Value;

                // Save to database
                await ServiceHelper.Database.SaveBrewAsync(brewToEdit);

                await DisplayAlert("Success", "Brew updated successfully!", "OK");

                // Navigate back
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to update brew: {ex.Message}", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Cancel", "Are you sure you want to cancel? Any unsaved changes will be lost.", "Yes", "No");
            if (answer)
            {
                await Navigation.PopAsync();
            }
        }
    }
}