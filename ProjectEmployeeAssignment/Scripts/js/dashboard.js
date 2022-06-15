
var token = $("input[name=__RequestVerificationToken]").val();
//variable for module level
var mPagingSelAll = [];
var mEMPT_ID;
var mEMPT_UPD_ID;

//load data after page is ready
$(document).ready(function () {

    $('.employeedate').each(function () {
        console.log(new Date());
        $(this).datepicker({
            uiLibrary: 'bootstrap4',
            format: 'dd mmm yyyy',
            iconsLibrary: 'fontawesome',
            value: $.datepicker.formatDate('dd M yy', new Date()),
            minDate: $.datepicker.formatDate('dd M yy', new Date()),
            maxDate: $.datepicker.formatDate('dd M yy', new Date((new Date).setMonth((new Date).getMonth() + 1)))
        });
    });
    GetData();
});





//onchange event
// Handle click on "Select all" control
$('#select-all').on('click', function () {

    var table = $('#tblEmployee').DataTable();
    var info = table.page.info();

    if ($('#select-all').is(":checked")) {

        mPagingSelAll.push(info.page);
    }
    else {
        mPagingSelAll = jQuery.grep(mPagingSelAll, function (value) {
            return value != info.page;
        });
    }

    // Get all rows with search applied
    //var rows = table.rows({ 'search': 'applied' }).nodes();

    var rows = table.rows({ page: 'current' }).nodes();

    // Check/uncheck checkboxes for all rows in the table
    $('input[type="checkbox"]', rows).prop('checked', this.checked);
    //$('#check').prop('checked', this.checked);
});


$('#tblEmployee').on('page.dt', function () {
    var table = $('#tblEmployee').DataTable();

    var info = table.page.info();
    //alert('Showing page: ' + info.page + ' of ' + info.pages);
    if (jQuery.inArray(info.page, mPagingSelAll) >= 0) {
        $('#select-all').prop('checked', true);
    }
    else {
        $('#select-all').prop('checked', false);
    }

});

//end onchange event





//click event

$(document).on('click', '#createEmployee', function () {
    $('#Modal-createEmployee').modal('show');
});

$(document).on('click', '#btnCreateEmployeeCancel', function () {
    $('#Modal-createEmployee').modal('hide');
});

$(document).on("click", "#btnCreateEmployeeConfirm", function () {
    var checkActive = false;
    if ($('#checkActive').is(':checked')) {
        checkActive = true;
    }
    else {
        checkActive = false;
    }

    var employeeid = $('#employeeId').val();
    var name = $('#name').val();
    var password = $('#password').val();
    var confirmpassword = $('#confirmpassword').val();
    var email = $('#email').val();
    var phoneno = $('#phoneno').val();
    var joinDate = $('#joinDate').val();
    var department = $('#department').find(":selected").val();
    if (ValidationForEmployeeData(employeeid, name, password, confirmpassword,email,phoneno,department)) {
        ShowLoadingOverlay();
        $.ajax({
            url: '/Employee/createEmployee',
            type: 'POST',
            data: { "__RequestVerificationToken": token, "empId": employeeid, "name": name, "email": email, "phone": phoneno, "departmentId": department, "joindate": joinDate, "password": password},
            bDestroy: true,
            responsive: true,
            language: { loadingRecords: "<i class='fa fa-spinner fa-spin'></i> Loading data..." },
            success: async function (data) {

                if (data.result == "success") {
                    HideLoadingOverlay();
                    $('#createTitle').val("");
                    $('#Modal-createEmployee').modal('hide');
                    await GetEmployeeList();
                   
                    var header = "Meassage";
                    var type = "message";

                    msgBox("Create Employee Success.", header, "", type);

                }
                else {
                    HideLoadingOverlay();
                    var header = "Error";
                    var type = "error";

                    msgBox("Create Employee Fail.", header, "", type);
                }
            }
        });
    }
   
});


//to uncheck select all button and now click id
$(document).on('click', '.chkEmployeeID', function () {
    var id = this.id;
    if ($('#' + id).is(':checked')) {
        $("#" + id).prop("checked", false);
        $("#select-all").prop("checked", false);
    }
    else {
        $("#" + id).prop("checked", true);
    }

    if (!$(this).checked) {
        var table = $('#tblEmployee').DataTable();
        var info = table.page.info();
        if (jQuery.inArray(info.page, mPagingSelAll) >= 0) {
            mPagingSelAll = jQuery.grep(mPagingSelAll, function (value) {
                return value != info.page;
            });
            $('#select-all').prop('checked', false);
        }
    }
});

//to select now click id
$(document).on('click', '.rowcheck', function () {
    var resourceid = $(this).data("id");
    if ($('#' + resourceid).is(':checked')) {
        $("#" + resourceid).prop("checked", false);
        $("#select-all").prop("checked", false);
    }
    else {
        $("#" + resourceid).prop("checked", true);
    }

    var EmployeeID = [];
    EmployeeID.push({
        mEMPT_ID: resourceid
    });

    if (!$(this).checked) {
        var table = $('#tblEmployee').DataTable();
        var info = table.page.info();
        if (jQuery.inArray(info.page, mPagingSelAll) >= 0) {
            mPagingSelAll = jQuery.grep(mPagingSelAll, function (value) {
                return value != info.page;
            });
            $('#select-all').prop('checked', false);
        }
    }
});


//click update button from list
$(document).on('click', '#btnEmployeeUpdate', function () {
    var array = [];

    var tabel = $('#tblEmployee').DataTable().rows().nodes().to$().find('input[type="checkbox"]:checked');
    //if (tabel.length == 0) {
    //    DisplayNotification("3019", "", "", "", "Resource(s)");
    //}
    tabel.each(function () {
        var row = $(this).closest("tr")[0];
        var uniqueId = $(row.cells[0].innerHTML).val();
        if ((jQuery.inArray(uniqueId, array)) == -1 && uniqueId != "") {
            array.push(uniqueId);

        }
    });
    console.log(array);
    if (array.length < 1) {
        msgBox("Please select at least one Employee", "warning", "", "");
    }
    else if (array.length > 1) {
        msgBox("Please select only one Employee", "warning", "", "");
    }
    else {
        GetEmployeeById(array[0]);
        mEMPT_UPD_ID = array[0];
    }
});

//click update button from updatepage
$(document).on('click', '#btnupdateEmployee', function () {
    var updatepriorityCheck = false;
    if ($('#updatepriority').is(':checked')) {
        updatepriorityCheck = true;
    }
    else {
        updatepriorityCheck = false;
    }
    var updateDate = $('#updateDate').val();
    var updateTitle = $('#updateTitle').val();
    if (updateDate == undefined || updateDate == "" || updateTitle == undefined || updateTitle == "") {
        var header = "Error";
        var type = "error";

        msgBox("All field are required.", header, "", type);
    }
    else {
        var header = "Confirmation";
        var type = "confirm";
        var btn = '<button id="btnUpdateEmployeeConfirm" class="btn btn-min btn-primary" type="button" data-dismiss="modal">Yes</button>';
        btn = btn + '<button class="btn btn-min btn-secondary" type="button" data-dismiss="modal">No</button>';
        msgBox("Are you sure you want to update this Employee?", header, btn, type);
    }

});
// cancel button for update
$(document).on('click', '#btnUpdateEmployeeCancel', function () {
    $('#Modal-updateEmployee').modal('hide');
});
//confirm button for update
$(document).on("click", "#btnUpdateEmployeeConfirm", function () {
    var checkActive = false;
    if ($('#updatecheckActive').is(':checked')) {
        checkActive = true;
    }
    else {
        checkActive = false;
    }

    var employeeid = $('#updateemployeeId').val();
    var name = $('#updatename').val();
    var password = $('#updatepassword').val();
    var confirmpassword = $('#updateconfirmpassword').val();
    var email = $('#updateemail').val();
    var phoneno = $('#updatephoneno').val();
    var joinDate = $('#updatejoinDate').val();
    var department = $('#updatedepartment').find(":selected").val();
    if (ValidationForEmployeeData(employeeid, name, password, confirmpassword, email, phoneno, department)) {
        ShowLoadingOverlay();
        ShowLoadingOverlay();
        $.ajax({
            url: '/Employee/updateEmployee',
            type: 'POST',
            data: { "__RequestVerificationToken": token, "id": mEMPT_UPD_ID, "empId": employeeid, "name": name, "email": email, "phone": phoneno, "departmentId": department, "joindate": joinDate, "password": password, "checkActive": checkActive },
            bDestroy: true,
            responsive: true,
            language: { loadingRecords: "<i class='fa fa-spinner fa-spin'></i> Loading data..." },
            success: async function (data) {

                if (data.result == "success") {
                    HideLoadingOverlay();

                    $('#Modal-updateEmployee').modal('hide');
                    await GetEmployeeList();
                    await GetAllDataCount();
                    var header = "Meassage";
                    var type = "message";

                    msgBox("Update Employee Success.", header, "", type);

                }
                else {
                    HideLoadingOverlay();
                    var header = "Error";
                    var type = "error";

                    msgBox("Update Employee Fail.", header, "", type);
                }
            }
        });
    }

});

//click delete button from list
$(document).on('click', '#btnEmployeeDelete', function () {
    var array = [];

    var tabel = $('#tblEmployee').DataTable().rows().nodes().to$().find('input[type="checkbox"]:checked');
    //if (tabel.length == 0) {
    //    DisplayNotification("3019", "", "", "", "Resource(s)");
    //}
    tabel.each(function () {
        var row = $(this).closest("tr")[0];
        var uniqueId = $(row.cells[0].innerHTML).val();
        if ((jQuery.inArray(uniqueId, array)) == -1 && uniqueId != "") {
            array.push(uniqueId);

        }
    });
    console.log(array);
    if (array.length < 1) {
        msgBox("Please select at least one employee", "warning", "", "");
    }
    else {
        var header = "Confirmation";
        var type = "confirm";
        var btn = '<button id="btnConfirmDelete" class="btn btn-min btn-primary" type="button" data-dismiss="modal">Yes</button>';
        btn = btn + '<button class="btn btn-min btn-secondary" type="button" data-dismiss="modal">No</button>';
        msgBox("Are you sure you want to delete this Employee?", header, btn, type);
    }
});
//confrim delete
$(document).on('click', "#btnConfirmDelete", function () {
    var array = [];

    var tabel = $('#tblEmployee').DataTable().rows().nodes().to$().find('input[type="checkbox"]:checked');
    //if (tabel.length == 0) {
    //    DisplayNotification("3019", "", "", "", "Resource(s)");
    //}
    tabel.each(function () {
        var row = $(this).closest("tr")[0];
        var uniqueId = $(row.cells[0].innerHTML).val();
        if ((jQuery.inArray(uniqueId, array)) == -1 && uniqueId != "") {
            array.push(uniqueId);

        }
    });
    ShowLoadingOverlay();
    $.ajax({
        url: '/Employee/EmployeeDelete',
        type: 'POST',
        dataType: 'json',
        data: { EMT_ID_LIST: array, "__RequestVerificationToken": token },
        bDestroy: true,
        responsive: true,
        language: { loadingRecords: "<i class='fa fa-spinner fa-spin'></i> Loading data..." },
        success: async function (data) {
            HideLoadingOverlay();
            if (data.result == 'success') {
               
                await GetEmployeeList();
           
                await GetAllDataCount();
                $("#select-all").prop("checked", false);
                var header = "Message";
                var type = "message";

                msgBox("Employee Delete  Successful.", header, "", type);
            }

            else if (data.result =='alreadyinactive'){
                var header = "Warning";
                var type = "warning";

                msgBox("Please Select Only Active User.", header, "", type);
            }

        },
        error: function (xhr) {
            HideLoadingOverlay();
            alert(xhr);
            //msgBox(xhr.responseText, header, btn, type)
        }
    });
});




//end click event





//function region

//get employee list, count and department data
async function GetData() {
    ShowLoadingOverlay();
   await GetEmployeeList();
   // await GetFinishedTaskList();
   await GetAllDataCount();
    GetDepartmentList("department",'');
    HideLoadingOverlay();
}

//get employee list
function GetEmployeeList() {
    $("#tblEmployee").DataTable({
        "ajax": {
            "url": '/Employee/GetEmployeeList',
            "type": "POST",
            "data": { "__RequestVerificationToken": token}
        },
        "bDestroy": true,
        "responsive": true,
        "columnDefs": [

            {
                targets: [0], 'searchable': false, 'orderable': false,
                "createdCell": function (td, cellData, rowData, row, col) {
                    $(td).addClass('left');
                },
                render: function (data, type, row, meta) {
                    if (data) {
                        return '<input type="checkbox" id="' + $('<div/>').text(data).html() + '" data-Employeeid="' + $('<div/>').text(data).html() + '" class="chkEmployeeID move-left "  value="' + $('<div/>').text(data).html() + '">';
                    }
                    else {
                        return '';
                    }
                }
            },

            {
                "targets": [9],
                "createdCell": function (td, cellData, rowData, row, col) {
                    $(td).addClass('right');
                }
            }

        ],
        "order": [1, 'asc'],
        "language": {
            "loadingRecords": "<i class='fa fa-spinner fa-spin'></i> Loading data...",
            "sEmptyTable": "Employee List is Empty"
        },
        "rowCallback": function (row, data, index) {

            $(row).addClass("rowcheck");
            $(row).attr('data-id', data[0]);
        },
        "bPaginate": true,
        "deferRender": true,
        "lengthMenu": [[10, 50, 100, 500, 5000, -1], [10, 50, 100, 500, 5000, "All"]],
        "fnInitComplete": function () {

            $(document).ready(function () {
                // HideLoadingOverlay();
            });
        },
        drawCallback: function () {
            var pagination = $(this).closest('.dataTables_wrapper').find('.dataTables_paginate');
            pagination.toggle(this.api().page.info().pages > 1);
        }
    });
}
//validation for input parameter
function ValidationForEmployeeData(employeeid, name, password, confirmpassword, email, phoneno, department) {
    if (employeeid == '') {
        msgBox("Employee Id cannot be empty.", "warning", "", "");
        return false;
    }
    else if (name == '') {
        msgBox("Name cannot be empty.", "warning", "", "");
        return false;
    }
    else if (password == '') {
        msgBox("Password cannot be empty.", "warning", "", "");
        return false;
    }
    else if (confirmpassword == '') {
        msgBox("ConfirmPassword cannot be empty.", "warning", "", "");
        return false;
    }
    else if (password != confirmpassword) {
        msgBox("Password and confirm password are not same.", "warning", "", "");
        return false;
    }

    else if (email == '') {
        msgBox("Email cannot be empty.", "warning", "", "");
        return false;
    }

    else if (phoneno == '') {
        msgBox("Phone No cannot be empty.", "warning", "", "");
        return false;
    }
    else if (department == 'noselected') {
        msgBox("Please select department name.", "warning", "", "");
        return false;
    }

    var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

    if (!filter.test(email)) {
        msgBox("Please enter valid email.", "warning", "", "");
        return false;
    }

    return true;
}
//get data count
function GetAllDataCount() {
    $.ajax({
        url: '/Employee/GetAllDataCount',
        type: 'POST',
        dataType: 'json',
        data: { "__RequestVerificationToken": token },
        bDestroy: true,
        responsive: true,
        language: { loadingRecords: "<i class='fa fa-spinner fa-spin'></i> Loading data..." },
        success: async function (data) {
            if (data.result == 'success') {
                $("#activeCount").text(data.activeCount);
                $("#inactiveCount").text(data.inactiveCount);
              
            }
            else {

            }

        },
        error: function (xhr) {
            //  HideLoadingOverlay();
            alert(xhr);
            //msgBox(xhr.responseText, header, btn, type)
        }
    });
}
//get employee data with id for only one to update data
function GetEmployeeById(id) {
    $.ajax({
        url: '/Employee/GetEmployeeById',
        type: 'POST',
        dataType: 'json',
        data: { "__RequestVerificationToken": token, "id": id },
        bDestroy: true,
        responsive: true,
        language: { loadingRecords: "<i class='fa fa-spinner fa-spin'></i> Loading data..." },
        success: async function (data) {
            if (data.result == 'success') {
                GetDepartmentList("updatedepartment", data.empt.Department);
                $('#updateemployeeId').val(data.empt.EmployeeID);
                $('#updatename').val(data.empt.Name);
               
                $('#updateemail').val(data.empt.Email);
                $('#updatephoneno').val(data.empt.Phone);
                $('#updatejoinDate').val(data.empt.JointDate);
                if (data.empt.Active == true) {
                    $("#updatecheckActive").prop("checked", true);
                }
                else {
                    $("#updatecheckActive").prop("checked", false);
                }
                $('#Modal-updateEmployee').modal('show');
            }
            
            else if (data.result == 'unauthorized') {
                var header = "Warning";
                var type = "Warning";

                msgBox("Only Admin 1 and Owner can update employee information.", header, "", type);
            }


        },
        error: function (xhr) {
            //  HideLoadingOverlay();
            alert(xhr);
            //msgBox(xhr.responseText, header, btn, type)
        }
    });
}
//get department data list for drop down list
function GetDepartmentList(ctrID, defaultValue) {
    $('#' + ctrID).empty();
    $('#' + ctrID).append('<option value="noselected" selected>Select Department</option>');
    $.ajax({
        url: '/Department/GetDepartmentList',
        type: 'POST',
        dataType: 'json',
        data: { "__RequestVerificationToken": token},
        success: function (data) {

            $(data).each(function (index, retData) {
                if (retData.ID === defaultValue) {
                    $('#' + ctrID).append('<option selected="selected" value="' + retData.ID + '">' + retData.NAME + '</option>');
                }
                else {
                    $('#' + ctrID).append('<option value="' + retData.ID + '">' + retData.NAME + '</option>');
                }
            });
        },
        error: function (xhr) {
        }
    });
}
//end region

