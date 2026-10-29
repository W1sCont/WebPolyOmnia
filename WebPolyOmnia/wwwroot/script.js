import { VideoService } from './videoService.js';
import { CategoryService } from './categoryService.js';
import { UserService } from './userService.js';
import { VideoHelper } from './videoHelper.js';

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
        this.contentContainer.innerHTML = '<p class="text-center py-4 text-muted">Завантаження...</p>';
        try {
            if (view === 'categories') await this.renderCategoryList();
            else if (view === 'categories-form') await this.renderCategoryForm(id);
            else if (view === 'users') await this.renderUsersList();
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
        try {
            const categories = await CategoryService.getCategories();

            let html = `
            <div class="d-flex justify-content-between align-items-center mb-4">
                <h2>Список категорій</h2>
                <button onclick="app.navigateTo('categories-form')" class="btn btn-brand rounded-pill px-3">
                    <i class="bi bi-plus-lg me-1"></i> Додати категорію
                </button>
            </div>

            <div class="card poly-card border-secondary overflow-hidden shadow-sm">
                <div class="table-responsive mb-0">
                    <table class="table table-dark table-hover align-middle mb-0 border-secondary">
                        <thead>
                            <tr>
                                <th>Назва</th>
                                <th class="text-end pe-4">Дії</th>
                            </tr>
                        </thead>
                        <tbody>
        `;

            if (categories.length === 0) {
                html += `<tr><td colspan="2" class="text-center text-muted py-4">Категорії відсутні</td></tr>`;
            } else {
                categories.forEach(cat => {
                    html += `
                    <tr>
                        <td class="text-light fw-medium">${cat.name}</td>
                        <td class="text-end pe-4">
                            <button onclick="app.navigateTo('categories-form', ${cat.id})" class="btn btn-sm btn-outline-brand rounded-pill px-3 me-1">
                                <i class="bi bi-pencil"></i> Редагувати
                            </button>
                            <button onclick="app.deleteCategory(${cat.id})" class="btn btn-sm btn-outline-danger rounded-pill px-3">
                                <i class="bi bi-trash"></i> Видалити
                            </button>
                        </td>
                    </tr>
                `;
                });
            }

            html += `
                        </tbody>
                    </table>
                </div>
            </div>
        `;
            this.contentContainer.innerHTML = html;
        } catch (error) {
            this.contentContainer.innerHTML = `<p style="color: red;">Помилка: ${error.message}</p>`;
        }
    }

    async renderCategoryForm(id) {
        let category = { id: 0, name: '' };

        // Виправлено завантаження існуючої категорії за ID
        if (id) {
            try {
                const responce = await CategoryService.getCategoriesById(id);
                category = responce;
            } catch (e) {
                console.error('Помилка завантаження категорії:', e);
            }
        }
        console.log(category)

        this.contentContainer.innerHTML = `
            <div class="row justify-content-center my-4">
                <div class="col-12 col-md-8 col-lg-6">
                    <div class="card poly-card bg-dark text-white border-secondary p-4 p-sm-5 shadow-lg position-relative overflow-hidden">
                        
                        <div class="position-absolute top-0 start-0 end-0" style="height: 4px; background: linear-gradient(135deg, var(--electric-indigo) 0%, var(--bright-coral) 100%);"></div>

                        <div class="d-flex align-items-center mb-4">
                            <button type="button" onclick="app.navigateTo('categories')" class="btn btn-outline-secondary btn-sm rounded-pill px-3 me-3 text-light">
                                <i class="bi bi-arrow-left"></i>
                            </button>
                            <h2 class="fw-bold text-white mb-0 fs-3">${id ? 'Редагувати категорію' : 'Додати нову категорію'}</h2>
                        </div>

                        <!-- Додано передачу event у виклик методу saveCategory -->
                        <form onsubmit="app.saveCategory(event)" class="d-flex flex-column gap-3">
                            <input type="hidden" id="categoryId" value="${category.id || 0}">
                            
                            <div>
                                <label class="form-label text-light small fw-semibold">Назва категорії:</label>
                                <div class="input-group">
                                    <span class="input-group-text bg-secondary bg-opacity-25 border-secondary text-light">
                                        <i class="bi bi-tag-fill"></i>
                                    </span>
                                    <input type="text" id="categoryName" class="form-control form-control-poly py-2" value="${category.name || ''}" placeholder="Введіть назву категорії..." required>
                                </div>
                            </div>

                            <div class="d-flex gap-2 mt-3">
                                <button type="submit" class="btn btn-brand py-2 px-4 rounded-pill fw-semibold shadow-sm">
                                    <i class="bi bi-check-lg me-1"></i> Зберегти
                                </button>
                                <button type="button" onclick="app.navigateTo('categories')" class="btn btn-outline-secondary py-2 px-4 rounded-pill text-light border-secondary">
                                    Скасувати
                                </button>
                            </div>
                        </form>

                    </div>
                </div>
            </div>
        `;
    }

    async saveCategory(event) {
        event.preventDefault();
        console.log("saveCategory")
        const id = parseInt(document.getElementById('categoryId').value);

        const categoryDto = {
            id: isNaN(id) ? 0 : id,
            name: document.getElementById('categoryName').value
        };

        try {
            await CategoryService.saveCategory(categoryDto);
            this.navigateTo('categories');

        } catch (error) {
            console.error("Повна помилка:", error);
            this.contentContainer.innerHTML = `<p style="color: red;">Помилка: ${error.message}</p>`;
        }
    }

    async deleteCategory(id) {
        if (!confirm('Ви впевнені, що хочете видалити цю категорію?')) return;
        try {
            await CategoryService.deleteCategory(id);
            this.navigateTo('categories');
        } catch (error) {
            alert(error.message);
        }
    }

    // ==========================================
    // Users
    // ==========================================

    async renderUsersList() {
        try {
            const users = await UserService.getUsers();

            let html = `
            <div class="d-flex justify-content-between align-items-center mb-4">
                <h2>Список не затверджених користувачів</h2>
            </div>

            <div class="card poly-card border-secondary overflow-hidden shadow-sm">
                <div class="table-responsive mb-0">
                    <table class="table table-dark table-hover align-middle mb-0 border-secondary">
                        <thead>
                            <tr>
                                <th>Ім'я</th>
                                <th>Прізвище</th>
                                <th>Мейл</th>
                                <th>Логін</th>
                                <th>Дата створення</th>
                                <th>Роль</th>
                                <th class="text-end pe-4">Дії</th>
                            </tr>
                        </thead>
                        <tbody>
            `;

            if (users.length === 0) {
                html += `<tr><td colspan="7" class="text-center text-muted py-4">Користувачі відсутні</td></tr>`;
            } else {
                users.forEach(user => {
                    html += `
                    <tr>
                        <td class="text-light fw-medium">${user.name || ''}</td>
                        <td class="text-light fw-medium">${user.surname || ''}</td>
                        <td class="text-light fw-medium">${user.email || ''}</td>
                        <td class="text-light fw-medium">${user.login || ''}</td>
                        <td class="text-light fw-medium">${formatDate(user)}</td>
                        <td class="text-light fw-medium">${user.role || ''}</td>
                        <td class="text-end pe-4">
                            <button onclick="app.aproveUser(event, ${user.id})" class="btn btn-sm btn-outline-brand rounded-pill px-3 me-1">
                                <i class="bi bi-check-lg"></i> Затвердити
                            </button>
                            <button onclick="app.deleteUser(event, ${user.id})" class="btn btn-sm btn-outline-danger rounded-pill px-3">
                                <i class="bi bi-trash"></i> Видалити
                            </button>
                        </td>
                    </tr>
                `;
                });
            }

            html += `
                        </tbody>
                    </table>
                </div>
            </div>
            `;
            this.contentContainer.innerHTML = html;
        } catch (error) {
            this.contentContainer.innerHTML = `<p style="color: red;">Помилка: ${error.message}</p>`;
        }
    }

    async aproveUser(event, id) {
        if (event) event.preventDefault();
        try {
            await UserService.approveUser(id);
            this.navigateTo('users');
        } catch (error) {
            this.contentContainer.innerHTML = `<p style="color: red;">Помилка: ${error.message}</p>`;
        }
    }

    async deleteUser(event, id) {
        if (event) event.preventDefault();
        if (!confirm('Ви впевнені, що хочете видалити цього користувача?')) return;
        try {
            await UserService.deleteUser(id);
            this.navigateTo('users');
        } catch (error) {
            this.contentContainer.innerHTML = `<p style="color: red;">Помилка: ${error.message}</p>`;
        }
    }

    // ==========================================
    // Videos
    // ==========================================

    async renderVideoList() {
        try {
            const video = await VideoService.getUnapprovedVideos();

            let html = `
            <div class="d-flex justify-content-between align-items-center mb-4">
                <h2>Список не затверджених відео</h2>
            </div>

            <div class="card poly-card border-secondary overflow-hidden shadow-sm">
                <div class="table-responsive mb-0">
                    <table class="table table-dark table-hover align-middle mb-0 border-secondary">
                        <thead>
                            <tr>
                                <th>Назва</th>
                                <th>Автор</th>
                                <th class="text-end pe-4">Дії</th>
                            </tr>
                        </thead>
                        <tbody>
        `;

            if (video.length === 0) {
                html += `<tr><td colspan="3" class="text-center text-muted py-4">Відео відсутні</td></tr>`;
            } else {
                video.forEach(v => {
                    html += `
                    <tr>
                        <td class="text-light fw-medium">${v.title}</td>
                        <td class="text-light fw-medium">${v.user || 'Не вказано'}</td>
                        <td class="text-end pe-4">
                            <button onclick="app.navigateTo('videos-form', ${v.id})" class="btn btn-sm btn-outline-brand rounded-pill px-3 me-1">
                                <i class="bi bi-eye"></i> Детальніше
                            </button>
                            <button onclick="app.deleteVideo(event, ${v.id})" class="btn btn-sm btn-outline-danger rounded-pill px-3">
                                <i class="bi bi-trash"></i> Видалити
                            </button>
                        </td>
                    </tr>
                `;
                });
            }

            html += `
                        </tbody>
                    </table>
                </div>
            </div>
        `;
            this.contentContainer.innerHTML = html;
        } catch (error) {
            this.contentContainer.innerHTML = `<p style="color: red;">Помилка: ${error.message}</p>`;
        }
    }

    async renderVideo(id) {
        this.contentContainer.innerHTML = `
            <div class="text-center py-5">
                <div class="spinner-border text-primary" role="status"></div>
                <p class="text-muted mt-2">Завантаження відео...</p>
            </div>
        `;

        try {
            const video = await VideoService.getVideoById(id);
            const isYouTube = VideoHelper.isYouTubeUrl(video.urlOrPath);
            const finalUrl = isYouTube ? VideoHelper.convertToEmbedUrl(video.urlOrPath) : video.urlOrPath;

            let playerHtml = '';
            if (isYouTube) {
                playerHtml = `
                    <iframe src="${finalUrl}"
                            title="${video.title || 'YouTube video'}"
                            allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share"
                            allowfullscreen
                            class="w-100 h-100 border-0">
                    </iframe>`;
            } else {
                playerHtml = `
                    <video controls poster="${video.thumbnailUrlOrPath || ''}" class="w-100 h-100" style="object-fit: contain;">
                        <source src="${finalUrl}" type="video/mp4">
                        Ваш браузер не підтримує тег відео.
                    </video>`;
            }

            const authorInitial = video.user && typeof video.user === 'string' ? video.user.charAt(0).toUpperCase() : 'A';
            const authorName = video.user || 'Не вказано';
            const categoryName = video.category || 'Без категорії';

            this.contentContainer.innerHTML = `
                <div class="container-fluid px-lg-5 py-4">
                    <div class="mb-3">
                        <button onclick="app.navigateTo('videos')" class="btn btn-outline-secondary btn-sm rounded-pill px-3 text-light">
                            <i class="bi bi-arrow-left me-1"></i> Назад до списку
                        </button>
                    </div>

                    <div class="row g-4">
                        <div class="col-lg-8 col-xl-9">
                            <div class="ratio ratio-16x9 bg-black rounded-3 border border-secondary shadow-lg overflow-hidden mb-3">
                                ${playerHtml}
                            </div>

                            <div class="card bg-dark text-white border-secondary p-4 shadow-sm rounded-3">
                                <h5 class="fw-semibold text-light mb-2 fs-6 text-uppercase tracking-wider">
                                    Опис відео
                                </h5>
                                <p class="text-secondary fs-6 lh-base mb-0">
                                    ${video.description || 'Опис відсутній'}
                                </p>
                            </div>
                        </div>

                        <div class="col-lg-4 col-xl-3">
                            <div class="d-flex flex-column gap-3 sticky-top" style="top: 20px;">
                                
                                <div class="card bg-dark text-white border-secondary p-3 shadow-sm rounded-3">
                                    <div class="d-flex align-items-center gap-2 mb-2 flex-wrap">
                                        <span class="badge bg-primary px-2 py-1 fs-8 rounded-pill">
                                            <i class="bi bi-tag-fill me-1"></i> ${categoryName}
                                        </span>
                                        <span class="text-muted fs-7">
                                            <i class="bi bi-eye me-1"></i> ${video.viewCount || 0}
                                        </span>
                                    </div>
                                    <h1 class="fw-bold text-white fs-4 mb-0">${video.title || 'Без назви'}</h1>
                                </div>

                                <div class="card bg-dark text-white border-secondary p-3 shadow-sm rounded-3">
                                    <div class="d-flex align-items-center gap-3">
                                        <div class="avatar-circle bg-primary text-white d-flex align-items-center justify-content-center fw-bold fs-5 rounded-circle shadow-sm" style="width: 45px; height: 45px;">
                                            ${authorInitial}
                                        </div>
                                        <div class="overflow-hidden">
                                            <h6 class="mb-0 fw-bold text-white fs-6 text-truncate">${authorName}</h6>
                                            <small class="text-muted fs-7">Автор контенту</small>
                                        </div>
                                    </div>
                                </div>

                                <div class="card bg-dark text-white border-secondary p-3 shadow-sm rounded-3 d-flex flex-column gap-2">
                                    <button onclick="app.aproveVideo(${video.id})" class="btn btn-brand w-100 d-flex align-items-center justify-content-center gap-2 py-2 fs-7 fw-semibold">
                                        <i class="bi bi-check-circle"></i> Затвердити відео
                                    </button>
                                    <button onclick="app.deleteVideo(event, ${video.id})" class="btn btn-outline-danger w-100 d-flex align-items-center justify-content-center gap-2 py-2 fs-7">
                                        <i class="bi bi-trash"></i> Видалити
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            `;
        } catch (error) {
            this.contentContainer.innerHTML = `<div class="alert alert-danger m-4">Помилка: ${error.message}</div>`;
        }
    }

    async aproveVideo(id) {
        try {
            const response = await VideoService.approveVideo(id);
            if (response.ok || response.status === 200 || response.status === 204) {
                this.navigateTo('videos');
            } else {
                alert('Помилка при затвердженні відео');
            }
        } catch (error) {
            this.contentContainer.innerHTML = `<p style="color: red;">Помилка: ${error.message}</p>`;
        }
    }

    async deleteVideo(event, id) {
        if (event) event.preventDefault();
        if (!confirm('Ви впевнені, що хочете видалити це відео?')) return;
        try {
            await VideoService.deleteVideo(id);
            this.navigateTo('videos');
        } catch (error) {
            this.contentContainer.innerHTML = `<p style="color: red;">Помилка: ${error.message}</p>`;
        }
    }
}

window.app = new WebPolyOmniaApp();

// Допоміжна функція для безпечного виведення дати
function formatDate(item) {
    const rawDate = item.createdAt || item.CreatedAt || item.dateCreated || item.DateCreated;
    if (!rawDate) return 'Дата відсутня';
    const date = new Date(rawDate);
    return isNaN(date.getTime()) ? 'Дата відсутня' : date.toLocaleDateString('uk-UA');
}