using System;
using System.Collections.Generic;
using System.Text;
using Constants.Enums;
using DataAccess.Entities;

namespace BusinessLogic.DTO
{
    public class LoanViewDTO
    {
        public LoanDTO loan { get; set; }
        public IList<CollateralDTO> collaterals { get; set; }
        public float TotalColleteralValue { get; set; }
        public LoanStatus status { get; set; }
        public LoanStates state { get; set; }
    }
}
