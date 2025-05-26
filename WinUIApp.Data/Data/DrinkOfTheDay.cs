using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUiApp.Data.Data
{
    public class DrinkOfTheDay
    {
        public int DrinkId { get; set; }
        public DateTime DrinkTime { get; set; }

        public Drink Drink { get; set; }
    }
}
