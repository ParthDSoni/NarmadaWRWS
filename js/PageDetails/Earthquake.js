
$(document).ready(function () {
    BindEarthquakeGrid();
});
$("#SearchTR").click(function (e) {
    var NewsData = $('#NewsSearchParam').val();
    var FromMagnitude = $('#FromMagnitude').val();
    var ToMagnitude = $('#ToMagnitude').val();
    var FromDate = $('#FromDate').val();
    var ToDate = $('#ToDate').val();
    BindEarthquakeGrid(FromMagnitude, ToMagnitude, FromDate, ToDate);
});

$("#SearchClear").click(function (e) {
    $('#FromMagnitude').val("");
    $('#ToMagnitude').val("");
    $('#ToDate').val("");
    $('#FromDate').val("");
    BindEarthquakeGrid();
});

function BindEarthquakeGrid(FromMagnitude = '', ToMagnitude = '', FromDate = '', ToDate = '' ) {
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
            "url": ResolveUrl("/BindEarthqaukeGridFront"),
            "contentType": "application/x-www-form-urlencoded",
            "data": { "FromMagnitude": FromMagnitude, "ToMagnitude": ToMagnitude, "FromDate": FromDate, "ToDate": ToDate, "AntiforgeryFieldname": token },
            "dataType": "json",
            "dataSrc": function (json) {
                var jsonObj = json.data;
                console.log(jsonObj);
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
            { data: "magnitude", name: "Magnitude", autoWidth: true },
            {
                data: "date",
                name: "date",
                autoWidth: true,
                render: function (data, type, row) {
                    if (data) {
                        var date = new Date(data);
                        return date.toLocaleDateString('en-GB'); // Formats to "dd-mm-yyyy"
                    }
                    return "";
                }
            },
            {
                data: "date",
                name: "time",
                autoWidth: true,
                render: function (data, type, row) {
                    if (data) {
                        var date = new Date(data);
                        return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }); // Formats to "HH:mm"
                    }
                    return "";
                }
            },
            { data: "latitude", name: "Latitude", autoWidth: true },
            { data: "longitude", name: "Longitude", autoWidth: true },
            { data: "depth", name: "Depth", autoWidth: true },
            { data: "location", name: "Location", autoWidth: true },
            {
                "data": null,
                "render": function (data, type, row) {
                    //console.log(row.id);
                    if (data.link) {
                        return '<a target="_blank" href="' + ResolveUrl(data.link) + '" class="theme-btn style-four ">ReadMore</a>';

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
