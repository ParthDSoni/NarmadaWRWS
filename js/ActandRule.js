$(document).ready(function () {
    BindRTIGrid();
    var lgLangId = $("#lgLanguageId").val();
    if (lgLangId == "2") {
        $("#lblsrno").html('ક્રમ નંબર');
        $("#lbltitle").html('શીર્ષક');
        $("#lbldocument").html('ડાઉનલોડ');
    }
    else {
        $("#lblsrno").html('Sr.No');
        $("#lbltitle").html('Title');
        $("#lbldocument").html('Download');
    }
});
$("#SearchGR").click(function (e) {


    var title = $('#Title').val();

    BindRTIGrid( title)
});
$("#SearchClear").click(function (e) {

    $('#Title').val('');
    BindRTIGrid();
});
function BindRTIGrid(title) {

    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({

        type: "POST", url: ResolveUrl("/BindActandRuleGrid"),
        contentType: "application/x-www-form-urlencoded",
        data: { "title": title,"AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {

            i = 1;
            $("#tbodyDV").empty();

            var strTBody = "";
            strTBody = strTBody + "";
            $.each(res, function (data, value) {
                var mainData = value;

                var strpath = mainData.imagePath;
                if (strpath != null && strpath != "") {
                    strpath = ResolveUrl("/ViewFile?fileName=" + GreateHashString(strpath));
                }

                strTBody = strTBody + "  <tr>";
                strTBody = strTBody + "      <td>" + i + "</td>";
                strTBody = strTBody + "      <td>" + mainData.ecitizenTitle + "</td>";
                /*strTBody = strTBody + "   <td>";
                if (mainData.newsDate != null) {

                    strTBody = strTBody + "       " + mainData.newsDate + "";
                }

                strTBody = strTBody + "   </td>";*/
                if (strpath != null && strpath != "") {
                    strTBody = strTBody + "      <td class=\"file_td\"><a href =\"" + strpath + "\" target=\"_blank\">";
                    strTBody = strTBody + "               <i class=\"fa fa-file-pdf fa-2x\">&nbsp;</i>";
                    strTBody = strTBody + "       </a>";
                    strTBody = strTBody + "      </td>";
                }
                else {
                    strTBody = strTBody + "<td></td>";
                }
                strTBody = strTBody + "  </tr>";
                i++;

            });
            $("#tbodyDV").html(strTBody);
        }
    });
}

