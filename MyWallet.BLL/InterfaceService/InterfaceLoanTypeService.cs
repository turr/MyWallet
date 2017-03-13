using MyWallet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.BLL.InterfaceService
{
    public interface InterfaceLoanTypeService : InterfaceBaseService<t_loan_type>
    {
        IQueryable<t_loan_type> SearchByCode(int code);
    }
}
