(function () {
    emailjs.init("NKaEqYATtE-LtFLAr"); // Replace 'user_youruserid' with your user ID from EmailJS

    document.getElementById('contact-form').addEventListener('submit', function (event) {
        event.preventDefault();
        // this is the ID of your form
        const formData = {
            name: this.name.value,
            email: this.email.value,
            message: this.message.value,
            from_name: this.name.value,
            from_mail: this.email.value,
        };

        // Change 'your_service_id', 'your_template_id', and 'user_youruserid' with your own values
        emailjs.send('service_r1s888n', 'template_8im6bag', formData)
            .then(function (response) {
                console.log('Success!', response.status, response.text);
                alert('Your message has been sent successfully!');
            }, function (error) {
                console.error('Failed...', error);
                alert('Oops! Something went wrong. Please try again later.');
            });
    });
})();
