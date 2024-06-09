window.addEventListener("load", () => {
    const uri = document.getElementById("qrCodeData").getAttribute('data-url');
    new QRCode(document.getElementById("qrCode"),
        {
            text: uri,
            width: 180,
            height: 180,
            colorDark: "#000000",
            colorLight: "#ffffff",
            correctLevel: QRCode.CorrectLevel.L
        });
});