using System.Collections.ObjectModel;
using PourfectApp.Models;

namespace PourfectApp.Views
{
    public partial class PastBrewsPage : ContentPage
    {
        private ObservableCollection<Brew> allBrews;
        private ObservableCollection<Brew> filteredBrews;

        public PastBrewsPage()
        {
            InitializeComponent();
            allBrews = new ObservableCollection<Brew>();
            filteredBrews = new ObservableCollection<Brew>();
            BrewsCollectionView.ItemsSource = filteredBrews;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadBrews();
        }

        private async Task LoadBrews()
        {
            try
            {
                // Get current user
                string username = Preferences.Get("username", "Guest");

                // Load brews from database
                var brewList = await ServiceHelper.Database.GetBrewsByUserAsync(username);

                // Update collections
                allBrews.Clear();
                foreach (var brew in brewList)
                {
                    allBrews.Add(brew);
                }

                // Apply search filter if any
                FilterBrews();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load brews: {ex.Message}", "OK");
            }
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            FilterBrews();
        }

        private void FilterBrews()
        {
            string searchText = BrewSearchBar.Text?.ToLower() ?? "";

            filteredBrews.Clear();

            var filtered = string.IsNullOrWhiteSpace(searchText)
                ? allBrews
                : allBrews.Where(b =>
                    b.CoffeeName?.ToLower().Contains(searchText) == true ||
                    b.Roaster?.ToLower().Contains(searchText) == true ||
                    b.Dripper?.ToLower().Contains(searchText) == true ||
                    b.Notes?.ToLower().Contains(searchText) == true ||
                    b.GrindSize?.ToLower().Contains(searchText) == true);

            foreach (var brew in filtered)
            {
                filteredBrews.Add(brew);
            }
        }

        private async void OnBrewTapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is Brew brew)
            {
                var action = await DisplayActionSheet(
                    "Brew Details",
                    "Cancel",
                    null,
                    "View Details",
                    "Edit",
                    "Delete",
                    "Brew Again");

                switch (action)
                {
                    case "View Details":
                        await ShowBrewDetails(brew);
                        break;
                    case "Edit":
                        await EditBrew(brew);
                        break;
                    case "Delete":
                        await DeleteBrew(brew);
                        break;
                    case "Brew Again":
                        await BrewAgain(brew);
                        break;
                }
            }
        }

        private async Task ShowBrewDetails(Brew brew)
        {
            string details = $"Coffee: {brew.CoffeeName}\n" +
                           $"Roaster: {brew.Roaster}\n" +
                           $"Roast Date: {brew.RoastDate:MMM dd, yyyy}\n\n" +
                           $"Method: {brew.Dripper}\n" +
                           $"Ratio: {brew.CoffeeWeight}g : {brew.WaterWeight}g ({brew.Ratio})\n" +
                           $"Temperature: {brew.WaterTemperature}°C\n" +
                           $"Grind: {brew.GrindSize}\n" +
                           $"Time: {brew.BrewTime}\n\n" +
                           $"Rating: {brew.Rating:F1}/5.0\n\n";

            if (!string.IsNullOrWhiteSpace(brew.Notes))
            {
                details += $"Notes:\n{brew.Notes}";
            }

            await DisplayAlert($"Brew from {brew.BrewDate:MMM dd}", details, "OK");
        }

        private async void OnEditBrewClicked(object sender, EventArgs e)
        {
            if (sender is SwipeItem swipeItem && swipeItem.CommandParameter is Brew brew)
            {
                await EditBrew(brew);
            }
        }

        private async Task EditBrew(Brew brew)
        {
            await Navigation.PushAsync(new EditBrewPage(brew));
        }

        private async void OnDeleteBrewClicked(object sender, EventArgs e)
        {
            if (sender is SwipeItem swipeItem && swipeItem.CommandParameter is Brew brew)
            {
                await DeleteBrew(brew);
            }
        }

        private async Task DeleteBrew(Brew brew)
        {
            bool answer = await DisplayAlert(
                "Delete Brew",
                $"Are you sure you want to delete this brew of {brew.CoffeeName}?",
                "Yes",
                "No");

            if (answer)
            {
                try
                {
                    await ServiceHelper.Database.DeleteBrewAsync(brew);
                    allBrews.Remove(brew);
                    filteredBrews.Remove(brew);

                    // Show brief confirmation
                    var successLabel = new Label
                    {
                        Text = "Brew deleted",
                        BackgroundColor = Color.FromArgb("#4CAF50"),
                        TextColor = Colors.White,
                        Padding = 10,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.End,
                        Margin = new Thickness(0, 0, 0, 50)
                    };

                    (this.Content as Grid).Children.Add(successLabel);

                    await Task.Delay(2000);
                    (this.Content as Grid).Children.Remove(successLabel);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to delete brew: {ex.Message}", "OK");
                }
            }
        }

        private async Task BrewAgain(Brew brew)
        {
            // Navigate to Record page with pre-filled values
            var recordPage = new RecordBrewPage();
            await Navigation.PushAsync(recordPage);

            await DisplayAlert(
                "Brew Again",
                $"Use these parameters:\n\n" +
                $"Coffee: {brew.CoffeeName}\n" +
                $"{brew.CoffeeWeight}g : {brew.WaterWeight}g\n" +
                $"Temp: {brew.WaterTemperature}°C\n" +
                $"Grind: {brew.GrindSize}",
                "OK");
        }
    }
}