const API_BASE_URL = 'https://localhost:7175/api'; 

export class UserService {
    static async getUsers() {
        const response = await fetch(`${API_BASE_URL}/users`);
        if (!response.ok) throw new Error('Не вдалося завантажити список користувачів');
        return await response.json();
    }

    static async getUserById(id) {
        const response = await fetch(`${API_BASE_URL}/users/${id}`);
        if (!response.ok) throw new Error('Користувача не знайдено');
        return await response.json();
    }

    static async approveUser(id) {
        const response = await fetch(`${API_BASE_URL}/users/${id}`, {
            method: 'PUT'
        });
        if (!response.ok) throw new Error('Помилка при затвердженні користувача');
        return true;
    }

    static async deleteUser(id) {
        const response = await fetch(`${API_BASE_URL}/users/${id}`, {
            method: 'DELETE'
        });
        if (!response.ok) throw new Error('Помилка при видаленні користувача');
        return true;
    }
}