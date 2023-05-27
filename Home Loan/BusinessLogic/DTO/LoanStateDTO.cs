using Constants.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.DTO
{
    public class LoanStateDTO
    {
        public string Id { get; set; }
        public string LoanID { get; set; }
        public LoanStates State { get; set; }
        public DateTime Date { get; set; }
        public string UserID { get; set; }
        public string Notes { get; set; }
    }
}
