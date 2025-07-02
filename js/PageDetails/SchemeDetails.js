$(document).ready(function () {
    BindSchmes();
    BindScheme();
});

function BindSchmes() {
    
    $.ajax({
        type: "post",
        url: ResolveUrl("/BindSchemeList"),
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (res) {
           // var dataList = res.result;
            debugger
            if (res.isError == true) {
                /*ShowMessage(res.strMessage, "", "error");*/
            }
            else {
                var strInnerHtml = "";
                var i = "0";
          
                $.each(res, function (data, value) {
                    var strSubStr = "";
                    var strpath = "";

                    if (value.imagePath != null && value.imagePath != undefined && value.imagePath != '') {
                        strpath = ResolveUrl("/ViewFile?fileName=" + GreateHashString(value.imagePath));
                    }

                    strSubStr += '<li class="recent-post-list-li">';
                    strSubStr += '    <a class="recent-post-thum" href="' + ResolveUrl("/SchemeDetails/" + FrontValue(value.researchId)) + '" style="margin-right: 10px;">';
                    if (strpath != "") {
                        strSubStr += '        <img src="' + strpath + '" alt="post-img">';
                    }
                    strSubStr += '    </a>';
                    strSubStr += '    <div class="media-body">';
                    strSubStr += '        <a href="' + ResolveUrl("/SchemeDetails/" + FrontValue(value.researchId)) + '">' + value.researchTitle + '</a>';
                    strSubStr += '    </div>';
                    strSubStr += '</li>';
                    strInnerHtml += strSubStr;
                });
                document.getElementById("LatestSchemeArea").innerHTML = strInnerHtml;
                $('.ltn__blog-slider-one-active').slick({
                    arrows: true,
                    dots: false,
                    infinite: true,
                    speed: 300,
                    slidesToShow: 3,
                    slidesToScroll: 1,
                    prevArrow: '<a class="slick-prev"><i class="fas fa-arrow-left" alt="Arrow Icon"></i></a>',
                    nextArrow: '<a class="slick-next"><i class="fas fa-arrow-right" alt="Arrow Icon"></i></a>',
                    responsive: [
                        {
                            breakpoint: 1200,
                            settings: {
                                slidesToShow: 2,
                                slidesToScroll: 1,
                                arrows: false,
                                dots: true
                            }
                        },
                        {
                            breakpoint: 992,
                            settings: {
                                slidesToShow: 2,
                                slidesToScroll: 1,
                                arrows: false,
                                dots: true
                            }
                        },
                        {
                            breakpoint: 768,
                            settings: {
                                slidesToShow: 2,
                                slidesToScroll: 1,
                                arrows: false,
                                dots: true
                            }
                        },
                        {
                            breakpoint: 575,
                            settings: {
                                arrows: false,
                                dots: true,
                                slidesToShow: 1,
                                slidesToScroll: 1
                            }
                        }
                    ]
                });

            }
        
            
        }
    });
}
function StartAutoScroll() {
    const container = document.getElementById("LatestSchemeArea");
    let scrollInterval = setInterval(function () {
        container.scrollTop += 1;
        if (container.scrollTop + container.clientHeight >= container.scrollHeight) {
            container.scrollTop = 0; // loop to top
        }
    }, 50); // speed of scroll
}
function BindScheme() {

    var schemeId = $('#HiddenSchemeId').val();

    $.ajax({
        type: "post", url: ResolveUrl("/BindScheme/?schemeId=" + schemeId),
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
                                   <h3>${value.researchTitle}</h3>
                                <img src="${strpath}" class="img-fluid w-100" alt="">
                                </div>
                                </div>
                                </div>
                                <div class="post-content">
                                <div class="pbmit-blog-meta pbmit-blog-meta-top">
                             
                                </div>${value.researchDesc}</div>
                                </article>`;

            strInnerHtml = strInnerHtml + strSubStr;
            document.getElementById("image").innerHTML = strInnerHtml;
        }

    });
}