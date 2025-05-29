using DataAccess.Constants;
using DataAccess.Model.AdminDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUiApp.Data.Data;

namespace DataAccess.Data;

public class DrinkModificationRequest
{
    public int DrinkModificationRequestId { get; set; }
    public DrinkModificationRequestType ModificationType { get; set; }
    public Drink? OldDrink { get; set; }
    public Drink? NewDrink { get; set; }
    public User RequestingUser { get; set; }
}
