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
    const inputs = form.querySelectorAll('input, select, textarea');

    inputs.forEach(input => {
        input.style.borderColor = '';

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
    debugger
    const form = document.getElementById('karage-main-form');
    if (!form) return;

    const successDiv = document.getElementById('success-message');
    const errorDiv = document.getElementById('error-message');
    const zohoForm = document.getElementById('zoho-hidden-form');

    successDiv.style.display = 'none';
    errorDiv.style.display = 'none';

    form.addEventListener('submit', function (e) {
        e.preventDefault();
        
        successDiv.style.display = 'none';
        errorDiv.style.display = 'none';

        if (!validateForm(form)) return;

        const formData = new FormData(form);


        const fullName = formData.get('FullName') || '';
        const parts = fullName.trim().split(' ');
        zohoForm.querySelector('input[name="SingleLine"]').value = parts[0] || '';
        zohoForm.querySelector('input[name="SingleLine1"]').value = parts.slice(1).join(' ') || '';
        zohoForm.querySelector('input[name="SingleLine2"]').value = formData.get('Company') || '';
        zohoForm.querySelector('input[name="Email"]').value = formData.get('Email') || '';
        zohoForm.querySelector('select[name="Address_Country"]').value = formData.get('Country') || '';
        zohoForm.querySelector('input[name="Address_City"]').value = formData.get('City') || '';
        zohoForm.querySelector('select[name="Dropdown"]').innerHTML = `<option selected>${formData.get('BusinessType')}</option>`;
        zohoForm.querySelector('select[name="Dropdown1"]').innerHTML = `<option selected>${formData.get('PrefilledProducts')}</option>`;



       const phoneValue = formData.get('PhoneNumber')?.trim() || '';
        const phoneVal = phoneValue.startsWith('+') ? phoneValue : '+' + phoneValue;
        try {
            const parsed = libphonenumber.parsePhoneNumber(phoneVal);
            if (parsed.isValid()) {
                zohoForm.querySelector('input[name="PhoneNumber_countrycodeval"]').value = `+${parsed.countryCallingCode}`;
                zohoForm.querySelector('input[name="PhoneNumber_countrycode"]').value = parsed.nationalNumber;
            } else {
                zohoForm.querySelector('input[name="PhoneNumber_countrycodeval"]').value = '';
                zohoForm.querySelector('input[name="PhoneNumber_countrycode"]').value = phoneValue;
            }
        } catch {
            zohoForm.querySelector('input[name="PhoneNumber_countrycodeval"]').value = '';
            zohoForm.querySelector('input[name="PhoneNumber_countrycode"]').value = phoneValue;
        }


        zohoForm.setAttribute('target', 'hidden_iframe');
        const iframe = document.getElementById('hidden_iframe');

        showLoader();

        try {
    zohoForm.submit();

    iframe.onload = function () {
        hideLoader();

        const isArabic = window.location.pathname.includes('/ar');


        const successMessage = isArabic
            ? "شكرًا لك! تم إرسال طلبك للتجربة المجانية بنجاح، سنتواصل معك قريباً."
            : "Your request was successfully submitted!";

        showMessage(form, "success", successMessage);
        form.reset();
        document.querySelector(".w-checkbox-input")?.classList.remove("w--redirected-checked");
    };

} catch (err) {
    console.error('Zoho form submission error:', err);
    hideLoader();

    const isArabic = window.location.pathname.includes('/ar');
    const errorMessage = isArabic
        ? "حدث خطأ ما. يرجى المحاولة مرة أخرى."
        : "Something went wrong. Please try again.";

    showMessage(form, "error", errorMessage);
}

    });
});