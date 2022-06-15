var token = $("input[name=__RequestVerificationToken]").val();

$(document).on("click", "#btnLogin", function () {
    var userId = $('.userId').val();
    var password = $('.password').val();

    if (userId == '') {
        var header = "Message";
        var type = "warning";

        msgBox("Please enter user id!", header, "", type);
    }
    else if (password == '') {
        var header = "Message";
        var type = "warning";

        msgBox("Please enter password!", header, "", type);
    }
    else {
        ShowLoadingOverlay();
        $.ajax({
            url: '/Login/Login',
            type: 'POST',
            data: { "__RequestVerificationToken": token, "userId": userId, "password": password },
            bDestroy: true,
            responsive: true,

            success: function (data) {
                if (data.result == "success") {
                    HideLoadingOverlay();
                    window.location.href = '/Dashboard/DashboardPage';
                }
                else {
                    HideLoadingOverlay();
                    var header = "Error";
                    var type = "error";

                    msgBox("Login fail.", header, "", type);
                }
            }
        });
    }

});



