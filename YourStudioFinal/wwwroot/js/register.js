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
var confirmPasswordInput = document.getElementById('floatingConfirmPassword'); 
var usernameInput = document.getElementById('floatingInput');
var emailInput = document.getElementById('floatingEmail');
var fnameInput = document.getElementById('floatingFname');
var lnameInput = document.getElementById('floatingLname');

var passwordChanged = false; 
usernameInput.addEventListener('input', function () {
    var usernameValue = usernameInput.value.trim();
    var containsSpecialChar = /[ `!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?~]/.test(usernameValue);
    var containsSpace = /\s/.test(usernameValue);

    if (containsSpecialChar || containsSpace) {
        usernameInput.setCustomValidity('Username should not contain special characters or spaces');
    } else {
        usernameInput.setCustomValidity('');
    }
});

fnameInput.addEventListener('input', function () {
    var fnameValue = fnameInput.value.trim();
    var isEmail = /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(fnameValue);
    var containsNumber = /\d/.test(fnameValue);

    if (isEmail) {
        fnameInput.setCustomValidity('First name cannot be an email address');
    } else if (containsNumber) {
        fnameInput.setCustomValidity('First name cannot contain numbers');
    } else {
        fnameInput.setCustomValidity('');
    }
});

lnameInput.addEventListener('input', function () {
    var lnameValue = lnameInput.value.trim();
    var isEmail = /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(lnameValue);
    var containsNumber = /\d/.test(lnameValue);

    if (isEmail) {
        lnameInput.setCustomValidity('Last name cannot be an email address');
    } else if (containsNumber) {
        lnameInput.setCustomValidity('Last name cannot contain numbers');
    } else {
        lnameInput.setCustomValidity('');
    }
});

emailInput.addEventListener('input', function () {
    var emailValue = emailInput.value.trim();
    var isValidEmail = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.(?:com|org|net|edu|gov|mil|biz|info|mobi|name|pro|tel|asia|jobs|museum|[a-zA-Z]{2})$/.test(emailValue);

    if (!isValidEmail) {
        emailInput.setCustomValidity('Invalid email address');
    } else {
        emailInput.setCustomValidity('');
    }
});


passwordInput.addEventListener('input', function () {
    passwordChanged = true;
    validatePassword();
});

var registrationForm = document.getElementById('registration-form');
registrationForm.addEventListener('input', function () {
    registrationForm.reportValidity();
});

fnameInput.addEventListener('input', function (event) {
    if (event.key === 'Enter') {
        event.preventDefault();
    }
});

lnameInput.addEventListener('input', function (event) {
    if (event.key === 'Enter') {
        event.preventDefault();
    }
});

passwordInput.addEventListener('keyup', function (event) {
    if (event.key === 'Enter') {
        event.preventDefault();
    }
    confirmPasswordInput.setCustomValidity('');
    passwordChanged = false;
});

confirmPasswordInput.addEventListener('keyup', function (event) {
    if (event.key === 'Enter') {
        event.preventDefault();
    }
    validatePassword();
});

emailInput.addEventListener('keyup', function (event) {
    if (event.key === 'Enter') {
        event.preventDefault();
    }
});

mobileNumberInput.addEventListener('keyup', function (event) {
    if (event.key === 'Enter') {
        event.preventDefault();
    }
});

rememberPasswordCheck.addEventListener('keyup', function (event) {
    if (event.key === 'Enter') {
        event.preventDefault();
    }
});

function validatePassword() {
    var passwordValue = passwordInput.value;
    var confirmPasswordValue = confirmPasswordInput.value;

    var passwordRegex = /^(?=.*[A-Z])(?=.*\d).{8,}$/;

    if (passwordRegex.test(passwordValue)) {
        passwordFormat.style.display = 'none'; 
        if (passwordValue === confirmPasswordValue) {
            confirmPasswordInput.setCustomValidity(''); 
        } else {
            confirmPasswordInput.setCustomValidity('Passwords must match'); y
        }
    } else {
        passwordFormat.style.display = 'inline'; 
        confirmPasswordInput.setCustomValidity(''); 
    }
}
