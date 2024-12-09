using CaloriesCalculator.Data;
using Project.Models;

public class Food
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Calories { get; set; }

    // Add a foreign key to the Category
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}
