function BindDepartmnetList() {
    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({
        type: "GET", url: ResolveUrl("/GetDepartmentData"),
        contentType: "application/x-www-form-urlencoded",
        data: { "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {
            $("#Department").empty();
            $("#Department").append($("<option></option>").val(null).text("Select Department"));

            $.each(res.data, function (data, value) {
                $("#Department").append($("<option></option>").val(value.id).html(value.depName));
            });
            HideLoader();
        }
    });
    HideLoader();
}
$(document).ready(function () {
    BindCMVideoList(0,null,null,null);
    BindDepartmnetList();

});
$("#SearchTR").click(function (e) {

    var department = $('#Department').val();
    var title = $('#Title').val();
    var date = $('#date').val();
    var date1 = $('#date1').val();
    BindCMVideoList(department, title, date, date1)
});
$("#SearchClear").click(function (e) {
    $('#Department').val();
    $('#Title').val('');
    $('#date').val('');
    $('#date1').val('');
    BindCMVideoList();
});

function BindCMVideoList(department, title, date, date1, index) {
    var currentpage = 1;
    if (index)
        currentpage = index;

    var token = $('input[name="AntiforgeryFieldname"]',).val();

    $.ajax({
        type: "POST", url: ResolveUrl("/GetCMVideo"),
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        data: { "department": department, "title": title, "date": date, "date1": date1,"currentPage": currentpage, "AntiforgeryFieldname": token },
        success: function (res) {
            var dataList = res.data;
            if (res.isError == true) {
                ShowMessage(res.strMessage, "", "error");
            }
            else {
                var strInnerHtml = "";

                $.each(dataList, function (data, value) {
                    var strSubStr = "";
                    //var strpath = ResolveUrl("/ViewFile?fileName=" + GreateHashString(value.thumbImage));

                    strSubStr = strSubStr + '<div class="col-lg-4">';
                    strSubStr = strSubStr + '<div class="ltn__gallery-item">';
                    strSubStr = strSubStr + '<div class="ltn__gallery-item-inner">';
                    strSubStr = strSubStr + '<div class="ltn__gallery-item-img">';
                    strSubStr = strSubStr + '<a href="' + value.videoUrl + '" data-fancybox="gallery"class="lightbox-image">';
                    strSubStr = strSubStr + '<img src="' + value.thumbImage + '" alt="Image" height="260px" width="360">';
                    strSubStr = strSubStr + '<span class="ltn__gallery-action-icon">';
                    strSubStr = strSubStr + '<i class="fab fa-youtube"></i>';
                    strSubStr = strSubStr + '</span>';
                    strSubStr = strSubStr + '</a>';
                    strSubStr = strSubStr + ' </div>';
                    strSubStr = strSubStr + '<div class="ltn__gallery-item-info">';
                    strSubStr = strSubStr + '<h4>' + value.videoTitle + ' </h4>';
                    strSubStr = strSubStr + ' </div>';
                    strSubStr = strSubStr + '</div>';
                    strSubStr = strSubStr + '</div>';
                    strSubStr = strSubStr + ' </div>';
                    strInnerHtml = strInnerHtml + strSubStr;
                });
                document.getElementById("CMVideoGalleryArea").innerHTML = strInnerHtml;

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
                            pagstrSubStr += `<li class="active text-white"><a href=\"javascript:BindCMVideoList('${department}',${title},${date},${date1},${i});\"><i class="fa fa-angle-left"></i></a></li>`;
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
                            pagstrSubStr += `<li class="active text-white"><a href=\"javascript:BindCMVideoList('${department}',${title},${date},${date1},${i});\">${i}</a></li>`;
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
                            pagstrSubStr += `<li class="active text-white"><a href=\"javascript:BindCMVideoList('${department}',${title},${date},${date1},${i});\"><i class="fa fa-angle-right"></i></a></li>`;
                        }
                        else {
                            pagstrSubStr += '<li class="text-white"><a>' + i + '</a></li>';
                        }
                    }
                }
                document.getElementById("PaginationVideo").innerHTML = pagstrSubStr;
            }
        }
    });
}

