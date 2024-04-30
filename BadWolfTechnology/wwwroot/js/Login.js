function loginAuth() {
    

    var formData = new FormData(document.getElementById("account-login"));
    formData.append("returnPathName", window.location.pathname);
    formData.set("RememberMe", document.getElementById("RememberMe").checked);

    fetch(`${window.location.origin}/Account/Login`, {
        method: "POST",
        body: formData,
    })
    .then(response => {
        if (response.redirected) {
            document.getElementById("auth-error").style.display = "none";
            window.location.replace(response.url);
        }
        else if (response.ok) {
            return response.text();
        }
    })
    .then(json => {
        if (json === undefined) {
            return;
        }
        showToast(JSON.parse(json).error);
        document.getElementById("auth-error").innerText = JSON.parse(json).error;
        document.getElementById("auth-error").style.display = "block";
    })
    .catch(error => {
        showToast("Неверный логин или пароль.");
        document.getElementById("auth-error").innerText = "Неверный логин или пароль.";
        document.getElementById("auth-error").style.display = "block";
    });
}