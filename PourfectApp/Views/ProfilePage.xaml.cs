namespace PourfectApp.Views
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadUserStats();
            UpdateUI();
        }

        private async Task LoadUserStats()
        {
            try
            {
                string username = Preferences.Get("username", "Guest");
                bool isLoggedIn = Preferences.Get("isLoggedIn", false);

                UsernameLabel.Text = username;
                UserTypeLabel.Text = isLoggedIn ? "Registered User" : "Guest User";

                // Load stats
                var brews = await ServiceHelper.Database.GetBrewsByUserAsync(username);
                TotalBrewsLabel.Text = brews.Count.ToString();

                var recipes = await ServiceHelper.Database.GetRecipesByUserAsync(username);
                TotalRecipesLabel.Text = recipes.Count.ToString();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load stats: {ex.Message}", "OK");
            }
        }

        private void UpdateUI()
        {
            bool isLoggedIn = Preferences.Get("isLoggedIn", false);

            LoginRegisterButton.IsVisible = !isLoggedIn;
            LogoutButton.IsVisible = isLoggedIn;
        }

        private async void OnLoginRegisterClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
            if (answer)
            {
                // Clear preferences
                Preferences.Set("username", "Guest");
                Preferences.Set("isLoggedIn", false);

                // Navigate to login page
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }

        private async void OnExportDataClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Export Data", "Data export functionality coming soon! This will allow you to export your brews and recipes as CSV files.", "OK");
        }

        private async void OnAboutClicked(object sender, EventArgs e)
        {
            await DisplayAlert("About Pourfect",
                "Pourfect v1.0\n\n" +
                "Your personal pour-over coffee companion.\n\n" +
                "Track your brews, save recipes, and perfect your coffee technique.\n\n" +
                "Created with ☕ and ❤️",
                "OK");
        }
    }
}