using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUiApp.Data.Data;

namespace DataAccess.Data
{
    public class DrinkRequestingApproval {
        public int DrinkId { get; set; }
        public string DrinkName { get; set; }
        public string DrinkURL { get; set; }
        public int? BrandId { get; set; }
        public decimal AlcoholContent { get; set; }

        public Brand Brand { get; set; }
    }

}
