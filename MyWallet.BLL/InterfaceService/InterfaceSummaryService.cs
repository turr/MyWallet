using MyWallet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.BLL.InterfaceService
{
    public interface InterfaceSummaryService : InterfaceBaseService<t_summary>
    {
        IQueryable<t_summary> SearchByManagerID(int id);
    }
}
