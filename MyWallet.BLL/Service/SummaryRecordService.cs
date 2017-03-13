using MyWallet.BLL.InterfaceService;
using MyWallet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.BLL.Service
{
    public class SummaryRecordService : BaseService<t_summary_record>, InterfaceSummaryRecordService
    {
        public IQueryable<t_summary_record> NotDealLoan(int mana_id)
        {
            return Table().Where(M => M.mana_id == mana_id && M.is_deal == false);
        }

        public IQueryable<t_summary_record> Search(int mana_id, DateTime start_date, DateTime end_date, int search_record_type, int search_summary)
        {
            IQueryable<t_summary_record> result = Table().Where(M => M.mana_id == mana_id && M.add_time >= start_date && M.add_time <= end_date);

            if (search_record_type != -1)
            {
                result = result.Where(M => M.reco_type_code == search_record_type);
            }
            if (search_summary != -1)
            {
                result = result.Where(M => M.summ_id == search_summary || M.summ_tran_id == search_summary);
            }
            return result;
        }

        public IQueryable<t_summary_record> SearchByManagerID(int id)
        {
            return Table().Where(M => M.mana_id == id);
        }
    }
}
