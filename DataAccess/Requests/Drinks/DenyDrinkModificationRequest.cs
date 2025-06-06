using System;

namespace DataAccess.Requests.Drinks
{
    /// <summary>
    /// Request model for approving a drink modification request.
    /// </summary>
    public class DenyDrinkModificationRequest
    {
        /// <summary>
        /// Gets or sets the ID of the modification request to approve.
        /// </summary>
        public int ModificationRequestId { get; set; }
        public Guid UserId { get; set; }
    }
}