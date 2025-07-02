
$(document).ready(function () {
    BindSchemes();
});
function BindSchemes() {

    $.ajax({
        type: "get", url: ResolveUrl("/BindResearch"),
        contentType: "application/x-www-form-urlencoded",
        data: { "id": 1 },
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

                    strSubStr += `
                    <div class="col-md-4">
                            <!--featured-imagebox-->
                            <div class="featured-imagebox featured-imagebox-services style2">
                                <!-- featured-thumbnail -->
                                <div class="featured-thumbnail">
                                    <a href="${ResolveUrl("/SchemeDetails/" + FrontValue(value.researchId))}"> <img class="img-fluid" src="${strpath}"
                                            alt="image"></a>
                                </div><!-- featured-thumbnail end-->
                                <div class="featured-content">
                                    <div class="featured-title">
                                        <h3><a href="${ResolveUrl("/SchemeDetails/" + FrontValue(value.researchId))}"> ${value.researchTitle}</a></h3>
                                    </div>
                                    <div class="bottom-footer">
                                        <a class="ttm-btn ttm-btn-size-sm ttm-icon-btn-right ttm-btn-color-dark btn-inline"
                                            href="${ResolveUrl("/SchemeDetails/" + FrontValue(value.researchId))}">view more<i class="fa fa-angle-right"></i></a>
                                    </div>
                                </div>
                            </div><!-- featured-imagebox end-->
                        </div>`;
                    strInnerHtml = strInnerHtml + strSubStr;
                });

                document.getElementById("Schemes").innerHTML = strInnerHtml;

            }
        }
    });
}