function isPRIDAlreadyVisited(PRID, LangId) {
    return sessionStorage.getItem(PRID + '_' + LangId) !== null;
}

function incrementVisitorCount(PRID, LangId) {
    var token = $('input[name="AntiforgeryFieldname"]').val();
    var LangId = $("#LanguageId").val();
    $.ajax({

        type: "POST", url: ResolveUrl("/GetPressViewCount"),
        contentType: "application/x-www-form-urlencoded",
        data: { "AntiforgeryFieldname": token, "PressId": PRID },
        dataType: "json",
        success: function (data) {
            if (data.result != null && data.result != undefined) {
                newcount = data.result.visitorCount;
                $('#lblViews').html(`<b> Visitor Counter : </b>${newcount}`);
            }
            else {
                console.log("data is null");
            }
            sessionStorage.setItem(PRID + '_' + LangId, 'visited');
        },
        error: function () {
            $('#visitorCount').text('Error updating count');
        }
    });
}

function BindPressReleaseLogo() {
    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({
        type: "POST", url: ResolveUrl("/BindPressReleaseLogo"),
        contentType: "application/x-www-form-urlencoded",
        data: { "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {
            $("#PressReleaseLogo").empty();
            $("#PressReleaseLogo").html(res);
            ResolveUrlHTML();
        }
    });
}

function BindDistrictList() {
    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({
        type: "GET", url: ResolveUrl("/GetDistrictData"),
        contentType: "application/x-www-form-urlencoded",
        data: { "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {
            $("#billing_state").empty();
            $("#billing_state").append($("<option></option>").val(null).text("Select District"));

            $.each(res.data, function (data, value) {
                $("#billing_state").append($("<option></option>").val(value.distId).html(value.distName));
            });
            HideLoader();
        }
    });
    HideLoader();
}

function BindDepartmnetList() {
    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({
        type: "GET", url: ResolveUrl("/GetDepartmentData"),
        contentType: "application/x-www-form-urlencoded",
        data: { "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {
            $("#DepartmentId").empty();
            $("#DepartmentId").append($("<option></option>").val(null).text("Select Department"));
            $.each(res.data, function (data, value) {
                $("#DepartmentId").append($("<option></option>").val(value.depId).html(value.depName));
            });
            HideLoader();
        }
    });
    HideLoader();
}

function PrintPressReleasePDF() {
    var divToPrint = document.getElementById('mainPressData');

    var newWin = window.open('', 'Print-Window');
    newWin.document.open();
    newWin.document.write('<html><head>');

    var stylesheets = document.querySelectorAll('link[rel="stylesheet"]');
    stylesheets.forEach(function (stylesheet) {
        newWin.document.write(stylesheet.outerHTML);
    });
    newWin.document.write('</head><body>' + divToPrint.innerHTML + '</body></html>');

    newWin.onload = function () {
        newWin.print();
    };

    newWin.document.close();
}

var currentDate = new Date().toISOString().split('T')[0];

document.getElementById('date').setAttribute('max', currentDate);
document.getElementById('date1').setAttribute('max', currentDate);