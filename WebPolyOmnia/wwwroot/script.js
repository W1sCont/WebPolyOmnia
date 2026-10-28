const API_BASE_URL = 'https://localhost:7175/api';

class WebPolyOmniaApp {
    constructor() {
        this.contentContainer = document.getElementById('app-content');
        this.init();
    }

    init() {
        this.navigateTo('categories');
    }

    async navigateTo(view, id = null) {
        this.contentContainer.innerHTML = '<p>Завантаження...</p>';
        try {
            if (view === 'categories') await this.renderCategoryList();
            else if (view === 'categories-form') await this.renderCategoryForm(id);
            else if (view === 'users') await this.renderUsersList();
            else if (view === 'users-form') await this.aproveUser(id);
            else if (view === 'videos') await this.renderVideoList();
            else if (view === 'videos-form') await this.renderVideo(id);
        } catch (error) {
            this.contentContainer.innerHTML = `<p style="color: red;">Помилка: ${error.message}</p>`;
        }
    }

    // ==========================================
    // CRUD Category
    // ==========================================

    async renderCategoryList() {
        const response = await fetch(`${API_BASE_URL}/categories`);
        const category = await response.json();

        let html = `
            <h2>Список категорій</h2>
            <button onclick="app.navigateTo('categories-form')" style="margin-bottom: 1rem;">+ Додати категорію</button>
            <table>
                <thead>
                    <tr><th>Назва</th><th>Дії</th></tr>
                </thead>
                <tbody>
        `;

        category.forEach(cat => {
            html += `
                <tr>
                    <td>${cat.name}</td>
                    <td>
                        <button onclick="app.navigateTo('categories-form', ${cat.id})">Редагувати</button>
                        <button onclick="app.Delete(${cat.id})" style="background-color: #dc3545;">Видалити</button>
                    </td>
                </tr>
            `;
        });

        html += `</tbody></table>`;
        this.contentContainer.innerHTML = html;
    }

    async renderCategoryForm(id) {
        let category = { id: 0, name: '' };

        if (id) {
            const response = await fetch(`${API_BASE_URL}/categories/${id}`); 
            category = await response.json();
        }

        this.contentContainer.innerHTML = `
            <h2>${id ? 'Редагувати' : 'Додати'} категорію</h2>
            <form onsubmit="app.saveCategory(event)">
                <input type="hidden" id="categoryId" value="${category.id}">
                <div class="form-group">
                    <label>Назва категорії:</label>
                    <input type="text" id="categoryName" value="${category.name}" required>
                </div>
                <button type="submit">Зберегти</button>
                <button type="button" onclick="app.navigateTo('categories')" style="background-color: #6c757d;">Скасувати</button>
            </form>
        `;
    }

    async saveCategory(event) {
        event.preventDefault();
        const id = parseInt(document.getElementById('categoryId').value);

        const categoryDto = {
            id: id,
            name: document.getElementById('categoryName').value
        };

        const isNew = id === 0;
        const method = isNew ? 'POST' : 'PUT'; // POST або PUT
        const url = isNew ? `${API_BASE_URL}/categories` : `${API_BASE_URL}/categories/${id}`;

        const response = await fetch(url, {
            method: method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(categoryDto)
        });

        if (response.ok) {
            await this.navigateTo('categories');
        } else {
            alert('Помилка при збереженні категорії');
        }
    }

    async deleteCategory(id) {
        if (!confirm('Ви впевнені, що хочете видалити цю категорію?')) return;

        const response = await fetch(`${API_BASE_URL}/categories/${id}`, { // DELETE: api/teams/{id}
            method: 'DELETE'
        });

        if (response.ok) await this.renderCategoryList();
        else alert('Помилка при видаленні категорії');
    }

    // ==========================================
    // Users
    // ==========================================

    async renderUsersList() {
        const response = await fetch(`${API_BASE_URL}/users`);
        const users = await response.json();

        let html = `
            <h2>Список не затверджених користувачів</h2>
            <table>
                <thead>
                    <tr><th>Ім'я</th><th>Прізвище</th><th>Мейл</th><th>Логін</th><th>Дата створення</th><th>Роль</th><th>Дії</th></tr>
                </thead>
                <tbody>
        `;

        users.forEach(user => {
            html += `
                <tr>
                    <td>${user.name}</td>
                    <td>${user.surname}</td>
                    <td>${user.email}</td>
                    <td>${user.login}</td>
                    <td>${user.createAt}</td>
                    <td>${user.role}</td>
                    <td>
                        <button onclick="app.navigateTo('users-form', ${user.id})">Підтвердити</button>
                        <button onclick="app.Delete(${user.id})" style="background-color: #dc3545;">Видалити</button>
                    </td>
                </tr>
            `;
        });

        html += `</tbody></table>`;
        this.contentContainer.innerHTML = html;
    }


    async aproveUser(id) {
        event.preventDefault();
        const response = await fetch(`${API_BASE_URL}/users/${id}`, {
            method: "POST"
        });

        if (response.ok) {
            await this.navigateTo('users');
        } else {
            alert('Помилка при зміні ролі користувача');
        }
    }

    async deleteUser(id) {
        if (!confirm('Ви впевнені, що хочете видалити цього користувача?')) return;

        const response = await fetch(`${API_BASE_URL}/users/${id}`, { 
            method: 'DELETE'
        });

        if (response.ok) await this.renderUsersList();
        else alert('Помилка при видаленні користувача');
    }

    // ==========================================
    // Videos
    // ==========================================

    async renderVideoList() {
        const response = await fetch(`${API_BASE_URL}/videos`);
        const video = await response.json();

        let html = `
            <h2>Список не затверджених відео</h2>
            <table>
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Назва</th>
                        <th>Автор</th>
                        <th>Дії</th>
                    </tr>
                </thead>
                <tbody>
        `;

        video.forEach(v => {
            html += `
                <tr>
                    <td>${v.id}</td>
                    <td>${v.title}</td>
                    <td>${v.author || 'Не вказано'}</td>
                    <td>
                        <button class="btn-green" onclick="app.navigateTo('videos-form', ${v.id})" style="background-color: #28a745; color: white; border:none; padding:4px 8px; border-radius:4px; cursor:pointer;">Детальніше</button>
                        <button class="btn-red" onclick="app.Delete(${v.id})" style="background-color: #dc3545; color: white; border:none; padding:4px 8px; border-radius:4px; cursor:pointer;">Видалити</button>
                    </td>
                </tr>
            `;
        });

        html += `</tbody></table>`;
        this.contentContainer.innerHTML = html;
    }

    async renderVideo(id) {
        this.contentContainer.innerHTML = '<p>Завантаження відео...</p>';
        try {
            const response = await fetch(`${API_BASE_URL}/videos/${id}`);
            if (!response.ok) throw new Error('Не вдалося завантажити інформацію про відео');

            const video = await response.json();

            this.contentContainer.innerHTML = `
                <div class="video-detail-container" style="max-width: 800px; margin: 0 auto;">
                    <button onclick="app.renderVideoList()" style="margin-bottom: 1rem; background-color: #6c757d; color: white; border:none; padding:6px 12px; border-radius:4px; cursor:pointer;">← Назад до списку</button>
                    
                    <h2>${video.title || 'Без назви'}</h2>
                    
                    <div class="video-player-wrapper" style="margin-bottom: 1rem; background: #000; border-radius: 8px; overflow: hidden;">
                        <video width="100%" controls style="display: block;">
                            <!-- video.urlOrPath береться з твоїх полів сутності в базі даних -->
                            <source src="${video.urlOrPath}" type="video/mp4">
                            Ваш браузер не підтримує тег відео.
                        </video>
                    </div>

                    <div class="video-info" style="background: #f8f9fa; padding: 15px; border-radius: 8px; margin-bottom: 1rem;">
                        <p><strong>Опис:</strong> ${video.description || 'Опис відсутній'}</p>
                        <p><strong>Автор:</strong> ${video.user || 'Не вказано'}</p>
                        <p><strong>Категорія:</strong> ${video.category || 'Не вказана'}</p>
                        <p><strong>Дата створення:</strong> ${new Date(video.createdAt).toLocaleDateString()}</p>
                        <p><strong>Кількість переглядів:</strong> ${video.viewCount || 0}</p>
                    </div>

                    <div class="video-actions">
                        <button onclick="app.aproveVideo(${video.id})" style="background-color: #28a745; color: white; border:none; padding:8px 16px; border-radius:4px; cursor:pointer; margin-right: 10px;">Затвердити</button>
                        <button onclick="app.delete(${video.id})" style="background-color: #dc3545; color: white; border:none; padding:8px 16px; border-radius:4px; cursor:pointer;">Видалити</button>
                    </div>
                </div>
            `;
        } catch (error) {
            this.contentContainer.innerHTML = `<p style="color: red;">Помилка: ${error.message}</p>`;
        }

    }

    async aproveVideo(id) {
        event.preventDefault();
        const response = await fetch(`${API_BASE_URL}/videos/${id}`, {
            method: "POST"
        });

        if (response.ok) {
            await this.navigateTo('videos');
        } else {
            alert('Помилка при затвердженні відео');
        }
    }
}

const app = new WebPolyOmniaApp();
window.app = app;