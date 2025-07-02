$(document).ready(function () {
    var globRes;
    var siteUrl = window.location.href;
    BindPressReleasesGrid();

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
    strTBody = strTBody + `<div class="title">`
    strTBody = strTBody + `<h4>${filteredData[0].pressTitle}</h4>`
    strTBody = strTBody + `</div>`
    strTBody = strTBody + `<div class="subtitle">`
    strTBody = strTBody + `<p>${filteredData[0].pressSubTitle}</p>`
    strTBody = strTBody + `</div>`
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
    strTBody = strTBody + `<br>`
    strTBody = strTBody + `<div class="ttm-service-description">`
    strTBody = strTBody + `${htmlDecode(filteredData[0].pressDesc)}`
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
            const strLink = ResolveUrl(`/PressReleaseDetailsL/?id=${pressId}&DepId=${filteredData[0].departmentId}&DistrictId=${filteredData[0].districtId}`);

            strTBody += `<a href="${strLink}" target="_blank">${language}</a>  `;
        });

        strTBody = strTBody + `</div>`
    }
    strTBody = strTBody + `</div>`
    $("#mainPressData").html(strTBody);
    BindPressReleaseLogo();
}

function BindPressReleasesGrid() {
    const resid = $("#Id").val();
    const respressid = $("#PressId").val();
    const depName1 = $("#DepId").val();
    const distName1 = $("#DistrictId").val();
    const LangId = $("#LanguageId").val();
    const laluLangId = $("#lglaluId").val();

    if (!isPRIDAlreadyVisited(resid, LangId)) {
        incrementVisitorCount(resid, LangId);
    } else {
        console.log("PRID IS ALREADY VISITED");
    }
    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({
        type: "POST", url: ResolveUrl("/PressReleaseDetailsDataL"),
        contentType: "application/x-www-form-urlencoded",
        "data": { "id": resid, "DepId": depName1, "DistrictId": distName1, "LangId": laluLangId, "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {
            ShowLoader();
            const depName = res?.depName ? `${res?.depName} ` : '';
            const distName = res?.distName ? ` ${res?.distName}` : '';
            $("#mainPressData").empty();

            var strTBody = "";
            strTBody = strTBody + "";

            var strpath = res?.imagePath;

            if (strpath != null && strpath != "") {
                strpath = ResolveUrl("/ViewFile?fileName=" + GreateHashString(strpath));
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
            strTBody = strTBody + `<h4>${res.pressTitle}</h4>`
            strTBody = strTBody + `</div>`
            if (!(res.pressSubTitle == null)) {
                strTBody = strTBody + `<h5 class="subtitle text-center">`
                strTBody = strTBody + `${res.pressSubTitle}`
                strTBody = strTBody + `</h5>`
            }
            if (!(res.pressMultiTitle == null)) {
                var titles = res.pressMultiTitle.split(';');
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
            }).format(new Date(res.pressReleaseDate)).toUpperCase().replace(',', '').replace(',', '')
                } BY ${depName} ${distName}`
            strTBody = strTBody + `</div>`
            strTBody = strTBody + `<div class="ttm-service-description text-center mt-2">`
            strTBody = strTBody + `${res.pressDesc}`
            strTBody = strTBody + `</div>`
            strTBody = strTBody + `</div>`
            strTBody = strTBody + `<div class="doilatestupdates">`
            strTBody = strTBody + `<span id="lblViews"><b> Visitor Counter : </b>${res.visitorCount}</span>`
            strTBody = strTBody + `</div>`
            if (res.otherLangData != null) {
                strTBody = strTBody + `<div class="ReleaseLang" ><b> Read this release in : </b>`
                const OBJ_LANG = JSON.parse(res.otherLangData);
                Object.keys(OBJ_LANG).forEach(language => {
                    const pressId = OBJ_LANG[language];
                    if (res.departmentId == 0) {
                        res.departmentId = null;
                    }
                    if (res.districtId == 0) {
                        res.districtId = null;
                    }
                    const strLink = ResolveUrl(`/PressReleaseDetailsL/?id=${pressId.split('|')[0]}&DepId=${res.departmentId}&DistrictId=${res.districtId}&LanguageId=${pressId.split('|')[1]}`);
                    strTBody += `<a href="${strLink}" target="_blank">${language}</a>  `;
                });

                strTBody = strTBody + `</div>`
            }
            strTBody = strTBody + `</div>`
            $("#mainPressData").html(strTBody);
            BindPressReleaseLogo();
            HideLoader();
        }
    });
}