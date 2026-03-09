import axios from 'axios';

export const api = axios.create({
	baseURL: 'http://localhost:8080/api',
	withCredentials: true,
});

api.interceptors.response.use(
	(response) => response,
	async (error) => {
		const originalRequest = error.config;

		if (error.response?.status === 401 && !originalRequest._retry) {
			originalRequest._retry = true;
			try {
				await api.post('/auth/refresh');
				return api(originalRequest);
			} catch (error) {
				if (typeof window !== undefined) {
					window.location.href = '/login';
				}
				return Promise.reject(error);
			}
		}
		return Promise.reject(error);
	},
);
