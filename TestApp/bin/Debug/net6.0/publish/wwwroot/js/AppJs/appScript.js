function saveEmployees() {
    //if ($('#frmSaveEmployee').valid() != true) {
    //    return;
    //}
    $.ajax({
        url: '/Employee/SaveEmployee',
        type: 'POST',
        data: $("#frmSaveEmployee").serialize(),
        success: function (data) {
            //alert('Data: ' + data);
            $("#btnAddEmp").show();
            $("#_divList").html(data);
            $('#tblEmpList').DataTable();
            $('#alertMSG').show();
            HideAlert();
        },
        error: function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}

function addEmployees(id) {
    var data = { id: id }
    $.ajax({
        url: '/Employee/AddEmployee',
        type: 'GET',
        data: data,
        success: function (data) {
            //alert('Data: ' + data);
            $("#btnAddEmp").hide();
            $("#_divList").html(data);
            $('#tblEmpList').DataTable();
            $('#alertMSG').show();
            HideAlert();
        },
        error: function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}

function deleteEmployees(id) {
    var data = { id: id }
    $.ajax({
        url: '/Employee/DeleteEmployee',
        type: 'POST',
        data: data,
        success: function (data) {
           alert('Employee Record is deleted: ' + id);
            $("#btnAddEmp").show();
            $("#_divList").html(data);
            $('#tblEmpList').DataTable();
            $('#alertMSG').show();
            HideAlert();
        },
        error: function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}

function stopJobs() {
    
    $.ajax({
        url: '/Employee/Stop',
        type: 'POST',
        success: function (data) {
            alert('Employee Record is deleted: ' + id);
            $("#btnAddEmp").show();
            $("#_divList").html(data);
            $('#tblEmpList').DataTable();
            $('#alertMSG').show();
            HideAlert();
        },
        error: function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}

function startJobs() {

    $.ajax({
        url: '/Employee/Start',
        type: 'POST',
        success: function (data) {
            alert('Employee Record is deleted: ' + id);
            $("#btnAddEmp").show();
            $("#_divList").html(data);
            $('#tblEmpList').DataTable();
            $('#alertMSG').show();
            HideAlert();
        },
        error: function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}

$(document).ready(function () {
    $('#tblEmpList').DataTable();
    $('#alertMSG').hide();
});

function HideAlert() {
    setTimeout(function () {
        $('#alertMSG').hide();
    }, 30000);
}