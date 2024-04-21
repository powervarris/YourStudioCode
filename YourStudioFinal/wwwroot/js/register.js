var currentHour = new Date().getHours();
var greetingElement = document.getElementById("greeting");

if (currentHour >= 0 && currentHour < 12) {
    greetingElement.innerHTML = "<h1>GOOD MORNING</h1>";
} else if (currentHour >= 12 && currentHour < 18) {
    greetingElement.innerHTML = "<h1>GOOD AFTERNOON</h1>";
} else {
    greetingElement.innerHTML = "<h1>GOOD EVENING</h1>";
}

/*var passwordInput = document.querySelector('input[name="password"]');
var confirmPasswordInput = document.querySelector('input[name="confirmPassword"]');
var emailInput = document.querySelector('input[name="Email"]');
var phoneNumberInput = document.querySelector('input[name="phoneNumber"]');
var rememberPasswordCheck = document.getElementById('rememberPasswordCheck');
var registerButton = document.getElementById('registerButton');
var registrationForm = document.getElementById('registration-form');

function doPasswordsMatch() {
    return passwordInput.value === confirmPasswordInput.value;
}

function isEmailValid() {
    var email = emailInput.value.trim();
    var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

// Function to check if all required fields are filled
function areFieldsFilled() {
    return (
        passwordInput.value.trim().length > 0 &&
        confirmPasswordInput.value.trim().length > 0 &&
        emailInput.value.trim().length > 0 &&
        phoneNumberInput.value.trim().length === 10 && // Check if mobile number is filled
        rememberPasswordCheck.checked // Check if checkbox is checked
    );
}

function validateForm() {
    var passwordsMatch = doPasswordsMatch();
    var emailValid = isEmailValid();
    var fieldsFilled = areFieldsFilled();

    if (passwordsMatch && emailValid && fieldsFilled) {
        registerButton.removeAttribute('disabled');
    } else {
        registerButton.setAttribute('disabled', 'disabled');
    }
}

// Add event listeners
var inputFields = [passwordInput, confirmPasswordInput, emailInput, phoneNumberInput];
inputFields.forEach(field => {
    field.addEventListener('input', validateForm);
});

rememberPasswordCheck.addEventListener('change', validateForm);

registrationForm.addEventListener('submit', function (event) {
    event.preventDefault();
});

// Initial form validation
validateForm();*/