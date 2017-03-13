using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.Model.Request.Financing
{
    public class AddSummaryTypeModel
    {
        [Required]
        public string add_summary { get; set; }
       
    }
}
