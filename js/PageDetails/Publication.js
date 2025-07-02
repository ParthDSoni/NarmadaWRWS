$(document).ready(function () {
    BindPublication();
});

$(document).on('click', '#viewDoc', function (e) {
    var token = $('input[name="AntiforgeryFieldname"]').val();
    var publicationId = $(this).data('publication-id');
    $.ajax({

        type: "POST", url: ResolveUrl("/GetViewCount"),
        contentType: "application/x-www-form-urlencoded",
        data: { "AntiforgeryFieldname": token, "PublicationId": publicationId, "isDownload": 0 },
        dataType: "json",
        success: function (res) {
            $(`#PublicationViewCount-${publicationId}`).html("View Count: " + res.result.viewCount);
            console.log(res)
        }
    });
});

$(document).on('click', '#downloadDoc', function (e) {
    var token = $('input[name="AntiforgeryFieldname"]').val();
    var publicationId = $(this).data('publication-id');
    $.ajax({

        type: "POST", url: ResolveUrl("/GetViewCount"),
        contentType: "application/x-www-form-urlencoded",
        data: { "AntiforgeryFieldname": token, "PublicationId": publicationId, "isDownload": 1 },
        dataType: "json",
        success: function (res) {
            $(`#PublicationDownloadCount-${publicationId}`).html("Download Count: " + res.result.downloadCount);
            console.log(res)
        }
    });
});

function BindPublication(index) {
    var currentpage = 1;
    if (index)
        currentpage = index;
    const publicationTID = $("#PublicationTypeId").val();
    var token = $('input[name="AntiforgeryFieldname"]').val();

    $.ajax({
        type: "POST", url: ResolveUrl("/BindPublicationGrid"),
        contentType: "application/x-www-form-urlencoded",
        "data": { "publicationTypeId": publicationTID, "currentPage": currentpage, "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {
            $("#publicationDiv").empty();

            var strTBody = "";
            strTBody = strTBody + "";
            $.each(res.data, function (data, value) {
                mainData = value;
                console.log(mainData)
                var strpath = mainData.coverPhotoPath;
                if (strpath != null && strpath != "") {
                    var strpath = ResolveUrl("/ViewFile?fileName=" + GreateHashString(strpath));
                }
                var docpath = ResolveUrl("/DownloadFile?fileName=" + GreateHashString(mainData.imagePath));
                var viewdoc = ResolveUrl("/ViewFile?fileName=" + GreateHashString(mainData.imagePath));

                strTBody = strTBody + `<div class="col-md-6">`
                strTBody = strTBody + `    <div class="tab-pane fade show active" id="tab_1" role="tabpanel">`
                strTBody = strTBody + `        <div class="gujarati-publication news_side">`
                strTBody = strTBody + `            <div class="row no-gutters featured-imagebox featured-imagebox-blog ttm-box-view-left-image box-shadow">`
                strTBody = strTBody + `                <div class="col-lg-4 col-md-12 col-sm-6 ttm-featured-img-left">`
                strTBody = strTBody + `                    <div class="featured-thumbnail">`
                strTBody = strTBody + `                        <img class="img-fluid" src="${strpath}" alt="blog">`
                strTBody = strTBody + `                    </div>`
                strTBody = strTBody + `                </div>`
                strTBody = strTBody + `                <div class="col-lg-8 col-md-12 col-sm-6 featured-content">`
                strTBody = strTBody + `                    <div class="post-meta">`
                strTBody = strTBody + `                        <span class="ttm-meta-line">Published On : ${mainData.blogdateconvert}</span>`
                strTBody = strTBody + `<span class="ttm-meta-line" id="PublicationViewCount-${value.publicationId}">View Count: ${value.viewCount}</span >`
                strTBody = strTBody + `<span class="ttm-meta-line" id="PublicationDownloadCount-${value.publicationId}">Download Count: ${value.downloadCount}</span>`
                strTBody = strTBody + `                    </div>`
                strTBody = strTBody + `                    <div class="post-title featured-title">`
                strTBody = strTBody + `                        <h5><a href="#">${mainData.publicationTitle}</a></h5>`
                strTBody = strTBody + `                    </div>`
                strTBody = strTBody + `                    <span class="category">`
                strTBody = strTBody + `<a href="${docpath}" id="downloadDoc" data-publication-id="${value.publicationId}"> <i class="fa fa-download"></i> Download</a>`
                strTBody = strTBody + `<a href="${viewdoc}" target="_blank" id="viewDoc" data-publication-id="${value.publicationId}"> <i class="fa fa-eye"></i> View</a>`
                strTBody = strTBody + `                    </span>`
                strTBody = strTBody + `                </div>`
                strTBody = strTBody + `            </div>`
                strTBody = strTBody + `        </div>`
                strTBody = strTBody + `    </div>`
                strTBody = strTBody + `</div>`
            });
            $("#publicationDiv").html(strTBody);

            // pagination
            var lastCount = currentpage + 2;
            var frstCount = currentpage - 2;

            if (currentpage == res.pageCount || res.pageCount == (currentpage + 1)) {
                frstCount = currentpage - 3;
            }

            if ((currentpage == 1) || (currentpage == 2)) {
                lastCount = currentpage + 3;
            }

            var pagstrSubStr = "";
            for (var i = 1; i <= res.pageCount; i++) {
                if ((i == 1) && res.pageCount != 1) {
                    if (i != res.currentpage) {
                        pagstrSubStr += '<li class="active text-white"><a href=\"javascript:BindPublication(' + i + ');\"><i class="fa fa-angle-left"></i></a></li>';
                    }
                    else {
                        pagstrSubStr += '<li class="text-white"><a>' + i + '</a></li>';
                    }
                    if (i != currentpage) {
                        pagstrSubStr += '<li>..</li>';
                    }
                }
                if (lastCount > i && frstCount < i && i != 1 && i != res.pageCount) {

                    if (i != res.currentpage) {
                        pagstrSubStr += '<li class="active text-white"><a href=\"javascript:BindPublication(' + i + ');\">' + i + '</a></li>';
                    }
                    else {
                        pagstrSubStr += '<li class="text-white"><a>' + i + '</a></li>';
                    }
                }
                if (i == res.pageCount) {

                    if (i != currentpage) {
                        pagstrSubStr += '<li>..</li>';
                    }
                    if (i != res.currentPage) {
                        pagstrSubStr += '<li class="active text-white"><a href=\"javascript:BindPublication(' + i + ');\"><i class="fa fa-angle-right"></i></a></li>';
                    }
                    else {
                        pagstrSubStr += '<li class="text-white"><a>' + i + '</a></li>';
                    }
                }
            }
            document.getElementById("PaginationPublication").innerHTML = pagstrSubStr;
        }
    });
}