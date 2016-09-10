//供应商查询下拉
function LoadGYSCategory(selectID)
{
    LoadDictCategory(selectID, 'SupplierCategory');
}
//省份查询下拉
function LoadProvince(selectID) {
    LoadDictCategory(selectID, 'Province');
}
//商品查询下拉
function LoadGoodsCategory(selectID) {
    LoadDictCategory(selectID, 'GoodsCategory');
}
//计量单位查询下拉
function LoadUnit(selectID) {
    LoadDictCategory(selectID, 'Unit');
}

function LoadDictCategory(selectID, CategoryID)
{
    var url = "/DATADICT/GetCategoryJSON";
    if (CategoryID != "" && CategoryID != undefined)
    {
        url += "?FCATEGORY="+CategoryID;
    }
    $(this).bjuiajax("doAjax", {
        url: url,
        async: false,
        callback: function (json) {
            $("#"+selectID).html('');
            var _html = "<option value=''>全部</option>";
            for (var i = 0; i < json.length; i++) {
                _html += "<option value='" + json[i].FID + "'>" + json[i].FNAME + "</option>"
            }
            $("#"+selectID).html(_html);
        }
    });
}
function LoadWareHose(selectID, CategoryID) {
    var url = "/WareHose/GetAllWareHoseJson?FSTATUS=2";
    if (CategoryID != "" && CategoryID != undefined) {
        url += "&FCATEGORY=" + CategoryID;
    }
    $(this).bjuiajax("doAjax", {
        url: url,
        async: false,
        callback: function (json) {
            $.CurrentNavtab.find("#" + selectID).html('');
            var _html = "<option value=''>全部</option>";
            for (var i = 0; i < json.length; i++) {
                _html += "<option value='" + json[i].FID + "'>" + json[i].FNAME + "</option>"
            }
            $.CurrentNavtab.find("#" + selectID).html(_html);
        }
    });
}
//供应商数据表格编辑添加下拉
function LoadGYSDataForRowSel() {
    return LoadDataForRowSel( 'SupplierCategory');
}
////省份数据表格编辑添加下拉
function LoadProvinceDataForRowSel() {
    return LoadDataForRowSel( 'Province');
}
//商品数据表格编辑添加下拉
function LoadGoodsDataForRowSel() {
    return LoadDataForRowSel( 'GoodsCategory');
}
//计量单位数据表格编辑添加下拉
function LoadUnitDataForRowSel() {
    return LoadDataForRowSel( 'Unit');
}

function LoadDataForRowSel(categoryId)
{
    var rel ;
    var url = "/DATADICT/GetDataCategoryJson";
    if (categoryId != "" && categoryId != undefined) {
        url += "?FCATEGORY=" + categoryId;
    }
    $(this).bjuiajax("doAjax", {
        url: url,
        async: false,
        callback: function (json) {
            rel = json;
        }
    });
    return rel;
}
function LoadSupplierForRowSel()
{
    var rel;
    var url = "/SUPPLIER/GetSupplierJson";
    $(this).bjuiajax("doAjax", {
        url: url,
        async: false,
        callback: function (json) {
            rel = json;
        }
    });
    return rel;
}
function LoadPersonForRowSel() {
    var rel;
    var url = "/Person/GetPersonJson";
    $(this).bjuiajax("doAjax", {
        url: url,
        async: false,
        callback: function (json) {
            rel = json;
        }
    });
    return rel;
}
function LoadPerson(selectID) {
    var url = "/Person/GetPersonJsonInfo";
    $(this).bjuiajax("doAjax", {
        url: url,
        async: false,
        callback: function (json) {
            $.CurrentNavtab.find("#" + selectID).html('');
            var _html = "<option value=''>全部</option>";
            for (var i = 0; i < json.length; i++) {
                _html += "<option value='" + json[i].FID + "'>" + json[i].FNAME + "</option>"
            }
            $.CurrentNavtab.find("#" + selectID).html(_html);
        }
    });
}
//hpf 表格下来列表根据value获得text
(function ($) {
    $.fn.GetSelectText = function () {
        var lst = arguments[0];
        var key = arguments[1];
        for (var i = 0, l = lst.length; i < l; i++) {
            for (var item in lst[i]) {
                if (item == key) {
                    return lst[i][key];
                }
            }
        }
    };
    //hpf 2015 11 用来查找菜单数据item要查找字段值，lst菜单数据，name要查找字段名字
    $.IndexOf = function (item, lst, name) {
        if (!lst)
            return -1;
        var isObj = typeof (item) == "object";
        for (var i = 0, l = lst.length; i < l; i++) {
            if (isObj) {
                if (lst[i] == item)
                    return i;
            } else {
                if (lst[i][name] && lst[i][name].toString().toUpperCase() == item.toString().toUpperCase())
                    return i;
            }
        }
        return -1;
    }
    //sxj 2016 09 用来查找菜单数据item要查找字段值，lst菜单数据，name要查找字段名字
    $.IndexOfMore = function (item,item1, lst, name,name1) {
        if (!lst)
            return -1;
        var isObj = typeof (item) == "object";
        var isObj1 = typeof (item1) == "object";
        for (var i = 0, l = lst.length; i < l; i++) {
            if (isObj && isObj1) {
                if (lst[i] == item)
                    return i;
            } else {
                if (lst[i][name] && lst[i][name1] && lst[i][name].toString().toUpperCase() == item.toString().toUpperCase() && lst[i][name1].toString().toUpperCase() == item1.toString().toUpperCase())
                    return i;
            }
        }
        return -1;
    }
})(jQuery);
//hpf 局部打印
(function ($) {
    var printAreaCount = 0;
    $.fn.printArea = function () {
        var ele = $(this);
        var idPrefix = "printArea_";
        removePrintArea(idPrefix + printAreaCount);
        printAreaCount++;
        var iframeId = idPrefix + printAreaCount;
        var iframeStyle = 'position:absolute;width:0px;height:0px;left:-500px;top:-500px;';
        iframe = document.createElement('IFRAME');
        $(iframe).attr({
            style: iframeStyle,
            id: iframeId
        });
        document.body.appendChild(iframe);
        var doc = iframe.contentWindow.document;
        $(document).find("link").filter(function () {
            return $(this).attr("rel").toLowerCase() == "stylesheet";
        }).each(
                function () {
                    doc.write('<link type="text/css" rel="stylesheet" href="'
                            + $(this).attr("href") + '" >');
                });
        doc.write('<div class="' + $(ele).attr("class") + '">' + $(ele).html()
                + '</div>');
        doc.close();
        var frameWindow = iframe.contentWindow;
        frameWindow.close();
        frameWindow.focus();
        frameWindow.print();
    }
    var removePrintArea = function (id) {
        $("iframe#" + id).remove();
    };
})(jQuery);