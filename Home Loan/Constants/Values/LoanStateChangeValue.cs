using System;
using System.Collections.Generic;
using System.Text;
using Constants.Enums;

namespace Constants.Values
{
    public class LoanStateChangeValue
    {
        static public Dictionary<LoanStates, IList<LoanStates>> value = new Dictionary<LoanStates, IList<LoanStates>>()
        {
            {LoanStates.Created, new List<LoanStates>(){} },
            {LoanStates.InProgress, new List<LoanStates>(){LoanStates.Accepted,LoanStates.Recommended, LoanStates.Rejected } },
            {LoanStates.Accepted, new List<LoanStates>(){} },
            {LoanStates.Rejected, new List<LoanStates>(){LoanStates.InProgress } },
            {LoanStates.Recommended, new List<LoanStates>(){LoanStates.Rejected, LoanStates.InProgress } },
        };
    }
}
