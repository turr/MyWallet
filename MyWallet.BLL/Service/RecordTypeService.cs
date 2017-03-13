using MyWallet.BLL.InterfaceService;
using MyWallet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.BLL.Service
{
    public class RecordTypeService : BaseService<t_record_type>, InterfaceRecordTypeService
    {
        public IQueryable<t_record_type> SearchByCode(int code)
        {
            return Table().Where(M => M.reco_code == code);
        }
    }
}
