$(function () {

    //get modal-dialog class of modal popup
    var PlaceHolderElement = $('#PlaceHolderHere');
    var PlaceHolderElement1 = $('#PlaceHolderHere1');

    //SweetAlert Declaration
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: "btn btn-de-primary",
            cancelButton: "btn btn-de-danger me-2"
        },
        buttonsStyling: false
    });

    // Script for Popup of Create & Edit
    $('button[data-toggle="ajax-modal"]').click(function (event) {
        //$("#divBlocker").removeClass("d-none").addClass("screenblocker");
        var url = $(this).data('url');
        var decodedUrl = decodeURIComponent(url);
        $.get(decodedUrl).done(function (data, status) {
            PlaceHolderElement.html(data);
            //$("#divBlocker").addClass("d-none").removeClass("screenblocker");
            if ((PlaceHolderElement.find('.modal').length) == 0) {
                PlaceHolderElement.parents('.modal').modal('show');
            }
            else {
                PlaceHolderElement.find('.modal').modal('show');
            }
        })
    });

   

    PlaceHolderElement.on('click', '[data-save="modal"]', function (event) {

        if ((document.querySelector('div[id="divErrorList"]') === null) != true) {
            $("#divErrorList").remove();
        }
        event.preventDefault();

        $.ajax({
            type: 'POST',
            url: $(this).parents('.modal').find('form').attr('action'),
            data: $(this).parents('.modal').find('form').serialize(),
            success: function (response) {

                if (response.success) {
                    var urlEndPoint = $('.modal-body').find('form').attr('data-url');
                    if (urlEndPoint == 'Create') { urlEndPoint = "Created"; urlEndPointIcon = "success"; }
                    else if (urlEndPoint == 'Edit' || urlEndPoint == 'Update') { urlEndPoint = "Updated"; urlEndPointIcon = "warning"; }
                    else { urlEndPoint = "Completed"; urlEndPointIcon = "success"; }
                    swalWithBootstrapButtons.fire({
                        title: urlEndPoint + "!",
                        text: urlEndPoint + " successfully!",
                        icon: urlEndPointIcon,
                        allowOutsideClick: false
                    }).then(okay => {
                        if (okay) {
                            location.reload();
                        }
                    });
                } else {
                    // Display validation errors
                    if ((document.querySelector('div[id="divErrorList"]') === null) != true) {
                        $("#divErrorList").remove();
                    }
                     var errors = response.errors;
                     var errorList = '<ul>';
                     for (var i = 0; i < errors.length; i++) {
                         errorList += '<li>' + errors[i] + '</li>';
                     }
                     errorList += '</ul>';
                     $('.modal-body').prepend('<div id="divErrorList" class="alert alert-danger">' + errorList + '</div>');
                }
            },
            error: function (error) {
                alert('Error submitting form');
            }
        });
    });

    $('a[data-toggle="ajax-modal"]').click(function (event) {
        event.preventDefault();


        swalWithBootstrapButtons.fire({
            title: "Are you sure?",
            text: "Do You want to delete this?",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: '<i class="mdi mdi-check-all me-2"></i>Delete',
            cancelButtonText: '<i class="mdi mdi-window-close me-2"></i>Close',
            // confirmButtonText: "Yes, delete it!",
            // cancelButtonText: "No, cancel!",
            reverseButtons: true,
            allowOutsideClick: false
        }).then((result) => {
            if (result.isConfirmed) {
                decodedUrl = decodeURIComponent((this).dataset.url);
                $.ajax({
                    type: 'POST',
                    url: decodedUrl,
                    data: {},
                    success: function (response) {
                        //$("#mdlLeadSourceShow .modal-body").html(response);

                        if (response.success) {
                            swalWithBootstrapButtons.fire({
                                title: "Deleted!",
                                text: "Deleted successfully!",
                                icon: "success",
                                allowOutsideClick: false
                            }).then(okay => {
                                if (okay) {
                                    location.reload();
                                }
                            });
                        } else {
                            // Display validation errors
                            $("#divErrorList").remove();
                            var errors = response.errors;
                            var errorList = '<ul>';
                            for (var i = 0; i < errors.length; i++) {
                                errorList += '<li>' + errors[i] + '</li>';
                            }
                            errorList += '</ul>';
                            $('.modal-body').prepend('<div id="divErrorList" class="alert alert-danger">' + errorList + '</div>');
                        }
                    },
                    error: function (error) {
                        alert('Error submitting form');
                    }
                });
            } else if (
                result.dismiss === Swal.DismissReason.cancel
            ) {
                // swalWithBootstrapButtons.fire({
                // title: "Cancelled",
                // text: "Your imaginary file is safe :)",
                // icon: "error",
                // 	allowOutsideClick: false
                // });
            }
        });
    });




    $('button[data-toggle="ajax-modal1"]').click(function (event) {
        //$("#divBlocker").removeClass("d-none").addClass("screenblocker");
        var url = $(this).data('url');
        var decodedUrl = decodeURIComponent(url);
        $.get(decodedUrl).done(function (data, status) {
            PlaceHolderElement1.html(data);
            //$("#divBlocker").addClass("d-none").removeClass("screenblocker");
            if ((PlaceHolderElement1.find('.modal').length) == 0) {
                PlaceHolderElement1.parents('.modal').modal('show');
            }
            else {
                PlaceHolderElement1.find('.modal').modal('show');
            }
        })
    });

    PlaceHolderElement1.on('click', '[data-save="modal"]', function (event) {
        //$("#divBlocker").removeClass("d-none").addClass("screenblocker");
        var form = $(this).parents('.modal').find('form');
        //$('#PlaceHolderHere').find('input').attr('readonly', 'readonly');
        //$('#PlaceHolderHere').find('button').attr('disabled', true);
        //$('option:not(:selected)').prop('disabled', true);

        var actionName = form.attr('action');
        var currentUrl = window.location.href;
        currentUrl = currentUrl.substring(0, currentUrl.lastIndexOf("/") + 1);
        var url = currentUrl + actionName;
        var sendData = form.serialize();
        $.post(url, sendData).done(function (data, status) {
            //$("#divBlocker").addClass("d-none").removeClass("screenblocker");
            if ((PlaceHolderElement1.find('.modal').length) == 0) {
                PlaceHolderElement1.parents('.modal').modal('hide');
            }
            else {
                PlaceHolderElement1.find('.modal').modal('hide');
            }
            location.reload();
        })
    });
});



