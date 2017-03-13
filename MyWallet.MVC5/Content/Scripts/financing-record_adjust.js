define(["jquery", "common"], function ($, common) {
    var SubmitCheckOut = function () {
        $("form").submit(function () {
            var result = true;
            $("form input[name='adjust_amont']").each(function () {
                var val = $(this).val();
                if (val === "") {
                    $(this).siblings("span[name='amount_error_message']").text(financing_js.adjust_record_null_data);
                    result = false;
                } else if (isNaN(val)) {
                    $(this).siblings("span[name='amount_error_message']").text(financing_js.adjust_record_error_data);
                    result = false;
                }
            });
            if (result) {
                $("button[type='submit']").attr("disabled", "true");
            }
            return result;
        });
    }

    return {
        Init: function () {
            SubmitCheckOut();
        }
    }
});

