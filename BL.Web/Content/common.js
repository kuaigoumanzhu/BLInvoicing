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