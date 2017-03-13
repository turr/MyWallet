using MyWallet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.BLL.InterfaceService
{
    public interface InterfaceSettingService : InterfaceBaseService<t_setting>
    {
        IQueryable<t_setting> SearchByManagerID(int id);
    }
}
