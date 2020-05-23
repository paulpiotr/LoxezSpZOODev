// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.
$(document).ready(function () {
    $("input[name='Ilosc']").each(function (i, element) {
        $(element).change(function () {
            var value = parseFloat($(this).val().replace(",", "."));
            if (value < 0 || !value) {
                value = 1;
            }
            $(this).val(value.toFixed(2).replace(".", ","));
        }); 
    });
});