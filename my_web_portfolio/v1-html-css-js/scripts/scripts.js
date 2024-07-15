document.addEventListener('DOMContentLoaded', function () {
    console.log('Portfolio loaded');
});

document.addEventListener('DOMContentLoaded', function () {
    const hamburger = document.getElementById('hamburger');
    const navMenu = document.getElementById('navbar');

    hamburger.addEventListener('click', function () {
        navMenu.classList.toggle('show');
        // hamburger.style.display = 'none';
    });
});