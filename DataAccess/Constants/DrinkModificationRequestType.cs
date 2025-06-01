using System.ComponentModel.DataAnnotations;

namespace DataAccess.Constants
{
    public enum DrinkModificationRequestType
    {
        [Display(Name = "Add")]
        Add = 0,

        [Display(Name = "Update")]
        Edit = 1,

        [Display(Name = "Remove")]
        Remove = 2
    }
}