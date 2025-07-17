
import { apiCall } from './api.js';

document.addEventListener('DOMContentLoaded', () => {
    const loginForm = document.getElementById('loginForm');
    const registerForm = document.getElementById('registerForm');
    const loginMessage = document.getElementById('loginMessage');
    const registerMessage = document.getElementById('registerMessage');

    // Función para mostrar mensajes de error/éxito
    const displayMessage = (element, message, type = 'danger') => {
        element.textContent = message;
        element.className = `alert alert-${type}`;
        element.classList.remove('d-none');
    };

    // Ocultar mensajes al cambiar de pestaña
    const pillsTab = document.getElementById('pills-tab');
    if (pillsTab) {
        pillsTab.addEventListener('shown.bs.tab', () => {
            loginMessage.classList.add('d-none');
            registerMessage.classList.add('d-none');
        });
    }

    // Manejar el envío del formulario de Login
    if (loginForm) {
        loginForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            loginMessage.classList.add('d-none'); // Ocultar mensaje anterior

            const email = document.getElementById('loginEmail').value;
            const password = document.getElementById('loginPassword').value;

            try {
                // Asumiendo que tu endpoint de login es /api/Auth/login
                const data = await apiCall('/Auth/login', 'POST', { email, password });

                if (data && data.token) {
                    localStorage.setItem('jwtToken', data.token);
                    // Almacenar información adicional del usuario en localStorage
                    localStorage.setItem('userEmail', data.email || email); // Guarda el email
                    localStorage.setItem('userFirstName', data.firstName || ''); // Guarda el nombre
                    localStorage.setItem('userLastName', data.lastName || '');   // Guarda el apellido
                    localStorage.setItem('userName', data.firstName || email); // Para compatibilidad con userName existente

                    console.log('Login exitoso:', data);

                    // Redirigir a la página principal (home.html)
                    window.location.href = 'home.html';
                } else {
                    displayMessage(loginMessage, 'Credenciales incorrectas o respuesta inválida.');
                }
            } catch (error) {
                console.error('Error durante el login:', error);
                const errorMessage = error.details?.message || error.message || 'Error al iniciar sesión.';
                displayMessage(loginMessage, errorMessage);
            }
        });
    }

    // Manejar el envío del formulario de Registro
    if (registerForm) {
        registerForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            registerMessage.classList.add('d-none'); // Ocultar mensaje anterior

            const firstName = document.getElementById('registerFirstName').value;
            const lastName = document.getElementById('registerLastName').value;
            const email = document.getElementById('registerEmail').value;
            const password = document.getElementById('registerPassword').value;

            // Validación básica en frontend (opcional pero buena práctica)
            if (!firstName || !lastName || !email || !password) {
                displayMessage(registerMessage, 'Por favor, completa todos los campos (Nombre(s), Apellido(s), Correo y Contraseña).');
                return;
            }

            try {
                const data = await apiCall('/Auth/register', 'POST', { firstName, lastName, email, password });

                if (data && data.userId) { // Asumiendo que el registro devuelve un 'userId'
                    displayMessage(registerMessage, 'Registro exitoso. ¡Ahora puedes iniciar sesión!', 'success');
                    // Opcional: cambiar a la pestaña de login automáticamente
                    const loginTab = new bootstrap.Tab(document.getElementById('pills-login-tab'));
                    loginTab.show();
                    // Limpiar el formulario después del registro exitoso
                    registerForm.reset();
                } else {
                    displayMessage(registerMessage, data?.message || 'Error al registrar el usuario o respuesta inválida.');
                }
            } catch (error) {
                console.error('Error durante el registro:', error);
                let errorMessage = 'Error al registrar usuario.';
                if (error.details && typeof error.details === 'object') {
                    if (error.details.errors) {
                        const validationErrors = Object.values(error.details.errors).flat();
                        errorMessage = validationErrors.join('<br>');
                    } else if (error.details.message) {
                        errorMessage = error.details.message;
                    }
                } else if (error.message) {
                    errorMessage = error.message;
                }
                displayMessage(registerMessage, errorMessage);
            }
        });
    }
});
