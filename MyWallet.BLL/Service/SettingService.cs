using MyWallet.BLL.InterfaceService;
using MyWallet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.BLL.Service
{
    public class SettingService : BaseService<t_setting>, InterfaceSettingService
    {
        public IQueryable<t_setting> SearchByManagerID(int id)
        {
            return Table().Where(M=>M.mana_id == id);
        }
    }
}
