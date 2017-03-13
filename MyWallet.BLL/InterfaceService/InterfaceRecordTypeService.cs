using MyWallet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.BLL.InterfaceService
{
    public interface InterfaceRecordTypeService : InterfaceBaseService<t_record_type>
    {
        IQueryable<t_record_type> SearchByCode(int code);
    }
}
