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
                return "";
            }
            else if (response.ok) {
                return response.text();
            }
        })
        .then(json => {
            if (JSON.parse(json).url) {
                document.getElementById("auth-error").style.display = "none";
                window.location.replace(JSON.parse(json).url);
            }

            if (json != undefined || json != "") {
                console.log();
                showToast(JSON.parse(json).error);
                document.getElementById("auth-error").innerText = JSON.parse(json).error;
                document.getElementById("auth-error").style.display = "block";
            }
        })
            .catch(error => {
        });
}