using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.Model.Request.Financing
{
    public class AdjustRecordModel
    {
        public int[] summ_id { get; set; }//账号类型ID
        public decimal[] adjust_amont { get; set; }//调整金额
    }
}
