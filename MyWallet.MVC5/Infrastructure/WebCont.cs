using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWallet.MVC5.Infrastructure
{
    public class WebCont
    {
        #region 语言码
        public static string[] STA_ALL_LANG = { "cn" };
        public const string DEFAULT_LANG = "cn";
        #endregion

        #region 权限序号
        public const string ROLE_RECORD = "1";//记一笔(1)
        #endregion

        #region 记账类型序号
        public const int RECORD_TYPE_PAY = 1;//支出-
        public const int RECORD_TYPE_INCOME = 2;//收入+
        public const int RECORD_TYPE_TRANSFER = 3;//转账-+
        public const int RECORD_TYPE_LOAN = 4;//借贷
        #endregion

        #region 借贷类型序号
        public const int LOAN_TYPE_BORROW_IN = 41;//借入+
        public const int LOAN_TYPE_BORROW_OUT = 42;//借出-
        public const int LOAN_TYPE_REPAY_OUT = 43;//还款-
        public const int LOAN_TYPE_REPAY_IN = 44;//收款+
        #endregion
    }
}