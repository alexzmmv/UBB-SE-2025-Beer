namespace WebApplication.Models;

public class HomeViewModel
{
	public Drink DrinkOfTheDay { get; set; }
	public List<Category> drinkCategories { get; set; }
	public List<Brand> drinkBrands { get; set; }
	public List<Drink> drinks { get; set; }
}