async function loginAuth() {
    const loginError = "Ошибка входа.";

    var formData = new FormData(document.getElementById("account-login"));
    formData.append("returnPathName", window.location.pathname);
    formData.set("RememberMe", document.getElementById("RememberMe").checked);

    try {
        // Send a POST request to /Account/Login
        const response = await fetch('/Account/Login', {
            method: 'POST',
            body: formData
        });

        const contentType = response.headers.get('Content-Type') || '';
        if (contentType.includes('application/json')) {

            const result = await response.json();

            if (result.error) {
                // Если авторизация не удалась, вывести сообщение об ошибке.
                showError(result.error);
            }
        } else if (response.ok) {
            // При успешной авторизации, производит перенаправление пользователя.
            document.getElementById("auth-error").style.display = "none";
            window.location.href = response.url;
        } else {
            // Если авторизация не удалась, вывести сообщение об ошибке.
            showError(loginError);
        }
    } catch (error) {
        showError(loginError);
    }
}

function showError(errorText) {
    // Если авторизация не удалась, вывести сообщение об ошибке.
    showToast(errorText);
    document.getElementById("auth-error").innerText = errorText;
    document.getElementById("auth-error").style.display = "block";
}
