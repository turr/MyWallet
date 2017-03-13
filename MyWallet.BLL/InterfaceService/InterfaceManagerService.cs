using MyWallet.Model;
using MyWallet.Model.Request.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.BLL.InterfaceService
{
    public interface InterfaceManagerService : InterfaceBaseService<t_manager>
    {
        IQueryable<t_manager> Login(LogOnModel model);
    }
}
