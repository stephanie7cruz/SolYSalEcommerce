

const API_BASE_URL = 'https://localhost:7191/api'; //  URL donde corre el backend

// Función genérica para realizar llamadas a la API
export async function apiCall(endpoint, method = 'GET', data = null, requiresAuth = false) {
    const url = `${API_BASE_URL}${endpoint}`;
    const headers = {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    };

    if (requiresAuth) {
        const token = localStorage.getItem('jwtToken');
        if (!token) {
            console.error('No JWT token found. Redirecting to login.');
            window.location.href = 'auth.html'; // Redirige al login si no hay token
            throw new Error('Authorization required.');
        }
        headers['Authorization'] = `Bearer ${token}`;
    }

    const config = {
        method,
        headers,
    };

    if (data) {
        config.body = JSON.stringify(data);
    }

    try {
        const response = await fetch(url, config);
        // Manejar respuestas 204 No Content
        if (response.status === 204) {
            return null;
        }
        const responseData = await response.json();

        if (!response.ok) {
            // Si la respuesta no es OK (ej. 400, 401, 500), lanzar un error
            const error = new Error(responseData.message || 'Something went wrong');
            error.statusCode = response.status;
            error.details = responseData; // Para errores más detallados
            throw error;
        }

        return responseData;
    } catch (error) {
        console.error('API Call Error:', error);
        throw error; // Re-lanza el error para que sea manejado por la función que llama
    }
}