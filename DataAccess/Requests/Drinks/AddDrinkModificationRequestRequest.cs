using DataAccess.Constants;
using System;

namespace DataAccess.Requests.Drinks
{
    public class AddDrinkModificationRequestRequest
    {
        public DrinkModificationRequestType ModificationType { get; set; }
        public int? OldDrinkId { get; set; }
        public int? NewDrinkId { get; set; }
        public Guid RequestingUserId { get; set; }
    }
} 