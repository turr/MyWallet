using MyWallet.BLL.InterfaceService;
using MyWallet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.BLL.Service
{
    public class SummaryService : BaseService<t_summary>, InterfaceSummaryService
    {
        public IQueryable<t_summary> SearchByManagerID(int id)
        {
            return Table().Where(M => M.mana_id == id).OrderBy(M=>M.sort_by);
        }
    }
}
