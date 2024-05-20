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
        const formURL = "https://docs.google.com/forms/d/e/1FAIpQLSdiWqXbxQHFeaKlp7nT5DyyVtPoVgRiEwoDHZYwqePxI7YJXQ/formResponse";
        const formData = new FormData();
        formData.append("entry.877646087", data.name);
        formData.append("entry.1667317933", data.email);
        formData.append("entry.1983709190", data.message);

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