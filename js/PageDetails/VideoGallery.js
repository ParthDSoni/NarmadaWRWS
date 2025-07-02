
$(document).ready(function () {
    BindVideoList(0, null, null, null);


});
$("#SearchTR").click(function (e) {

 
    var title = $('#Title').val();
    var date = $('#date').val();
    var date1 = $('#date1').val();
    BindVideoList(department, title, date, date1)
});
$("#SearchClear").click(function (e) {
   
    $('#Title').val('');
    $('#date').val('');
    $('#date1').val('');
    BindVideoList();
});


function BindVideoList( title, date, date1, index) {
    var currentpage = 1;
    if (index)
        currentpage = index;

    var token = $('input[name="AntiforgeryFieldname"]',).val();

  
    if (date == undefined || date == '') {
        date = null;
    }
    if (date1 == undefined || date1 == '') {
        date1 = null;
    }

    $.ajax({
        type: "POST", url: ResolveUrl("/GetVideo"),
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        data: { "department": department, "title": title, "date": date, "date1": date1, "currentPage": currentpage, "AntiforgeryFieldname": token },
        success: function (res) {
            var dataList = res.data;
            var res = res;
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
                document.getElementById("VideoGalleryArea").innerHTML = strInnerHtml;

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
                    if (i == 1 && res.pageCount != 1) {
                        if (i != res.currentpage) {
                            pagstrSubStr += `<li class="active text-white"><a href=\"javascript:BindVideoList('${department}',${title},${date},${date1},${i});\">${i}</a></li>`;
                        } else {
                            pagstrSubStr += '<li class="text-white"><a>' + i + '</a></li>';
                        }
                        if (i != currentpage) {
                            pagstrSubStr += '<li>..</li>';
                        }
                    } else if (lastCount > i && frstCount < i && i != 1 && i != res.pageCount) {
                        if (i != res.currentpage) {
                            pagstrSubStr += `<li class="active text-white"><a href=\"javascript:BindVideoList('${department}',${title},${date},${date1},${i});\">${i}</a></li>`;
                        } else {
                            pagstrSubStr += '<li class="text-white"><a>' + i + '</a></li>';
                        }
                    } else if (i == res.pageCount) {
                        if (i != currentpage) {
                            pagstrSubStr += '<li>..</li>';
                        }
                        if (i != res.currentPage) {
                            pagstrSubStr += `<li class="active text-white"><a href=\"javascript:BindVideoList('${department}',${title},${date},${date1},${i});\"><i class="fa fa-angle-right"></i></a></li>`;
                        } else {
                            pagstrSubStr += '<li class="text-white"><a>' + i + '</a></li>';
                        }
                    }
                }

                document.getElementById("PaginationVideo").innerHTML = pagstrSubStr;
            }
        }
    });
}

