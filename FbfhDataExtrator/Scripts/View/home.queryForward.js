
$('#dgCust').datagrid({
    idField: 'CompanyUni',
    iconCls: 'icon-save',
    selectOnCheck: false,
    checkOnSelect: false,
    singleSelect: true,
    pagination: false,
    toolbar: '#tbar',
    pageList: [10],
    columns: [[
        { field: 'ck', checkbox: true },
        { field: 'ComopanyCName', title: '廠商中文名稱' },
        {
            field: 'CompanyEName',
            title: '廠商英文名稱',
            formatter: function (v, r, i) {
                var button =
                    '<a href="#" onclick="openCustDetailWindow(' + i + ')">' + v + '</a>';

                return button;
            }
        },
        { field: 'COMPANY_OWNER', title: '廠商代表人姓名' }
    ]],
    data: formModel
});

//分頁動作
//var pg = $('#dgCust').datagrid('getPager');
//$(pg).pagination({
//    onSelectPage: function (pgn, pgs) {
//        $('#bdy').showLoading();

//        var ajaxSettings = {
//            type: 'get',
//            datatype: 'json',
//            url: '/home/PagingQuery',
//            data: {page:pgn,rows:pgs},
//            success: function (d, txtStatus, xhr) {

//                var gg = d;

//            },
//            error: function (jqXHR, txtStatus, errorThrown) {
//                alert(jqXHR);
//            },
//            complete: function (jqXHR, txtStatus) {
//                $('#bdy').hideLoading();
//            }
//        };


//        $.ajax(ajaxSettings);
//    }
//});



function openCustDetailWindow(index) {
    $('#bdy').showLoading();

    var rowCust = $('#dgCust').datagrid('getRows')[index];
    var dContent = '';

    var ajaxSettings = {
        type: 'get',
        datatype: 'json',
        url: '/home/GetCustDetailHtml',
        data: rowCust,
        success: function (d, txtStatus, xhr) {

            dContent = d;
            $('#winDetail').window('open');
            $('#pnlDetail').panel({
                fit: true,
                noheader: true,
                content: dContent
            });

        },
        error: function (jqXHR, txtStatus, errorThrown) {
            alert(jqXHR);
        },
        complete: function (jqXHR, txtStatus) {
            $('#bdy').hideLoading();
        }
    };

    $.ajax(ajaxSettings);



    //var url = "https://fbfh.trade.gov.tw/rich/text/fbj/asp/fbje140Q2.asp?uni_no=" + rowCust.CompanyUni + "&pcy=" + rowCust.PCY;

}

//儲存按鈕
function Import() {
    $('#bdy').showLoading();
    var rows = $('#dgCust').datagrid('getChecked');

    if (rows) {
        var ajaxSettings = {
            type: 'post',
            datatype: 'json',
            url: '/home/SaveData',
            data: {lst:rows},
            success: function (d, txtStatus, xhr) {

                if (d.isSuccess) {
                    $.messager.alert('系統訊息', d.msg, 'info', function () {
                        window.location = "/home/index";
                    });
                }
                else {
                    $.messager.show({
                        title: 'Warning',
                        msg: d.msg,
                        timeout: 0,
                        height: 'auto',
                        width:'auto',
                        showType: 'slide'
                    });
                }




            },
            error: function (jqXHR, txtStatus, errorThrown) {
                alert(jqXHR);
            },
            complete: function (jqXHR, txtStatus) {
                $('#bdy').hideLoading();
            }
        };

        $.ajax(ajaxSettings);
    }
    else { $.messager.alert('Warning', '請至少選擇一筆資料'); }
}