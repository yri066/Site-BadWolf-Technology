// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// Получить отформатированное время в текущем часовом поясе
function localDate(date, spacer = "-") {
    var localDate = new Date(date);

    // Формат даты по умолчанию "dd-MM-yyyy H:mm"
    var formattedDate = ("0" + localDate.getDate()).slice(-2) + spacer +
        ("0" + (localDate.getMonth() + 1)).slice(-2) + spacer +
        localDate.getFullYear() + " " +
        ("0" + localDate.getHours()).slice(-2) + ":" +
        ("0" + localDate.getMinutes()).slice(-2);

    return formattedDate;
}

// Вывести уведомление о ошибке

let timeoutShowToastMessage;
function showToast(text) {
    if (timeoutShowToastMessage === undefined) {

        let toast = document.getElementById("snackbar");
        toast.className = "show";
        toast.textContent = text;
        timeoutShowToastMessage = setTimeout(function () {
            toast.className = "";
            clearTimeout(timeoutShowToastMessage);
            timeoutShowToastMessage = undefined;
        }, 3000);
    }
}