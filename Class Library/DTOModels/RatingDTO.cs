using WinUIApp.WebAPI.Constants;

namespace WinUIApp.WebAPI.Models;

public class RatingDTO
{
    /// <summary>
    /// Gets or sets the unique identifier for the rating.
    /// </summary>
    public int RatingId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the associated drink.
    /// </summary>
    public int DrinkId { get; set; } 

    /// <summary>
    /// Gets or sets the identifier of the user who submitted the rating.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the rating value, ranging from 1 to 5 stars.
    /// </summary>
    public float RatingValue { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the rating was submitted.
    /// </summary>
    public DateTime RatingDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the rating is active.
    /// </summary>
    public bool IsActive { get; set; }
}
