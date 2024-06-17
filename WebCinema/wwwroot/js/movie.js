document.addEventListener('DOMContentLoaded', function () {
    const trailerButtons = document.querySelectorAll('.open-popup-youtube');
    const popupContainer = document.getElementById('popup-container');
    const closeBtn = document.getElementById('close-btn');
    const trailerVideo = document.getElementById('trailer-video');

    trailerButtons.forEach(function (btn) {
        btn.addEventListener('click', function (event) {
            event.preventDefault();
            const trailerUrl = btn.getAttribute('data-trailer');
            const videoId = extractVideoId(trailerUrl); // Hàm để trích xuất ID video từ URL

            // Cập nhật src của iframe với URL nhúng từ YouTube tương ứng
            trailerVideo.src = `https://www.youtube.com/embed/${videoId}`;

            // Hiển thị popup với hiệu ứng fade
            popupContainer.classList.add('show');
        });
    });

    closeBtn.addEventListener('click', function () {
        // Ẩn popup với hiệu ứng fade
        popupContainer.classList.remove('show');
        // Đợi cho đến khi hiệu ứng fade kết thúc trước khi xóa src của iframe
        setTimeout(function () {
            trailerVideo.src = '';
        }, 500); // Thời gian khớp với transition duration trong CSS
    });

    // Đóng popup nếu người dùng nhấp vào bên ngoài nội dung popup
    window.addEventListener('click', function (event) {
        if (event.target === popupContainer) {
            // Ẩn popup với hiệu ứng fade
            popupContainer.classList.remove('show');
            // Đợi cho đến khi hiệu ứng fade kết thúc trước khi xóa src của iframe
            setTimeout(function () {
                trailerVideo.src = '';
            }, 500); // Thời gian khớp với transition duration trong CSS
        }
    });

    // Hàm để trích xuất ID video từ URL của YouTube
    function extractVideoId(url) {
        // Biểu thức chính quy để trích xuất ID video YouTube
        var regExp = /^(?:https?:\/\/)?(?:www\.)?(?:youtube\.com\/(?:[^\/\n\s]+\/\S+\/|(?:v|e(?:mbed)?)\/|\S*?[?&]v=)|youtu\.be\/)([^"&?\/\n]{11})/;
        var match = url.match(regExp);
        if (match && match[1]) {
            return match[1];
        } else {
            console.error('URL YouTube không hợp lệ');
            return '';
        }
    }
});
