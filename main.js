// script.js
document.addEventListener('DOMContentLoaded', function() {
    var alertButton = document.createElement('button');
    alertButton.id = 'alertButton';
    alertButton.textContent = 'Click me!';
    document.body.appendChild(alertButton);

    alertButton.addEventListener('click', function() {
        alert('Button clicked!');
    });
});
