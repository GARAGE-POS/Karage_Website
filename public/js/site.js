// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function getmail() {
    // Create an object to store the form data
    var obj = {
        name: $(".Name").val(),
        email: $(".Email").val(),
        phone: $(".Phone").val(),
        company: $(".Company").val(),
        companysize: $(".CompanySize").val(),
        position: $(".Position").val()
    };

    // Clear the form fields
    $('.Name').val("");
    $('.Email').val("");
    $('.Phone').val("");
    $('.Company').val("");
    $('.CompanySize').val("");
    $('.Position').val("");

    // Make the AJAX request
    $.ajax({
        type: "GET",
        url: '/Home/Subscribe',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: obj, // Send the object directly
        success: function (res) {
            console.log("Subscription successful!", res);
        },
        error: function (xhr, textStatus, errorThrown) {
            console.error("Error occurred: " + errorThrown);
        }
    });
}


/********** Script for Free Trial Form *********/

function showMessage(form, type, message) {
    const successDiv = form.parentElement.querySelector('.w-form-done');
    const errorDiv = form.parentElement.querySelector('.w-form-fail');

    if (type === "success") {
        successDiv.querySelector('div').textContent = message;
        errorDiv.style.display = 'none';
        successDiv.style.display = 'block';
    } else if (type === "error") {
        errorDiv.querySelector('div').textContent = message;
        errorDiv.style.display = 'block';
    }
}

function showLoader() {
    const loader = document.getElementById("loader-overlay");
    if (loader) loader.style.display = "flex";
}
function hideLoader() {
    const loader = document.getElementById("loader-overlay");
    if (loader) loader.style.display = "none";
}

function validateForm(form) {
    let isValid = true;
    const inputs = form.querySelectorAll('input, textarea');

    inputs.forEach(input => {
        input.style.borderColor = ''; // reset

        if (input.hasAttribute('required') && !input.value.trim()) {
            input.style.borderColor = 'red';
            showMessage(form, "error", `${input.name} is required.`);
            isValid = false;
        }

        if (input.type === 'email' && input.value && !input.value.includes('@')) {
            input.style.borderColor = 'red';
            showMessage(form, "error", "Please enter a valid email address");
            isValid = false;
        }

        if (input.name.toLowerCase() === 'phonenumber' && input.value) {
            const phoneVal = input.value.trim().startsWith('+')
                ? input.value.trim()
                : '+' + input.value.trim();

            try {
                const parsed = libphonenumber.parsePhoneNumber(phoneVal);
                if (!parsed.isValid()) {
                    input.style.borderColor = 'red';
                    showMessage(form, "error", "Please enter a valid phone number with country code");
                    isValid = false;
                }
            } catch (e) {
                input.style.borderColor = 'red';
                showMessage(form, "error", "Please enter a valid phone number with country code");
                isValid = false;
            }
        }
    });

    return isValid;
}

document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById('wf-form-Create-Account-Form');
    if (!form) return; // avoid errors on pages without the form

    const successDiv = form.parentElement.querySelector('.w-form-done');
    const errorDiv = form.parentElement.querySelector('.w-form-fail');

    successDiv.style.display = 'none';
    errorDiv.style.display = 'none';

    form.addEventListener('submit', function (e) {
        e.preventDefault();
        successDiv.style.display = 'none';
        successDiv.querySelector('div').textContent = '';
        errorDiv.style.display = 'none';
        errorDiv.querySelector('div').textContent = '';

        if (!validateForm(form)) return;

        const formData = new FormData(form);
        const data = Object.fromEntries(formData.entries());

        if (data.PhoneNumber) {
            try {
                const phoneVal = data.PhoneNumber.trim().startsWith('+')
                    ? data.PhoneNumber.trim()
                    : '+' + data.PhoneNumber.trim();

                const parsed = libphonenumber.parsePhoneNumber(phoneVal);
                if (parsed && parsed.isValid()) {
                    data.PhoneCode = `+${parsed.countryCallingCode}`;
                    data.PhoneNumber = parsed.nationalNumber;
                }
            } catch (err) {
                console.error("Phone parsing failed:", err);
            }
        }

        const currentLang = window.location.pathname.startsWith("/ar") ? "ar" : "en";

        showLoader();

        fetch(`/Home/SubmitTrialForm?lang=${currentLang}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        })
            .then(res => res.json())
            .then(result => {
                hideLoader();
                if (result.success) {
                    showMessage(form, "success", result.message);
                    form.reset();
                    const customCheckbox = form.querySelector('.w-checkbox-input');
                    if (customCheckbox) {
                        customCheckbox.classList.remove('w--redirected-checked');
                    }
                } else {
                    showMessage(form, "error", result.message || "Something went wrong.");
                }
            })
            .catch(() => {
                hideLoader();
                showMessage(form, "error", "Something went wrong. Please try again later.");
            });
    });
});
