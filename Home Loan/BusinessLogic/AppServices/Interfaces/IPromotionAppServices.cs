using BusinessLogic.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.AppServices
{
   public interface IPromotionAppServices
    {
        ResultDTO<PromotionDTO> GetPromotion();
        ResultDTO AddPromotion(PromotionDTO pdetails);
    }
}
