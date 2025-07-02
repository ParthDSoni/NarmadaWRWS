$(document).ready(function () {
    const form = $('#frmAdd');
    const token = $('input[name="AntiforgeryFieldname"]', form).val();

    $("#State").change(function () {
        var stateValue = $(this).val();
        if (stateValue != "7") {
            $("#dvCityName").removeAttr('hidden');
            $("#dvCity").hide();
            $("#City").val("0");
        } else {
            $("#dvCityName").attr('hidden', 'hidden');
            $("#dvCity").show();
        }
    });
    document.getElementById('Country').addEventListener('change', function () {
        var countryValue = this.value;
        if (countryValue !== '76') {
            $("#dvStateName").removeAttr('hidden');
            $("#dvCityName").removeAttr('hidden');
            $("#State").val("0");
            $("#City").val("0");
            $("#dvCity").hide();
            $("#dvState").hide();

        } else {
            $("#dvStateName").attr('hidden', 'hidden');
            $("#dvCityName").attr('hidden', 'hidden');
            $("#dvCity").show();
            $("#dvState").show();
        }
    });

    HideLoader();
    BindCountries(token, "Country");
    BindState(token, "State", 76);
    BindCity(token, "City", 7);

    resetCaptchaImage();

    $("#SearchClear").click(function (e) {
        ClearForm();
    });

    $('#btnMdlSave').click(function () {
        ShowLoader();
        var isError = false;

        if (!ValidateControl($('#FName'))) {
            ShowMessage("Enter First Name !", "", "error");
            isError = true;
            return;
        }
        else if (!ValidateControl($('#LName'))) {
            ShowMessage("Enter Last Name !", "", "error");
            isError = true;
            return;
        }
        else if (!ValidateControl($('#Email'))) {
            ShowMessage("Enter Email !", "", "error");
            isError = true;
            return;
        }
        else if (!isValidEmail($('#Email').val())) {
            ShowMessage("Enter a valid Email!", "", "error");
            isError = true;
            return;
        }
        else if (!ValidateControl($('#PhoneNo'))) {
            ShowMessage("Enter Mobile Number !", "", "error");
            isError = true;
            return;
        }
        else if (!isValidMobile($('#PhoneNo').val())) {
            ShowMessage("Enter a valid 10-digit Mobile Number!", "", "error");
            isError = true;
            return;
        }
        else if (!ValidateControl($('#Zip'))) {
            ShowMessage("Enter Zip Code !", "", "error");
            isError = true;
            return;
        }
        else if (!ValidateControl($('#Subject'))) {
            ShowMessage("Enter Subject !", "", "error");
            isError = true;
            return;
        }
        else if ($('#Country').val() == "0") {
            ShowMessage("Enter Country !", "", "error");
            isError = true;
            return;
        }
        
        if ($("#Country").val() == "76") {
            if ($("#State").val() == "0") {
                ShowMessage("Select State!", "", "error");
                isError = true;
                return;
            }
        }
        else {
            if (!ValidateControl($('#StateName'))) {
                ShowMessage("Enter State!", "", "error");
                isError = true;
                return;
            }
        }

        if ($("#State").val() == "7") {
            if ($("#City").val() == "0") {
                ShowMessage("Select City!", "", "error");
                isError = true;
                return;
            }
        }
        else {
            if (!ValidateControl($('#CityName'))) {
                ShowMessage("Enter City!", "", "error");
                isError = true;
                return;
            }
        }
        if (!ValidateControl($('#Address'))) {
            ShowMessage("Enter Address !", "", "error");
            isError = true;
            return;
        }
        else if (!ValidateControl($('#FeedbackDetails'))) {
            ShowMessage("Enter Comments/Quries!", "", "error");
            isError = true;
            return;
        }

        if (!isError) {
            var Captcha = document.getElementById("Captcha");
            $('#hfEmail').val(FrontValue($('#Email').val() + "--exegil--" + $('#Captcha').val()));
            var formdata = new FormData(document.getElementById("frmAdd"));

            $.ajax({
                type: "post",
                url: ResolveUrl("/Admin/AddFeedback"),
                data: formdata,
                processData: false,
                contentType: false,
                dataType: 'json',
                success: function (data) {
                    if (data != null && data != undefined) {

                        ShowMessage(data.strMessage, "", data.type);

                        if (data.isError != true) {
                            ClearForm();
                        }
                        HideLoader();
                        resetCaptchaImage();
                        $('#Captcha').val('');
                    }
                    else {
                        ShowMessage("Record not saved, Try again", "", "error");
                        resetCaptchaImage();
                        HideLoader();
                    }
                },
                error: function (ex) {
                    ShowMessage("Something went wrong, Try again!", "", "error");
                    resetCaptchaImage();
                    HideLoader();
                }
            });
        }
    });
});

async function resetCaptchaImage() {
    const fdhfCaptcha = document.getElementById("hfCaptcha");

    try {
        const response = await fetch(ResolveUrl("/GetCaptchaDetails"), {
            method: "GET",
            headers: { "Content-Type": "application/json" }
        });

        if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);

        const data = await response.json();

        if (!data.isError) {
            document.getElementById("imgCapcha").src = `data:image/png;base64, ${data.result.fileSRC}`;
            fdhfCaptcha.value = data.result.captchaval;
        } else {
            ShowMessage(data.strMessage, "", data.type);
        }
    } catch (error) {
        console.error("Error fetching captcha details:", error);
    }
}

function ClearForm() {
    $('#FName').val('');
    $('#LName').val('');
    $('#Email').val('');
    $('#MobileNo').val('');
    $('#Zip').val('');
    $('#Subject').val('');
    $('#Country').val('0');
    $('#State').val('0');
    $('#StateName').val('');
    $('#City').val('0');
    $('#CityName').val('');
    $('#Address').val('');
    $('#FeedbackDetails').val('');
    $('#PhoneNo').val('');
    $('#Department').val('');
}

const BindCountries = async (token, id) => {
    ShowLoader();
    try {
        const response = await fetch(ResolveUrl("/GetAllDistrict"), {
            method: "GET",
        });

        if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);

        let result = await response.json();
        result = result.result;

        $("#" + id).html(`<option value="0">--Select Country--</option>` + result.map(item => `<option value="${item.Id}">${item.CountryName}</option>`).join(""));
    } catch (err) {
        console.error("Error While Getting Language data:", err);
    } finally {
        HideLoader();
    }
};

const BindState = async (token, id, countryId) => {
    ShowLoader();
    try {
        const response = await fetch(ResolveUrl(`/GetAllStatesByCountry?country=${countryId}`), {
            method: "GET",
        });

        if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);

        let result = await response.json();

        result = result.result;

        $("#" + id).html(`<option value="0">--Select State--</option>` + result.map(item => `<option value="${item.Id}">${item.StateName}</option>`).join(""));
    } catch (err) {
        console.error("Error While Getting Language data:", err);
    } finally {
        HideLoader();
    }
};

const BindCity = async (token, id, stateId) => {
    ShowLoader();
    try {
        const response = await fetch(ResolveUrl(`/GetAllDistrictsByState?state=${stateId}`), {
            method: "GET",
        });

        if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);

        let result = await response.json();

        result = result.result;

        $("#" + id).html(`<option value="0">--Select City--</option>` + result.map(item => `<option value="${item.DistId}">${item.DistrictName}</option>`).join(""));

    } catch (err) {
        console.error("Error While Getting Language data:", err);
    } finally {
        HideLoader();
    }
};
function isValidEmail(email) {
    var regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return regex.test(email);
}
function isValidMobile(mobile) {
    var regex = /^[0-9]{10}$/;
    return regex.test(mobile);
}