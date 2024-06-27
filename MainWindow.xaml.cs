using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RecipeWPFApp
{
    public partial class MainWindow : Window
    {
        private List<Recipe> recipes = new List<Recipe>();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void btnAddRecipe_Click(object sender, RoutedEventArgs e)
        {
            ClearUI();
            PrintHeader("Add a New Recipe");

            string recipeName = GetValidStringInput("Please enter the name of your recipe:");
            int ingredientCount = GetInputCount("Enter the number of ingredients:");
            int stepCount = GetInputCount("Enter the number of steps:");

            Recipe recipe = new Recipe(recipeName, ingredientCount, stepCount);

            // Delegate for calorie alerts
            Action<double, string> calorieAlert = (totalCalories, foodGroup) =>
            {
                if (totalCalories > 300)
                {
                    MessageBox.Show($"Alert: Total calories exceed 300! Current calories: {totalCalories}. Food group causing the excess: {foodGroup}", "Calorie Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            };

            for (int i = 0; i < ingredientCount; i++)
            {
                string ingredientName = GetValidStringInput($"Enter name for Ingredient {i + 1}:");
                double ingredientQuantity = GetDoubleInput($"Enter quantity for {ingredientName}:");
                double ingredientCalories = GetDoubleInput($"Enter calories for {ingredientName}:");
                string ingredientUnit = GetValidStringInput($"Enter unit for {ingredientName}:");
                string ingredientFoodGroup = SelectFoodGroup();

                recipe.AddIngredient(ingredientName, ingredientQuantity, ingredientUnit, ingredientCalories, ingredientFoodGroup);

                double totalCalories = recipe.CalculateTotalCalories();
                calorieAlert(totalCalories, ingredientFoodGroup);
            }

            for (int i = 0; i < stepCount; i++)
            {
                string stepDescription = GetValidStringInput($"Enter Step {i + 1} description:");
                recipe.AddStep(stepDescription);
            }

            recipes.Add(recipe);

            MessageBox.Show("Recipe added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Prompt user if they want to scale the recipe
            MessageBoxResult scaleRecipeResult = MessageBox.Show("Would you like to scale the recipe?", "Scale Recipe", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (scaleRecipeResult == MessageBoxResult.Yes)
            {
                Scaling(recipe);
            }
        }

        // Event handler for "List Recipes" button click
        private void btnfilterRecipes_Click(object sender, RoutedEventArgs e)
        {
            ClearUI();
            PrintHeader("List All Recipes");

            if (recipes.Count == 0)
            {
                MessageBox.Show("No recipes available.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Prepare a StringBuilder to accumulate the recipes' details
            StringBuilder recipesDetails = new StringBuilder();

            // Loop through each recipe to gather details
            foreach (var recipe in recipes)
            {
                recipesDetails.AppendLine($"Recipe Name: {recipe.RecipeName}");
                recipesDetails.AppendLine("\nIngredients:");
                recipesDetails.AppendLine("-------------");
                foreach (var ingredient in recipe.Ingredients)
                {
                    recipesDetails.AppendLine($"{ingredient.Name}: {ingredient.Quantity} {ingredient.Unit} ({ingredient.Calories} calories)");
                }
                recipesDetails.AppendLine("\nSteps:");
                recipesDetails.AppendLine("-------------");
                for (int i = 0; i < recipe.Steps.Count; i++)
                {
                    recipesDetails.AppendLine($"{i + 1}. {recipe.Steps[i]}");
                }
                recipesDetails.AppendLine($"\nTotal Calories: {recipe.CalculateTotalCalories()}");
                recipesDetails.AppendLine("--------------------------------------------");
            }

            MessageBox.Show(recipesDetails.ToString(), "All Recipes", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Event handler for "List Recipes" button click
        private void btnListRecipes_Click(object sender, RoutedEventArgs e)
        {
            ClearUI();
            PrintHeader("List All Recipes");

            if (recipes.Count == 0)
            {
                MessageBox.Show("No recipes available.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Prepare a StringBuilder to accumulate the filtered recipes' details
            StringBuilder recipesDetails = new StringBuilder();

            // Filter recipes based on user input
            List<Recipe> filteredRecipes = FilterRecipes();

            // Loop through each filtered recipe to gather details
            foreach (var recipe in filteredRecipes)
            {
                recipesDetails.AppendLine($"Recipe Name: {recipe.RecipeName}");
                recipesDetails.AppendLine("\nIngredients:");
                recipesDetails.AppendLine("-------------");
                foreach (var ingredient in recipe.Ingredients)
                {
                    recipesDetails.AppendLine($"{ingredient.Name}: {ingredient.Quantity} {ingredient.Unit} ({ingredient.Calories} calories)");
                }
                recipesDetails.AppendLine("\nSteps:");
                recipesDetails.AppendLine("-------------");
                for (int i = 0; i < recipe.Steps.Count; i++)
                {
                    recipesDetails.AppendLine($"{i + 1}. {recipe.Steps[i]}");
                }
                recipesDetails.AppendLine($"\nTotal Calories: {recipe.CalculateTotalCalories()}");
                recipesDetails.AppendLine("--------------------------------------------");
            }
            MessageBox.Show(recipesDetails.ToString(), "All Recipes", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        // Event handler for "Delete Recipe" button click
        private void btnDeleteRecipe_Click(object sender, RoutedEventArgs e)
        {
            ClearUI();
            PrintHeader("Delete a Recipe");

            if (recipes.Count == 0)
            {
                MessageBox.Show("No recipes available.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            int count = 1;
            foreach (var recipe in recipes)
            {
                MessageBox.Show($"{count}. {recipe.RecipeName}");
                count++;
            }

            int recipeIndexToDelete = GetInputCount("Enter the number of the recipe to delete:") - 1;

            if (recipeIndexToDelete >= 0 && recipeIndexToDelete < recipes.Count)
            {
                recipes.RemoveAt(recipeIndexToDelete);
                MessageBox.Show("Recipe deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Invalid recipe selection.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Event handler for "Clear all data" button click
        private void btnClearData_Click(object sender, RoutedEventArgs e)
        {
            ClearUI();
            recipes.Clear();
            MessageBox.Show("All recipes have been cleared.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Event handler for "Exit" button click
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        // Event handler for "Reset Recipe" button click
        private void btnResetRecipe_Click(object sender, RoutedEventArgs e)
        {
            ClearUI();
            PrintHeader("Reset a Recipe");

            if (recipes.Count == 0)
            {
                MessageBox.Show("No recipes available.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            int count = 1;
            foreach (var recipe in recipes)
            {
                MessageBox.Show($"{count}. {recipe.RecipeName}");
                count++;
            }

            int recipeIndexToReset = GetInputCount("Enter the number of the recipe to reset:") - 1;

            if (recipeIndexToReset >= 0 && recipeIndexToReset < recipes.Count)
            {
                ResetRecipe(recipes[recipeIndexToReset]);
            }
            else
            {
                MessageBox.Show("Invalid recipe selection.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Method to reset recipe to its original values
        private void ResetRecipe(Recipe recipe)
        {
            recipe.ResetToOriginal();
            MessageBox.Show("Recipe has been reset to its original values.", "Reset Recipe", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Method to clear UI output
        private void ClearUI()
        {
            // Clear any UI elements as needed
        }

        // Method to print header in UI
        private void PrintHeader(string header)
        {
            // Display header in UI as needed
        }

        // Method to get integer input from user
        private int GetInputCount(string prompt)
        {
            int count;
            bool isValidInput = false;

            do
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox(prompt, "Input", "");
                isValidInput = int.TryParse(input, out count);

                if (!isValidInput)
                {
                    MessageBox.Show("Invalid input. Please enter a valid integer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            } while (!isValidInput);

            return count;
        }

        // Method to get double input from user
        private double GetDoubleInput(string prompt)
        {
            double value;
            bool isValidInput = false;

            do
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox(prompt, "Input", "");
                isValidInput = double.TryParse(input, out value);

                if (!isValidInput || value < 0)
                {
                    MessageBox.Show("Invalid input. Please enter a valid non-negative number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            } while (!isValidInput || value < 0);

            return value;
        }

        // Method to get string input from user
        private string GetValidStringInput(string prompt)
        {
            string input;

            do
            {
                input = Microsoft.VisualBasic.Interaction.InputBox(prompt, "Input", "");

                if (string.IsNullOrWhiteSpace(input))
                {
                    MessageBox.Show("Input cannot be empty. Please enter a valid input.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            } while (string.IsNullOrWhiteSpace(input));

            return input;
        }

        // Method to select a food group from user
        private string SelectFoodGroup()
        {
            string[] foodGroups = { "Fruits", "Vegetables", "Grains", "Protein", "Dairy", "Fats" };
            int selectedIndex = GetInputCount("Select a food group:\n1. Fruits\n2. Vegetables\n3. Grains\n4. Protein\n5. Dairy\n6. Fats") - 1;

            return foodGroups[selectedIndex];
        }

        // Method to handle recipe scaling
        private void Scaling(Recipe recipe)
        {
            MessageBox.Show("You have chosen to scale the recipe.", "Scaling Recipe", MessageBoxButton.OK, MessageBoxImage.Information);

            double scale = GetScalingFactor();
            recipe.ScaleRecipe(scale);

            MessageBox.Show("Recipe scaled successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Method to get scaling factor from user
        private double GetScalingFactor()
        {
            double scale = 1.0;
            bool isValidInput = false;

            do
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Enter the scaling factor (0.5, 2, or 3):", "Scaling Factor", "");
                isValidInput = double.TryParse(input, out scale);

                if (!isValidInput || (scale != 0.5 && scale != 2 && scale != 3))
                {
                    MessageBox.Show("Invalid input. Please enter 0.5, 2, or 3.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            } while (!isValidInput || (scale != 0.5 && scale != 2 && scale != 3));

            return scale;
        }

        // Event handler for "Filter Recipes" button click
        // Event handler for "Filter Recipes" button click
        private void btnFilterRecipes_Click(object sender, RoutedEventArgs e)
        {
            ClearUI();
            PrintHeader("Filter Recipes");

            // Get filter criteria from user
            string ingredientFilter = GetValidStringInput("Enter an ingredient name to filter by:");
            string foodGroupFilter = SelectFoodGroup();
            double maxCalories = GetDoubleInput("Enter the maximum calories:");

            // Filter recipes based on criteria
            var filteredRecipes = FilterRecipes(ingredientFilter, foodGroupFilter, maxCalories);

            // Display filtered recipes
            if (filteredRecipes.Count == 0)
            {
                MessageBox.Show("No recipes match the filter criteria.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                StringBuilder filteredRecipesDetails = new StringBuilder();
                foreach (var recipe in filteredRecipes)
                {
                    filteredRecipesDetails.AppendLine($"Recipe Name: {recipe.RecipeName}");
                    filteredRecipesDetails.AppendLine("\nIngredients:");
                    filteredRecipesDetails.AppendLine("-------------");
                    foreach (var ingredient in recipe.Ingredients)
                    {
                        filteredRecipesDetails.AppendLine($"{ingredient.Name}: {ingredient.Quantity} {ingredient.Unit} ({ingredient.Calories} calories)");
                    }
                    filteredRecipesDetails.AppendLine("\nSteps:");
                    filteredRecipesDetails.AppendLine("-------------");
                    for (int i = 0; i < recipe.Steps.Count; i++)
                    {
                        filteredRecipesDetails.AppendLine($"{i + 1}. {recipe.Steps[i]}");
                    }
                    filteredRecipesDetails.AppendLine($"\nTotal Calories: {recipe.CalculateTotalCalories()}");
                    filteredRecipesDetails.AppendLine("--------------------------------------------");
                }

                MessageBox.Show(filteredRecipesDetails.ToString(), "Filtered Recipes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Method to filter recipes based on criteria
        private List<Recipe> FilterRecipes(string ingredientFilter, string foodGroupFilter, double maxCalories)
        {
            List<Recipe> filteredRecipes = new List<Recipe>();

            foreach (var recipe in recipes)
            {
                bool passesFilter = true;

                // Filter by ingredient name
                if (!string.IsNullOrEmpty(ingredientFilter))
                {
                    bool hasIngredient = recipe.Ingredients.Any(i => i.Name.Equals(ingredientFilter, StringComparison.OrdinalIgnoreCase));
                    if (!hasIngredient)
                    {
                        passesFilter = false;
                    }
                }

                // Filter by food group
                if (!string.IsNullOrEmpty(foodGroupFilter))
                {
                    bool hasFoodGroup = recipe.Ingredients.Any(i => i.FoodGroup.Equals(foodGroupFilter, StringComparison.OrdinalIgnoreCase));
                    if (!hasFoodGroup)
                    {
                        passesFilter = false;
                    }
                }

                // Filter by maximum calories
                if (maxCalories > 0)
                {
                    if (recipe.CalculateTotalCalories() > maxCalories)
                    {
                        passesFilter = false;
                    }
                }

                if (passesFilter)
                {
                    filteredRecipes.Add(recipe);
                }
            }

            return filteredRecipes;
        }


        // Method to filter recipes based on user criteria
        private List<Recipe> FilterRecipes()
        {
            List<Recipe> filteredRecipes = new List<Recipe>();

            // Prompt user for filtering criteria
            int filterOption = GetInputCount("How would you like to filter recipes?\n1. Ingredient name\n2. Food group\n3. Maximum calories");

            switch (filterOption)
            {
                case 1:
                    string ingredientName = GetValidStringInput("Enter the name of the ingredient to filter by:");
                    filteredRecipes = recipes.Where(r => r.Ingredients.Any(i => i.Name.Equals(ingredientName, StringComparison.OrdinalIgnoreCase))).ToList();
                    break;
                case 2:
                    string foodGroup = SelectFoodGroup();
                    filteredRecipes = recipes.Where(r => r.Ingredients.Any(i => i.FoodGroup.Equals(foodGroup, StringComparison.OrdinalIgnoreCase))).ToList();
                    break;
                case 3:
                    double maxCalories = GetDoubleInput("Enter the maximum calories to filter by:");
                    filteredRecipes = recipes.Where(r => r.CalculateTotalCalories() <= maxCalories).ToList();
                    break;
                default:
                    MessageBox.Show("Invalid selection. Showing all recipes.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    filteredRecipes = recipes.ToList();
                    break;
            }

            return filteredRecipes;
        }
    }
}
