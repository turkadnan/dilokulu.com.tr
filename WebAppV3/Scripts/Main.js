var mainSearchTool = (function () {
    var myGetDiller = function () {
        alert('Deneme');
        //Ajax Start
        //$.ajax({
        //    type: "POST",
        //    url: "KursTipiSil",
        //    data: { id: id },
        //    dataType: "json",
        //    success: function (data) {

        //        if (data.status == "ok") {
        //            GeneralObj.DilokuluMessageBox.init({ status: 'ok', message: data.message + '<br/>Liste yüklenirken lütfen bekleyin...' });
        //            //alanlariTemizle();
        //            closeModal();
        //            locationReload(500);
        //        } else {
        //            GeneralObj.DilokuluMessageBox.init({ status: 'error', message: data.message });
        //            closeModal();
        //        }
        //    }
        //});
        //Ajax End
    };

    return {
        diller: myGetDiller
    };

})();