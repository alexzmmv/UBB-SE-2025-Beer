namespace DataAccess.DTORequests.Drink
{
    public class DeleteDrinkRequest
    {
        public Guid RequestingUserId { get; set; }
        public int drinkId { get; set; }
    }
}
