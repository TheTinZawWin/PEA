function msgBox(msg, header, btn, type) {
    //default
    //console.log(header);
    var vheader = header;
    if (header === "") { vheader = "Message"; }
    if (btn === "") { btn = '<button class="btn btn-min btn-primary" type="button" data-dismiss="modal">Ok</button>'; }
    var icon = "<i class='fa fa-info-circle fa-stack-3x text-info'></i>";

    //Set Icon
    if (type.toLowerCase() === "warning") {
        if (header === "") { vheader = "Warning"; }
        icon = "<i class='fa fa-warning fa-stack-3x text-primary'></i>";
    }
    if (type.toLowerCase() === "confirm") {
        if (header === "") { vheader = "Confirmation"; }
        icon = "<i class='fa fa-question-circle fa-stack-3x text-primary'></i>";
    }
    if (type.toLowerCase() === "error") {
        if (header === "") { vheader = "Error"; }
        icon = "<i class='fa fa-exclamation fa-stack-3x text-danger'></i>";
    }

    $('#ModalHeader').html(icon + '&nbsp;&nbsp;' + vheader);
    $('#ModalBody').html($.parseHTML(msg));
    $('#ModalFooter').html(btn);

    $(document).ready(function () {
        $('#MyModel').modal('show');
    });
    return false;
}


$(document).on('click', '#createANewEmployee', function () {
    $('#Modal-createEmployee').modal('show');
});


$(document).on('click', '#btnLogout', function () {
    var header = "Confirmation";
    var type = "confirm";
    var btn = '<button id="btnLogoutConfirm" class="btn btn-min btn-primary" type="button" data-dismiss="modal">Yes</button>';
    btn = btn + '<button class="btn btn-min btn-secondary" type="button" data-dismiss="modal">No</button>';
    msgBox("Are you sure you want to logout?", header, btn, type);
});

$(document).on('click', '#btnLogoutConfirm', function () {
    $('#MyModel').on('hidden.bs.modal', function (e) {
        $.post("/Login/ClearSSO", { "__RequestVerificationToken": token }, function (data) {
            window.location = '/Login/Login';
        });
    });
});

var myVar;
function ShowLoadingOverlay() {
    console.log('ShowLoadingOverlay');
    $("#loading-overlay").removeClass("d-none");
    $("body").addClass("hideCursor");
    clearTimeout(myVar);
    myVar = setTimeout(function () {
        HideLoadingOverlay();
    }, 30000);
}

function HideLoadingOverlay() {
    setTimeout(function () {
        $("#loading-overlay").addClass("d-none");
        $("body").removeClass("hideCursor");
    }, 50);
    clearTimeout(myVar);
}