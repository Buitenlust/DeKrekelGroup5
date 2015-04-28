
$(document).ready(function () {

    var settings = {
        url: "UploadData",
        method: "POST",
        allowedTypes: "csv",
        fileName: "csv",
        multiple: false,
        showFileCounter: false,
        statusBarWidth: 250,
        dragdropWidth: 250,
        dragDropStr: "",

        onSuccess: function (files, data, xhr) {
            $("#status").show(500);
            $('#importlist').load('/Uitleners/GroupDetails');
            $("#multiplefileuploader").hide();
            $("#save").show();
            console.log(data); 
        },
        onError: function (files, status, errMsg) {
            $("#status").html("<font color='red'>Upload is Failed</font>");

        }
    }

    if ($('.ajax-upload-dragdrop').length < 1) {
        $("#multiplefileuploader").uploadFile(settings);
        $("#status").show(500);
        $(".ajax-upload-dragdrop").show(500);
    } 
});