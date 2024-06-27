public class Ingredient
{
    public string Name { get; set; }
    public double Quantity { get; set; }
    public string Unit { get; set; } // Ensure this has a public setter
    public double Calories { get; set; }
    public string FoodGroup { get; set; }

    // Constructor
    public Ingredient(string name, double quantity, string unit, double calories, string foodGroup)
    {
        Name = name;
        Quantity = quantity;
        Unit = unit;
        Calories = calories;
        FoodGroup = foodGroup;
    }

    // Example method to change unit
    public void ChangeUnit(string newUnit)
    {
        Unit = newUnit;
    }
}
