
$(document).ready(function () {
    BindNewsGrid();
});
$("#SearchTR").click(function (e) {
    var NewsData = $('#NewsSearchParam').val();
    BindNewsGrid(NewsData);
});

$("#SearchClear").click(function (e) {
    $('#NewsSearchParam').val("");
    BindNewsGrid();
});

function BindNewsGrid(NewsData = '') {
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
            "url": ResolveUrl("/BindNewsGridFront"),
            "contentType": "application/x-www-form-urlencoded",
            "data": { "NewsSearchParam": NewsData, "AntiforgeryFieldname": token },
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
            { "data": "newsTitle", "name": "news title", "autoWidth": true },
            //{
            //    "data": "newsDesc", "name": "News Details", "width": "45%", "createdCell": function (td, cellData, rowData, row, col) {
            //        $(td).css({
            //            "white-space": "normal",
            //            "word-wrap": "break-word",
            //            "overflow-wrap": "break-word"
            //        });
            //    }
            //},
            {
                "data": null, "name": "News Published Date", "autoWidth": true,
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
                    debugger
                    if (data.isLink) {
                        if (data.shortDescription) {
                            return '<a target="_blank" href="' + data.shortDescription + ' "style="color: rgb(14, 32, 77);""><b>Read More</b></a>';
                        } else {
                            return '';
                        }
                    }
                    else {
                        if (data.imagePath) {
                            var strpath = ResolveUrl("/ViewFile?fileName=" + GreateHashString(row.imagePath));
                            return '<a target="_blank" href="' + strpath + '" style="color: rgb(14, 32, 77);"><b>Read More</b></a>';
                        }

                        else {
                            return '';
                        }
                    }

                    
                },
                "className": "text-center",
                "autoWidth": true
            }
        ]
    });
}
