$(document).ready(function () {
    var lgLangId = $("#lgLanguageId").val();

    //if (lgLangId == "1") {
    //    // document.getElementById("Announcement").innerHTML = 'Announcement';
    //    document.getElementById("lblPhotoGallery").innerHTML = 'Photo Gallery';
    //    document.getElementById("lblVideoGallery").innerHTML = 'Video Gallery';
    //    document.getElementById("lblNews").innerHTML = 'News';
    //    document.getElementById("lblTender").innerHTML = 'Tenders';
    //    document.getElementById("lblViewMore").innerHTML = ' view more';
    //    document.getElementById("lblSchemes").innerHTML = 'Schemes';
    //    document.getElementById("lblAnnouncement").innerHTML = 'Announcement';
    //    document.getElementById("lblknowledgecorner").innerHTML = 'Knowledge Corner';
    //}
    //else if (lgLangId == "2") {
    //    //document.getElementById("Announcement").innerHTML = 'જાહેરાત';
    //    document.getElementById("lblPhotoGallery").innerHTML = 'ફોટો ગેલેરી   ';
    //    document.getElementById("lblVideoGallery").innerHTML = 'વિડિઓ ગેલેરી';
    //    document.getElementById("lblTender").innerHTML = 'ટેન્ડરો';
    //    document.getElementById("lblNews").innerHTML = 'સમાચાર';
    //    document.getElementById("lblViewMore").innerHTML = ' વધુ જોવો';
    //    document.getElementById("lblSchemes").innerHTML = 'યોજનાઓ';
    //    document.getElementById("lblAnnouncement").innerHTML = 'જાહેરાત';
    //    document.getElementById("lblknowledgecorner").innerHTML = 'જ્ઞાન ખૂણો';
    //}
    BindGRGrid(null, null, null, null);
    BindNotificationGrid(null, null, null, null)
    BindBanner();
    BindMinister();
    BindWelComeNote();
    BindSchemes();
    BindTheRegion();
    ImportantNewsAndAll();
    BindTenders();
    BindGalleryTemplate();
    //BindAnnouncements();
    BindPopup();
    ////BindProjects();
    BindBrandLogo();
    //BindResearch();
    //BindMedia();
    //BindQuickLinks();
});


function BindProjects() {
    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({
        type: "POST", url: ResolveUrl("/BindProjectTemplate"),
        contentType: "application/x-www-form-urlencoded",
        data: { "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {
            $("#divProjects").empty();
            $("#divProjects").html(res);
            ResolveUrlHTML();
            /* ==========================================================================
            //Bind project Carousel
            ========================================================================== */

            if ($('.three-item-carousel').length) {
                $('.three-item-carousel').owlCarousel({
                    loop: true,
                    margin: 30,
                    nav: true,
                    smartSpeed: 3000,
                    dots: false,
                    mouseDrag: true,
                    touchDrag: true,
                    autoplay: 4000,
                    navText: ['<span class="flaticon-left"></span>', '<span class="fas fa-chevron-right"></span>'],
                    responsive: {
                        0: {
                            items: 1
                        },
                        480: {
                            items: 1
                        },
                        600: {
                            items: 2
                        },
                        800: {
                            items: 2
                        },
                        1024: {
                            items: 4
                        }
                    }
                });
            }
        }
    });
}

$("#publicationTabs").on('click', 'a[data-toggle="tab"]', function (e) {
    const publicationTypeId = $(this).attr('data-target');
    fetchDataBasedOnTab(publicationTypeId);
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
            fetchDataBasedOnTab($('#publicationTabs .nav-link.active').attr('data-target'));
            $('#tabs_1').owlCarousel('destroy');
            $('#tabs_1').owlCarousel({
                items: 1,
                items: 2,
                loop: true,
                dots: false,
                autoplay: false,
                autoplayTimeout: 2000,
            });
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
            fetchDataBasedOnTab($('#publicationTabs .nav-link.active').attr('data-target'));
            $('#tabs_1').owlCarousel('destroy');
            $('#tabs_1').owlCarousel({
                items: 1,
                items: 2,
                loop: true,
                dots: false,
                autoplay: false,
                autoplayTimeout: 2000,
            });
        }
    });
});

function BindDepartmentPressRealese() {
    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({
        type: "POST", url: ResolveUrl("/BindDepartmentPressRealese"),
        contentType: "application/x-www-form-urlencoded",
        "data": { "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {
            globRes = res;
            $("#PressDepartmentSldier").empty();

            var strTBody = "";
            strTBody = strTBody + "";
            $.each(res, function (data, value) {
                if (value.id != null) {
                    strLinkQuery = value.id;
                    var idValue = strLinkQuery;
                }
                var strLink = ResolveUrl(`/PressReleaseDetails/?id=${idValue}&DepId=${value.departmentId}`);
                mainData = value;

                strTBody = strTBody + `<li class="popular-post-widget-item clearfix" width="200px">`
                strTBody = strTBody + `<div class="popular-post-widget-brief">`
                strTBody = strTBody + `<a href="${strLink}" class="ltn__blog-meta">`
                strTBody = strTBody + `<div class="ltn__blog-date"><i class="fa fa-university mr-10"></i>${mainData.depName}</div>`
                strTBody = strTBody + `<div>${mainData.pressTitle}</div>`
            });
            $("#PressDepartmentSldier").html(strTBody);
        }
    });
}

function BindDistrictPressRealese() {
    var token = $('input[name="AntiforgeryFieldname"]').val();

    $.ajax({
        type: "POST", url: ResolveUrl("/BindDistrictPressRealese"),
        contentType: "application/x-www-form-urlencoded",
        "data": { "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {
            globRes = res;
            $("#PressDistrictSldier").empty();

            var strTBody = "";
            strTBody = strTBody + "";
            $.each(res, function (data, value) {
                if (value.id != null) {
                    strLinkQuery = value.id;
                    var idValue = strLinkQuery;
                }
                var strLink = ResolveUrl(`/PressReleaseDetails/?id=${idValue}&DistrictId=${value.districtId}`);
                mainData = value;

                strTBody = strTBody + `<li class="popular-post-widget-item clearfix">`
                strTBody = strTBody + `<div class="popular-post-widget-brief">`
                strTBody = strTBody + `<a href="${strLink}" class="ltn__blog-meta">`
                strTBody = strTBody + `<div class="ltn__blog-date"><i class="fa fa-university mr-10"></i>${mainData.distName}</div>`
                strTBody = strTBody + `<div>${mainData.pressTitle}</div>`
                strTBody = strTBody + ' < button type = "button" class="close" data - dismiss="modal" aria - label="Close" >';
                strTBody = strTBody + '<span aria-hidden="true">&times;</span>';
                strTBody = strTBody + '</button >';
            });
            $("#PressDistrictSldier").html(strTBody);
        }
    });
}

function fetchDataBasedOnTab(publicationTypeId) {
    $.ajax({
        type: "POST", url: ResolveUrl("/BindPublicationGrid"),
        contentType: "application/x-www-form-urlencoded",
        data: { "publicationTypeId": publicationTypeId, "currentPage": 1 },
        dataType: "json",
        success: function (res) {
            var dataList = res.data;
            if (res.isError == true) {
                /*ShowMessage(res.strMessage, "", "error");*/
            }
            else {
                var strInnerHtml = "";
                var i = "0";
                $.each(dataList, function (data, value) {

                    i++;
                    var strSubStr = "";
                    var strpath = ResolveUrl("/ViewFile?fileName=" + GreateHashString(value.coverPhotoPath));
                    var docpath = ResolveUrl("/DownloadFile?fileName=" + GreateHashString(value.imagePath));
                    var viewdoc = ResolveUrl("/ViewFile?fileName=" + GreateHashString(value.imagePath));

                    strSubStr = strSubStr + `<div id="MYDIV-${value.publicationId}" class="row no-gutters featured-imagebox featured-imagebox-blog ttm-box-view-left-image box-shadow">`
                    strSubStr = strSubStr + `<div class="col-lg-4 col-md-12 col-sm-6 ttm-featured-img-left">`
                    strSubStr = strSubStr + `<div class="featured-thumbnail">`
                    strSubStr = strSubStr + `<img class="img-fluid" src="${strpath}" alt="image">`
                    strSubStr = strSubStr + `</div>`
                    strSubStr = strSubStr + `</div>`
                    strSubStr = strSubStr + `<div class="col-lg-8 col-md-12 col-sm-6 featured-content">`
                    strSubStr = strSubStr + `<div class="post-meta">`
                    strSubStr = strSubStr + `<span class="ttm-meta-line">Published On : ${value.blogdateconvert}</span>`
                    strSubStr = strSubStr + `<span class="ttm-meta-line" id="PublicationViewCount-${value.publicationId}">View Count: ${value.viewCount}</span >`
                    strSubStr = strSubStr + `<span class="ttm-meta-line" id="PublicationDownloadCount-${value.publicationId}">Download Count: ${value.downloadCount}</span>`
                    strSubStr = strSubStr + `</div>`
                    strSubStr = strSubStr + `<div class="post-title featured-title">`
                    strSubStr = strSubStr + `<h5><a href="#">${value.publicationTitle}</a></h5>`
                    strSubStr = strSubStr + `</div>`
                    strSubStr = strSubStr + `<span class="category">`
                    strSubStr = strSubStr + `<a href="${docpath}" id="downloadDoc" data-publication-id="${value.publicationId}"> <i class="fa fa-download"></i> Download</a>`
                    strSubStr = strSubStr + `<a href="${viewdoc}" target="_blank" id="viewDoc" data-publication-id="${value.publicationId}"> <i class="fa fa-eye"></i> View</a>`
                    strSubStr = strSubStr + `</span>`
                    strSubStr = strSubStr + `</div>`
                    strSubStr = strSubStr + `</div>`;

                    strInnerHtml = strInnerHtml + strSubStr;

                });

                document.getElementById("tabs_1").innerHTML = strInnerHtml;

                $('#tabs_1').owlCarousel('destroy');
                $('#tabs_1').owlCarousel({
                    items: 1,
                    items: 2,
                    loop: true,
                    dots: false,
                    autoplay: false,
                    autoplayTimeout: 2000,
                });
            }
        }
    });
}

function BindCitizenCornerArea() {

    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({

        type: "POST", url: ResolveUrl("/BindCitizenCorner"),
        contentType: "application/x-www-form-urlencoded",
        data: { "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {
            $("#divCitizenCorner").empty();
            $("#divCitizenCorner").html(res);
            ResolveUrlHTML();
        }

    });
}

function BindHomeSideBarLink() {

    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({

        type: "POST", url: ResolveUrl("/BindHomeSideBarLink"),
        contentType: "application/x-www-form-urlencoded",
        data: { "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {
            $("#divHomeSideBarLink").empty();
            $("#divHomeSideBarLink").html(res);
            ResolveUrlHTML();
        }

    });
}
function BindPublicationTabs() {
    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({
        type: "POST",
        url: ResolveUrl("/BindPublicationTypeData"),
        contentType: "application/x-www-form-urlencoded",
        data: { "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {
            var dataList = res.data;
            var tabHtml = "";
            $("#publicationTabs").empty();

            $.each(dataList, function (index, value) {
                var isActive = index === 0 ? "active" : "";
                tabHtml += '<li class="nav-item">';
                tabHtml += '<a class="nav-link ' + isActive + '" data-toggle="tab" data-target="' + value.publicationTypeId + '" type="button" role="tab">' + value.publicationTypeName + '</a>';
                tabHtml += '</li>';
            });

            $("#publicationTabs").html(tabHtml);

            var firstTabId = $('#publicationTabs li:first-child a').attr('data-target');
            fetchDataBasedOnTab(firstTabId);
        }
    });
}

function BindHomeQuickLinks() {

    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({

        type: "POST", url: ResolveUrl("/BindHomeQuickLinksList"),
        contentType: "application/x-www-form-urlencoded",
        data: { "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {
            $("#divQuickLinks").empty();

            var lgLangId = $("#langId").val();

            var strInnerHtml = "";
            document.getElementById("divQuickLinks").innerHTML = "";


            strInnerHtml = strInnerHtml + "";

            $.each(res, function (data, value) {
                var strSInnerHtml = "";
                var strpath = ResolveUrl((value.menuURL));

                strSInnerHtml = strSInnerHtml + "";
                strSInnerHtml = strSInnerHtml + "<li>";
                strSInnerHtml = strSInnerHtml + "    <a href='" + strpath + "' target='_blank'>";
                strSInnerHtml = strSInnerHtml + "        <div class='media'>";
                strSInnerHtml = strSInnerHtml + "            <div class='media-body space-sm'>";
                strSInnerHtml = strSInnerHtml + "                <div class='title'>";
                strSInnerHtml = strSInnerHtml + "                    " + value.menuName;
                strSInnerHtml = strSInnerHtml + "                </div>";
                strSInnerHtml = strSInnerHtml + "            </div>";
                strSInnerHtml = strSInnerHtml + "        </div>";
                strSInnerHtml = strSInnerHtml + "    </a>";
                strSInnerHtml = strSInnerHtml + "</li>";

                strInnerHtml = strInnerHtml + strSInnerHtml;
            });
            $("#divQuickLinks").html(strInnerHtml);
            ResolveUrlHTML();
        }

    });
}

function BindQuickLinks() {

    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({

        type: "POST", url: ResolveUrl("/BindQuickLinks"),
        contentType: "application/x-www-form-urlencoded",
        data: { "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {
            $("#divQuickLinks").empty();
            $("#divQuickLinks").html(res);
            ResolveUrlHTML();
        }

    });
}

function BindWelComeNote() {

    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({

        type: "POST", url: ResolveUrl("/BindWelComeNote"),
        contentType: "application/x-www-form-urlencoded",
        data: { "templatename": "WelcomeNote", "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {
            $("#divWelComeNote").empty();
            $("#divWelComeNote").html(res);
            ResolveUrlHTML();
        }

    });
}

function BindBrandLogo() {
    $.ajax({

        type: "get", url: ResolveUrl("/GetAllBrandLogo"),
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (res) {
            var dataList = res.result;
            if (res.isError == true) {
                // handle error if needed
            } else {
                var strInnerHtml = '';
                $.each(dataList, function (index, value) {
                    var strpath = "#";
                    if (value.imagePath) {
                        strpath = ResolveUrl("/ViewFile?fileName=" + GreateHashString(value.imagePath));
                    }
                    strInnerHtml +=
                        '<div class="client-box">' +
                        '<div class="ttm-client-logo-tooltip">' +
                        '<div class="ttm-client-logo-tooltip-inner">' +
                        '<div class="client-thumbnail">' +
                        '<a href="' + value.url + '" target="_blank">' +
                        '<img src="' + strpath + '" alt="Logo">' +
                        '</a>' +
                        '</div>' +
                        '<div class="client-thumbnail_hover">' +
                        '<a href="' + value.url + '" target="_blank">' +
                        '<img src="' + strpath + '" alt="Logo">' +
                        '</a>' +
                        '</div>' +
                        '</div>' +
                        '</div>' +
                        '</div>';
                });
                $("#BrandLogos").html(strInnerHtml);

                // Initialize slick AFTER content injected
                $("#BrandLogos").slick({
                    speed: 1000,
                    infinite: true,
                    arrows: true,
                    dots: false,
                    autoplay: true,
                    autoplaySpeed: 2000,
                    slidesToShow: 5,
                    slidesToScroll: 1,
                    responsive: [
                        { breakpoint: 1360, settings: { slidesToShow: 3 } },
                        { breakpoint: 1024, settings: { slidesToShow: 3 } },
                        { breakpoint: 680, settings: { slidesToShow: 2 } },
                        { breakpoint: 575, settings: { slidesToShow: 1 } }
                    ]
                });
            }
        }

    });
}

function BindGalleryTemplate() {

    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({

        type: "POST", url: ResolveUrl("/BindWelComeNote"),
        contentType: "application/x-www-form-urlencoded",
        data: { "templatename": "GalleryTemplate", "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {
            $("#GalleryTemplate").empty();
            $("#GalleryTemplate").html(res);
            ResolveUrlHTML();
        }

    });
}

function BindBanner() {

    $.ajax({
        type: "get", url: ResolveUrl("/GetAllBannerF"),
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (res) {
            var dataList = res.result;
            if (res.isError == true) {
                /*ShowMessage(res.strMessage, "", "error");*/
            }
            else {

                var strInnerHtml = "";
                var i = 0;
                $.each(dataList, function (data, value) {
                    i++

                    var strSubStr = "";
                    if (value.imagePath != null && value.imagePath != undefined && value.imagePath != '') {
                        var strpath = ResolveUrl("/ViewFile?fileName=" + GreateHashString(value.imagePath));
                    }
                    else {
                        var strpath = ("#");
                    }
                    strSubStr = ` <div class="slide">
                                       <div class="slide_img">
                                        <img  src=${strpath}>
                                    </div>
                                </div>`;

                    strInnerHtml = strInnerHtml + strSubStr;

                });
                document.getElementById("Banners").innerHTML = strInnerHtml;
                var $bannerSlider = jQuery('.banner_slider');
                var $bannerFirstSlide = $('div.slide:first-child');

                $bannerSlider.on('init', function (e, slick) {
                    var $firstAnimatingElements = $bannerFirstSlide.find('[data-animation]');
                    slideanimate($firstAnimatingElements);
                });
                $bannerSlider.on('beforeChange', function (e, slick, currentSlide, nextSlide) {
                    var $animatingElements = $('div.slick-slide[data-slick-index="' + nextSlide + '"]').find('[data-animation]');
                    slideanimate($animatingElements);
                });
                $bannerSlider.slick({
                    slidesToShow: 1,
                    slidesToScroll: 1,
                    arrows: true,
                    fade: true,
                    dots: false,
                    swipe: true,
                    responsive: [

                        {
                            breakpoint: 1200,
                            settings: {
                                arrows: false
                            }
                        },
                        {
                            breakpoint: 767,
                            settings: {
                                slidesToShow: 1,
                                slidesToScroll: 1,
                                arrows: false,
                                autoplay: false,
                                autoplaySpeed: 4000,
                                swipe: true
                            }
                        }]
                });
           
             
                ResolveUrlHTML();
            }
        }
    });
}
function slideanimate(elements) {
    var animationEndEvents = 'webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend';
    elements.each(function () {
        var $this = $(this);
        var $animationDelay = $this.data('delay');
        var $animationType = 'animated ' + $this.data('animation');
        $this.css({
            'animation-delay': $animationDelay,
            '-webkit-animation-delay': $animationDelay
        });

        $this.addClass($animationType).one(animationEndEvents, function () {
            $this.removeClass($animationType);
        });
    });
}
function BindMinister() {
    $.ajax({
        type: "get", url: ResolveUrl("/GetAllMinister"),
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (res) {
            var dataList = res.result;
            if (res.isError == true) {
                /*ShowMessage(res.strMessage, "", "error");*/
            }
            else {
                var strInnerHtml = "";
                var i = "0";
                $.each(dataList, function (data, value) {

                    i++;
                    var strSubStr = "";
                    var strpath = ResolveUrl("/ViewFile?fileName=" + GreateHashString(value.imagePath));

                    strSubStr += `<div class="minister_main">
                                    <div class="featured-icon">
                                        <div class="ttm-icon ttm-icon_element-border ttm-icon_element-color-skincolor ttm-icon_element-size-md">
                                            <img src="${strpath}" alt="Image"
                                                 style="width: 100px;">
                                        </div>
                                    </div>
                                    <div class="featured-content ms-3">
                                        <div class="featured-title">
                                            <h5>${value.ministerName}</h5>
                                        </div>
                                        <div class="featured-desc">
                                            <p>${value.ministerDescription.replace('<p>', '').replace('</p>', '')}</p>
                                        </div>
                                    </div>
                                </div>`;
                    strInnerHtml = strInnerHtml + strSubStr;
                });
                document.getElementById("dvminister").innerHTML = strInnerHtml;
            }
        }
    });
}


function BindSchemes() {

    $.ajax({
        type: "get", url: ResolveUrl("/BindResearch"),
        contentType: "application/x-www-form-urlencoded",
        data: { "id": 1 },
        dataType: "json",
        success: function (res) {
            if (res.isError) {
                // Handle error, if any
                return;
            }

            var dataList = res.result;
            var slidesHtml = '';

            $.each(dataList, function (index, value) {
                var strpath = ResolveUrl("/ViewFile?fileName=" + GreateHashString(value.imagePath));
                var link = ResolveUrl("/SchemeDetails/" + FrontValue(value.researchId));
                var title = value.researchTitle;

                slidesHtml += `
            <div class="featured-imagebox featured-imagebox-services style2">
                <div class="featured-thumbnail">
                    <a href="${link}">
                        <img src="${strpath}" alt="image" />
                    </a>
                </div>
                <div class="featured-content">
                 <div class="featured-title">
                    <h3><a href="${link}">${title}</a></h3>
                       </div>
                    <div class="bottom-footer">
                        <a href="${link}" class="ttm-btn ttm-btn-size-sm ttm-icon-btn-right ttm-btn-color-dark btn-inline">
                            view more <i class="fa fa-angle-right"></i>
                        </a>
                    </div>
                </div>
            </div>
        `;
            });

            // Inject slides HTML into the slider container
            $('#Schemes').html(slidesHtml);

            // If slick slider is already initialized, destroy it before reinitializing
            if ($('#Schemes').hasClass('slick-initialized')) {
                $('#Schemes').slick('unslick');
            }

            // Initialize slick slider with desired options
            $('#Schemes').slick({
                slidesToShow: 3,          // Show 3 slides on desktop
                slidesToScroll: 1,
                arrows: true,
                dots: false,  
                autoplay: true,
                autoplaySpeed: 3000,
                infinite: true,
                responsive: [
                    {
                        breakpoint: 1100,
                        settings: {
                            slidesToShow: 3
                        }
                    },
                    {
                        breakpoint: 940,
                        settings: {
                            slidesToShow: 2
                        }
                    },
                    {
                        breakpoint: 575,
                        settings: {
                            slidesToShow: 1
                        }
                    }
                ]
            });
        }

        //success: function (res) {
        //    var dataList = res.result;
        //    if (res.isError == true) {
        //        /*ShowMessage(res.strMessage, "", "error");*/
        //    }
        //    else {
        //        var strInnerHtml = "";
        //        var i = "0";
        //        $.each(dataList, function (data, value) {

        //            i++;
        //            var strSubStr = "";
        //            var strpath = ResolveUrl("/ViewFile?fileName=" + GreateHashString(value.imagePath));

        //            strSubStr += `
        //            <div class="col-md-4 col-sm-6">
        //                    <!--featured-imagebox-->
        //                    <div class="featured-imagebox featured-imagebox-services style2">
        //                        <!-- featured-thumbnail -->
        //                        <div class="featured-thumbnail">
        //                            <a href="${ResolveUrl("/SchemeDetails/" + FrontValue(value.researchId))}"> <img class="img-fluid" src="${strpath}"
        //                                    alt="image"></a>
        //                        </div><!-- featured-thumbnail end-->
        //                        <div class="featured-content">
        //                            <div class="featured-title">
        //                                <h3><a href="${ResolveUrl("/SchemeDetails/" + FrontValue(value.researchId))}"> ${value.researchTitle}</a></h3>
        //                            </div>
        //                            <div class="bottom-footer">
        //                                <a class="ttm-btn ttm-btn-size-sm ttm-icon-btn-right ttm-btn-color-dark btn-inline"
        //                                    href="${ResolveUrl("/SchemeDetails/" + FrontValue(value.researchId))}">view more<i class="fa fa-angle-right"></i></a>
        //                            </div>
        //                        </div>
        //                    </div><!-- featured-imagebox end-->
        //                </div>`;
        //            strInnerHtml = strInnerHtml + strSubStr;
        //        });

        //        document.getElementById("Schemes").innerHTML = strInnerHtml;

        //    }
        //}
    });
}
function BindTheRegion() {

    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({

        type: "POST", url: ResolveUrl("/BindWelComeNote"),
        contentType: "application/x-www-form-urlencoded",
        data: { "templatename": "TheRegion", "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {
            $("#TheRegion").empty();
            $("#TheRegion").html(res);
            ResolveUrlHTML();
        }

    });
}

function BindMedia() {
    $.ajax({
        type: "get", url: ResolveUrl("/BindMedia"),
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (res) {
            var dataList = res.result;
            console.log(res);
            console.log(dataList);
            if (res.isError == true) {
                /*ShowMessage(res.strMessage, "", "error");*/
            }
            else {
                var strInnerHtml = "";
                var itemCount = dataList.length;
                var itemsPerRow = 5;

                for (var start = 0; start < itemCount; start += itemsPerRow) {
                    var remainingItems = itemCount - start;

                    // Open a new row
                    strInnerHtml += `<div class="row clearfix">`;

                    // First column (col-lg-3) with two items
                    strInnerHtml += `<div class="col-lg-3 col-md-12 col-sm-12 project-block">`;
                    for (var i = 0; i < 2 && (start + i) < itemCount; i++) {
                        var height = i == 1 ? "400px" : "270px";
                        var width = "450px";
                        var index = start + i;
                        var strpath = ResolveUrl("/ViewFile?fileName=" + GreateHashString(dataList[index].imagePath));
                        strInnerHtml += `
                <div class="project-block-two">
                    <div class="inner-box">
                        <figure class="image-box"><img src="${strpath}" style="height:${height};width:${width}" alt=""></figure>
                        <div class="content-box">
                            <div class="view-btn">
                                <a href="${strpath}" class="lightbox-image" data-fancybox="gallery">
                                    <i class="flaticon-zoom-in"></i>
                                </a>
                            </div>
                            <div class="text">
                                <h6>Album</h6>
                                <h3><a href="index-2.html">${dataList[index].title}</a></h3>
                            </div>
                        </div>
                    </div>
                </div>`;
                    }
                    strInnerHtml += `</div>`; // Close first col-lg-3

                    // Middle column (col-lg-6) with one item
                    var middleIndex = start + 2;
                    if (middleIndex < itemCount) {
                        var height = "700px";
                        var width = "900px";
                        var strpathMiddle = ResolveUrl("/ViewFile?fileName=" + GreateHashString(dataList[middleIndex].imagePath));
                        strInnerHtml += `
                <div class="col-lg-6 col-md-12 col-sm-12 project-block">
                    <div class="project-block-two">
                        <div class="inner-box">
                            <figure class="image-box"><img src="${strpathMiddle}" style="height:${height};width:${width}" alt=""></figure>
                            <div class="content-box">
                                <div class="view-btn">
                                    <a href="${strpathMiddle}" class="lightbox-image" data-fancybox="gallery">
                                        <i class="flaticon-zoom-in"></i>
                                    </a>
                                </div>
                                <div class="text">
                                    <h6>Album</h6>
                                    <h3><a href="index-2.html">${dataList[middleIndex].title}</a></h3>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>`;
                    }

                    // Last column (col-lg-3) with two items
                    strInnerHtml += `<div class="col-lg-3 col-md-12 col-sm-12 project-block">`;
                    for (var j = 3; j < 5 && (start + j) < itemCount; j++) {
                        var height = j == 3 ? "400px" : "270px";
                        var width = "450px";
                        var index = start + j;
                        var strpathLast = ResolveUrl("/ViewFile?fileName=" + GreateHashString(dataList[index].imagePath));
                        strInnerHtml += `
                <div class="project-block-two">
                    <div class="inner-box">
                        <figure class="image-box"><img src="${strpathLast}" style="height:${height};width:${width}" alt=""></figure>
                        <div class="content-box">
                            <div class="view-btn">
                                <a href="${strpathLast}" class="lightbox-image" data-fancybox="gallery">
                                    <i class="flaticon-zoom-in"></i>
                                </a>
                            </div>
                            <div class="text">
                                <h6>Album</h6>
                                <h3><a href="index-2.html">${dataList[index].title}</a></h3>
                            </div>
                        </div>
                    </div>
                </div>`;
                    }
                    strInnerHtml += `</div>`; // Close last col-lg-3

                    // Close the row
                    strInnerHtml += `</div>`;
                }

                document.getElementById("media").innerHTML = strInnerHtml;

                // Reinitialize carousel if needed
                if ($('.single-item-carousel').length) {
                    $('.single-item-carousel').owlCarousel({
                        loop: true,
                        margin: 30,
                        nav: true,
                        smartSpeed: 500,
                        autoplay: false,
                        navText: ['<span class="flaticon-left-arrow"></span>', '<span class="flaticon-right-arrow"></span>'],
                        responsive: {
                            0: { items: 1 },
                            480: { items: 1 },
                            600: { items: 1 },
                            800: { items: 1 },
                            1200: { items: 1 }
                        }
                    });
                }
            }



        }
    });
}
function BindExamCategory() {

    $.ajax({
        type: "get", url: ResolveUrl("/GetAllExamCategory"),
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (res) {

            var dataList = res.result;
            if (res.isError == true) {
                /*ShowMessage(res.strMessage, "", "error");*/
            }
            else {
                var strInnerHtml = "";
                var i = "0";
                $.each(dataList, function (data, value) {

                    i++;
                    var strSubStr = "";
                    var strpath = ResolveUrl("/ExamCategoryDetails/" + encodeURIComponent(FrontValue(value.id)) + "");
                    strSubStr = strSubStr + '<div class="col-lg-4 col-sm-6">';
                    strSubStr = strSubStr + '<a href="' + strpath + '">';
                    strSubStr = strSubStr + '    <div class="single-services">';
                    strSubStr = strSubStr + '    <span class="' + value.icon + '"></span>';
                    strSubStr = strSubStr + '        <h3>' + value.examtype + '</h3>';
                    strSubStr = strSubStr + '        <div class="services-shape"><img src="' + ResolveUrl("/Users/images/online-exam.png") + '" alt="Image"></div>';
                    strSubStr = strSubStr + '    </div>';
                    strSubStr = strSubStr + '    </a>';
                    strSubStr = strSubStr + '</div>';

                    strInnerHtml = strInnerHtml + strSubStr;

                });

                document.getElementById("dvExamCategory").innerHTML = strInnerHtml;
            }
        }
    });
}

function SuccsessStory() {
    $.ajax({
        type: "get", url: ResolveUrl("/GetSuccsessStory"),
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (res) {

            var dataList = res.result;
            if (res.isError == true) {
                /*ShowMessage(res.strMessage, "", "error");*/
            }
            else {
                var strInnerHtml = "";

                $.each(dataList, function (data, value) {

                    var strSubStr = "";
                    //var strpath = ResolveUrl("/ViewFile?fileName=" + GreateHashString(value.filePath));

                    strSubStr = strSubStr + '<div class="item" >';
                    strSubStr = strSubStr + '   <div class="min-text-us">';
                    strSubStr = strSubStr + '       <div class="con-text">';
                    strSubStr = strSubStr + '         <a href="' + ResolveUrl("/Home/SuccsessStory/") + '"><p>' + value.details + '</p></a>';
                    strSubStr = strSubStr + '       </div>';
                    strSubStr = strSubStr + '            <div class="ri-test">';
                    strSubStr = strSubStr + '            <a href="' + ResolveUrl("/Home/SuccsessStory/") + '"><h4>' + value.title + '</h4></a>';
                    strSubStr = strSubStr + '            <p>' + value.dateconvert + '</p>';
                    strSubStr = strSubStr + '            </div>';
                    strSubStr = strSubStr + '            </div>';
                    strSubStr = strSubStr + '    </div>';
                    strSubStr = strSubStr + '</div>';

                    strInnerHtml = strInnerHtml + strSubStr;
                });
                //strInnerHtml = strInnerHtml + ' <div class="owl-nav"><button type="button" role="presentation" class="owl-prev"><i class="fa-solid fa-angle-left"></i></button><button type="button" role="presentation" class="owl-next"><i class="fa-solid fa-angle-right"></i></button></div>';
                document.getElementById("LatestSuccsessStory").innerHTML = strInnerHtml;

                $('.owl-demo-test-7').owlCarousel('destroy');
                $('.owl-demo-test-7').owlCarousel({
                    loop: true,
                    margin: 50,
                    autoplay: true,
                    responsiveClass: true,
                    navText: ["<i class='fa-solid fa-angle-left'></i>",
                        "<i class='fa-solid fa-angle-right'></i>"],
                    responsive: {
                        0: {
                            items: 1,
                            nav: true
                        },
                        600: {
                            items: 2,
                            nav: false
                        },
                        1000: {
                            items: 2,
                            nav: true,
                            dots: false,
                            loop: true
                        }
                    }
                })
            }
        }
    });
}

function htmlDecode(input) {
    var doc = new DOMParser().parseFromString(input, "text/html");
    return doc.documentElement.textContent;
}

function CurrentTenders() {
    $.ajax({
        type: "get", url: ResolveUrl("/GetTenders"),
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (res) {

            var dataList = res.result;
            console.log(dataList)
            if (res.isError == true) {
                /*ShowMessage(res.strMessage, "", "error");*/
            }
            else {
                var strInnerHtml = "";

                $.each(dataList, function (data, value) {
                    var date = new Date(value.ecitizenStartDate);
                    var day = ('0' + date.getDate()).slice(-2);
                    var monthIndex = date.getMonth();
                    var year = date.getFullYear();

                    var monthNames = [
                        "January", "February", "March", "April", "May", "June",
                        "July", "August", "September", "October", "November", "December"
                    ];

                    var formattedDate = monthNames[monthIndex] + ' ' + day + ', ' + year;
                    var strSubStr = "";

                    if (value.imagePath != null && value.imagePath != undefined && value.imagePath != '') {
                        var strpath = ResolveUrl("/ViewFile?fileName=" + GreateHashString(value.imagePath));
                    }
                    else {
                        var strpath = ("#");
                    }

                    strSubStr = strSubStr + '<li class="popular-post-widget-item clearfix">';
                    strSubStr = strSubStr + '<div class="popular-post-widget-brief">';
                    strSubStr = strSubStr + '<div class="ltn__blog-date"> <i class="far fa-calendar-alt mr-10"></i>' + formattedDate + '</div> <br/>';
                    if (strpath != '#') {
                        strSubStr = strSubStr + '<a href="' + strpath + '" target=\"_blank\" class="ltn__blog-meta"> ' + htmlDecode(value.ecitizenDesc) + '</a>';
                    }
                    else {
                        strSubStr = strSubStr + '<a> ' + htmlDecode(value.ecitizenDesc) + '</a>';
                    }
                    strSubStr = strSubStr + '</div>';
                    strSubStr = strSubStr + '</li>';

                    strInnerHtml = strInnerHtml + strSubStr;

                });
                document.getElementById("LatestCurrentTenders").innerHTML = strInnerHtml;
            }
        }
    });
}

function BindAnnouncements() {
    $.ajax({
        type: "get", url: ResolveUrl("/BindAnnouncements"),
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (res) {
            var dataList = res.result;
            if (res.isError == true) {
                //ShowMessage(res.strMessage, "", "error");
            }
            else {
                var strInnerHtml = "";
                $.each(dataList, function (data, value) {
                    var strSubStr = "";
                    if (value.imagePath != null && value.imagePath != undefined && value.imagePath != '') {
                        var strpath = ResolveUrl("/ViewFile?fileName=" + encodeURIComponent(GreateHashString(value.imagePath)));
                    }
                    else {
                        var strpath = ("#");
                    }
                    strSubStr = strSubStr + '<div class="carousel-item">';
                    strSubStr = strSubStr + '<a href=""></a><span class="position-relative mx-2 badge">' + value.announcementTitle + '</span>';
                    if (value.isLink == false) {
                        if (strpath != "#") {
                            strSubStr = strSubStr + '<a href="' + strpath + '" target=\"_blank\"></a>';
                            strSubStr = strSubStr + '<a class="show-read-more" href="' + strpath + '" target=\"_blank\"> ' + $(value.announcementDesc).text(); + '</a>';
                        }
                        else {
                            strSubStr = strSubStr + '<a class="show-read-more"> ' + $(value.announcementDesc).text(); + '</a>';
                        }
                    }
                    else {
                        strSubStr = strSubStr + '' + value.announcementDesc.replace('<p>', '').replace('</p>', '') + '';
                    }
                    strSubStr = strSubStr + '</div>';
                    strInnerHtml = strInnerHtml + strSubStr;
                });
                document.getElementById("dvAnnouncements").innerHTML = strInnerHtml;
                $("#dvAnnouncements div").first().addClass('active');
            }
        }
    });
}


function ImportantNewsAndAll() {
    $.ajax({
        type: "get",
        url: ResolveUrl("/GetNewsCircularResolution"),
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (res) {
            if (res.isError === true || !res.result || res.result.length === 0) {
                document.getElementById("newsletter").innerHTML =
                    '<div class="no-news">No news at the moment.</div>';
                return;
            }

            const dataList = res.result;

            let strNewsInnerHtml = "";
            let strCircularInnerHtml = "";
            let strResolutionInnerHtml = "";

            dataList.forEach(value => {
                if (!value || !value.newsTypeId) return;

                let month = "", day = "", year = "";
                if (value.blogdateconvert) {
                    const parts = value.blogdateconvert.split(/[\s,]+/);
                    month = parts[0]?.substring(0, 3) || "";
                    day = parts[1] || "";
                    year = parts[2] || "";
                }
                if (value.imagePath != null && value.imagePath != undefined && value.imagePath != '') {
                    var url = ResolveUrl("/ViewFile?fileName=" + encodeURIComponent(GreateHashString(value.imagePath)));
                }
                else {
                    var url = ("#");
                }
                const newsTitle = value.newsTitle || "";
                const strSubStr = `
                    <li class="popular-post-widget-item clearfix">
                        <div class="popular-post-widget-brief">
                            <a href="${url}" target="_blank" class="ltn__blog-meta">
                                <div class="ltn__blog-date">
                                    <i class="fa fa-calendar mr-10"></i>
                                    <span>${month} ${day}, ${year}</span>
                                </div>
                                <div class="ltn_desc">
                                    ${newsTitle}
                                </div>
                            </a>
                        </div>
                    </li>`;

                switch (value.newsTypeId) {
                    case "1":
                        strNewsInnerHtml += strSubStr;
                        break;
                    case "2":
                        strCircularInnerHtml += strSubStr;
                        break;
                    case "3":
                        strResolutionInnerHtml += strSubStr;
                        break;
                }
            });

            // If no items in any section, provide fallback text
            if (!strNewsInnerHtml) {
                strNewsInnerHtml = '<div class="no-news">No News & updates at the moment.</div>';
            }
            if (!strCircularInnerHtml) {
                strCircularInnerHtml = '<div class="no-news">No Circulars at the moment.</div>';
            }
            if (!strResolutionInnerHtml) {
                strResolutionInnerHtml = '<div class="no-news">No Resolutions at the moment.</div>';
            }

            document.getElementById("NewsTabContent").innerHTML = strNewsInnerHtml;
       

            // Append "View All" links
            appendViewAllLink("NewsTabContent", ResolveUrl("/News"));
            appendViewAllLink("CircularTabContent", ResolveUrl("/Notification"));
            appendViewAllLink("ResolutionTabContent", ResolveUrl("/GovernmentResolutions"));
        }
    });

    function appendViewAllLink(containerId, url) {
        const linkDiv = document.createElement("div");
        linkDiv.innerHTML = `<a target="_blank" href="${url}">View All</a>`;
        Object.assign(linkDiv.style, {
            position: 'sticky',
            bottom: '0',
            right: '10px',
            padding: '10px',
            backgroundColor: 'transparent',
            color: 'navy',
            fontSize: '16px',
            fontWeight: 'bold',
            cursor: 'pointer',
            borderRadius: '5px',
            textAlign: 'right'
        });

        const container = document.getElementById(containerId);
        if (container && container.parentNode && container.parentNode.parentNode) {
            container.parentNode.parentNode.appendChild(linkDiv);
        }
    }
}

function BindGRGrid(grno, title, branch, date, date1) {
    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({
        type: "POST", url: ResolveUrl("/BindGRNewGrid"),
        contentType: "application/x-www-form-urlencoded",
        data: { "grno": grno, "title": title, "Branch": branch, "date": date, "date1": date1, "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {
            i = 1;
            var strResolutionInnerHtml = "";
            strResolutionInnerHtml = strResolutionInnerHtml + "";
            $.each(res, function (data, value) {
                let month = "", day = "", year = "";
                if (value.blogdateconvert) {
                    const parts = value.blogdateconvert.split(/[\s,]+/);
                    month = parts[0]?.substring(0, 3) || "";
                    day = parts[1] || "";
                    year = parts[2] || "";
                }

                if (value.imagePath != null && value.imagePath != undefined && value.imagePath != '') {
                    var url = ResolveUrl("/ViewFile?fileName=" + encodeURIComponent(GreateHashString(value.imagePath)));
                }
                else {
                    var url = ("#");
                }
                const ecitizenTitle = value.ecitizenTitle || "";
                const strSubStr = `
                    <li class="popular-post-widget-item clearfix">
                        <div class="popular-post-widget-brief">
                            <a href="${url}" target="_blank" class="ltn__blog-meta">
                                <div class="ltn__blog-date">
                                    <i class="fa fa-calendar mr-10"></i>
                                    <span>${value.blogdateconvert}</span>
                                </div>
                                <div class="ltn_desc">
                                    ${ecitizenTitle}
                                </div>
                            </a>
                        </div>
                    </li>`;

      
                strResolutionInnerHtml += strSubStr;
                document.getElementById("ResolutionTabContent").innerHTML = strResolutionInnerHtml;

            });
        }
    });
}
function BindNotificationGrid(grno, title, date, date1) {
    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({
        type: "POST", url: ResolveUrl("/BindNotificationGrid"),
        contentType: "application/x-www-form-urlencoded",
        data: { "grno": grno, "title": title, "date": date, "date1": date1, "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {
            i = 1;
            var strCircularInnerHtml = "";
            strCircularInnerHtml = strCircularInnerHtml + "";
            $.each(res, function (data, value) {
                let month = "", day = "", year = "";
                if (value.blogdateconvert) {
                    const parts = value.blogdateconvert.split(/[\s,]+/);
                    month = parts[0]?.substring(0, 3) || "";
                    day = parts[1] || "";
                    year = parts[2] || "";
                }

                if (value.imagePath != null && value.imagePath != undefined && value.imagePath != '') {
                    var url = ResolveUrl("/ViewFile?fileName=" + encodeURIComponent(GreateHashString(value.imagePath)));
                }
                else {
                    var url = ("#");
                }
                const ecitizenTitle = value.ecitizenTitle || "";
                const strSubStr = `
                    <li class="popular-post-widget-item clearfix">
                        <div class="popular-post-widget-brief">
                            <a href="${url}" target="_blank" class="ltn__blog-meta">
                                <div class="ltn__blog-date">
                                    <i class="fa fa-calendar mr-10"></i>
                                    <span>${value.blogdateconvert}</span>
                                </div>
                                <div class="ltn_desc">
                                    ${ecitizenTitle}
                                </div>
                            </a>
                        </div>
                    </li>`;


                strCircularInnerHtml += strSubStr;
                document.getElementById("CircularTabContent").innerHTML = strCircularInnerHtml;

            });
        }
    });
}
function BindTenders() {
    $.ajax({
        type: "get",
        url: ResolveUrl("/GetTenderData"),
        contentType: "aplication/x-www-urlencoded",
        dataType: "json",
        success: function (res) {
            var dataList = res.result;
            if (res.isError == true) {
                ShowMessage(res.strMessage, "", "error");
            }
            else {
                let tenders = res.result;
                let strInnerHtml = "";
                if (tenders && tenders.length > 0) {
                    $.each(tenders, function (data, value) {
                        let month = null;
                        let day = null;
                        if (value && value.CreatedDate) {
                            const dateStr = value.CreatedDate;
                            const parts = dateStr.split(/[\s,]+/);
                            month = parts[0].substring(0, 3);
                            day = parts[1];
                            year = parts[2];
                        }
                        var tendertitle_ = value.TenderTitle;
                        //var tendercontent_ = value.TenderDetails.replace("<p>", "").replace("</p>", "");
                        var url = ResolveUrl("/Home/TenderDetails/" + FrontValue(value.TenderId));
                        strSubStr = `<li class="popular-post-widget-item clearfix">
                                        <div class="popular-post-widget-brief">
                                            <a href="${url}" target="_blank" class="ltn__blog-meta">
                                                <div class="ltn__blog-date">
                                                    <i class="fa fa-calendar mr-10"></i>
                                                    <span>${month} ${day}, ${year}</span>
                                                </div>
                                                <div class="ltn_desc">
                                                    ${tendertitle_}
                                                </div>
                                            </a>
                                        </div>
                                    </li>`;
                        strInnerHtml = strInnerHtml + strSubStr;
                    });
                    var tenderUrl = ResolveUrl("/Tenders");
                    var ReadMore = document.createElement('div');
                    ReadMore.innerHTML = `<a target="_blank" href=${tenderUrl}>View All</a>`;
                    ReadMore.style.position = 'sticky';
                    ReadMore.style.bottom = '0';
                    ReadMore.style.right = '10px';
                    ReadMore.style.padding = '10px';
                    ReadMore.style.backgroundColor = 'transparent';
                    ReadMore.style.color = 'navy';
                    ReadMore.style.fontSize = '16px';
                    ReadMore.style.fontWeight = 'bold';
                    ReadMore.style.cursor = 'pointer';
                    ReadMore.style.borderRadius = '5px';
                    ReadMore.style.textAlign = 'right';
                }
                else {
                    strInnerHtml = '<div class="no-news">No Tenders at the moment.</div>';
                }
                document.getElementById("TenderTabContent").innerHTML = strInnerHtml;
                document.getElementById("TenderTabContent").parentNode.parentNode.appendChild(ReadMore);

                ResolveUrlHTML();
            }
        }
    });
}
function BindPopup() {
    $.ajax({
        type: "GET",
        url: ResolveUrl("/GetAllPopupDetails"),
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (res) {
            if (res.isError) return;

            var content = "";
            $.each(res.result, function (i, val) {
                content += '<p>' + val.popupDescription + '</p>';
            });

            $("#DivDescriptionPopup").html(content);

            var myModal = new bootstrap.Modal(document.getElementById('mdlFront'));
            myModal.show();
        },
        error: function (xhr, status, error) {
            console.error("AJAX error:", status, error);
        }
    });
}



function GetVideoGallery() {
    $.ajax({
        type: "get", url: ResolveUrl("/GetVideoGallery"),
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (res) {

            var dataList = res.result;
            if (res.isError == true) {
                /*ShowMessage(res.strMessage, "", "error");*/
            }
            else {
                var strInnerHtml = "";
                var lgLangId = $("#lgLanguageId").val();
                $.each(dataList, function (data, value) {

                    var strSubStr = "";

                    strSubStr = strSubStr + '<div class="ltn__gallery-item">';
                    strSubStr = strSubStr + '<div class="ltn__gallery-item-inner">';
                    strSubStr = strSubStr + '<div class="ltn__gallery-item-img">';
                    strSubStr = strSubStr + '<a href="' + ResolveUrl(value.videoUrl) + '" data-fancybox="gallery" class="lightbox-image">';
                    strSubStr = strSubStr + '<img src="' + value.thumbImage + '" class="img-fluid" alt="Image">';
                    strSubStr = strSubStr + '<span class="ltn__gallery-action-icon">';
                    strSubStr = strSubStr + '<i class="fas fa-video"></i>';
                    strSubStr = strSubStr + '</span>';
                    strSubStr = strSubStr + '</a>';
                    strSubStr = strSubStr + '</div>';
                    strSubStr = strSubStr + '<div class="ltn__gallery-item-info">';
                    strSubStr = strSubStr + '<h5>' + value.videoTitle + '</h5>';
                    strSubStr = strSubStr + '<a href="' + ResolveUrl("/Home/VideoGallery") + '">Read more <i class="fas fa-arrow-right"></i></a>';
                    strSubStr = strSubStr + '</div>';
                    strSubStr = strSubStr + '</div>';
                    strSubStr = strSubStr + '</div>';
                    strInnerHtml = strInnerHtml + strSubStr;
                });

                document.getElementById("LatestAllVideo").innerHTML = strInnerHtml;

                $('.video-gallery').owlCarousel('destroy');
                $(".video-gallery").owlCarousel({
                    items: 2,
                    nav: true,
                    items: 1,
                    loop: true,
                    dots: false,
                    autoplay: true,
                    autoplayTimeout: 8000,
                    navText: ["<i class='fas fa-arrow-left'></i>", "<i class='fas fa-arrow-right'></i>"],
                });

            }
        }
    });
}

function BindMessage() {

    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({

        type: "POST", url: ResolveUrl("/BindMessage"),
        contentType: "application/x-www-form-urlencoded",
        data: { "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {

            $("#Messageid").empty();
            $("#Messageid").html(res);
            ResolveUrlHTML();

        }

    });

}

function BindTwitterFeed() {

    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({

        type: "POST", url: ResolveUrl("/BindTwitterFeed"),
        contentType: "application/x-www-form-urlencoded",
        data: { "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {

            $("#TwitterFeed").empty();
            $("#TwitterFeed").html(res);
            ResolveUrlHTML();

        }

    });

}

function BindInstagramFeed() {

    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({

        type: "POST", url: ResolveUrl("/BindInstagramFeed"),
        contentType: "application/x-www-form-urlencoded",
        data: { "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {

            $("#InstagramFeed").empty();
            $("#InstagramFeed").html(res);
            ResolveUrlHTML();

        }

    });

}
function BindFaceBookFeed() {

    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({

        type: "POST", url: ResolveUrl("/BindFaceBookFeed"),
        contentType: "application/x-www-form-urlencoded",
        data: { "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {

            $("#FaceBookFeed").empty();
            $("#FaceBookFeed").html(res);
            ResolveUrlHTML();

        }

    });

}
function SchemaArea() {
    $.ajax({

        type: "get", url: ResolveUrl("/GetLatestSchemes"),
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (res) {
            var dataList = res.result;
            if (res.isError == true) {
                //ShowMessage(res.strMessage, "", "error");
                HideLoader();
            }
            else {
                var strInnerHtml = "";
                $.each(dataList, function (data, value) {
                    var strSubStr = "";
                    if (value.firstImagePath != null && value.firstImagePath != undefined && value.firstImagePath != '') {
                        var strpath = ResolveUrl("/ViewFile?fileName=" + GreateHashString(value.firstImagePath));
                    }

                    strSubStr = strSubStr + '<div class="features-item text-center mb-30">';
                    strSubStr = strSubStr + '    <div class="features-thumb">';
                    if (strpath != null && strpath != '') {
                        strSubStr = strSubStr + '        <img src="' + strpath + '" />';
                    }
                    strSubStr = strSubStr + '    </div>';
                    strSubStr = strSubStr + '    <div class="features-content">';
                    strSubStr = strSubStr + '        <h4><a href="' + ResolveUrl("/Home/SchemesDetailsPage/" + FrontValue(value.blogMasterId)) + '" class="member-bio"><h4>' + value.blogName + '</h4></a></h4>';
                    strSubStr = strSubStr + '        <p><a href="' + ResolveUrl("/Home/SchemesDetailsPage/" + FrontValue(value.blogMasterId)) + '">Read More</a></p>';
                    strSubStr = strSubStr + '    </div>';
                    strSubStr = strSubStr + '</div>';

                    strInnerHtml = strInnerHtml + strSubStr;

                });
                document.getElementById("Schemeid").innerHTML = strInnerHtml;

                $('.schemes-slider').owlCarousel('destroy');
                $('.schemes-slider').owlCarousel({
                    loop: true,
                    autoplay: true,
                    autoplayTimeout: 5000,
                    autoplayHoverPause: true,
                    margin: 15,
                    responsiveClass: true,
                    navText: ["<i class='fa-solid fa-angle-left'></i>",
                        "<i class='fa-solid fa-angle-right'></i>"],
                    responsive: {
                        0: {
                            items: 1,
                            nav: false
                        },
                        600: {
                            items: 2,
                            nav: false
                        },
                        1000: {
                            items: 2,
                            nav: false,
                            dots: false,
                            loop: true
                        }
                    }
                })
            }
        }
    });
}

function Bindnews() {
    $.ajax({
        type: "get", url: ResolveUrl("/GetNews"),
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (res) {
            var dataList = res.result;
            if (res.isError == true) {
                /*ShowMessage(res.strMessage, "", "error");*/
            }
            else {
                var strInnerHtml = "";

                $.each(dataList, function (data, value) {
                    var strSubStr = "";

                    if (value.imagePath != null && value.imagePath != undefined && value.imagePath != '') {
                        var strpath = ResolveUrl("/ViewFile?fileName=" + GreateHashString(value.imagePath));
                    }
                    else {
                        var strpath = ("#");
                    }

                    strSubStr = strSubStr + '<li class="popular-post-widget-item clearfix">';
                    strSubStr = strSubStr + '<div class="popular-post-widget-brief">';
                    strSubStr = strSubStr + '<div class="ltn__blog-date"> <i class="far fa-calendar-alt mr-10"></i>' + value.blogdateconvert + '</div> <br/>';
                    if (value.isLink == false) {
                        if (strpath != '#') {
                            strSubStr = strSubStr + '<a href="' + strpath + '" target=\"_blank\" class="ltn__blog-meta"> </a>';
                            strSubStr = strSubStr + '<a href="' + strpath + '" target=\"_blank\"> ' + $(value.newsDesc).text(); + '</a>';
                        }
                        else {
                            strSubStr = strSubStr + '<a class="ltn__blog-meta"> ' + $(value.newsDesc).text(); + '</a>';
                        }
                    }
                    else {
                        strSubStr = strSubStr + '' + value.newsDesc.replace('<p>', '').replace('</p>', '') + '';
                    }
                    strSubStr = strSubStr + '</div>';
                    strSubStr = strSubStr + '</li>';

                    strInnerHtml = strInnerHtml + strSubStr;

                });
                document.getElementById("LatestCurrentNews").innerHTML = strInnerHtml;
            }
        }
    });
}
function BindStatesticCount() {
    $.ajax({
        type: "get", url: ResolveUrl("/GetStatasticcount"),
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (res) {

            var dataList = res.result;
            if (res.isError == true) {
                /*ShowMessage(res.strMessage, "", "error");*/
            }
            else {
                var strInnerHtml = "";
                document.getElementById("LatestStatesticCount").innerHTML = "";
                $.each(dataList, function (data, value) {

                    var strSubStr = "";
                    if (value.imagePath != null && value.imagePath != undefined && value.imagePath != '') {
                        var strpath = ResolveUrl("/ViewFile?fileName=" + GreateHashString(value.imagePath));
                    }
                    else {
                        var strpath = ("#");
                    }

                    strSubStr = strSubStr + '<div class="ttm-fid inside ttm-fid-view-lefticon newstyle">';
                    strSubStr = strSubStr + '<div class="ttm-fid-left">';
                    strSubStr = strSubStr + '<div class="ttm-fid-icon-wrapper" style="margin-top: 10px;">';

                    strSubStr = strSubStr + '<img src="' + strpath + '" alt="image" width="55px">';
                    strSubStr = strSubStr + '</div>';
                    strSubStr = strSubStr + '</div>';
                    strSubStr = strSubStr + '<div class="ttm-fid-contents text-left">';
                    strSubStr = strSubStr + '<div class="ttm-fid-inner">';
                    strSubStr = strSubStr + '<div class="timer count-title count-number" data-to="100" data-speed="1500">';
                    strSubStr = strSubStr + '</div>';
                    strSubStr = strSubStr + ' <h4 class="counter">' + value.count + '</h4>';
                    strSubStr = strSubStr + '</div>';
                    strSubStr = strSubStr + ' <h3 class="ttm-fid-title">' + value.title + '</h3>';
                    strSubStr = strSubStr + ' </div>';
                    strSubStr = strSubStr + ' </div>';


                    strInnerHtml = strInnerHtml + strSubStr;


                });
                document.getElementById("LatestStatesticCount").innerHTML = strInnerHtml;

                $('.counter-data').owlCarousel('destroy');
                $('.counter-data').owlCarousel({
                    loop: true,
                    autoplay: true,
                    autoplayTimeout: 5000,
                    autoplayHoverPause: true,
                    margin: 15,
                    nav: true,
                    responsiveClass: true,
                    navText: ["<i class='fas fa-arrow-left'></i>", "<i class='fas fa-arrow-right'></i>"],
                    responsive: {
                        0: {
                            items: 1,
                            nav: false
                        },
                        600: {
                            items: 2,
                            nav: true
                        },
                        1000: {
                            items: 4,
                            nav: true,
                            dots: false,
                            loop: true
                        }
                    }
                })
            }
            jQuery(document).ready(function ($) {
                $('.counter').counterUp({
                    delay: 10,
                    time: 1000
                });
            });
        }
    });
}
function BindActivity() {
    $.ajax({
        type: "get", url: ResolveUrl("/GetActivity"),
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (res) {

            var dataList = res.result;
            if (res.isError == true) {
                /*ShowMessage(res.strMessage, "", "error");*/
            }
            else {
                var strInnerHtml = "";
                document.getElementById("LatestActivity").innerHTML = "";
                $.each(dataList, function (data, value) {
                    var strSubStr = "";
                    var strpath = ResolveUrl("/ViewFile?fileName=" + GreateHashString(value.imagePath));

                    strSubStr = strSubStr + '<div class="featured-icon-box top-icon style2 text-center ttm-bgcolor-darkgrey">';
                    strSubStr = strSubStr + '<div class="featured-icon">';
                    strSubStr = strSubStr + '<div class="ttm-icon ttm-icon_element-color-skincolor ttm-icon_element-size-lg">';
                    strSubStr = strSubStr + '<img src="' + strpath + '" alt="image">';
                    strSubStr = strSubStr + '</div>';
                    strSubStr = strSubStr + '</div>';
                    strSubStr = strSubStr + '<div class="featured-content">';
                    strSubStr = strSubStr + '<div class="featured-title">';
                    if (value.url != null) {
                        strSubStr = strSubStr + '<a href="' + value.url + '"><h5>' + value.title + '</h5></a>';
                    }
                    else {
                        strSubStr = strSubStr + '<a href="#"><h5>' + value.title + '</h5></a>'
                    }
                    strSubStr = strSubStr + '</div>';
                    strSubStr = strSubStr + '</div>';
                    strSubStr = strSubStr + '</div>';
                    strInnerHtml = strInnerHtml + strSubStr;

                });
                document.getElementById("LatestActivity").innerHTML = strInnerHtml;

                $('.schemes-slider-one').owlCarousel('destroy');
                $('.schemes-slider-one').owlCarousel({
                    loop: true,
                    autoplay: true,
                    autoplayTimeout: 5000,
                    autoplayHoverPause: true,
                    margin: 15,
                    nav: true,
                    responsiveClass: true,
                    navText: ["<i class='fas fa-arrow-left'></i>", "<i class='fas fa-arrow-right'></i>"],
                    responsive: {
                        0: {
                            items: 1,
                            nav: false
                        },
                        600: {
                            items: 2,
                            nav: true
                        },
                        1000: {
                            items: 4,
                            nav: true,
                            dots: false,
                            loop: true
                        }
                    }
                })
            }
        }
    });
}
function BindPastEvent() {
    $.ajax({
        type: "get", url: ResolveUrl("/GetPastEvent"),
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (res) {

            var dataList = res.result;
            if (res.isError == true) {
                /*ShowMessage(res.strMessage, "", "error");*/
            }
            else {
                var strInnerHtml = "";

                $.each(dataList, function (data, value) {
                    var strSubStr = "";

                    if (value.imagePath != null && value.imagePath != undefined && value.imagePath != '') {
                        var strpath = ResolveUrl("/ViewFile?fileName=" + GreateHashString(value.imagePath));
                    }
                    else {
                        var strpath = ("#");
                    }
                    /*strSubStr = strSubStr + '     <a href="#"><span class="position-relative mx-2 badge">' + value.newsTypeText +'</span></a>';
                    strSubStr = strSubStr + '     <a href="' + ResolveUrl("/WhatsNew/Index/") + '">' + $(value.newsDesc).text(); + '</a>';*/

                    strSubStr = strSubStr + '<li>';
                    strSubStr = strSubStr + '<div class="about-icon-box">';
                    //strSubStr = strSubStr + '<a class="content">';
                    strSubStr = strSubStr + '<p>' + value.newsTitle + ' </p >';
                    strSubStr = strSubStr + '<p class="download">';
                    strSubStr = strSubStr + ' <i class="fas fa-download"></i><a href="' + strpath + '" target=\"_blank\" class="content"> Download</a>';

                    strSubStr = strSubStr + '</p>';
                    //strSubStr = strSubStr + '</a>';
                    strSubStr = strSubStr + '</div>';
                    strSubStr = strSubStr + '</li>';

                    strInnerHtml = strInnerHtml + strSubStr;

                });
                document.getElementById("LatestPastEvent").innerHTML = strInnerHtml;

            }
        }
    });
}


function BindKnowledgeCorner() {
    $.ajax({
        type: "get", url: ResolveUrl("/GetCitizenCorner"),
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (res) {

            var dataList = res.result;
            if (res.isError == true) {
                /*ShowMessage(res.strMessage, "", "error");*/
            }
            else {
                var strInnerHtml = "";
                document.getElementById("LatestCitizenCorner").innerHTML = "";
                $.each(dataList, function (data, value) {

                    var strSubStr = "";
                    if (value.imagePath != null && value.imagePath != undefined && value.imagePath != '') {
                        var strpath = ResolveUrl("/ViewFile?fileName=" + GreateHashString(value.imagePath));
                    }
                    else {
                        var strpath = ("#");
                    }

                    strSubStr = strSubStr + '<div class="col-lg-6">';
                    strSubStr = strSubStr + '<div class="featured-icon-box left-icon mb-10">';
                    strSubStr = strSubStr + '<div class="featured-icon">';
                    strSubStr = strSubStr + '<div class="ttm-icon ttm-icon_element-bgcolor-skincolor ttm-icon_element-style-rounded ttm-icon_element-size-lg">';
                    strSubStr = strSubStr + '<img src="' + strpath + '" alt="image">';
                    strSubStr = strSubStr + '</div>';
                    strSubStr = strSubStr + '</div>';
                    strSubStr = strSubStr + '<div class=" featured-content">';
                    strSubStr = strSubStr + ' <div class="featured-title">';
                    strSubStr = strSubStr + '<a href="' + value.url + '"><h5>' + value.title + '</h5></a>';
                    strSubStr = strSubStr + ' </div>';
                    strSubStr = strSubStr + ' </div>';
                    strSubStr = strSubStr + '</div>';
                    strSubStr = strSubStr + ' </div>';

                    strInnerHtml = strInnerHtml + strSubStr;

                });
                document.getElementById("LatestCitizenCorner").innerHTML = strInnerHtml;

            }
        }
    });
}