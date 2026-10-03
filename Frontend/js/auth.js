document.addEventListener('DOMContentLoaded', () => {
    const loginForm = document.getElementById('login-form');
    if (loginForm) {
        loginForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            const email = document.getElementById('email').value;
            const password = document.getElementById('password').value;
            const errorMsg = document.getElementById('error-msg');

            try {
                errorMsg.textContent = 'Verificando...';
                errorMsg.style.color = '#666';
                const response = await ApiService.login(email, password);

                localStorage.setItem('token', response.token);
                localStorage.setItem('role', response.role);
                localStorage.setItem('firstName', response.firstName);
                localStorage.setItem('email', response.email);

                if (response.role === 'Admin') {
                    window.location.href = 'panel-admin.html';
                } else {
                    window.location.href = 'index.html';
                }
            } catch (error) {
                errorMsg.textContent = 'Credenciales inválidas. Verifica tu correo y contraseña.';
                errorMsg.style.color = 'red';
            }
        });
    }
});
