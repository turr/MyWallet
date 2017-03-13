define(["jquery", "common"], function ($, common) {
    var SubmitCheckOut = function () {
        $("form").submit(function () {
            var result = true;

            if ($("form input[name='add_summary']").val() === "") {
                $("form input[name='add_summary']").focus();
                $("form span[name='add_error_message']").text(financing_js.add_summary_null);
                result = false;
            }
            if (result) {
                $("button[type='submit']").attr("disabled", "true");
            }
            return result;
        });
    }
   
    var DeleteClick = function () {
        $("a[name='delete']").click(function () {
            var obj = $(this);
            var delete_confirm = confirm(financing_js.sure_summary_delete);
            if (delete_confirm) {
                $.ajax({
                    url: "../Financing/DeleteSummary",
                    data: { id: obj.attr("title") },
                    type: 'POST',
                    dataType: "json",
                    success: function (data) {
                        if (data.Result == true) {
                            obj.parents("tr").remove();
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
            SubmitCheckOut();
            DeleteClick();
        }
    }
});


