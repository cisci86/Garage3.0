// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const message = document.getElementById('message');
const create = document.getElementById('create');

function onClick() {
    setTimeout(function () { message.style.display='none' } , 3000)
}

create.addEventListener('click', function () {
    onClick();
})