export class VideoHelper {
    static isYouTubeUrl(url) {
        if (!url) return false;
        return url.includes("youtube.com") || url.includes("youtu.be");
    }

    static convertToEmbedUrl(url) {
        if (!url) return '';
        const regex = /(?:youtu\.be\/|youtube\.com\/(?:embed\/|v\/|watch\?v=|watch\?.+&v=))([\w-]{11})/i;
        const match = url.match(regex);
        return match && match[1] ? `https://www.youtube.com/embed/${match[1]}` : url;
    }

    static formatDate(dateString) {
        if (!dateString) return 'Дата відсутня';
        const date = new Date(dateString);
        return isNaN(date.getTime()) ? 'Дата відсутня' : date.toLocaleDateString('uk-UA');
    }
}