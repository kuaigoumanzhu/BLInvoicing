/// <reference path="../Content/BJUI/js/jquery-1.11.3.min.js" />
/// <reference path="../Content/BJUI/js/bjui-all.js" />
/*
grid基类
*/
function Grid() {
    this.maingrid = undefined;
    this.selectObjs = function () {
        var selectedDatas = this.maingrid.data("selectedDatas");
        if (selectedDatas == undefined || selectedDatas.length == 0) {
            $(this).alertmsg('warn', '至少选择一个对象');
            return null;
        }
        return selectedDatas;
    };
}

//选择单行对象
Grid.prototype.getSelectedObj = function () {
    var selectedDatas = this.selectObjs();
    return selectedDatas == null ? null : selectedDatas[0];
};
//选择多行对象
Grid.prototype.getSelectedMany = function () {
    return this.selectObjs();
};
//返回选择多行的属性JSON，默认选择id属性，如果选择其他属性，可重写
Grid.prototype.getSelectedProperties = function (propName) {
    var selected = this.selectObjs();
    if (selected == null) return null;

    var ids = selected.map(function (elem) { return elem[propName]; }).join(",");
    ids = '[' + ids + ']'; //拼成一个JSON
    return ids;
};
