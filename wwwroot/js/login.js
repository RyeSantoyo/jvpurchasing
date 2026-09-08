const loginForm = document.getElementById("loginForm");

const loginButton = document.getElementById("loginButton");
const loginButtonText = document.getElementById("loginButtonText");
const loginSpinner = document.getElementById("loginSpinner");

const loginError = document.getElementById("loginError");
const loginErrorMessage = document.getElementById("loginErrorMessage");

const passwordInput = document.getElementById("password");
const togglePassword = document.getElementById("togglePassword");


/* =========================
   Password Toggle
========================= */

togglePassword.addEventListener("click", () => {

    const isPassword =
        passwordInput.type === "password";

    passwordInput.type =
        isPassword ? "text" : "password";

    togglePassword.innerHTML =
        isPassword
            ? '<i class="bi bi-eye-slash"></i>'
            : '<i class="bi bi-eye"></i>';

    togglePassword.setAttribute(
        "aria-label",
        isPassword
            ? "Hide password"
            : "Show password"
    );
});


/* =========================
   Login
========================= */

loginForm.addEventListener("submit", async (event) => {

    event.preventDefault();

    hideError();
    setLoading(true);


    const companyCode =
        document.getElementById("companyCode").value.trim();

    const username =
        document.getElementById("username").value.trim();

    const password =
        passwordInput.value;


    try {

        const response = await fetch("/api/Login/login", {

            method: "POST",

            headers: {
                "Content-Type": "application/json"
            },

            body: JSON.stringify({
                companyCode: companyCode,
                username: username,
                password: password
            })

        });


        if (!response.ok) {

            let message = "Unable to sign in.";

            try {

                const errorData =
                    await response.json();

                message =
                    errorData.message ||
                    message;

            } catch {

                // Ignore JSON parsing errors

            }

            throw new Error(message);
        }


            const token =
                await response.text();


            /*
            * Temporary token handling.
            *
            * We'll improve this once we decide
            * how authentication should work
            * throughout the application.
            */

            localStorage.setItem(
                "jwtToken",
                token
            );

            

        // Redirect after successful login
        window.location.href = "/Index";


    } catch (error) {

        showError(
            error.message ||
            "An unexpected error occurred."
        );

    } finally {

        setLoading(false);

    }

});


/* =========================
   Loading State
========================= */

function setLoading(isLoading) {

    loginButton.disabled = isLoading;

    loginButtonText.classList.toggle(
        "d-none",
        isLoading
    );

    loginSpinner.classList.toggle(
        "d-none",
        !isLoading
    );

}


/* =========================
   Error Handling
========================= */

function showError(message) {

    loginErrorMessage.textContent =
        message;

    loginError.classList.remove(
        "d-none"
    );

}


function hideError() {

    loginError.classList.add(
        "d-none"
    );

    loginErrorMessage.textContent =
        "";

}