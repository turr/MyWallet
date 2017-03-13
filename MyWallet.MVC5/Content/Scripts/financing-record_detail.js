define(["jquery", "common", "bootstrap-datetimepicker", "bootstrap-datetimepicker-cn"], function ($, common) {
    //查询时间选择格式
    var SearchTimeClick = function () {
        $("input[name='start_date'],input[name='end_date']").datetimepicker({
            language: 'zh-CN',//语言
            format: 'yyyy-mm-dd',//时间格式
            weekStart: 1,//一周从哪一天开始。0（星期日）到6（星期六）
            todayBtn: 1,
            autoclose: 1,//选择日期后自动关闭 
            todayHighlight: 1,
            startView: 2,
            minView: 2,
            forceParse: 0
        });
    }

    var search_row = "";//模板

    //查询点击方法
    var SearchClick = function () {
        search_row = $("div[name='row_module']").clone().prop("outerHTML");
        $("div[name='row_module']").remove();

        $("input[name='search']").click(function () {
            var start_date = $("input[name='start_date']").val();
            var end_date = $("input[name='end_date']").val();
            if(typeof(common.StringToDate(start_date)) == "undefined")
            {
                $("input[name='start_date']").focus();
                return;
            }
            if (typeof (common.StringToDate(end_date)) == "undefined") {
                $("input[name='end_date']").focus();
                return;
            }

            var search_record_type = $("select[name='search_record_type']").val();
            var search_summary = $("select[name='search_summary']").val();

            $.ajax({
                url: "../Financing/SearchRecord",
                data: {
                    s_date: start_date,
                    e_date: end_date,
                    record_type: search_record_type,
                    summary: search_summary
                },
                type: 'POST',
                dataType: "json",
                success: function (data) {
                    if (data.Result == true) {
                        $("div[name='result_box']").empty();
                        if (data.Message != null && data.Message != "") {
                            var list = $.parseJSON(data.Message);
                            CreateSearchData(list);
                            DeleteRecordActivate();
                            DealRecordActivate();
                        }
                    }
                    else {
                        alert(data.Message);
                    }
                },
                error: function () {
                    window.location.href = "../User/LogOn";
                }
            });
        });
    }

    //查询未处理的借贷记录点击方法
    var NotDealLoanClick = function () {
        $("input[name='not_deal_loan']").click(function () {
            $.ajax({
                url: "../Financing/NotDealLoan",
                data: {},
                type: 'POST',
                dataType: "json",
                success: function (data) {
                    if (data.Result == true) {
                        $("div[name='result_box']").empty();
                        if (data.Message != null && data.Message != "") {
                            var list = $.parseJSON(data.Message);
                            CreateSearchData(list);
                            DeleteRecordActivate();
                            DealRecordActivate();
                        }
                    }
                },
                error: function () {
                    window.location.href = "../User/LogOn";
                }
            });

        });
    }

    var CreateSearchData = function (list) {
        $.each(list, function (i, value) {
            var new_row = $(search_row);
            new_row.removeClass("hidden");
            if (value.is_deal) {
                new_row.find("[name='deal_record']").remove();
            }
            new_row.find("[name='record_id']").val(value.summary_record_id);
            new_row.find("[name='record_type_desc']").text(value.record_type_desc);
            new_row.find("[name='summary_desc']").text(value.summary_desc);
            new_row.find("[name='amount']").text(value.amount);
            new_row.find("[name='add_date']").text(value.add_date);
            new_row.find("[name='remark']").text(value.remark);
            $("div[name='result_box'").append(new_row.prop("outerHTML"));
        });
    }
    var DeleteRecordActivate = function() {
        $("input[name='delete_record']").click(function () {
            var delete_tr = $(this).parents("[name='row_module']");
            var record_id = $(this).siblings("input[name='record_id']").val();
            var delete_confirm = confirm(financing_js.sure_record_delete);
            if (delete_confirm) {
                $.ajax({
                    url: "../Financing/DeleteRecord",
                    data: { id: record_id },
                    type: 'POST',
                    dataType: "json",
                    success: function (data) {
                        if (data.Result == true) {
                            delete_tr.remove();
                        }
                        else {
                            alert(data.Message);
                        }
                    },
                    error: function () {
                        window.location.href = "../User/LogOn";
                    }
                });
            }
        });
    }
    var DealRecordActivate = function () {
        $("input[name='deal_record']").click(function () {
            var deal_button = $(this);

            var record_id = $(this).siblings("input[name='record_id']").val();
            var deal_confirm = confirm(financing_js.sure_record_filing);
            if (deal_confirm) {
                $.ajax({
                    url: "../Financing/DealRecord",
                    data: { id: record_id },
                    type: 'POST',
                    dataType: "json",
                    success: function (data) {
                        if (data.Result == true) {
                            deal_button.remove();
                        }
                        else {
                            alert(data.Message);
                        }
                    },
                    error: function () {
                        window.location.href = "../User/LogOn";
                    }
                });
            }
        });

    }


    return {
        Init: function () {
            SearchTimeClick();
            SearchClick();
            NotDealLoanClick();
        }
    }
});




