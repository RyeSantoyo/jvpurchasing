const notify = {
    show: function (message, type = "info", duration = 3000) {
        let background;

        switch (type) {
            case "success":
                background = "linear-gradient(to right, #00b09b, #96c93d)"; // Green
                break;
            case "error":
                background = "linear-gradient(to right, #ff5f6d, #ffc371)"; // Red/Orange
                break;
            case "warning":
                background = "linear-gradient(to right, #f8b500, #fceabb)"; // Yellow/Amber
                break;
            case "info":
            default:
                background = "linear-gradient(to right, #2193b0, #6dd5ed)"; // Blue
                break;
        }

        Toastify({
            text: message,
            duration: duration,
            close: true,
            gravity: "top", // 'top' or 'bottom'
            position: "right", // 'left', 'center' or 'right'
            stopOnFocus: true, // Prevents dismissing of toast on hover
            style: {
                background: background,
                borderRadius: "6px",
                fontSize: "14px"
            }
        }).showToast();
    },

    success: function (message, duration) {
        this.show(message, "success", duration);
    },

    error: function (message, duration) {
        this.show(message, "error", duration);
    },

    warning: function (message, duration) {
        this.show(message, "warning", duration);
    },

    info: function (message, duration) {
        this.show(message, "info", duration);
    }
};