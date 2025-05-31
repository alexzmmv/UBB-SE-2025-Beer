namespace DataAccess.DTORequests.Drink
{
    public class DeleteDrinkRequest
    {
        public Guid RequestingUserId { get; set; }
        public int DrinkId { get; set; }
    }
}
