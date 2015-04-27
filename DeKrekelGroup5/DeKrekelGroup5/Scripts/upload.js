
$(document).ready(function () {

    var settings = {
        url: "UploadImage",
        method: "POST",
        allowedTypes: "jpg,png,gif",
        fileName: "newimage",
        multiple: false,
        showFileCounter: false,
        statusBarWidth: 200,
        dragdropWidth: 200,
        dragDropStr: "",
        onSuccess: function (files, data, xhr) {
            $("#status").show(500);
            console.log(files.toString());
            $("#picItem").attr("src", "/FTP/Images/" + data.imagePath);
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