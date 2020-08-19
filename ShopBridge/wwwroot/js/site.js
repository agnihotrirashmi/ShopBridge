function SubmitJForm(id) {
    if ($(id).valid() == true) {
        var res = UploadFile();
        if (res != undefined && res.status.isSuccess) {
            $.ajax({ // create an AJAX call...
                data: $(id).serialize(), // get the form data
                type: $(id).attr('method'), // GET or POST
                url: $(id).attr('action'), // the file to call
                success: function (response) { // on success..
                    if (response.success) {
                        toastr.success(response.message);
                        window.location = response.url;
                    }
                    else {
                        toastr.error(response.message);
                    }
                },
                async: false,
                error: function (jqXHR, exception) {
                    switch (jqXHR.status) {

                        case 0:
                            alert('Not connect.\nVerify Network.');
                            break;
                        case 404:
                            alert('Requested page not found. [404]');
                            break;
                        case 500:
                            alert('Internal Server Error [500].');
                            break;
                        case 404:
                            alert('Requested page not found. [404]');
                            break;
                        default:
                            alert('Uncaught Error or Exceptions occured.\n' + jqXHR.responseText);
                            break;
                    }
                    if (exception === 'parsererror' || exception === 'abort' || exception === 'timeout') {
                        alert('Exception Occured' + exception);
                        return;
                    }
                }
            });
        }
        else {
            toastr.error("Error while uploading file.");
        }
    }
    else {
        var validator = $(id).validate();
        var errors = validator.numberOfInvalids();
        if (errors) {
            var elementid = $('#' + validator.errorList[0].element.id);
            $(document).scrollTop(0);
            elementid.focus().trigger('chosen:activate')
        }
    }
    return false;
}

function DeleteProduct(id) {
    var model = {
        ProductId: id
    };
    if (confirm("Are you you want to delete product?")) {
        $.ajax({
            type: "POST",
            url: "Products/Delete",
            data: model,
            success: function (response) {
                if (response.success) {
                    toastr.success(response.message);
                    BindDatatable();
                }
                else {
                    toastr.error(response.message);
                }
            },
            async: true,
            error: function (jqXHR, exception) {
            }
        });
    }
}

function BindDatatable() {
    if ($.fn.DataTable.isDataTable('#productsTable')) {
        $('#productsTable').DataTable().destroy();
    }
    $('#productsTable tbody').empty();

    var tableObj = $('#productsTable');
    var table = tableObj.DataTable({
        "serverSide": true,
        "proccessing": true,
        "ajax": {
            url: "/Products/GetList",
            type: 'POST',
            datatype: 'json'
        },
        "order": [0, "desc"],
        'language': {
            'processing': 'Loading...'
        },
        "columns": [
            { "data": "productId", "searchable": false, "visible": false },
            { "data": "productName" },
            { "data": "productPrice" },
            {
                "mRender": function (data, type, row) {
                    return "<a href=/Products/Details/" + row.productId + ">Details</a> | <a href='#' onclick=DeleteProduct(" + row.productId + ")>Delete</a>";
                }
            }
        ],
        "columnDefs": [{
            "targets": 3,
            "orderable": false
        }],
    });
}

function AddAsterick(id) {
    var idname = $(id).attr('name');
    var label = $(id).closest("form").find('label[for="' + idname + '"]');
    var text = label.text();
    if (text.length > 0) {
        $(label).children('span[class^="asterisk"]').remove();
        label.append(' <span class="asterisk">*</span>');
    }
    else if ($(id).attr('type') != 'checkbox' && $(id).attr('type') != 'radio' && $(id).attr('type') != 'hidden') {
        $(id).next('span[class^="asterisk"]').remove();
        $(id).after(' <span class="asterisk">*</span>');
    }
}

function UploadFile() {
    var formdata = new FormData();
    var file = $('#file')[0].files[0];
    var filetype = file.type;
    var filelength = file.size;
    var res = new Object();
    if (formdata) {
        formdata.append("file", file);
        $.ajax({
            url: '/FileHandling/UploadFile',
            type: "POST",
            async: true,
            data: formdata,
            processData: false,
            contentType: false,
            async: false,
            success: function (result) {
               res = result;
                if (result.status.isSuccess) {
                    $("#ProductImage").val(result.data);
                }
            },
            error: function (jqXHR, exception) {
            }
        });
    }
    return res;
}