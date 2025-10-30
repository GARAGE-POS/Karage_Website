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



document.addEventListener("DOMContentLoaded", () => {
  const form = document.getElementById("create-account-form");
  const countrySelect = document.querySelector('select[name="Address_Country"]');
  const hiddenCode = document.querySelector('input[name="PhoneNumber_countrycodeval"]');
  const checkboxDiv = document.querySelector(".w-checkbox-input");
  const checkboxInput = document.querySelector('input[name="checkbox"]');

  const countryCodes = {
    "Saudia Arabia": "+966",
    "UAE": "+971",
    "Bahrain": "+973",
    "Kuwait": "+965",
    "Qatar": "+974",
    "Oman": "+968",
    "Jordan": "+962",
    "Lebanon": "+961",
    "Egypt": "+20"
  };


  hiddenCode.value = localStorage.getItem("countryCode") || "";

  countrySelect.addEventListener("change", () => {
    const code = countryCodes[countrySelect.value] || "";
    hiddenCode.value = code;
    localStorage.setItem("countryCode", code);
  });


  form.addEventListener("submit", () => {
    localStorage.setItem("countryCode", hiddenCode.value);
    checkboxInput.checked = false;
    checkboxDiv?.classList.remove("w--redirected-checked");
  });
});



  function switchLang(lang) {
    let url = window.location.href;
    if (lang === 'ar') {
      url = url.replace('/en/', '/ar/');
    } else if (lang === 'en') {
      url = url.replace('/ar/', '/en/');
    }
    window.location.href = url;
  }

