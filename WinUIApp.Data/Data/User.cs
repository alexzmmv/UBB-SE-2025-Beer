using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUiApp.Data.Data
{
    public class User
    {
        public int UserId { get; set; }
        public ICollection<Vote> Votes { get; set; }
        public ICollection<UserDrink> UserDrinks { get; set; }
    }
}
