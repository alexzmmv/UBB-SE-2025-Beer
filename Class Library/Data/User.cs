using DataAccess.Model.AdminDashboard;

namespace WinUiApp.Data.Data
{
    public class User
    {
        public User(Guid userId, string emailAddress, string userName, int numberOfDeletedReviews, bool hasSubmittedAppeal, RoleType assignedRole, string fullName)
        {
            this.UserId = userId;
            this.EmailAddress = emailAddress;
            this.Username = userName;
            this.NumberOfDeletedReviews = numberOfDeletedReviews;
            this.HasSubmittedAppeal = hasSubmittedAppeal;
            this.AssignedRole = assignedRole;
        }
        public User(Guid userId, string emailAddress, string userName, int numberOfDeletedReviews, bool hasSubmittedAppeal, RoleType assignedRole, string fullName, ICollection<Vote> votes, ICollection<UserDrink> drinks)
        {
            this.UserId = userId;
            this.EmailAddress = emailAddress;
            this.Username = userName;
            this.NumberOfDeletedReviews = numberOfDeletedReviews;
            this.HasSubmittedAppeal = hasSubmittedAppeal;
            this.AssignedRole = assignedRole;
            this.Votes = votes;
            this.UserDrinks = drinks;
        }
        public User()
        {
            this.AssignedRole = RoleType.User;
            this.EmailAddress = string.Empty;
            this.Username = string.Empty;
            this.PasswordHash = string.Empty;
            this.TwoFASecret = null;
            this.Votes = new List<Vote>();
            this.UserDrinks = new List<UserDrink>();
        }
        public Guid UserId { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string? TwoFASecret { get; set; }

        public string EmailAddress { get; set; }

        public int NumberOfDeletedReviews { get; set; }

        public bool HasSubmittedAppeal { get; set; }

        public RoleType AssignedRole { get; set; }


        public override string ToString()
        {
            return "Id: " + UserId.ToString() + ", email: " + EmailAddress;
        }

        public ICollection<Vote> Votes { get; set; }

        public ICollection<UserDrink> UserDrinks { get; set; }
    }
}
