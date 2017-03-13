define(function () {
    return {
        RECORD_TYPE_TRANSFER: 3,//转账
        RECORD_TYPE_LOAN: 4,//借贷

        //Html转义
        HtmlEncode: function (value) {
            return $('<div/>').text(value).html();
        },
        //Html解码获取Html实体
        HtmlDecode: function (value) {
            return $('<div/>').html(value).text();
        },
        StringToDate: function (string) {
            var parse = new Date(string.replace(/-/g, "/"));
            if (parse == "Invalid Date") {
                return undefined;
            }
            return parse;
        }
    }
});






