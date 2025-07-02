let globalData = [];

document.addEventListener("DOMContentLoaded", () => {
    const form = $('#frmAddData');
    const token = $('input[name="AntiforgeryFieldname"]', form).val();

    BindRegion(token);
    BindBasin(token, "DBasinId", 1);
    BindBasin(token, "RBasinId", 1);

    const urlParams = new URLSearchParams(window.location.search);
    const riverId = urlParams.get("riverId");
    const canalId = urlParams.get("canalId");
    const damId = urlParams.get("damId");

    if (riverId) {
        RenderRiver(token, riverId);
    } else if (canalId) {
        RenderCanal(token, canalId);
    } else if (damId) {
        RenderDam(token, damId);
    } else {
        console.log("default");
    }

    $("#DRegionId").on("change", () => {
        BindBasin(token, "DBasinId", parseInt($("#DRegionId").val()));
    });

    $("#RRegionId").on("change", () => {
        BindBasin(token, "RBasinId", parseInt($("#RRegionId").val()));
    });

    $("#DBasinId").on("change", () => {
        BindDam(token, "DamId", parseInt($("#DRegionId").val()), parseInt($("#DBasinId").val()));
    });

    $("#RBasinId").on("change", () => {
        BindRiver(token, "RiverId", parseInt($("#RRegionId").val()), parseInt($("#RBasinId").val()));
    });

    $("#CRegionId").on("change", () => {
        BindCanal(token, "CanalId", parseInt($("#CRegionId").val()));
    });

    $("#DamBtn").on("click", function (e) {
        const regionId = parseInt($("#DRegionId").val());
        const basinId = parseInt($("#DBasinId").val());
        const damId = parseInt($("#DamId").val());

        if (regionId && basinId && damId && globalData.length > 0) {
            showdetails("dam", damId);
        } else {
            ShowMessage("Please select all required filters", "", "error");
        }
    });

    $("#RiverBtn").on("click", function (e) {

        const regionId = parseInt($("#RRegionId").val());
        const basinId = parseInt($("#RBasinId").val());
        const riverId = parseInt($("#RiverId").val()); // Add this input if needed

        if (regionId && basinId && riverId && globalData.length > 0) {
            showdetails("river", riverId);
        } else {
            ShowMessage("Please select all required filters", "", "error");
        }
    });

    $("#CanalBtn").on("click", function (e) {

        const regionId = parseInt($("#CRegionId").val());
        const canalId = parseInt($("#CanalId").val()); // Add this input if needed

        if (regionId && canalId && globalData.length > 0) {
            showdetails("canal", canalId);
        } else {
            ShowMessage("Please select all required filters", "", "error");
        }
    });
});

function showdetails(type, id) {
    let item;
    // Use field name based on type
    switch (type) {
        case "dam":
            item = globalData.find(x => parseInt(x.value) === id); // or x.damId === id
            break;
        case "river":
            item = globalData.find(x => parseInt(x.value) === id); // or x.riverId === id
            break;
        case "canal":
            item = globalData.find(x => parseInt(x.value) === id); // or x.canalId === id
            break;
        default:
            item = null;
    }

    if (!item) {
        ShowMessage("No data found for the selected item", "", "error");
        return;
    }

    const imageHtml = item.uploadDocumentPath
        ? `<img class="image-fluid" src="${ResolveUrl("/ViewFile?fileName=") + GreateHashString(item.uploadDocumentPath)}" alt="Document">`
        : "No image found.";

    const detailHtml = `
        <h3>${item.text}</h3>
        <div class="image">${imageHtml}</div>
        <div class="content">${item.description}</div>
    `;

    $('#MainContentId').html(detailHtml);
}

function RenderPage(data) {
    const imageHtml = data.uploadDocumentPath
        ? `<img class="image-fluid" src="${ResolveUrl("/ViewFile?fileName=") + GreateHashString(data.uploadDocumentPath)}" alt="Document">`
        : "No image found.";

    const detailHtml = `
        <h3>${data.name}</h3>
        <div class="image">${imageHtml}</div>
        <div class="content">${data.description}</div>
    `;

    $('#MainContentId').html(detailHtml);
}

async function BindRegion(token) {
    ShowLoader();
    try {
        const response = await fetch(ResolveUrl("/GetAllRegionFront"), {
            method: "POST",
            body: new URLSearchParams({ languageId: 1, AntiforgeryFieldname: token }),
        });
        if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);
        const data = await response.json();

        const regionOptions = `<option value="0">--Select Region--</option>` +
            data.map(item => `<option value="${item.value}">${item.text}</option>`).join("");

        $("#DRegionId, #RRegionId, #CRegionId").html(regionOptions);
    } catch (err) {
        console.error("Error While Getting Region data:", err);
    } finally {
        HideLoader();
    }
}

async function BindBasin(token, id, regionId) {
    ShowLoader();
    try {
        const response = await fetch(ResolveUrl("/GetAllBasinFront"), {
            method: "POST",
            body: new URLSearchParams({ RegionId: regionId, languageId: 1, AntiforgeryFieldname: token }),
        });
        if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);
        const data = await response.json();

        const basinOptions = `<option value="0">--Select Basin--</option>` +
            data.map(item => `<option value="${item.value}">${item.text}</option>`).join("");

        $("#" + id).html(basinOptions);
    } catch (err) {
        console.error("Error While Getting Basin data:", err);
    } finally {
        HideLoader();
    }
}

async function BindDam(token, id, regionId, basinId) {
    ShowLoader();
    try {
        const response = await fetch(ResolveUrl("/GetAllDamFront"), {
            method: "POST",
            body: new URLSearchParams({ RegionId: regionId, BasinId: basinId, languageId: 1, AntiforgeryFieldname: token }),
        });
        if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);
        const result = await response.json();
        globalData = result || [];

        const damOptions = `<option value="0">--Select Dam--</option>` +
            globalData.map(item => `<option value="${item.value}">${item.text}</option>`).join("");

        $("#" + id).html(damOptions);
    } catch (err) {
        console.error("Error While Getting Dam data:", err);
    } finally {
        HideLoader();
    }
}

async function BindRiver(token, id, regionId, basinId) {
    ShowLoader();
    try {
        // Send parameters matching your backend action parameter names exactly:
        const postData = new URLSearchParams({
            languageid: 1,           
            regionId: regionId,      
            basinId: basinId,        
            __RequestVerificationToken: token  
        });

        const response = await fetch(ResolveUrl("/GetAllRiverFront"), {
            method: "POST",
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },  // important!
            body: postData
        });
        if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);
        const result = await response.json();
        globalData = result || [];

        const riverOptions = `<option value="0">--Select River--</option>` +
            globalData.map(item => `<option value="${item.value}">${item.text}</option>`).join("");

        $("#" + id).html(riverOptions);
    } catch (err) {
        console.error("Error While Getting River data:", err);
    } finally {
        HideLoader();
    }
}

async function BindCanal(token, id, regionId) {
    ShowLoader();
    try {
        const response = await fetch(ResolveUrl("/GetAllCanalFront"), {
            method: "POST",
            body: new URLSearchParams({ RegionId: regionId, languageId: 1, AntiforgeryFieldname: token }),
        });
        if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);
        const result = await response.json();
        globalData = result || [];

        const canalOptions = `<option value="0">--Select Canal--</option>` +
            globalData.map(item => `<option value="${item.value}">${item.text}</option>`).join("");

        $("#" + id).html(canalOptions);
    } catch (err) {
        console.error("Error While Getting Canal data:", err);
    } finally {
        HideLoader();
    }
}

async function RenderDam(token, damId) {
    ShowLoader();
    try {
        const response = await fetch(ResolveUrl("/GetDamById"), {
            method: "POST",
            body: new URLSearchParams({ damId, AntiforgeryFieldname: token }),
        });
        if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);
        const result = await response.json();

        RenderPage(result);

    } catch (err) {
        console.error("Error While Getting Canal data:", err);
    } finally {
        HideLoader();
    }
}

async function RenderRiver(token, riverId) {
    ShowLoader();
    try {
        const response = await fetch(ResolveUrl("/GetRiverById"), {
            method: "POST",
            body: new URLSearchParams({ riverId, AntiforgeryFieldname: token }),
        });
        if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);
        const result = await response.json();

        RenderPage(result);

    } catch (err) {
        console.error("Error While Getting Canal data:", err);
    } finally {
        HideLoader();
    }
}

async function RenderCanal(token, canalId) {
    ShowLoader();
    try {
        const response = await fetch(ResolveUrl("/GetCanalById"), {
            method: "POST",
            body: new URLSearchParams({ canalId, AntiforgeryFieldname: token }),
        });
        if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);
        const result = await response.json();

        RenderPage(result);

    } catch (err) {
        console.error("Error While Getting Canal data:", err);
    } finally {
        HideLoader();
    }
}