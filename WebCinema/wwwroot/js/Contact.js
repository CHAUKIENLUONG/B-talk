document.addEventListener("DOMContentLoaded", () => {
    const name = document.getElementById("name");
    const email = document.getElementById("email");
    const message = document.getElementById("message");
    const submit = document.getElementById("submit");

    submit.addEventListener("click", (e) => {
        e.preventDefault(); // Ensure default form submission is prevented
        const data = {
            name: name.value,
            email: email.value,
            message: message.value,
        };
        if (!data.name || !data.email || !data.message) {
            alert("Vui lòng điền đầy đủ thông tin.");
        } else if (!data.email.includes('@')) {
            alert("Phải là địa chỉ Email!");
        } else {
            postGoogle(data);
        }
    });

    async function postGoogle(data) {
        const formURL = "https://docs.google.com/forms/d/e/1FAIpQLScPw_wcIqIfz7gqf6ULil8VD9yPft8N573hSK_R3c69uN8cDA/formResponse";
        const formData = new FormData();
        formData.append("entry.1094708025", data.name);
        formData.append("entry.706364320", data.email);
        formData.append("entry.228466318", data.message);

        try {
            await fetch(formURL, {
                method: "POST",
                body: formData,
                mode: "no-cors"
            });
            alert("Thông tin liên hệ đã được gửi.");
        } catch (error) {
            alert("Có lỗi xảy ra khi gửi thông tin.");
            console.error("Error:", error);
        }
    }
});