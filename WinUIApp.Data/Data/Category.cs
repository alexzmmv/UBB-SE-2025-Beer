namespace WinUiApp.Data.Data
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public ICollection<DrinkCategory> DrinkCategories { get; set; }
    }
}
