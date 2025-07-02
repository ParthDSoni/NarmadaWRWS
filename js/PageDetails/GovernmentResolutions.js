$(document).ready(function () {
    BindBranch();
    BindGRGrid(null,null,null,null);
});
function BindBranch() {
    ShowLoader();
    var form = $('#frmAddEdit');
    var token = $('input[name="AntiforgeryFieldname"]', form).val();
    $.ajax({
        type: "POST", url: ResolveUrl("/BindBranch"),
        contentType: "application/x-www-form-urlencoded",
        data: { "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {
            $("#BranchId").empty();
            $.each(res, function (data, value) {

                $("#BranchId").append($("<option></option>").val(value.value).html(value.text));
            });
            HideLoader();
        }
    });
    HideLoader();
}
$("#SearchGR").click(function (e) {

    var grno = $('#grno').val();
    var title = $('#Title').val();
    var date = $('#date').val();
    var date1 = $('#date1').val();
    var branch = $('#BranchId :selected').val();

    BindGRGrid(grno, title,branch, date, date1)
});
$("#SearchClear").click(function (e) {
    $('#grno').val('');
    $('#Title').val('');
    $('#BranchId').val('');
    $('#date').val('');
    $('#date1').val('');
    BindGRGrid();
});

function BindGRGrid(grno, title, branch, date,date1) {
    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({
        type: "POST", url: ResolveUrl("/BindGRNewGrid"),
        contentType: "application/x-www-form-urlencoded",
        data: { "grno": grno, "title": title, "Branch": branch,"date": date, "date1": date1, "AntiforgeryFieldname": token },
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
                strTBody = strTBody + "      <td>"+ i +"</td>";
                strTBody = strTBody + "   <td>";

                if (mainData.blogdateconvert != null) {

                    strTBody = strTBody + "       " + mainData.blogdateconvert + "";
                }

                // var branch = "";
                // if (mainData.branch != null && mainData.branch != '') {

                //     branch = mainData.branch;
                // } else {
                //     branch = "-";
                // }

                strTBody = strTBody + "   </td>";
                strTBody = strTBody + "      <td>" + mainData.grNumber + "</td>";
                strTBody = strTBody + "      <td>" + mainData.ecitizenTitle + "</td>";
                strTBody = strTBody + "      <td>" + (mainData.branch_name ?? '-') + "</td>";
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

