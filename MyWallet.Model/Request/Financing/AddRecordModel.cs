using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.Model.Request.Financing
{
    public class AddRecordModel
    {
        public int record_type_code { get; set; }
        public int summary_id { get; set; }
        public int summary_transfer_id { get; set; }//记账类型为转账,这字段则是 转账到的账号类型
        public int loan_type_code { get; set; }//记账类型为借贷,这字段则是  借贷类型

        [Required]
        public decimal record_amount { get; set; }

        public string remark { get; set; }

        [Required]
        public DateTime add_time { get; set; }
    }
}
