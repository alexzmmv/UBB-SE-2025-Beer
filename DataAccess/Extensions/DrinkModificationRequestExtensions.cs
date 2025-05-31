using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Data;
using DataAccess.DTOModels;

namespace DataAccess.Extensions
{
    public static class DrinkModificationRequestExtensions
    {
        public static DrinkModificationRequest ConvertDTOToEntity(DrinkModificationRequestDTO requestDTO)
        {
            return new DrinkModificationRequest
            {
                DrinkModificationRequestId = requestDTO.DrinkModificationRequestId,
                ModificationType = requestDTO.ModificationType,
                OldDrinkId = requestDTO.OldDrinkId,
                NewDrinkId = requestDTO.NewDrinkId,
                RequestingUserId = requestDTO.RequestingUserId
            };
        }

        public static DrinkModificationRequestDTO ConvertEntityToDTO(DrinkModificationRequest request)
        {
            return new DrinkModificationRequestDTO
            {
                DrinkModificationRequestId = request.DrinkModificationRequestId,
                ModificationType = request.ModificationType,
                OldDrinkId = request.OldDrinkId,
                NewDrinkId = request.NewDrinkId,
                RequestingUserId = request.RequestingUserId
            };
        }
    }
}
