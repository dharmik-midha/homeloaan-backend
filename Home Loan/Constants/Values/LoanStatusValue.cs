using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Constants.Enums;

namespace Constants.Values
{
    public static class LoanStatusValue
    {
        public static LoanStatus getLoanStatus(float val)
        {
            if (val <= 40)      return LoanStatus.RED;
            else if (val <= 70) return LoanStatus.YELLOW;
            else                return LoanStatus.GREEN;
        }
    }
}
