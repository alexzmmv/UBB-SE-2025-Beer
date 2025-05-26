using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUiApp.Data.Data
{
    public class Vote
    {
        public int VoteId { get; set; }
        public int UserId { get; set; }
        public int DrinkId { get; set; }
        public DateTime VoteTime { get; set; }

        public User User { get; set; }
        public Drink Drink { get; set; }
    }
}
