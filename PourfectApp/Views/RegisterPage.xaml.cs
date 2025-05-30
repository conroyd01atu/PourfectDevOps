using PourfectApp.Models;

namespace PourfectApp.Views
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(UsernameEntry.Text))
            {
                await DisplayAlert("Error", "Please enter a username", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(EmailEntry.Text))
            {
                await DisplayAlert("Error", "Please enter an email address", "OK");
                return;
            }

            if (!EmailEntry.Text.Contains("@"))
            {
                await DisplayAlert("Error", "Please enter a valid email address", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                await DisplayAlert("Error", "Please enter a password", "OK");
                return;
            }

            if (PasswordEntry.Text.Length < 6)
            {
                await DisplayAlert("Error", "Password must be at least 6 characters long", "OK");
                return;
            }

            if (PasswordEntry.Text != ConfirmPasswordEntry.Text)
            {
                await DisplayAlert("Error", "Passwords do not match", "OK");
                return;
            }

            try
            {
                // Check if username already exists
                bool userExists = await ServiceHelper.Database.UserExistsAsync(UsernameEntry.Text);
                if (userExists)
                {
                    await DisplayAlert("Error", "Username already taken. Please choose another.", "OK");
                    return;
                }

                // Create new user
                var user = new User
                {
                    Username = UsernameEntry.Text,
                    Email = EmailEntry.Text,
                    PasswordHash = User.HashPassword(PasswordEntry.Text),
                    CreatedDate = DateTime.Now,
                    LastLoginDate = DateTime.Now
                };

                // Save to database
                await ServiceHelper.Database.SaveUserAsync(user);

                // Store login info
                Preferences.Set("username", user.Username);
                Preferences.Set("isLoggedIn", true);

                await DisplayAlert("Success", "Account created successfully!", "OK");

                // Navigate to main app
                Application.Current.MainPage = new AppShell();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Registration failed: {ex.Message}", "OK");
            }
        }

        private async void OnLoginTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
