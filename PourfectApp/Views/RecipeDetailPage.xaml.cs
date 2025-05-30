using PourfectApp.Models;

namespace PourfectApp.Views
{
    public partial class RecipeDetailPage : ContentPage
    {
        private Recipe recipe;

        public RecipeDetailPage(Recipe recipe)
        {
            InitializeComponent();
            this.recipe = recipe;
            BindingContext = recipe;

            LoadRecipeDetails();
        }

        private void LoadRecipeDetails()
        {
            // Load advanced parameters
            if (recipe.Parameters != null)
            {
                int row = 0;

                if (!string.IsNullOrEmpty(recipe.Parameters.BloomTime))
                {
                    AddParameterRow("Bloom Time", recipe.Parameters.BloomTime, row++);
                }

                if (recipe.Parameters.BloomWater > 0)
                {
                    AddParameterRow("Bloom Water", $"{recipe.Parameters.BloomWater}g", row++);
                }

                if (!string.IsNullOrEmpty(recipe.Parameters.PourTechnique))
                {
                    AddParameterRow("Pour Technique", recipe.Parameters.PourTechnique, row++);
                }

                if (!string.IsNullOrEmpty(recipe.Parameters.FilterType))
                {
                    AddParameterRow("Filter Type", recipe.Parameters.FilterType, row++);
                }

                // Hide frame if no parameters
                if (row == 0)
                {
                    AdvancedParamsFrame.IsVisible = false;
                }
            }
            else
            {
                AdvancedParamsFrame.IsVisible = false;
            }

            // Load steps
            if (recipe.Steps != null && recipe.Steps.Any())
            {
                foreach (var step in recipe.Steps.OrderBy(s => s.StepNumber))
                {
                    AddStepView(step);
                }
            }
            else
            {
                // Add a placeholder if no steps
                var noStepsLabel = new Label
                {
                    Text = "No detailed steps provided for this recipe.",
                    TextColor = Color.FromArgb("#999"),
                    FontAttributes = FontAttributes.Italic
                };
                StepsContainer.Children.Add(noStepsLabel);
            }

            // Show notes if available
            if (!string.IsNullOrEmpty(recipe.Parameters?.Notes))
            {
                NotesLabel.Text = recipe.Parameters.Notes;
                NotesFrame.IsVisible = true;
            }
        }

        private void AddParameterRow(string label, string value, int row)
        {
            // Add row definition
            ParametersGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Add label
            var labelView = new Label
            {
                Text = label,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#666")
            };
            ParametersGrid.Add(labelView, 0, row);

            // Add value
            var valueView = new Label
            {
                Text = value,
                HorizontalOptions = LayoutOptions.End,
                TextColor = Color.FromArgb("#333")
            };
            ParametersGrid.Add(valueView, 1, row);
        }

        private void AddStepView(RecipeStep step)
        {
            var stepFrame = new Frame
            {
                BackgroundColor = Color.FromArgb("#F9F9F9"),
                CornerRadius = 5,
                Padding = 10,
                HasShadow = false
            };

            var stepGrid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star }
                },
                ColumnSpacing = 10
            };

            // Step number circle
            var numberFrame = new Frame
            {
                BackgroundColor = Color.FromArgb("#8B4513"),
                CornerRadius = 15,
                Padding = 0,
                WidthRequest = 30,
                HeightRequest = 30,
                HasShadow = false
            };

            var numberLabel = new Label
            {
                Text = step.StepNumber.ToString(),
                TextColor = Colors.White,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            numberFrame.Content = numberLabel;
            stepGrid.Add(numberFrame, 0, 0);

            // Step details
            var detailsStack = new VerticalStackLayout { Spacing = 5 };

            // Time range
            if (!string.IsNullOrEmpty(step.TimeRange))
            {
                var timeLabel = new Label
                {
                    Text = step.TimeRange,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.FromArgb("#4A2C17")
                };
                detailsStack.Children.Add(timeLabel);
            }

            // Description
            var descLabel = new Label
            {
                Text = step.Description,
                TextColor = Color.FromArgb("#666")
            };
            detailsStack.Children.Add(descLabel);

            // Water amount
            if (step.WaterAmount > 0)
            {
                var waterLabel = new Label
                {
                    Text = $"💧 {step.WaterAmount}g water",
                    TextColor = Color.FromArgb("#8B4513"),
                    FontSize = 12
                };
                detailsStack.Children.Add(waterLabel);
            }

            stepGrid.Add(detailsStack, 1, 0);
            stepFrame.Content = stepGrid;
            StepsContainer.Children.Add(stepFrame);
        }

        private async void OnBrewThisClicked(object sender, EventArgs e)
        {
            // Navigate to Record Brew page with pre-filled values from this recipe
            var recordPage = new RecordBrewPage();

            // Pre-fill the form (you'll need to make the entries public or add a method)
            // For now, just navigate
            await Navigation.PushAsync(recordPage);

            await DisplayAlert("Tip",
                $"Use these parameters for your brew:\n" +
                $"Coffee: {recipe.CoffeeWeight}g\n" +
                $"Water: {recipe.WaterWeight}g\n" +
                $"Temp: {recipe.Temperature}°C\n" +
                $"Method: {recipe.Method}",
                "OK");
        }

        private async void OnEditClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Edit Recipe", "Edit functionality coming soon!", "OK");
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Delete Recipe",
                $"Are you sure you want to delete '{recipe.Name}'?",
                "Yes", "No");

            if (answer)
            {
                try
                {
                    await ServiceHelper.Database.DeleteRecipeAsync(recipe);
                    await DisplayAlert("Success", "Recipe deleted successfully", "OK");
                    await Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to delete recipe: {ex.Message}", "OK");
                }
            }
        }
    }
}