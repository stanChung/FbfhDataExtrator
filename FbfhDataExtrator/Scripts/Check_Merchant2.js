function merchant_dispatch(form) {
    $('#bdy').showLoading();
    if (form.basic_select[0].checked) {
        if (!businesscode(form.uni_number.value)) {
            form.uni_number.focus();
            $('#bdy').showLoading();
            return (false);
        }
    }

    if (form.basic_select[1].checked && form.chinese_name.value == "") {
        alert("請輸入廠商中文名稱");
        form.chinese_name.focus();
        $('#bdy').hideLoading();
        return (false);
    }
    if (form.basic_select[1].checked && form.chinese_name.value.length < 2) {
        alert("請至少輸入二碼廠商中文名稱");
        form.chinese_name.focus();
        $('#bdy').hideLoading();
        return (false);
    }
    if (form.basic_select[2].checked && form.english_name.value == "") {
        alert("請輸入廠商英文名稱");
        form.english_name.focus();
        $('#bdy').hideLoading();
        return (false);
    }
    if (form.basic_select[3].checked && form.owner.value == "") {
        alert("請輸入廠商負責人姓名");
        form.owner.focus();
        $('#bdy').hideLoading();
        return (false);
    }
    if (form.basic_select[3].checked && form.owner.value.length < 2) {
        alert("請至少輸入二碼中文姓名");
        form.owner.focus();
        $('#bdy').hideLoading();
        return (false);
    }
    if (form.basic_select[4].checked && form.product.value == "") {
        alert("請輸入ＣＣＣcode 前四碼");
        form.product.focus();
        $('#bdy').hideLoading();
        return (false);
    }
    if (form.basic_select[5].checked && form.product_name.value == "") {
        alert("請輸入出進口貨品");
        form.product_name.focus();
        $('#bdy').hideLoading();
        return (false);
    }

    //if (form.txtCheckCode.value=="")
    //{
    //  	alert("請輸入圖片驗證碼");
    //  	form.txtCheckCode.focus();
    //  	return(false);
    //}

    form.action = "/home/QueryForward";
    //form.action="redirect.asp";

    form.submit();
}