using BusinessLogic.DTO;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.AppServices
{
    public interface ICollateralAppService
    {
        ResultDTO<IList<Collateral>> GetAllCollateral(string email);
        ResultDTO AddCollateral(CollateralDTO collateralDto);
        ResultDTO DeleteCollateral(string Id);
        ResultDTO UpdateCollateral(string id , CollateralDTO collateralDto);
    }
}
