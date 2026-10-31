const API_URL = 'http://localhost:5058/api';

class ApiService {
    static async request(endpoint, method = 'GET', body = null) {
        const token = localStorage.getItem('token');
        const headers = { 'Content-Type': 'application/json' };
        if (token) headers['Authorization'] = `Bearer ${token}`;

        const config = { method, headers };
        if (body) config.body = JSON.stringify(body);

        try {
            const response = await fetch(`${API_URL}${endpoint}`, config);
            const data = await response.json().catch(() => null);

            if (!response.ok) {
                if (response.status === 401) {
                    throw new Error(data?.message || 'No autorizado. Por favor inicia sesión.');
                }
                if (response.status === 403) {
                    throw new Error(data?.message || 'Acceso denegado. Se requieren permisos de administrador.');
                }
                if (data?.errors && typeof data.errors === 'object') {
                    const messages = Object.values(data.errors).flat().join(' ');
                    if (messages) throw new Error(messages);
                }
                throw new Error(data?.message || 'Error al procesar la solicitud.');
            }

            return data;
        } catch (error) {
            console.error('API Error:', error);
            throw error;
        }
    }

    static login(email, password) {
        return this.request('/Auth/login', 'POST', { email, password });
    }

    static register(dto) {
        return this.request('/Auth/register', 'POST', dto);
    }
}
