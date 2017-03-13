define(["jquery"], function ($) {
    var SubmitCheckOut = function () {
        $("form").submit(function () {
            var result = true;
            $("form input").each(function () {
                if ($(this).val() === "") {
                    $(this).focus();
                    $("form span[name='error_message']").text(user_js.need_input);
                    result = false;
                    return false;
                }
            });
            return result;
        });
    }

    return {
        Init: function () {
            SubmitCheckOut();
        }
    }
});