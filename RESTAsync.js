///
/// Akutra R.A> Cea

/// REST DB Access JS
/// Routines for threaded access to REST API interface
/// --- JQuery version ---

function httpGetDataCount(controller, findfield, callback) {

    var _articleId = findfield;

    if (_articleId != null){
        _articleId = "searchfor=" + _articleId + "&";} else { _articleId = ""; }

    _articleId += "count=true";


    $.ajax({
        // HTTP Call
        url: "/" + controller + '/get',
        type: "GET",
        dataType: 'json',
        data: _articleId
   ,
        // Headers
        headers: {
            "Content-Type": "application/json"
        },
        success: function (data) {
            // Succesful Call
            callback(JSON.stringify(data));
            //console.write(data);
        },
        error: function (jqxhr, textStatus, errorThrown) {
            //report error
            console.log(textStatus, errorThrown);
            callback(errorThrown + ": " + textStatus);
        }
    });

}

function httpGetDataContent(controller, findfield, callback) {

    var _querystring = findfield;

    if (_querystring != null) {
        _querystring = "searchfor=" + _querystring;
    } else { _querystring = ""; }

    $.ajax({
        // HTTP Call
        url: "/" + controller + '/get',
        type: "GET",
        dataType: 'json',
        data: _querystring
   ,
        // Headers
        headers: {
            "Content-Type": "application/json"
        },
        success: function (data) {
            // Succesful Call
            callback(JSON.stringify(data));
        },
        error: function (jqxhr, textStatus, errorThrown) {
            //report error
            console.log(textStatus, errorThrown);
            callback(errorThrown + ": " + textStatus);
        }
    });

}

function httpPutDataContent(controller, newdata, callback) {

    $.ajax({
        // HTTP Call
        url: "/" + controller + '/put',
        type: "PUT",
        dataType: 'json',
        data: newdata
   ,
        // Headers
        headers: {
            "Content-Type": "application/json"
        },
        success: function (data) {
            // Succesful Call
            callback(JSON.stringify(data));
        },
        error: function (jqxhr, textStatus, errorThrown) {
            //report error
            console.log(textStatus, errorThrown);
            callback(errorThrown + ": " + textStatus);
        }
    });

}

function httpPostDataContent(controller, findfield, updatedata, callback) {


    $.ajax({
        // HTTP Call
        url: "/" + controller + '/POST',
        type: "POST",
        dataType: 'json',
        search: findfield,
		data: updatedata
   ,
        // Headers
        headers: {
            "Content-Type": "application/json"
        },
        success: function (data) {
            // Succesful Call
            callback(JSON.stringify(data));
        },
        error: function (jqxhr, textStatus, errorThrown) {
            //report error
            console.log(textStatus, errorThrown);
            callback(errorThrown + ": " + textStatus);
        }
    });

}
