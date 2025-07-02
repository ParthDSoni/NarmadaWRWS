
$(document).ready(function () {
    BindResolutionGrid();
});
$("#SearchTR").click(function (e) {
    var ResolutionData = $('#ResolutionSearchParam').val();
    BindResolutionGrid(ResolutionData);
});

$("#SearchClear").click(function (e) {
    $('#ResolutionSearchParam').val("");
    BindResolutionGrid();
});

function BindResolutionGrid(ResolutionData = '') {
    var token = $('input[name="AntiforgeryFieldname"]').val();
    if ($.fn.DataTable.isDataTable('#tableDV')) {
        $('#tableDV').DataTable().clear().destroy();
    }
    $("#tableDV").DataTable({
        "serverSide": false,
        "processing": true,
        "filter": false,
        //"searching": true,
        "pageLength": 5,
        "lengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]],
        "ajax": {
            "type": "POST",
            "url": ResolveUrl("/BindResolutionGrid"),
            "contentType": "application/x-www-form-urlencoded",
            "data": { "ResolutionSearchParam": ResolutionData, "AntiforgeryFieldname": token },
            "dataType": "json",
            "dataSrc": function (json) {
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
            { "data": "newsTitle", "name": "Recruitment title", "autoWidth": true },
            //{
            //    "data": "newsDesc", "name": "Recruitment Details", "width": "45%", "createdCell": function (td, cellData, rowData, row, col) {
            //        $(td).css({
            //            "white-space": "normal",
            //            "word-wrap": "break-word",
            //            "overflow-wrap": "break-word"
            //        });
            //    }
            //},
            {
                "data": null, "name": "Recruitment Start Date", "autoWidth": true,
                "render": function (data, type, row, meta) {
                    let date = new Date(data.newsStartDate);
                    let day = date.getDate();
                    let month = date.toLocaleString('default', { month: 'short' });
                    let year = date.getFullYear();
                    let formattedDate = `${day} ${month} ${year}`;
                    return formattedDate;
                }
            },
            {
                "data": null,
                "render": function (data, type, row) {
                    //console.log(row.id);
                    if (data.newsId) {
                        return '<a target="_blank" href="' + ResolveUrl("/Home/RecruitmentDetails/" + FrontValue(data.newsId)) + '" class="theme-btn style-four ">Read More</a>';

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
