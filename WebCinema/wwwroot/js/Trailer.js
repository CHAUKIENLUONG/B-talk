document.addEventListener('DOMContentLoaded', function () {
    var modal = document.getElementById('trailerModal');
    var iframe = document.getElementById('trailerIframe');
    var span = document.getElementsByClassName('close')[0];

    document.querySelectorAll('.view-trailer').forEach(function (button) {
        button.addEventListener('click', function (event) {
            event.preventDefault();  // Ngăn chặn hành động mặc định của thẻ a
            var trailer = this.getAttribute('data-trailer');
            // Kiểm tra và chuyển đổi URL thành định dạng nhúng nếu cần
            if (trailer.startsWith("https://www.youtube.com/watch?v=")) {
                let videoId = trailer.split("v=")[1];
                trailer = "https://www.youtube.com/embed/" + videoId;
            }
            else if (!trailer.startsWith("https://www.youtube.com/embed/")) {
                trailer = "https://www.youtube.com/embed/" + trailer;
            }
            iframe.src = trailer;
            modal.style.display = 'flex';// Hiển thị modal
        });
    });

    span.onclick = function () {
        modal.style.display = 'none';
        iframe.src = '';
    };

    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = 'none';
            iframe.src = '';
        }
    };
});
