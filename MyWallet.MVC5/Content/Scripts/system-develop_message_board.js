define(["jquery","common", "ueditor-config", "ueditor-all-min", "ueditor-lang", "zero-clipboard"], function ($, common) {
    $("#develop_message_board").val(common.HtmlDecode(common.HtmlDecode(message)));
    //UE.getEditor('develop_message_board');
    var editor = new UE.ui.Editor({ initialFrameHeight: 250, initialFrameWidth: "auto" });
    editor.render("develop_message_board");

    var SaveClick = function () {
        $("button[name='save']").click(function () {
            var message = UE.getEditor('develop_message_board').getContent();
            $("input[name='dev_message']").val(common.HtmlEncode(message));
        });
    }

    return {
        Init: function () {
            SaveClick();
        }
    }
});

