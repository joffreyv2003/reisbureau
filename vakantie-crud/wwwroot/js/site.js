// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
document.getElementById('btnHideModal').addEventListener('click', function () {
    localStorage.setItem('HideSpecialOfferModal', 'true');
});

$(document).ready(function () {
    var hideModal = localStorage.getItem('HideSpecialOfferModal') === 'true' || document.cookie.includes('HideSpecialOfferModal=true');

    if (!hideModal) {
        $('#specialOfferModal').modal('show');
    }
});

document.getElementById('contactForm').addEventListener('submit', function (event) {
    event.preventDefault();
    alert("Bedankt voor je verzending! We komen zo snel mogelijk bij je terug. (Dit bericht is fake)");
});

$(document).ready(function () {
    $("#editDestinationDiscount-@bestemming.id").change(function () {
        if ($(this).is(":checked")) {
            $("#editDestinationDiscountValue-@bestemming.id").show();
        } else {
            $("#editDestinationDiscountValue-@bestemming.id").hide();
        }
    });
});