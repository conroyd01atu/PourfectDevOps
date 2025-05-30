using PourfectApp.Views;

namespace PourfectApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Check if user is already logged in
            bool isLoggedIn = Preferences.Get("isLoggedIn", false);

            if (isLoggedIn)
            {
                // User is logged in, go straight to main app
                MainPage = new AppShell();
            }
            else
            {
                // Show login page
                MainPage = new LoginPage();
            }
        }
    }
}
