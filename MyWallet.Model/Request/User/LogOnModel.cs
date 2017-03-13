using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.Model.Request.User
{
    public class LogOnModel
    {
        [Required]
        public string login_name { get; set; }

        [Required]
        public string login_password { get; set; }
    }
}
