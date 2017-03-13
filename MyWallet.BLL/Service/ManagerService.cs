#if DEBUG
#define login_debug
#endif
using MyWallet.BLL.InterfaceService;
using MyWallet.Model;
using MyWallet.Model.Request.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.BLL.Service
{
    public class ManagerService : BaseService<t_manager>, InterfaceManagerService
    {
        public IQueryable<t_manager> Login(LogOnModel model)
        {
            return Table()
                .Where(M => M.mana_login_name == model.login_name &&
                       M.mana_login_password == model.login_password);
        }
    }
}
