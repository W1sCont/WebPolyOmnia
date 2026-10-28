const API_BASE_URL = 'https://localhost:7175/api';
export class VideoService {
    static async getUnapprovedVideos() {
        const response = await fetch(`${API_BASE_URL}/videos`);
        if (!response.ok) throw new Error('Не вдалося завантажити відео');
        return await response.json();
    }

    static async getVideoById(id) {
        const response = await fetch(`${API_BASE_URL}/videos/${id}`);
        if (!response.ok) throw new Error('Відео не знайдено');
        return await response.json();
    }

    static async approveVideo(id) {
        return await fetch(`${API_BASE_URL}/videos/${id}`, { method: 'PUT' });
    }

    static async deleteVideo(id) {
        return await fetch(`${API_BASE_URL}/videos/${id}`, { method: 'DELETE' });
    }
}