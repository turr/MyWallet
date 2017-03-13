#if DEBUG
//#define login_debug
#endif

using MyWallet.BLL.InterfaceService;
using MyWallet.BLL.Service;
using MyWallet.Model;
using MyWallet.Model.Request.Financing;
using MyWallet.MVC5.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MyWallet.MVC5.Controllers
{
    public class FinancingController : BaseController
    {
        #region 记一笔

        /// <summary>
        /// 记一笔页面
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = WebCont.ROLE_RECORD)]
        public ActionResult Record()
        {
            ViewBag.Title = HtmlExtensions.Lang("Financing_Record_Title");

            FormsAuthenticationTicket authentication = CommonFuntion.GetAuthenticationTicket();
            int mana_id = authentication == null ? 0 : Convert.ToInt32(authentication.Name);

            InterfaceRecordTypeService record_type_service = new RecordTypeService();
            InterfaceSummaryService summary_service = new SummaryService();
            InterfaceLoanTypeService loan_type_service = new LoanTypeService();

            List<t_record_type> record_type_list = new List<t_record_type>();
            List<t_summary> summary_list = new List<t_summary>();
            List<t_loan_type> loan_type_list = new List<t_loan_type>();

            try
            {
                record_type_list = record_type_service.Table().ToList();
                summary_list = summary_service.SearchByManagerID(mana_id).ToList();
                loan_type_list = loan_type_service.Table().ToList();
            }
            catch
            {

            }
#if login_debug
            t_record_type type1 = new t_record_type();
            type1.reco_type_id = 1;
            type1.reco_desc = "支出";
            type1.reco_code = 1;
            record_type_list.Add(type1);
            t_record_type type2 = new t_record_type();
            type2.reco_type_id = 2;
            type2.reco_desc = "收入";
            type2.reco_code = 2;
            record_type_list.Add(type2);
            t_record_type type3 = new t_record_type();
            type3.reco_type_id = 3;
            type3.reco_desc = "转账";
            type3.reco_code = 3;
            record_type_list.Add(type3);
            t_record_type type4 = new t_record_type();
            type4.reco_type_id = 4;
            type4.reco_desc = "借贷";
            type4.reco_code = 4;
            record_type_list.Add(type4);

            t_summary summary1 = new t_summary();
            summary1.mana_id = 1;
            summary1.summ_desc = "Summary";
            summary1.summ_id = 1;
            summary_list.Add(summary1);

            t_loan_type loan1 = new t_loan_type();
            loan1.loan_type_id = 10;
            loan1.loan_desc = "借入";
            loan1.loan_code = 41;
            loan_type_list.Add(loan1);
            t_loan_type loan2 = new t_loan_type();
            loan2.loan_type_id = 11;
            loan2.loan_desc = "借出";
            loan2.loan_code = 42;
            loan_type_list.Add(loan2);
            t_loan_type loan3 = new t_loan_type();
            loan3.loan_type_id = 12;
            loan3.loan_desc = "还款";
            loan3.loan_code = 43;
            loan_type_list.Add(loan3);
            t_loan_type loan4 = new t_loan_type();
            loan4.loan_type_id = 13;
            loan4.loan_desc = "收款";
            loan4.loan_code = 44;
            loan_type_list.Add(loan4);
#endif
            ViewBag.RECORD_TYPE = JsonConvert.SerializeObject(record_type_list);
            ViewBag.SUMMARY = JsonConvert.SerializeObject(summary_list);
            ViewBag.LOAN_TYPE = JsonConvert.SerializeObject(loan_type_list);

            return View();
        }

        /// <summary>
        /// 记一笔提交
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = WebCont.ROLE_RECORD)]
        [HttpPost]
        public ActionResult Record(AddRecordModel model)
        {
            if (ModelState.IsValid)
            {
                string check_result = CheckRecord(model);
                if (!string.IsNullOrEmpty(check_result))
                {
                    return Content(ReturnMessageAndRedirect(check_result, "Financing", "Record"));
                }
                else
                {
                    InterfaceSummaryRecordService summary_record_service = new SummaryRecordService();

                    t_summary_record summary_record = CreateSummaryRecord(model);
                    try
                    {
                        summary_record_service.Insert(summary_record);
                    }
                    catch
                    {

                    }
                }
            }
            return RedirectToAction("Record", "Financing");
        }
        private string CheckRecord(AddRecordModel model)
        {
            string result = "";

            InterfaceRecordTypeService record_type_service = new RecordTypeService();
            InterfaceSummaryService summary_service = new SummaryService();
            InterfaceLoanTypeService loan_type_service = new LoanTypeService();

            #region 常规检查
            t_record_type record_type = null;
            try
            {
                record_type = record_type_service.SearchByCode(model.record_type_code).FirstOrDefault();
            }
            catch
            {

            }
            if (record_type == null)
            {
                result = HtmlExtensions.Lang("_Error_Financing_Record_RecordType");
            }
            else
            {
                t_summary summary = null;
                try
                {
                    summary = summary_service.GetByID(model.summary_id);
                }
                catch
                {

                }
                if (summary == null)
                {
                    result = HtmlExtensions.Lang("_Error_Financing_Record_Summary");
                }
                else
                {
                    if (model.record_amount <= 0)
                    {
                        result = HtmlExtensions.Lang("_Error_Financing_Record_Amount");
                    }
                    else
                    {
                        if (model.remark != null && model.remark.Length > 100)
                        {
                            result = HtmlExtensions.Lang("_Error_Financing_Record_Remark");
                        }
                    }
                }
            }

            #endregion
            #region 特殊字段检查
            if (model.record_type_code == WebCont.RECORD_TYPE_TRANSFER)
            {
                t_summary summary_transfer = null;
                try
                {
                    summary_transfer = summary_service.GetByID(model.summary_transfer_id);
                }
                catch
                {

                }
                if (summary_transfer == null)
                {
                    result = HtmlExtensions.Lang("_Error_Financing_Record_ToSummary");
                }
            }
            else
            {
                model.summary_transfer_id = 0;
            }

            if (model.record_type_code == WebCont.RECORD_TYPE_LOAN)
            {
                t_loan_type loan_type = null;
                try
                {
                    loan_type = loan_type_service.SearchByCode(model.loan_type_code).FirstOrDefault();
                }
                catch
                {

                }
                if (loan_type == null)
                {
                    result = HtmlExtensions.Lang("_Error_Financing_Record_LoanType");
                }
            }
            else
            {
                model.loan_type_code = 0;
            }
            #endregion
            return result;
        }
        private t_summary_record CreateSummaryRecord(AddRecordModel model)
        {
            t_summary_record insert = null;

            if (model.record_type_code == WebCont.RECORD_TYPE_PAY)
            {
                insert = CreateSummaryRecord(model, -model.record_amount, 0, true);
            }
            else if (model.record_type_code == WebCont.RECORD_TYPE_INCOME)
            {
                insert = CreateSummaryRecord(model, model.record_amount, 0, true);
            }
            else if (model.record_type_code == WebCont.RECORD_TYPE_TRANSFER)
            {
                insert = CreateSummaryRecord(model,  -model.record_amount, model.record_amount, true);
            }
            else if (model.record_type_code == WebCont.RECORD_TYPE_LOAN)
            {
                if (model.loan_type_code == WebCont.LOAN_TYPE_BORROW_IN || model.loan_type_code == WebCont.LOAN_TYPE_REPAY_IN)
                {
                    insert = CreateSummaryRecord(model,  model.record_amount, 0, false);
                }
                else if (model.loan_type_code == WebCont.LOAN_TYPE_BORROW_OUT || model.loan_type_code == WebCont.LOAN_TYPE_REPAY_OUT)
                {
                    insert = CreateSummaryRecord(model,  -model.record_amount, 0, false);
                }
            }
            return insert;
        }
        private t_summary_record CreateSummaryRecord(AddRecordModel model,decimal record_amount,decimal tran_record_amount,bool is_deal)
        {
            FormsAuthenticationTicket authentication = CommonFuntion.GetAuthenticationTicket();
            int mana_id = authentication == null ? 0 : Convert.ToInt32(authentication.Name);

            t_summary_record insert = new t_summary_record();
            insert.mana_id = mana_id;
            insert.reco_type_code = model.record_type_code;
            insert.summ_id = model.summary_id;
            insert.summ_tran_id = model.summary_transfer_id;
            insert.loan_type_code = model.loan_type_code;
            insert.amount = record_amount;
            insert.tran_amount = tran_record_amount;
            insert.remark = model.remark == null ? "" : model.remark.Trim();
            insert.add_time = model.add_time;
            insert.is_deal = is_deal;
            return insert;
        }

        #endregion

        #region 资金明细

        [Authorize(Roles = WebCont.ROLE_RECORD)]
        public ActionResult RecordDetail()
        {
            ViewBag.Title = HtmlExtensions.Lang("Financing_RecordDetail_Title");

            FormsAuthenticationTicket authentication = CommonFuntion.GetAuthenticationTicket();
            int mana_id = authentication == null ? 0 : Convert.ToInt32(authentication.Name);

            InterfaceRecordTypeService record_type_service = new RecordTypeService();
            InterfaceSummaryService summary_service = new SummaryService();
            InterfaceSummaryRecordService summary_record_service = new SummaryRecordService();

            List<t_record_type> record_type_list = new List<t_record_type>();
            List<t_summary> summary_list = new List<t_summary>();
            List<t_summary_record> summary_record_list = new List<t_summary_record>();

            try
            {
                record_type_list = record_type_service.Table().ToList();
                summary_list = summary_service.SearchByManagerID(mana_id).ToList();
                summary_record_list = summary_record_service.SearchByManagerID(mana_id).ToList();

            }
            catch
            {

            }
            ViewBag.RECORD_TYPE = JsonConvert.SerializeObject(record_type_list);
            ViewBag.SUMMARY = JsonConvert.SerializeObject(summary_list);
            ViewBag.SUMMARY_RECORD = CreateRecordDetailCount(summary_list, summary_record_list);
            return View();
        }
        private string CreateRecordDetailCount(List<t_summary> summary_list, List<t_summary_record> summary_record_list)
        {
            List<object> list = new List<object>();
            decimal total = 0;
            foreach (t_summary foreach_data in summary_list)
            {
                decimal sum_amount = CalAmount(foreach_data.summ_id, summary_record_list);
                var insert = new { summary_id = foreach_data.summ_id, summary_desc = foreach_data.summ_desc, summary_sum_amount = sum_amount };
                list.Add(insert);
                total += sum_amount;
            }
            var total_obj = new { summary_id = -1, summary_desc = HtmlExtensions.Lang("Financing_RecordDetail_Total"), summary_sum_amount = total };
            list.Add(total_obj);
            return JsonConvert.SerializeObject(list);
        }

        [Authorize(Roles = WebCont.ROLE_RECORD)]
        [HttpPost]
        public ActionResult SearchRecord(string s_date,string e_date,int record_type, int summary)
        {
            var json_result = new JsonResult();

            FormsAuthenticationTicket authentication = CommonFuntion.GetAuthenticationTicket();
            int mana_id = authentication == null ? 0 : Convert.ToInt32(authentication.Name);

            string check_result = CheckSearchRecord(s_date, e_date, record_type, summary);
            if (!string.IsNullOrEmpty(check_result))
            {
                json_result.Data = new { Result = false, Message = check_result };
            }
            else
            {
                DateTime start_date_datetime = DateTime.Parse(DateTime.Parse(s_date).ToString("yyyy-MM-dd"));
                DateTime end_date_datetime = DateTime.Parse(DateTime.Parse(e_date).ToString("yyyy-MM-dd")).AddDays(1);
                List<t_summary_record> search_list = new List<t_summary_record>();
                InterfaceSummaryRecordService summary_record_service = new SummaryRecordService();
                try
                {
                    search_list = summary_record_service.Search(mana_id, start_date_datetime,
                                                              end_date_datetime, record_type, summary)
                                                              .OrderByDescending(M => M.add_time).ToList();
                }
                catch
                {
                }
                json_result.Data = new { Result = true, Message = CreateShowSummaryRecord(mana_id, search_list) };
            }
            return json_result;
        }
        private string CheckSearchRecord(string s_date, string e_date, int record_type, int summary)
        {
            string result = "";

            DateTime start_date_datetime;
            DateTime end_date_datetime;
            bool try_start_date_result = DateTime.TryParse(s_date, out start_date_datetime);
            bool try_end_date_result = DateTime.TryParse(e_date, out end_date_datetime);

            if (try_start_date_result && try_end_date_result)
            {
                InterfaceRecordTypeService record_type_service = new RecordTypeService();
                InterfaceSummaryService summary_service = new SummaryService();

                t_record_type record_type_find = null;
                try
                {
                    record_type_find = record_type_service.SearchByCode(record_type).FirstOrDefault();
                }
                catch{}
                if (record_type_find == null && record_type != -1)
                {
                    result = HtmlExtensions.Lang("_Error_Financing_RecordDetail_RecordType");
                }
                else
                {
                    t_summary summary_find = summary_service.GetByID(summary);
                    if (summary_find == null && summary != -1)
                    {
                        result = HtmlExtensions.Lang("_Error_Financing_RecordDetail_Summary");
                    }
                }
            }
            else
            {
                result = HtmlExtensions.Lang("_Error_Financing_RecordDetail_Date");
            }
            return result;
        }
        
        [Authorize(Roles = WebCont.ROLE_RECORD)]
        public ActionResult NotDealLoan()
        {
            var json_result = new JsonResult();

            FormsAuthenticationTicket authentication = CommonFuntion.GetAuthenticationTicket();
            int mana_id = authentication == null ? 0 : Convert.ToInt32(authentication.Name);

            InterfaceSummaryRecordService summary_record_service = new SummaryRecordService();
            List<t_summary_record> search_list = new List<t_summary_record>();
            try
            {
                search_list = summary_record_service.NotDealLoan(mana_id).OrderByDescending(M => M.add_time).ToList();
            }
            catch
            {
            }
            json_result.Data = new { Result = true, Message = CreateShowSummaryRecord(mana_id, search_list) };
            return json_result;
        }

        private string CreateShowSummaryRecord(int mana_id, List<t_summary_record> summary_record_list)
        {
            List<object> list = new List<object>();

            InterfaceRecordTypeService record_type_service = new RecordTypeService();
            InterfaceSummaryService summary_service = new SummaryService();
            InterfaceLoanTypeService loan_type_service = new LoanTypeService();

            List<t_record_type> record_type_list = new List<t_record_type>();
            List<t_summary> summary_list = new List<t_summary>();
            List<t_loan_type> loan_type_list = new List<t_loan_type>();

            try
            {
                record_type_list = record_type_service.Table().ToList();
                summary_list = summary_service.SearchByManagerID(mana_id).ToList();
                loan_type_list = loan_type_service.Table().ToList();
            }
            catch
            {
            }

            foreach (t_summary_record foreach_data in summary_record_list)
            {
                t_record_type find_record_type = record_type_list.Where(M => M.reco_code == foreach_data.reco_type_code).FirstOrDefault();
                t_summary find_summary = summary_list.Where(M => M.summ_id == foreach_data.summ_id).FirstOrDefault();

                //转账处理
                t_summary find_tran_summary = null;
                if (foreach_data.summ_tran_id != 0)
                {
                    foreach_data.amount = -foreach_data.amount;
                    find_tran_summary = summary_list.Where(M => M.summ_id == foreach_data.summ_tran_id).FirstOrDefault();
                }
                string tran_summary_desc = find_tran_summary == null ? "" : "->" + find_tran_summary.summ_desc;

                //借贷处理
                t_loan_type loan_type = loan_type_list.Where(M => M.loan_code == foreach_data.loan_type_code).FirstOrDefault();
                string loan_desc = loan_type == null ? "" : "(" + loan_type.loan_desc + ")";

                if (find_record_type != null && find_summary != null)
                {
                    var insert = new
                    {
                        summary_record_id = foreach_data.summ_reco_id,
                        add_date = foreach_data.add_time.ToString("yyyy-MM-dd HH:mm:ss"),
                        record_type_desc = find_record_type.reco_desc + loan_desc,
                        summary_desc = find_summary.summ_desc + tran_summary_desc,
                        amount = foreach_data.amount,
                        remark = foreach_data.remark,
                        is_deal = foreach_data.is_deal
                    };
                    list.Add(insert);
                }
            }
            return JsonConvert.SerializeObject(list);
        }

        [Authorize(Roles = WebCont.ROLE_RECORD)]
        [HttpPost]
        public ActionResult DeleteRecord(int id = 0)
        {
            FormsAuthenticationTicket authentication = CommonFuntion.GetAuthenticationTicket();
            int mana_id = authentication == null ? 0 : Convert.ToInt32(authentication.Name);

            var json_result = new JsonResult();

            InterfaceSummaryRecordService summary_record_service = new SummaryRecordService();

            t_summary_record delete = null;
            try
            {
                delete = summary_record_service.GetByID(id);
                if (delete != null && delete.mana_id == mana_id)
                {
                    summary_record_service.Delete(delete);
                    json_result.Data = new { Result = true, Message = "" };
                }
                else
                {
                    json_result.Data = new { Result = false, Message = HtmlExtensions.Lang("_Error_Comm_Para") };
                }
            }
            catch
            {
                json_result.Data = new { Result = false, Message = HtmlExtensions.Lang("_Error_Comm_Para") };
            }
            return json_result;
        }

        [Authorize(Roles = WebCont.ROLE_RECORD)]
        [HttpPost]
        public ActionResult DealRecord(int id = 0)
        {
            FormsAuthenticationTicket authentication = CommonFuntion.GetAuthenticationTicket();
            int mana_id = authentication == null ? 0 : Convert.ToInt32(authentication.Name);

            var json_result = new JsonResult();

            InterfaceSummaryRecordService summary_record_service = new SummaryRecordService();

            t_summary_record deal = null;
            try
            {
                deal = summary_record_service.GetByID(id);
                if (deal != null && deal.mana_id == mana_id)
                {
                    deal.is_deal = true;
                    summary_record_service.Update(deal);
                    json_result.Data = new { Result = true, Message = "" };
                }
                else
                {
                    json_result.Data = new { Result = false, Message = HtmlExtensions.Lang("_Error_Comm_Para") };
                }
            }
            catch
            {
                json_result.Data = new { Result = false, Message = HtmlExtensions.Lang("_Error_Comm_Para") };
            }
            return json_result;
        }

        #endregion

        #region 资金调整页面

        /// <summary>
        /// 资金调整页面
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = WebCont.ROLE_RECORD)]
        public ActionResult RecordAdjust()
        {
            ViewBag.Title = HtmlExtensions.Lang("Financing_RecordAdjust_Title");

            FormsAuthenticationTicket authentication = CommonFuntion.GetAuthenticationTicket();
            int mana_id = authentication == null ? 0 : Convert.ToInt32(authentication.Name);

            InterfaceSummaryService summary_service = new SummaryService();
            InterfaceSummaryRecordService summary_record_service = new SummaryRecordService();

            List<t_summary> summary_list = new List<t_summary>();
            List<t_summary_record> summary_record_list = new List<t_summary_record>();

            try
            {
                summary_list = summary_service.SearchByManagerID(mana_id).ToList();
                summary_record_list = summary_record_service.SearchByManagerID(mana_id).ToList();
            }
            catch
            {

            }
            ViewBag.DATA = CreateRecordAdjustCount(summary_list, summary_record_list);
            return View();
        }
        private string CreateRecordAdjustCount(List<t_summary> summary_list, List<t_summary_record> summary_record_list)
        {
            List<object> list = new List<object>();
            foreach (t_summary foreach_data in summary_list)
            {
                decimal sum_amount = CalAmount(foreach_data.summ_id, summary_record_list);
                var insert = new { summary_id = foreach_data.summ_id, summary_desc = foreach_data.summ_desc, summary_sum_amount = sum_amount };
                list.Add(insert);
            }
            return JsonConvert.SerializeObject(list); 
        }
        
        [Authorize(Roles = WebCont.ROLE_RECORD)]
        [HttpPost]
        public ActionResult RecordAdjust(AdjustRecordModel model)
        {
            FormsAuthenticationTicket authentication = CommonFuntion.GetAuthenticationTicket();
            int mana_id = authentication == null ? 0 : Convert.ToInt32(authentication.Name);

            InterfaceSummaryRecordService summary_record_service = new SummaryRecordService();

            if (ModelState.IsValid)
            {
                string check_result = CheckRecordAdjust(mana_id,model);
                if (!string.IsNullOrEmpty(check_result))
                {
                    return Content(ReturnMessageAndRedirect(check_result, "Financing", "RecordAdjust"));
                }
                else
                {
                    List<t_summary_record> summary_record_list = new List<t_summary_record>();
                    try
                    {
                        summary_record_list = summary_record_service.SearchByManagerID(mana_id).ToList();
                    }
                    catch{}

                    for (int i = 0; i < model.summ_id.Count(); i++)
                    {
                        int id = model.summ_id[i];
                        decimal summ_total_amount = CalAmount(id, summary_record_list);
                        decimal adjust_amount = model.adjust_amont[i];
                        //如果调整数和数据库记录数不一致,则需要插入数据
                        decimal diff = adjust_amount - summ_total_amount;
                        if (diff != 0)
                        {
                            t_summary_record insert = new t_summary_record();
                            insert.mana_id = mana_id;
                            insert.summ_id = id;
                            insert.summ_tran_id = 0;
                            insert.loan_type_code = 0;
                            insert.amount = diff;
                            insert.tran_amount = 0;
                            insert.remark = "AUTO";
                            insert.add_time = DateTime.Now;
                            insert.is_deal = true;
                            if (diff > 0)
                            {
                                insert.reco_type_code = WebCont.RECORD_TYPE_INCOME;
                            }
                            else
                            {
                                insert.reco_type_code = WebCont.RECORD_TYPE_PAY;
                            }
                            try
                            {
                                summary_record_service.DelayInsert(insert);
                            }
                            catch { }
                        }
                    }

                    try
                    {
                        summary_record_service.DelaySubmit();
                    }
                    catch { }
                }
            }
            return RedirectToAction("RecordAdjust", "Financing");
        }
        private string CheckRecordAdjust(int mana_id,AdjustRecordModel model)
        {
            string result = "";

            InterfaceSummaryService summary_service = new SummaryService();
            List<t_summary> summary_list = new List<t_summary>();
            try
            {
                summary_list = summary_service.SearchByManagerID(mana_id).ToList();
            }
            catch
            {
            }

            foreach (int id in model.summ_id)
            {
                t_summary summary = summary_list.Where(M => M.summ_id == id).FirstOrDefault();
                if (summary == null)
                {
                    result = HtmlExtensions.Lang("_Error_Comm_Para");
                }
            }
            return result;
        }

        private decimal CalAmount(int summary_id, List<t_summary_record> summary_record_list)
        {
            decimal sum_amount = 0;
            List<t_summary_record> record_list = summary_record_list.Where(M => M.summ_id == summary_id).ToList();
            sum_amount += record_list.Sum(M => M.amount);
            List<t_summary_record> tran_record_list = summary_record_list.Where(M => M.summ_tran_id == summary_id).ToList();
            sum_amount += tran_record_list.Sum(M => M.tran_amount);
            return sum_amount;
        }

        #endregion

        #region 账号类型模块

        /// <summary>
        /// 账号类型
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = WebCont.ROLE_RECORD)]
        public ActionResult SummaryType()
        {
            ViewBag.Title = HtmlExtensions.Lang("Financing_SummaryType_Title");

            FormsAuthenticationTicket authentication = CommonFuntion.GetAuthenticationTicket();
            int mana_id = authentication == null ? 0 : Convert.ToInt32(authentication.Name);

            List<t_summary> summary_list = new List<t_summary>();
            InterfaceSummaryService summary_service = new SummaryService();
            try
            {
                summary_list = summary_service.SearchByManagerID(mana_id).ToList();
            }
            catch
            {

            }
#if login_debug
            t_summary add1 = new t_summary();
            add1.mana_id = 1;
            add1.summ_desc = "123";
            add1.summ_id = 1;
            summary_list.Add(add1);
#endif
            ViewBag.DATA = JsonConvert.SerializeObject(summary_list);
            return View();
        }

        /// <summary>
        /// 提交账号类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = WebCont.ROLE_RECORD)]
        [HttpPost]
        public ActionResult SummaryType(AddSummaryTypeModel model)
        {
            FormsAuthenticationTicket authentication = CommonFuntion.GetAuthenticationTicket();
            int mana_id = authentication == null ? 0 : Convert.ToInt32(authentication.Name);

            string check_result = CheckSummary(model);
            if (!string.IsNullOrEmpty(check_result))
            {
                ViewBag.SubmitError = check_result;
            }

            InterfaceSummaryService summary_service = new SummaryService();
            if (ModelState.IsValid && string.IsNullOrEmpty(check_result))
            {
                t_summary add_summary = new t_summary();
                add_summary.summ_desc = model.add_summary;
                add_summary.mana_id = mana_id;
                try
                {
                    summary_service.Insert(add_summary);
                }
                catch
                {
                }
            }
            else
            {
                List<t_summary> summary_list = new List<t_summary>();
                try
                {
                    summary_list = summary_service.SearchByManagerID(mana_id).ToList();
                }
                catch
                {
                }
                ViewBag.DATA = JsonConvert.SerializeObject(summary_list);
                return View();
            }
            return RedirectToAction("SummaryType", "Financing");

        }
        private string CheckSummary(AddSummaryTypeModel model)
        {
            string result = "";
            if (string.IsNullOrEmpty(model.add_summary) || model.add_summary.Length > 10)
            {
                result = HtmlExtensions.Lang("_Error_Financing_SummaryType_Add");
            }
            return result;
        }

        /// <summary>
        /// 删除账号类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = WebCont.ROLE_RECORD)]
        [HttpPost]
        public ActionResult DeleteSummary(int id = 0)
        {
            FormsAuthenticationTicket authentication = CommonFuntion.GetAuthenticationTicket();
            int mana_id = authentication == null ? 0 : Convert.ToInt32(authentication.Name);

            var json_result = new JsonResult();

            InterfaceSummaryService summary_service = new SummaryService();
            try
            {
                t_summary delete = summary_service.GetByID(id);
                if (delete != null && delete.mana_id == mana_id)
                {
                    summary_service.Delete(delete);
                    json_result.Data = new { Result = true, Message = "" };
                }
                else
                {
                    json_result.Data = new { Result = false, Message = HtmlExtensions.Lang("_Error_Comm_Para") };
                }
            }
            catch
            {
                json_result.Data = new { Result = false, Message = HtmlExtensions.Lang("_Error_Comm_Para") };
            }
            return json_result;
        }
       
        #endregion
    }
}