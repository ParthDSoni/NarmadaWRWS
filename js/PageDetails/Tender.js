$(document).ready(function () {
    BindTenderGrid();
    //BindDistrictList();
    //BindDepartmnetList();
    //LatestTender();
});

$("#SearchTR").click(function (e) {
    var TenderData = $('#TenderSearchParam').val();
    BindTenderGrid(TenderData);
});

$("#SearchClear").click(function (e) {
    $('#TenderSearchParam').val("");
    BindTenderGrid();
});


function BindTenderGrid(TenderData = '') {
    var token = $('input[name="AntiforgeryFieldname"]').val();
    if ($.fn.DataTable.isDataTable('#tableDV')) {
        $('#tableDV').DataTable().clear().destroy();
    }
    $("#tableDV").DataTable({
        "serverSide": true,
        "processing": true,
        "filter": false,
        //"searching": true,
        "pageLength": 10,
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
        "ajax": {
            "type": "POST",
            "url": ResolveUrl("/BindTenderGrid"),
            "contentType": "application/x-www-form-urlencoded",
            "data": { "TenderSearchParam": TenderData, "AntiforgeryFieldname": token },
            "dataType": "json",
            "dataSrc": function (json) {
                //var uploadCount = json.upload[0].todayUploadTenders;
                //var totalcount = json.recordsTotal;
                //$("#uploadCount").text(uploadCount);
                //$("#totalrecords").text(totalcount);
                var jsonObj = json.data;
                HideLoader();
                return jsonObj;
            }
        },
        "columns": [
            {
                "data": null,
                "render": function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                },
                "autoWidth": true,
                "className": "text-center",
                "title": "Sr No"
            },
            {
                "data": "tenderTitle", "name": "Tender Title", "width": "45%", "createdCell": function (td, cellData, rowData, row, col) {
                    $(td).css({
                        "white-space": "normal",
                        "word-wrap": "break-word",
                        "overflow-wrap": "break-word"
                    });
                }
            },
            { "data": "preBidMeetingStartDate", "name": "Pre-Bid Meeting Start Date", "autoWidth": true },
            { "data": "preBidMeetingEndDate", "name": "Pre-Bid Meeting End Date", "autoWidth": true },
            { "data": "lastDateOfSubmition", "name": "Last Date of Submission", "autoWidth": true },
            {
                "data": null,
                "render": function (data, type, row) {
                    //console.log(row.id);
                    if (data.tenderId) {
                        var strpath = ResolveUrl("/Home/TenderDetails/" + FrontValue(data.tenderId));
                        return '<a target="_blank" href="' + strpath + '" style="color: rgb(14, 32, 77);"><b>Read More</b></a>';

                    } else {
                        return '';
                    }
                },
                "className": "text-center",
                "autoWidth": true
            }
        ]
    });
}


function LatestTender() {
    $.ajax({
        type: "get", url: ResolveUrl("/GetTender"),
        "contentType": "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (res) {
            var dataList = res.data;
            if (res.isError == true) {
                ShowMessage(res.strMessage, "", "error");
            }
            else {
                var strInnerHtml = "";
                $.each(dataList, function (data, value) {
                    var strSubStr = "";
                    strSubStr = strSubStr + '<li><a href="#">' + value.tenderTitle + '</a></li>';
                    strInnerHtml = strInnerHtml + strSubStr;
                });
                document.getElementById("TenderListData").innerHTML = strInnerHtml;
            }
        }
    });
}


function BindDistrictList() {
    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({
        type: "GET", url: ResolveUrl("/GetDistrictData"),
        contentType: "application/x-www-form-urlencoded",
        data: { "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {
            $("#billing_state").empty();
            $("#billing_state").append($("<option></option>").val(null).text("Select District"));
            $.each(res.data, function (data, value) {
                $("#billing_state").append($("<option></option>").val(value.distId).html(value.distName));
            });
            HideLoader();
        }
    });
    HideLoader();
}

function BindDepartmnetList() {
    var token = $('input[name="AntiforgeryFieldname"]').val();
    $.ajax({
        type: "GET", url: ResolveUrl("/GetDepartmentData"),
        contentType: "application/x-www-form-urlencoded",
        data: { "AntiforgeryFieldname": token },
        dataType: "json",
        success: function (res) {
            $("#DepartmentId").empty();
            $("#DepartmentId").append($("<option></option>").val(null).text("Select Department"));
            $.each(res.data, function (data, value) {
                $("#DepartmentId").append($("<option></option>").val(value.depId).html(value.depName));
            });
            HideLoader();
        }
    });
    HideLoader();
}
