

document.getElementById('loginForm').addEventListener('submit', function (e) {
    const button = this.querySelector('.btn-login');
    const errorAlert = document.getElementById('errorAlert');

    button.classList.add('loading');
    button.disabled = true;

    errorAlert.style.display = 'none';

    setTimeout(() => {
        button.classList.remove('loading');
        button.disabled = false;
    }, 2000);
});

// Clear error when user starts typing
document.getElementById('email').addEventListener('input', function () {
    document.getElementById('errorAlert').style.display = 'none';
});

document.getElementById('password').addEventListener('input', function () {
    document.getElementById('errorAlert').style.display = 'none';
});