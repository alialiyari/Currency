var setInnerHTML = function (elm, html) {
    elm.innerHTML = html;
    Array.from(elm.querySelectorAll("script")).forEach(oldScript => {
        const newScript = document.createElement("script");
        Array.from(oldScript.attributes)
            .forEach(attr => newScript.setAttribute(attr.name, attr.value));
        newScript.appendChild(document.createTextNode(oldScript.innerHTML));
        oldScript.parentNode.replaceChild(newScript, oldScript);
    });
}

function JsonToFormData(item) {
    var form_data = new FormData();

    for (var key in item) {
        form_data.append(key, item[key]);
    }
    return form_data;
}

async function FechData(request) {

    if (request.data == null) { request.data = {} }
    if (request.method == null) { request.method = 'POST' }

    ShowLoading();
    var formData = JsonToFormData(request.data);


    let response = await fetch(request.url, {
        credentials: 'include',
        method: request.method,
        headers: {
            'Accept': '*/*',
            //'Content-Type': 'application/json; charset=utf-8'
            //'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'
        },
        //data: JSON.stringify(request.data)
        //body: request.data
        //body: new URLSearchParams(request.data)
        //body: JSON.stringify(request.data)
        body: formData
    });
    HideLoading();

    if (response.status !== 200) {
        console.log(request);
        console.log(response.status + " - " + response.statusText);
        return null;
    }


    if (response.headers.get('content-type').indexOf('text/html') > -1) {
        return await response.text();
    } else {
        return await response.json();
    }
}



function IsAndroidApp() {
    if (typeof (invokeCSharpAction) == 'undefined') return false;
    return true;
}

function CallCSharpFunction(data) {
    if (IsAndroidApp() == false) return { Status: 0, Message: "این امکان برای برنامه موبایل پیش بینی شده است" }

    try {
        var result = invokeCSharpAction(JSON.stringify(data));
        return JSON.parse(result);
    }
    catch (e) {
        console.log(e);
    }
}

function ShowLoading() {
    if (IsAndroidApp() == true) {
        CallCSharpFunction({ action: "ShowLoading" });
    } else {
        $('.se-pre-con').removeClass('d-none');
    }
}

function HideLoading() {
    if (IsAndroidApp() == true) {
        CallCSharpFunction({ action: "HideLoading" });
    } else {
        $('.se-pre-con').addClass('d-none');
    }
}


$.ajaxSetup({
    beforeSend: function () { ShowLoading() },
    complete: function () { HideLoading() },
    success: function () { }
});

function ShowLoading() {
    if (IsAndroidApp() == true) {
        CallCSharpFunction({ action: "ShowLoading" });
    } else {
        $('.se-pre-con').removeClass('d-none');
    }
}
function HideLoading() {
    if (IsAndroidApp() == true) {
        CallCSharpFunction({ action: "HideLoading" });
    } else {
        $('.se-pre-con').addClass('d-none');
    }
}


async function OffCanvasShow(options) {
    ShowLoading();
    var response = await fetch(options.Url, { method: 'get' });
    if (response.status != 200) { console.log(response); $('.se-pre-con').addClass('d-none'); return; }

    var html = await response.text();
    HideLoading();



    var offConvasDiv = document.createElement('div');
    offConvasDiv.setAttribute('id', 'OffConvas' + ModalIndex);
    document.body.appendChild(offConvasDiv);
    //offConvasDiv.innerHTML = html;
    $(offConvasDiv).html(html);

    var myOffcanvas = document.getElementById('OffConvas' + ModalIndex).getElementsByClassName('offcanvas')[0];
    var bsOffcanvas = new bootstrap.Offcanvas(myOffcanvas);


    myOffcanvas.addEventListener('hidden.bs.offcanvas', function () {
        bsOffcanvas.dispose();
        $(offConvasDiv).remove();

        if (typeof options.OnClose === "function") { options.OnClose(); }
    })


    bsOffcanvas.show();
    ModalIndex += 1;

    //myModal.show();
    //$(".modal-backdrop ").last().css('z-index', ModalIndex - 1)
    //$('#Modal' + ModalIndex).find(".modal").css('z-index', ModalIndex);

}