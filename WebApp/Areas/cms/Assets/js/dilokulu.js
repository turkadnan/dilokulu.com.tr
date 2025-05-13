var GeneralObj = {};
GeneralObj.DilokuluMessageBox =
    {
        /*init parameters:  'status' and 'message'*/
        init: function (objects) {
            $('#divStatusBox').show();

            if (objects.status == 'error') {

                $('#divStatusBox').removeClass();
                $('#divStatusBox').addClass('divErrBox');
                $('#divStatusBox').show();
                $('#spanMessageText').html(objects.message);
            }
            if (objects.status == 'ok') {
                $('#divStatusBox').removeClass();
                $('#divStatusBox').addClass('divSuccesBox');
                $('#divStatusBox').show();
                $('#spanMessageText').html(objects.message);
            }
        },
        hide: function () {
            $('#divStatusBox').hide();
        }
    }

function Loader() {
    var overlay = $('#overlay');
    var Loader = $('#Loader');
    overlay.css("opacity", ".10");

    overlay.css("display", "block");
    Loader.css("display", "block");
}

function closeModal() {
    var overlay = $('#overlay');
    var Loader = $('#Loader');
    overlay.css("display", "none");
    Loader.css("display", "none");
}