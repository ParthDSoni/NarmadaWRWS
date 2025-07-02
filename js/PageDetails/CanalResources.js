document.addEventListener("DOMContentLoaded", () => {
    const form = $('#frmAddData');
    const token = $('input[name="AntiforgeryFieldname"]', form).val();

    BindTable(token, "MainContentId", 0, 0);
});

async function BindTable(token, id, regionId, basinId) {
    ShowLoader();
    try {
        const response = await fetch(ResolveUrl("/GetAllCanalDataTable"), {
            method: "POST",
            body: new URLSearchParams({ RegionId: regionId, BasinId: basinId, languageId: 1, AntiforgeryFieldname: token }),
        });
        if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);
        const result = await response.json();

        const html = `<table class="table table-bordered" id="projectTable">
                            <thead>
                              <tr>
                                <th scope="col">Sr.No</th>
                                <th scope="col">Canal Resources Project</th>
                                <th scope="col">Visit Page</th>
                              </tr>
                            </thead>
                            <tbody>
                            </tbody>
                     </table>`;
        $("#" + id).html(html);

        const tbody = document.querySelector('#projectTable tbody');

        tbody.innerHTML = result.map((value, index) => `
              <tr>
                <th scope="row">${index + 1}</th>
                <td>${value.name}</td>
                <td><a href="${ResolveUrl("/Dam/?canalId=" + FrontValue(value.canalId))}">Visit Page</a></td>
              </tr>`).join('');

    } catch (err) {
        console.error("Error While Getting Canal data:", err);
    } finally {
        HideLoader();
    }
}