// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Page-load fade-in: the inline script in <head> sets opacity 0 before paint;
// clearing it here lets the CSS transition carry it back to 1.
document.addEventListener('DOMContentLoaded', function () {
    document.documentElement.style.opacity = '';
});
