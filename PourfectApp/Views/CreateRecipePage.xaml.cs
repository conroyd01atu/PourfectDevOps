using PourfectApp.Models;

namespace PourfectApp.Views
{
    public partial class CreateRecipePage : ContentPage
    {
        private List<RecipeStep> recipeSteps = new List<RecipeStep>();
        private int stepCounter = 0;

        public CreateRecipePage()
        {
            InitializeComponent();

            // Set default values
            MethodPicker.SelectedIndex = 0;
            PourTechniquePicker.SelectedIndex = 0;

            // Add initial bloom step
            AddStep("0:00-0:30", "Bloom - saturate grounds evenly", 30);
        }

        private void AddStep(string timeRange = "", string description = "", double waterAmount = 0)
        {
            stepCounter++;

            var stepFrame = new Frame
            {
                BackgroundColor = Color.FromArgb("#F9F9F9"),
                CornerRadius = 5,
                Padding = 10,
                HasShadow = false
            };

            var stepLayout = new VerticalStackLayout { Spacing = 5 };

            // Step header with delete button
            var headerLayout = new HorizontalStackLayout();
            var stepLabel = new Label
            {
                Text = $"Step {stepCounter}",
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#4A2C17"),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };

            var deleteButton = new Button
            {
                Text = "Remove",
                BackgroundColor = Color.FromArgb("#DC3545"),
                TextColor = Colors.White,
                CornerRadius = 3,
                Padding = new Thickness(10, 5),
                FontSize = 12
            };

            headerLayout.Children.Add(stepLabel);
            headerLayout.Children.Add(deleteButton);

            // Time range entry
            var timeLabel = new Label { Text = "Time Range", FontSize = 12, TextColor = Color.FromArgb("#666") };
            var timeEntry = new Entry
            {
                Placeholder = "0:30-1:00",
                Text = timeRange,
                BackgroundColor = Colors.White
            };

            // Description entry
            var descLabel = new Label { Text = "Description", FontSize = 12, TextColor = Color.FromArgb("#666") };
            var descEntry = new Entry
            {
                Placeholder = "Pour in circular motion...",
                Text = description,
                BackgroundColor = Colors.White
            };

            // Water amount entry
            var waterLabel = new Label { Text = "Water Amount (g)", FontSize = 12, TextColor = Color.FromArgb("#666") };
            var waterEntry = new Entry
            {
                Placeholder = "50",
                Text = waterAmount > 0 ? waterAmount.ToString() : "",
                Keyboard = Keyboard.Numeric,
                BackgroundColor = Colors.White
            };

            // Add all elements to step layout
            stepLayout.Children.Add(headerLayout);
            stepLayout.Children.Add(timeLabel);
            stepLayout.Children.Add(timeEntry);
            stepLayout.Children.Add(descLabel);
            stepLayout.Children.Add(descEntry);
            stepLayout.Children.Add(waterLabel);
            stepLayout.Children.Add(waterEntry);

            stepFrame.Content = stepLayout;

            // Store references for later retrieval
            stepFrame.BindingContext = new
            {
                TimeEntry = timeEntry,
                DescEntry = descEntry,
                WaterEntry = waterEntry,
                StepNumber = stepCounter
            };

            // Handle delete button
            deleteButton.Clicked += (s, e) =>
            {
                StepsContainer.Children.Remove(stepFrame);
                RenumberSteps();
            };

            StepsContainer.Children.Add(stepFrame);
        }

        private void RenumberSteps()
        {
            stepCounter = 0;
            foreach (var child in StepsContainer.Children)
            {
                if (child is Frame frame)
                {
                    stepCounter++;
                    var stepLayout = frame.Content as VerticalStackLayout;
                    var headerLayout = stepLayout.Children[0] as HorizontalStackLayout;
                    var label = headerLayout.Children[0] as Label;
                    label.Text = $"Step {stepCounter}";
                }
            }
        }

        private void OnAddStepClicked(object sender, EventArgs e)
        {
            AddStep();
        }

        private async void OnSaveRecipeClicked(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(RecipeNameEntry.Text))
            {
                await DisplayAlert("Error", "Please enter a recipe name", "OK");
                return;
            }

            if (!double.TryParse(CoffeeWeightEntry.Text, out var coffeeWeight) || coffeeWeight <= 0)
            {
                await DisplayAlert("Error", "Please enter a valid coffee weight", "OK");
                return;
            }

            if (!double.TryParse(WaterWeightEntry.Text, out var waterWeight) || waterWeight <= 0)
            {
                await DisplayAlert("Error", "Please enter a valid water weight", "OK");
                return;
            }

            try
            {
                // Collect steps
                var steps = new List<RecipeStep>();
                int stepNum = 1;

                foreach (var child in StepsContainer.Children)
                {
                    if (child is Frame frame && frame.BindingContext != null)
                    {
                        dynamic context = frame.BindingContext;
                        var timeEntry = context.TimeEntry as Entry;
                        var descEntry = context.DescEntry as Entry;
                        var waterEntry = context.WaterEntry as Entry;

                        if (!string.IsNullOrWhiteSpace(timeEntry.Text) && !string.IsNullOrWhiteSpace(descEntry.Text))
                        {
                            steps.Add(new RecipeStep
                            {
                                StepNumber = stepNum++,
                                TimeRange = timeEntry.Text,
                                Description = descEntry.Text,
                                WaterAmount = double.TryParse(waterEntry.Text, out var water) ? water : 0
                            });
                        }
                    }
                }

                // Create recipe parameters
                var parameters = new RecipeParameters
                {
                    BloomTime = BloomTimeEntry.Text ?? "",
                    BloomWater = double.TryParse(BloomWaterEntry.Text, out var bloom) ? bloom : 0,
                    PourTechnique = PourTechniquePicker.SelectedItem?.ToString() ?? "Continuous",
                    NumberOfPours = steps.Count,
                    FilterType = FilterTypeEntry.Text ?? "",
                    Notes = NotesEditor.Text ?? ""
                };

                // Create recipe
                var recipe = new Recipe
                {
                    Name = RecipeNameEntry.Text,
                    Description = DescriptionEditor.Text ?? "",
                    Method = MethodPicker.SelectedItem?.ToString() ?? "V60",
                    CoffeeWeight = coffeeWeight,
                    WaterWeight = waterWeight,
                    Temperature = int.TryParse(TemperatureEntry.Text, out var temp) ? temp : 93,
                    TotalTime = TotalTimeEntry.Text ?? "",
                    GrindSize = GrindSizeEntry.Text ?? "",
                    CreatedDate = DateTime.Now,
                    CreatedBy = Preferences.Get("username", "Guest"),
                    IsFavorite = false,
                    Steps = steps,
                    Parameters = parameters
                };

                // Save to database
                await ServiceHelper.Database.SaveRecipeAsync(recipe);

                await DisplayAlert("Success", "Recipe saved successfully!", "OK");

                // Navigate back
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to save recipe: {ex.Message}", "OK");
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
