const API_BASE_URL = 'https://localhost:7175/api';

export class CategoryService {
    static async getCategories() {
        const res = await fetch(`${API_BASE_URL}/categories`);
        return await res.json();
    }

    static async getCategoriesById(id) {
        const res = await fetch(`${API_BASE_URL}/categories/${id}`);
        return await res.json();
    }

    static async saveCategory(categoryDto) {
        const isNew = categoryDto.id === 0;
        const method = isNew ? 'POST' : 'PUT';
        const url = isNew ? `${API_BASE_URL}/categories` : `${API_BASE_URL}/categories/${categoryDto.id}`;

        const response = await fetch(url, {
            method: method,
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(categoryDto)
        });
        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(errorText || 'Помилка при збереженні категорії');
        }

        return true;
    }

    static async deleteCategory(id) {
        const response = await fetch(`${API_BASE_URL}/categories/${id}`, {
            method: 'DELETE'
        });
        if (!response.ok) throw new Error('Помилка при видаленні категорії');
        return true;
    }
}