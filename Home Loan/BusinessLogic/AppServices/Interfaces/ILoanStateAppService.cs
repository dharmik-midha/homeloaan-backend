using BusinessLogic.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.AppServices
{
    public interface ILoanStateAppService
    {
        public ResultDTO<LoanStateDTO> GetCurrentLoanState(string id);
        public ResultDTO AddLoanState(LoanStateDTO loanStateDTO);
        public ResultDTO<IList<LoanStateDTO>> GetLoanStateHistory(string id);
        public ResultDTO ChangeLoanState(LoanStateDTO loanStateDTO);
    }
}
