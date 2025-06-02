using System;

namespace DataAccess.DTOModels
{
    public class ReviewWithUserDTO : ReviewDTO
    {
        public string Username { get; set; }
        public string EmailAddress { get; set; }

        public ReviewWithUserDTO()
        {
            Username = string.Empty;
            EmailAddress = string.Empty;
        }
    }
} 