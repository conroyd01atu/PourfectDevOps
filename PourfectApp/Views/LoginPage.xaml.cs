using PourfectApp.Models;

namespace PourfectApp.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(UsernameEntry.Text) ||
                string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                await DisplayAlert("Error", "Please enter both username and password", "OK");
                return;
            }

            try
            {
                // Get user from database
                var user = await ServiceHelper.Database.GetUserAsync(UsernameEntry.Text);

                if (user == null)
                {
                    await DisplayAlert("Error", "Invalid username or password", "OK");
                    return;
                }

                // Verify password
                if (!user.VerifyPassword(PasswordEntry.Text))
                {
                    await DisplayAlert("Error", "Invalid username or password", "OK");
                    return;
                }

                // Update last login date
                user.LastLoginDate = DateTime.Now;
                await ServiceHelper.Database.SaveUserAsync(user);

                // Store username in preferences
                Preferences.Set("username", user.Username);
                Preferences.Set("isLoggedIn", true);

                // Navigate to main app
                Application.Current.MainPage = new AppShell();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Login failed: {ex.Message}", "OK");
            }
        }

        private void OnSkipLoginClicked(object sender, EventArgs e)
        {
            // Set guest mode
            Preferences.Set("username", "Guest");
            Preferences.Set("isLoggedIn", false);

            // Navigate to main app
            Application.Current.MainPage = new AppShell();
        }

        private async void OnRegisterTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}