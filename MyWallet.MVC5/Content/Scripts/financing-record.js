define(["jquery", "common", "bootstrap-datetimepicker", "bootstrap-datetimepicker-cn"], function ($, common) {
    var InputToTop = function () {
        $(".to_top input").click(function () {
            var target = this;
            setTimeout(function () {
                target.scrollIntoView(true);
            }, 100);
        });
    }

    var AddTimeClick = function () {
        $(".form_datetime").datetimepicker({
            language: 'zh-CN',//语言
            format: 'yyyy-mm-dd hh:ii:ss',//时间格式
            weekStart: 1,//一周从哪一天开始。0（星期日）到6（星期六）
            todayBtn: 1,
            autoclose: 1,//选择日期后自动关闭 
            todayHighlight: 1,
            startView: 2,
            forceParse: 0,
            showMeridian: 1
        });
    }

    //记账类型,账号类型改变事件
    var RecordTypeChange = function () {
        $("select[name='record_type_code'],select[name='summary_id']").change(function () {
            $("#summary_transfer_div").addClass("hidden");
            $("#loan_div").addClass("hidden");
            $("#loan_div select[name='loan_type_code']").attr("disabled", "disabled");

            //删除"to转账"的记账类型
            $("select[name='summary_transfer_id']").empty();
            var record_type_code = $("select[name='record_type_code'] option:selected").val();
            //转账
            if (record_type_code == common.RECORD_TYPE_TRANSFER) {
                //获取"form转账"当前的选择
                var summary_id = $("select[name='summary_id'] option:selected").val();
                $.each($("select[name='summary_id'] option"), function (i, temp) {
                    if (summary_id != temp.value) {
                        $("select[name='summary_transfer_id']").append(temp.outerHTML);
                    }
                });
                $("#summary_transfer_div").removeClass("hidden");
            }
            else if (record_type_code == common.RECORD_TYPE_LOAN) {
                $("#loan_div").removeClass("hidden");
                $("#loan_div select[name='loan_type_code']").removeAttr("disabled");
            }
        });
    }

    var SubmitCheckOut = function () {
        $("form").submit(function () {
            var result = true;

            $("form span[name='amount_error_message']").text("");
            if ($("form input[name='record_amount']").val() === "") {
                $("form input[name='record_amount']").focus();
                $("form span[name='amount_error_message']").text(financing_js.add_record_null_amount);
                result = false;
            } else if (isNaN($("form input[name='record_amount']").val())) {
                $("form input[name='record_amount']").focus();
                $("form span[name='amount_error_message']").text(financing_js.add_record_error_amount);
                result = false;
            }

            $("form span[name='add_time_error_message']").text("");
            if ($("form input[name='add_time']").val() === "") {
                $("form input[name='add_time']").focus();
                $("form span[name='add_time_error_message']").text(financing_js.add_record_null_date);
                result = false;
            } else if (typeof (common.StringToDate($("form input[name='add_time']").val())) == "undefined") {
                $("form input[name='add_time']").focus();
                $("form span[name='add_time_error_message']").text(financing_js.add_record_error_date);
                result = false;
            }
            if (result) {
                $("button[type='submit']").attr("disabled", "true");
            }
            return result;
        });
    }

    return {
        Init: function () {
            InputToTop();
            AddTimeClick();
            RecordTypeChange();
            SubmitCheckOut();
        }
    }
});




