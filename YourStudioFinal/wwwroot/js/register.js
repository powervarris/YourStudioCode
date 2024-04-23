var currentHour = new Date().getHours();
var greetingElement = document.getElementById("greeting");

if (currentHour >= 0 && currentHour < 12) {
    greetingElement.innerHTML = "<h1>GOOD MORNING</h1>";
} else if (currentHour >= 12 && currentHour < 18) {
    greetingElement.innerHTML = "<h1>GOOD AFTERNOON</h1>";
} else {
    greetingElement.innerHTML = "<h1>GOOD EVENING</h1>";
}

var passwordInput = document.getElementById('floatingPassword');
var passwordFormat = document.getElementById('passwordFormat');

passwordInput.addEventListener('input', function () {
    if (passwordInput.validity.valid) {
        passwordFormat.style.display = 'none';
    } else {
        passwordFormat.style.display = 'inline';
    }
});

var confirmPasswordInput = document.getElementById('floatingConfirmPassword');

confirmPasswordInput.addEventListener('input', function () {
    var passwordValue = passwordInput.value;
    var confirmPasswordValue = confirmPasswordInput.value;

    if (passwordValue === confirmPasswordValue) {
        confirmPasswordInput.setCustomValidity('');
    } else {
        confirmPasswordInput.setCustomValidity('Passwords must match');
    }
});

var registrationForm = document.getElementById('registration-form');
registrationForm.addEventListener('input', function () {
    registrationForm.reportValidity();
});
passwordInput.addEventListener('keypress', function (event) {
    if (event.key === 'Enter') {
        event.preventDefault();
    }
});

confirmPasswordInput.addEventListener('keypress', function (event) {
    if (event.key === 'Enter') {
        event.preventDefault();
    }
});

emailInput.addEventListener('keypress', function (event) {
    if (event.key === 'Enter') {
        event.preventDefault();
    }
});

mobileNumberInput.addEventListener('keypress', function (event) {
    if (event.key === 'Enter') {
        event.preventDefault();
    }
});

rememberPasswordCheck.addEventListener('keypress', function (event) {
    if (event.key === 'Enter') {
        event.preventDefault();
    }
});