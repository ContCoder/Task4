
document.getElementById('signupForm').addEventListener('submit', function (e) {
    const password = document.getElementById('password').value;

    const button = this.querySelector('.btn-signup');
    const errorAlert = document.getElementById('errorAlert');

    button.classList.add('loading');
    button.disabled = true;

    errorAlert.style.display = 'none';

    setTimeout(() => {
        button.classList.remove('loading');
        button.disabled = false;
    }, 2000);
});

function showError(message) {
    const errorAlert = document.getElementById('errorAlert');
    const errorMessage = document.getElementById('errorMessage');
    const successAlert = document.getElementById('successAlert');

    // Hide success message
    successAlert.style.display = 'none';

    errorMessage.textContent = message;
    errorAlert.style.display = 'block';

}

const inputs = document.querySelectorAll('input, select');
inputs.forEach(input => {
    input.addEventListener('input', function () {
        document.getElementById('errorAlert').style.display = 'none';
        document.getElementById('successAlert').style.display = 'none';
    });
});
