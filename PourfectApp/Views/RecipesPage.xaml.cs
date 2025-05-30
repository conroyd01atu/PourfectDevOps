using System.Collections.ObjectModel;
using System.Windows.Input;
using PourfectApp.Models;

namespace PourfectApp.Views
{
    public partial class RecipesPage : ContentPage
    {
        private ObservableCollection<Recipe> allRecipes;
        private ObservableCollection<Recipe> filteredRecipes;
        private string currentFilter = "All";

        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public RecipesPage()
        {
            InitializeComponent();

            allRecipes = new ObservableCollection<Recipe>();
            filteredRecipes = new ObservableCollection<Recipe>();
            RecipesCollectionView.ItemsSource = filteredRecipes;

            // Set up commands
            EditCommand = new Command<Recipe>(async (recipe) => await EditRecipe(recipe));
            DeleteCommand = new Command<Recipe>(async (recipe) => await DeleteRecipe(recipe));
            RecipesCollectionView.BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadRecipes();
        }

        private async Task LoadRecipes()
        {
            try
            {
                // Get current user
                string username = Preferences.Get("username", "Guest");

                // Load all recipes from database
                var recipes = await ServiceHelper.Database.GetRecipesAsync();

                // Filter by current user
                var userRecipes = recipes.Where(r => r.CreatedBy == username).ToList();

                // Update collections
                allRecipes.Clear();
                foreach (var recipe in userRecipes)
                {
                    allRecipes.Add(recipe);
                }

                // Apply current filter
                ApplyFilter();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load recipes: {ex.Message}", "OK");
            }
        }

        private void ApplyFilter()
        {
            filteredRecipes.Clear();

            IEnumerable<Recipe> filtered = currentFilter switch
            {
                "All" => allRecipes,
                "Favorites" => allRecipes.Where(r => r.IsFavorite),
                _ => allRecipes.Where(r => r.Method == currentFilter)
            };

            foreach (var recipe in filtered)
            {
                filteredRecipes.Add(recipe);
            }
        }

        private void OnFilterClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is string filter)
            {
                currentFilter = filter;

                // Define colors
                var selectedBgColor = Color.FromArgb("#4A2C17");
                var unselectedBgColor = Colors.White;
                var selectedTextColor = Colors.White;
                var unselectedTextColor = Color.FromArgb("#4A2C17");

                // Update button states
                foreach (var child in FilterButtons.Children)
                {
                    if (child is Button btn)
                    {
                        bool isSelected = btn.CommandParameter?.ToString() == filter;
                        btn.BackgroundColor = isSelected ? selectedBgColor : unselectedBgColor;
                        btn.TextColor = isSelected ? selectedTextColor : unselectedTextColor;
                    }
                }

                ApplyFilter();
            }
        }

        private async void OnAddRecipeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateRecipePage());
        }

        private async void OnFavoriteClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Recipe recipe)
            {
                recipe.IsFavorite = !recipe.IsFavorite;
                await ServiceHelper.Database.SaveRecipeAsync(recipe);

                // Update the display
                button.Text = recipe.FavoriteIcon;

                // If we're viewing favorites and unfavorited, remove from view
                if (currentFilter == "Favorites" && !recipe.IsFavorite)
                {
                    filteredRecipes.Remove(recipe);
                }
            }
        }

        private async void OnRecipeTapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is Recipe recipe)
            {
                await Navigation.PushAsync(new RecipeDetailPage(recipe));
            }
        }

        private async Task EditRecipe(Recipe recipe)
        {
            // For now, show alert - you can create an EditRecipePage later
            await DisplayAlert("Edit Recipe", $"Edit functionality for {recipe.Name} coming soon!", "OK");
        }

        private async Task DeleteRecipe(Recipe recipe)
        {
            bool answer = await DisplayAlert("Delete Recipe",
                $"Are you sure you want to delete '{recipe.Name}'?",
                "Yes", "No");

            if (answer)
            {
                try
                {
                    await ServiceHelper.Database.DeleteRecipeAsync(recipe);
                    allRecipes.Remove(recipe);
                    filteredRecipes.Remove(recipe);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to delete recipe: {ex.Message}", "OK");
                }
            }
        }
    }
}