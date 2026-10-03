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
            if (!response.ok) throw new Error(data?.message || 'Error en la petición');
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
