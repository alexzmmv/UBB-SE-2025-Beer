using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Constants;
using WinUiApp.Data.Data;
using WinUIApp.WebAPI.Models;

namespace DataAccess.DTOModels
{
    public class DrinkModificationRequestDTO
    {
        public int DrinkModificationRequestId { get; set; }
        public DrinkModificationRequestType ModificationType { get; set; }
        public int? OldDrinkId { get; set; }
        public int? NewDrinkId { get; set; }
        public Guid RequestingUserId { get; set; }
    }
}
