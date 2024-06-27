using System.Collections.Generic;
using System.Windows;

namespace RecipeWPFApp
{
    public class Recipe
    {
        public string RecipeName { get; private set; }
        public List<Ingredient> Ingredients { get; private set; }
        public List<string> Steps { get; private set; }

        private List<Ingredient> originalIngredients;
        private List<string> originalSteps;

        public Recipe(string recipeName, int ingredientCount, int stepCount)
        {
            RecipeName = recipeName;
            Ingredients = new List<Ingredient>();
            Steps = new List<string>();

            originalIngredients = new List<Ingredient>();
            originalSteps = new List<string>();
        }

        public void AddIngredient(string name, double quantity, string unit, double calories, string foodGroup)
        {
            Ingredients.Add(new Ingredient(name, quantity, unit, calories, foodGroup));
            originalIngredients.Add(new Ingredient(name, quantity, unit, calories, foodGroup));
        }

        public void AddStep(string stepDescription)
        {
            Steps.Add(stepDescription);
            originalSteps.Add(stepDescription);
        }

        public double CalculateTotalCalories()
        {
            double totalCalories = 0;
            foreach (var ingredient in Ingredients)
            {
                totalCalories += ingredient.Calories;
            }
            return totalCalories;
        }

        public void ScaleRecipe(double scale)
        {
            foreach (var ingredient in Ingredients)
            {
                ingredient.Quantity *= scale;
            }
        }

        public void ResetToOriginal()
        {
            Ingredients.Clear();
            Ingredients.AddRange(originalIngredients);

            Steps.Clear();
            Steps.AddRange(originalSteps);
        }
    }
}
