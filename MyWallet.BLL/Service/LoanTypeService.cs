using MyWallet.BLL.InterfaceService;
using MyWallet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.BLL.Service
{
    public class LoanTypeService : BaseService<t_loan_type>, InterfaceLoanTypeService
    {
        public IQueryable<t_loan_type> SearchByCode(int code)
        {
            return Table().Where(M => M.loan_code == code);
        }
    }
}
