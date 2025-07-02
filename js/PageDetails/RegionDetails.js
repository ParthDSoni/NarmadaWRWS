$(document).ready(function () {
    debugger
    BindRegion();
    BindscrollRegion();
});

function BindRegion() {
    $.ajax({
        type: "post",
        url: ResolveUrl("/BindRegionList"),
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (res) {
            var dataList = res;
            if (res.isError == true) {
                ShowMessage(res.strMessage, "", "error");
            } else {
                var strInnerHtml = "";

                $.each(res, function (data, value) {
                    var strSubStr = "";
                    var strpath = "";

                    if (value.imagePath != null && value.imagePath != undefined && value.imagePath != '') {
                        strpath = ResolveUrl("/ViewFile?fileName=" + GreateHashString(value.imagePath));
                    }

                    strSubStr += '<li class="recent-post-list-li" style="display: flex; margin-bottom: 10px;">';
                    strSubStr += '    <a class="recent-post-thum" href="' + ResolveUrl("/RegionDetails/" + FrontValue(value.researchId)) + '" style="margin-right: 10px;">';
                    if (strpath != "") {
                        strSubStr += '        <img src="' + strpath + '" alt="post-img" style="width: 60px; height: 60px; object-fit: cover;">';
                    }
                    strSubStr += '    </a>';
                    strSubStr += '    <div class="media-body">';
                    strSubStr += '        <a href="' + ResolveUrl("/RegionDetails/" + FrontValue(value.researchId)) + '">' + value.researchTitle + '</a>';
                    strSubStr += '    </div>';
                    strSubStr += '</li>';

                    strInnerHtml += strSubStr;
                });
                document.getElementById("LatestRegionArea").innerHTML = strInnerHtml;

                // Inject HTML into the UL inside #LatestSchemeArea
                document.querySelector("#LatestRegionArea ul").innerHTML = strInnerHtml;

                // Start auto-scroll
                StartAutoScroll();
            }
        }
    });
}
function StartAutoScroll() {
    const container = document.getElementById("LatestRegionArea");
    let scrollInterval = setInterval(function () {
        container.scrollTop += 1;
        if (container.scrollTop + container.clientHeight >= container.scrollHeight) {
            container.scrollTop = 0; // loop to top
        }
    }, 50); // speed of scroll
}
function BindscrollRegion() {

    var schemeId = $('#HiddenSchemeId').val();

    $.ajax({
        type: "post", url: ResolveUrl("/BindRegion/?schemeId=" + schemeId),
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (res) {
            let value = res;
            var strInnerHtml = "";

            var strSubStr = "";
            if (value.imagePath != null && value.imagePath != undefined && value.imagePath != '') {
                var strpath = ResolveUrl("/ViewFile?fileName=" + GreateHashString(value.imagePath));
            }

            strSubStr += `<article class="post blog-details">
                                <div class="post-thumbnail">
                                <div class="pbmit-featured-container">
                                <div class="pbmit-featured-wrapper">
                                <img src="${strpath}" class="img-fluid w-100" alt="">
                                </div>
                                </div>
                                </div>
                                <div class="post-content">
                                <div class="pbmit-blog-meta pbmit-blog-meta-top">
                                <h3>${value.researchTitle}</h3>
                                </div>${value.researchDesc}</div>
                                </article>`;

            strInnerHtml = strInnerHtml + strSubStr;
            document.getElementById("image").innerHTML = strInnerHtml;
        }

    });
}