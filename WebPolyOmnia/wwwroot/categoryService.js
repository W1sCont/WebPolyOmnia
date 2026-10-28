const API_BASE_URL = 'https://localhost:7175/api';

export class CategoryService {
    static async getCategories() {
        const res = await fetch(`${API_BASE_URL}/categories`);
        return await res.json();
    }

    static async saveCategory(categoryDto) {
        const isNew = categoryDto.id === 0;
        const method = isNew ? 'POST' : 'PUT';
        const url = isNew ? `${API_BASE_URL}/categories` : `${API_BASE_URL}/categories/${categoryDto.id}`;

        return await fetch(url, {
            method: method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(categoryDto)
        });
    }

    static async deleteCategory(id) {
        const response = await fetch(`${API_BASE_URL}/categories/${id}`, {
            method: 'DELETE'
        });
    }
}