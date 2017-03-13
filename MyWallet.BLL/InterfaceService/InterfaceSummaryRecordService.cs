using MyWallet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.BLL.InterfaceService
{
    public interface InterfaceSummaryRecordService : InterfaceBaseService<t_summary_record>
    {
        IQueryable<t_summary_record> SearchByManagerID(int id);

        IQueryable<t_summary_record> Search(int mana_id, DateTime start_date, DateTime end_date, int search_record_type, int search_summary);

        IQueryable<t_summary_record> NotDealLoan(int mana_id);
    }
}
