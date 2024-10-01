// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function getmail() {
    debugger;
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
