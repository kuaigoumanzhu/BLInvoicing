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