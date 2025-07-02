function BindFrontLanguage() {
    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({
        type: "POST", url: ResolveUrl("/BindLanguage"),
        contentType: "application/x-www-form-urlencoded",
        data: { "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {
            $("#langId").empty();
            $.each(res, function (data, value) {
                $("#langId").append($("<option></option>").val(value.value).html(value.text));
            });

            if ($("#lgLanguageId").val() != null && $("#lgLanguageId").val() != undefined) {
                if ($("#lgLanguageId").val() == "0") {
                    $("#lgLanguageId").val("1");
                }
                $("#langId").val($("#lgLanguageId").val());
            }
        }
    });
}

function BindFooter() {
    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({
        type: "POST", url: ResolveUrl("/BindFooter"),
        contentType: "application/x-www-form-urlencoded",
        data:
            { "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {
            $("#dvFooter").empty();
            $("#dvFooter").html(res);
            ResolveUrlHTML();
        }
    });
}

function BindRelatedlinks() {
    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({
        type: "POST", url: ResolveUrl("/BindRelatedlinks"),
        contentType: "application/x-www-form-urlencoded",
        data:
            { "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {
            $("#Relatedlinks").empty();
            $("#Relatedlinks").html(res);
            ResolveUrlHTML();
        }
    });
}

function BindHeader() {
    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({
        type: "POST", url: ResolveUrl("/BindHeader"),
        contentType: "application/x-www-form-urlencoded",
        data: { "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {

            $("#dvHeadermenu").empty();
            $("#dvHeadermenu").html(res);
            ResolveUrlHTML();
        }
    });
}
function BindMainMobileMenuFooter() {

    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({
        type: "POST", url: ResolveUrl("/BindMainMobileMenuFooter"),
        contentType: "application/x-www-form-urlencoded",
        data: { "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {
            $("#dvMainMobileMenuFooter").empty();
            $("#dvMainMobileMenuFooter").html(res);
            ResolveUrlHTML();
        }
    });
}

function Bindmainlogo() {
    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({
        type: "POST", url: ResolveUrl("/Bindmainlogo"),
        contentType: "application/x-www-form-urlencoded",
        data: { "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {

            $("#Dvmainlogo").empty();
            $("#Dvmainlogo").html(res);
            ResolveUrlHTML();
        }
    });
}




$(document).ready(function () {

    var lgLangId = $("#lgLanguageId").val();

    if (lgLangId == "1") {
        document.getElementById("ScreenReader").innerHTML = '<i class="fa fa-desktop"></i>Screen Reader Access';
        document.getElementById("Skiptomain").innerHTML = 'Skip to main Content';
    }
    else if (lgLangId == "2") {
        document.getElementById("ScreenReader").innerHTML = '<i class="fa fa-desktop"></i>સ્ક્રીન રીડર એક્સેસ';
        document.getElementById("Skiptomain").innerHTML = 'મુખ્ય વિષયવસ્તુ';
    }
    GetWebSiteVisitorsCount();
    BindFrontLanguage();
    BindHeader();
    BindFooter();
    Bindmainlogo();
    BindMainMobileMenuFooter();
    BindRelatedlinks();

    $("#langId").change(function () {

        var token = $('input[name="AntiforgeryFieldname"]').val();
        $.ajax({

            type: "POST", url: ResolveUrl("/UpdateLanguage"),
            contentType: "application/x-www-form-urlencoded",
            data: { "AntiforgeryFieldname": token, "langId": $('option:selected', this).val() },
            dataType: "json",
            success: function (res) {
                $("#lgLanguageId").val(res);
                $("#langId").val(res);
                location.reload();
            }
        });
    });

    var els = document.querySelectorAll("a[href*='/ViewFile?fileName=']");
    for (var i = 0; i < els.length; i++) {
        els[i].href = ResolveUrl(els[i].getAttribute('href'));
    }
});

function GetWebSiteVisitorsCount() {

    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({
        type: "POST",
        url: ResolveUrl("/GetWebSiteVisitorsCount"),
        contentType: "application/x-www-form-urlencoded",
        data: { "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {
            $("#CounterVisitor").empty();
            $("#CounterVisitor").html(res.result.totalCount);
            ResolveUrlHTML();
        }
    });

}

function ResolveUrlHTML() {


    let elss = document.querySelectorAll("img[src]");
    for (let i = 0; i < elss.length; i++) {
        var src = elss[i].getAttribute('src');
        if (src.includes("java")) {
            src = (src).split(baseUrl.substring(0, (baseUrl.length - 1))).join('').substring(1);
            elss[i].src = (src);
        }
        else if (!src.startsWith("http") && !src.includes("java") && !src.includes("#") && !src.includes("data:") && src.includes(baseUrl.substring(0, (baseUrl.length - 1)) + baseUrl.substring(0, (baseUrl.length - 1)))) {

            src = baseUrl.substring(0, (baseUrl.length - 1)) + ResolveUrl(src).split(baseUrl.substring(0, (baseUrl.length - 1))).join('');
            elss[i].src = (src);
        }
        if (!src.startsWith("#") && !src.startsWith("mailto") && !src.startsWith("tel:") && !src.includes("data:") && !src.includes("embed") && !src.startsWith(baseUrl) && !src.startsWith("User") && !src.startsWith("http") && !src.includes("java") && !src.includes("#")) {

            src = baseUrl.substring(0, (baseUrl.length - 1)) + ResolveUrl(src).split(baseUrl.substring(0, (baseUrl.length - 1))).join('');

            elss[i].src = src;

        }

    }

    let sadasd = document.querySelectorAll("a[href]");
    for (let i = 0; i < sadasd.length; i++) {
        var href = sadasd[i].getAttribute('href');

        if (href.includes("java")) {
            if (!href.startsWith("java")) {

                href = (href).split(baseUrl.substring(0, (baseUrl.length - 1))).join('').substring(1);
                sadasd[i].href = (href);
            }
        }
        else if (!href.startsWith("#") && !href.startsWith("mailto") && !href.startsWith("tel:") && !href.includes("embed") && !href.startsWith(baseUrl) && !href.startsWith("User") && !href.startsWith("http") && !href.startsWith("java") && !href.startsWith("#")) {

            href = baseUrl.substring(0, (baseUrl.length - 1)) + ResolveUrl(href).split(baseUrl.substring(0, (baseUrl.length - 1))).join('');

            sadasd[i].href = (href);

        }
        else if (!href.startsWith("http") && !href.includes("java") && !href.includes("#") && href.includes(baseUrl.substring(0, (baseUrl.length - 1)) + baseUrl.substring(0, (baseUrl.length - 1)))) {

            href = baseUrl.substring(0, (baseUrl.length - 1)) + ResolveUrl(href).split(baseUrl.substring(0, (baseUrl.length - 1))).join('');
            sadasd[i].href = (href);
        }
    }

}