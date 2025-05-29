namespace WinUiApp.Data.Data
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public ICollection<DrinkCategory> DrinkCategories { get; set; }

        public Category(int categoryId, string categoryName)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
            DrinkCategories = new List<DrinkCategory>();
        }

        public Category()
        {
        }
    }
}
