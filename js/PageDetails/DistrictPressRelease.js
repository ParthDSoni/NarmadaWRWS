$(document).ready(function () {
    var globRes;
    var siteUrl = window.location.href;

    BindDistrictList();
    BindDepartmnetList();
    BindDistrictReleaseSlider();

    $("#facebook-share").click(function () {
        var facebookUrl = "https://www.facebook.com/sharer/sharer.php?u=" + encodeURIComponent(siteUrl);
        window.open(facebookUrl, "_blank");
    });

    $("#twitter-share").click(function () {
        var twitterUrl = "https://twitter.com/intent/tweet?url=" + encodeURIComponent(siteUrl);
        window.open(twitterUrl, "_blank");
    });

    $("#linkedin-share").click(function () {
        var linkedinUrl = "https://www.linkedin.com/sharing/share-offsite/?url=" + encodeURIComponent(siteUrl);
        window.open(linkedinUrl, "_blank");
    });

    $("#whatsapp-share").click(function () {
        var whatsappUrl = "https://api.whatsapp.com/send?text=" + encodeURIComponent(siteUrl);
        window.open(whatsappUrl, "_blank");
    });
});

$('#DepartmentId').change(function () {
    var DepartmentId = $(this).val();
    var DistrictId = $('#billing_state').val();
    var date = $('#date').val();
    var date1 = $('#date1').val();
    BindPressSlider(DepartmentId, DistrictId, date, date1);
});

$('#billing_state').change(function () {
    var DepartmentId = $('#DepartmentId').val();
    var DistrictId = $(this).val();
    var date = $('#date').val();
    var date1 = $('#date1').val();
    BindPressSlider(DepartmentId, DistrictId, date, date1);
});

$('#date').change(function () {
    var DepartmentId = $('#DepartmentId').val();
    var DistrictId = $('#billing_state').val();
    var date = $(this).val();
    var date1 = $('#date1').val();
    BindPressSlider(DepartmentId, DistrictId, date, date1);
});

$('#date1').change(function () {
    var DepartmentId = $('#DepartmentId').val();
    var DistrictId = $('#billing_state').val();
    var date = $('#date').val();
    var date1 = $(this).val();
    BindPressSlider(DepartmentId, DistrictId, date, date1);
});

function BindRelease(id, depName1, distName1) {
    LangId = $("#LanguageId").val();
    if (!isPRIDAlreadyVisited(id, LangId)) {
        incrementVisitorCount(id, LangId);
    } else {
        console.log("PRID IS ALREADY VISITED");
    }
    var token = $('input[name="AntiforgeryFieldname"]').val();

    $("#mainPressData").empty();

    var strTBody = "";
    strTBody = strTBody + "";
    let filteredData = globRes.filter(item => (item.id == id));

    const depName = depName1 ? `${depName1}` : '';
    const distName = distName1 ? ` ${distName1}` : '';

    var strpath = filteredData[0].imagePath;

    if (filteredData[0].otherLangData != null) {
        var strLink = ResolveUrl("/PressReleaseDetails/?id=" + filteredData[0].otherLangData.split("|")[1]);
    }

    if (strpath != null && strpath != "") {
        strpath = ResolveUrl("/ViewFile?fileName=" + GreateHashString(strpath));
        var docpath = ResolveUrl("/DownloadFile?fileName=" + GreateHashString(strpath));
    }

    strTBody = strTBody + `<div class="MinistryNameSubhead text-center">${depName}${distName}</div>`
    strTBody = strTBody + `<div class="log_oo d-flex justify-content-between" id="PressReleaseLogo">`
    strTBody = strTBody + `</div>`
    strTBody = strTBody + `<br>`
    if (strpath != "") {
        strTBody = strTBody + `<div class="ttm_single_image-wrapper mb-35">`
        strTBody = strTBody + `<img class="img-fluid" src="${strpath}" alt="single-img-nine">`
        strTBody = strTBody + `</div>`
    }
    strTBody = strTBody + `<div class="title text-center">`
    strTBody = strTBody + `<h4>${filteredData[0].pressTitle}</h4>`
    strTBody = strTBody + `</div>`
    if (!(filteredData[0].pressSubTitle == null)) {
        strTBody = strTBody + `<h5 class="subtitle text-center">`
        strTBody = strTBody + `${filteredData[0].pressSubTitle}`
        strTBody = strTBody + `</h5>`
    }
    if (!(filteredData[0].pressMultiTitle == null)) {
        var titles = filteredData[0].pressMultiTitle.split(';');
        titles = titles.filter(title => title.trim() !== '');
        var formattedTitles = titles.join('<br><br>');
        strTBody = strTBody + `<h6 class="subtitle text-center">`
        strTBody = strTBody + `${formattedTitles}`
        strTBody = strTBody + `</h6>`
    }
    strTBody = strTBody + `<div class="ReleaseDateSubHeaddateTime text-center pt20">`
    strTBody = strTBody + `${Intl.DateTimeFormat('en-US', {
        day: '2-digit',
        month: 'short',
        year: 'numeric',
        hour: 'numeric',
        minute: '2-digit',
        hour12: true
    }).format(new Date(filteredData[0].pressReleaseDate)).toUpperCase().replace(',', '').replace(',', '')
        } BY ${depName}${distName}`
    strTBody = strTBody + `</div>`
    strTBody = strTBody + `<div class="ttm-service-description text-center mt-2">`
    strTBody = strTBody + `${filteredData[0].pressDesc}`
    strTBody = strTBody + `</div>`
    strTBody = strTBody + `</div>`
    strTBody = strTBody + `<div class="doilatestupdates">`
    strTBody = strTBody + `<span id="lblViews"><b> Visitor Counter : </b>${filteredData[0].visitorCount}</span>`
    strTBody = strTBody + `</div>`
    if (filteredData[0].otherLangData != null) {
        strTBody = strTBody + `<div class="ReleaseLang" ><b> Read this release in : </b>`
        const OBJ_LANG = JSON.parse(filteredData[0].otherLangData);
        Object.keys(OBJ_LANG).forEach(language => {
            const pressId = OBJ_LANG[language];
            if (filteredData[0].departmentId == 0) {
                filteredData[0].departmentId = null;
            }
            if (filteredData[0].districtId == 0) {
                filteredData[0].districtId = null;
            }
            const strLink = ResolveUrl(`/PressReleaseDetailsL/?id=${pressId.split('|')[0]}&DepId=${filteredData[0].departmentId}&DistrictId=${filteredData[0].districtId}&LanguageId=${pressId.split('|')[1]}`);

            strTBody += `<a href="${strLink}" target="_blank">${language}</a>  `;
        });

        strTBody = strTBody + `</div>`
    }
    strTBody = strTBody + `</div>`
    $("#mainPressData").html(strTBody);
    BindPressReleaseLogo();
}

function BindDistrictReleaseSlider() {
    var token = $('input[name="AntiforgeryFieldname"]').val();

    $.ajax({
        type: "POST", url: ResolveUrl("/BindDistrictPressRealese"),
        contentType: "application/x-www-form-urlencoded",
        "data": { "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {
            globRes = res;
            if (res && (Array.isArray(res) ? res.length : Object.keys(res).length)) {
                BindRelease(globRes[0].id, globRes[0].depName, globRes[0].distName);
            }
            else {
                $("#PressSldier").html(`<div class="norecord">***No Release Found***</div>`);
                return;
            }
            $("#PressSldier").empty();

            var strTBody = "";
            strTBody = strTBody + "";
            $.each(res, function (data, value) {
                mainData = value;
                var strpath = mainData.imagePath;
                if (strpath != null && strpath != "") {
                    strpath = ResolveUrl("/ViewFile?fileName=" + GreateHashString(strpath));
                }

                const depName = mainData.depName ? `${mainData.depName} ` : '';
                const distName = mainData.distName ? ` ${mainData.distName}` : '';

                strTBody = strTBody + `<li class="popular-post-widget-item clearfix">`
                strTBody = strTBody + `<div class="popular-post-widget-brief">`
                strTBody = strTBody + `<a href="javascript:void(0);" onclick="BindRelease('${mainData?.id}','${depName}','${distName}')" class="ltn__blog-meta">`
                strTBody = strTBody + `<div class="ltn__blog-date"><i class="fa fa-university mr-10"></i>${mainData.distName}</div>`
                strTBody = strTBody + `<div>${mainData.pressTitle}</div >`
            });
            $("#PressSldier").html(strTBody);
        }
    });
}

function BindPressSlider(DepartmentId, DistrictId, date, date1) {
    var token = $('input[name="AntiforgeryFieldname"]').val();

    $.ajax({
        type: "POST", url: ResolveUrl("/BindPressReleasesGrid"),
        contentType: "application/x-www-form-urlencoded",
        "data": { "DepartmentId": DepartmentId, "DistrictId": DistrictId, "date": date, "date1": date1, "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {
            globRes = res;
            if (res && (Array.isArray(res) ? res.length : Object.keys(res).length)) {
                BindRelease(globRes[0].id, globRes[0].depName, globRes[0].distName);
            }
            else {
                $("#PressSldier").html(`<div class="norecord">***No Release Found***</div>`);
                return;
            }
            $("#PressSldier").empty();

            var strTBody = "";
            strTBody = strTBody + "";
            $.each(res, function (data, value) {
                mainData = value;
                var strpath = mainData.imagePath;
                if (strpath != null && strpath != "") {
                    strpath = ResolveUrl("/ViewFile?fileName=" + GreateHashString(strpath));
                }

                const depName = mainData.depName ? `${mainData.depName}` : '';
                const distName = mainData.distName ? `, ${mainData.distName}` : '';

                strTBody = strTBody + `<li class="popular-post-widget-item clearfix">`
                strTBody = strTBody + `<div class="popular-post-widget-brief">`
                strTBody = strTBody + `<a href="javascript:void(0);" onclick="BindRelease('${mainData?.id}','${depName}','${distName}')" class="ltn__blog-meta">`
                strTBody = strTBody + `<div class="ltn__blog-date"><i class="fa fa-university mr-10"></i>${depName}${distName}</div>`
                strTBody = strTBody + `<div>${mainData.pressTitle}</div>`
            });
            $("#PressSldier").html(strTBody);
        }
    });
}
