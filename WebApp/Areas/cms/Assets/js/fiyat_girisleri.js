
//Kurs Tipi İşlemleri Start
var arrayKursTipiOzelTarihler = []; //Array

var kursTipi = (function () {

    var myInit = function () {
        $('.kursTipiTarih').on('click', function () {
            var value = $(this).val();

            if (value == 'herPazartesi') {
                if (arrayKursTipiOzelTarihler.length > 0) {
                    if (confirm('Tanımlı özel tarihler var. Liste silinecek!\nOnaylıyor musunuz?')) {
                        arrayKursTipiOzelTarihler = [];
                        myOzelTarihYazdir();

                        $('#divKursTipiOzelTarih').hide();

                    } else {

                        //radio gruptaki checked class ismi ile bulunup siliniyor. Sonra özel tarih radyo buttonu yeniden seçili hale getiriliyor.
                        $('.kursTipiTarih').removeAttr('checked');
                        $('input:radio[name=kursTipiTarih]')[1].checked = true;
                    }
                } else {
                    $('#divKursTipiOzelTarih').hide();
                }

            }
            if (value == 'ozel') {
                $('#divKursTipiOzelTarih').show();
            }
        });

        $('#btnTarihEkle').on('click', function () {
            var ozelTarih = $('#kursTipiOzelTarihGiris');
            myOzelTarihPushObject(ozelTarih)
        });

        $('#btnKaydet').on('click', function () {
            /* Variables */
            var errCount = 0;
            var errMessage = '';
            var okulId = $('#okulId').val();
            var baslik = $('#baslik').val();
            var kursKategoriId = $('#kursKategorisi').val();
            var aciklama = $('#kursTipiAciklama').val();
            var basTarihiStatus = $('.kursTipiTarih:checked').val();
            var ozelTarihListesi = $('#ozelTarihListesi').val();
            /* Variables End */

            /* Error Control */
            if ($.trim(kursKategoriId).length == 0) {
                errCount++;
                errMessage += "Kurs kategorisi seçin!</br>";
            }


            if ($.trim(baslik).length == 0) {
                errCount++;
                errMessage += "Kurs tipi girişi yapın!</br>";
            }

            if (basTarihiStatus == 'ozel' && arrayKursTipiOzelTarihler.length == 0) {
                errCount++;
                errMessage += "Özel tarih girişi yapın!";
            }

            /* Error Control End */

            if (errCount > 0) {
                GeneralObj.DilokuluMessageBox.init({ status: 'error', message: errMessage });
            } else {

                GeneralObj.DilokuluMessageBox.hide();

                Loader();

                //Ajax Start
                $.ajax({
                    type: "POST",
                    url: "KursTipiEkle",
                    data: { okulId: okulId, baslik: baslik, kursKategoriId: kursKategoriId, basTarihiStatus: basTarihiStatus, ozelTarihListesi: ozelTarihListesi, aciklama: encodeURIComponent(aciklama) },
                    dataType: "json",
                    success: function (data) {

                        if (data.status == "ok") {
                            GeneralObj.DilokuluMessageBox.init({ status: 'ok', message: 'Kayıt yapıldı.<br/>Liste yüklenirken lütfen bekleyin...' });
                            alanlariTemizle();
                            closeModal();

                            locationReload(500);
                        } else {
                            GeneralObj.DilokuluMessageBox.init({ status: 'error', message: data.message });
                            closeModal();
                        }
                    }
                });
                //Ajax End
            }
        })
    };

    var myOzelTarihPushObject = function (element) {
        if ($.trim(element.val()).length > 0) {

            arrayKursTipiOzelTarihler.push({ 'tarih': element.val() });
            element.val('');

            myOzelTarihYazdir();
        }

    };

    var myOzelTarihYazdir = function () {

        $('#divOzelTarihListesi').html('');
        $('#ozelTarihListesi').val('');

        if (arrayKursTipiOzelTarihler !== null && arrayKursTipiOzelTarihler.length > 0) {
            var display = '';
            var hiddenField = '';

            for (var i = 0; i < arrayKursTipiOzelTarihler.length; i++) {

                display += '<div style="float: left; padding: 5px; margin-right: 5px;font-weight:bold;">' + arrayKursTipiOzelTarihler[i].tarih + '<a href="javascript:void(0)" onclick="OzelTarihSil(this)" data-item="' + arrayKursTipiOzelTarihler[i].tarih + '">[X]</a></div>';
                hiddenField += arrayKursTipiOzelTarihler[i].tarih + '|';

            }

            $('#ozelTarihListesi').val(hiddenField);
            $('#divOzelTarihListesi').html(display);
        }

        var myTarihCikart = function (element) {
            if (true) {

            }

        };
    };

    var alanlariTemizle = function () {
        //form kontrolleri boşaltılıyor
        $('#baslik').val('');
        $('#kursKategorisi').val('');
        $('#kursTipiAciklama').val('');

        //Array temizleniyor
        arrayKursTipiOzelTarihler = [];

        //Eklenerek gösterilen tarihler siliniyor
        $('#divOzelTarihListesi').html('');

        //Seçilen tarihin database e gönderimi işini yapan hidden field temizleniyor
        $('#ozelTarihListesi').val('');

        //radio butonlar default değerine alınıyor
        $('.kursTipiTarih').removeAttr('checked');
        $('input:radio[name=kursTipiTarih]')[0].checked = true;

        //Tarih giriş textbox ı kapatılıyor
        $('#divKursTipiOzelTarih').hide();
    }

    var tarihAyarla = function (controller) {
        $("#" + controller).datepicker({
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 3,
            showWeek: true,
            minDate: 0
        });
    };

    var myKursTipiSil = function (id) {

        var cnt = confirm('Kayıt silinecek!\nOnaylıyor musunuz?');
        if (cnt) {
            Loader();
            //Ajax Start
            $.ajax({
                type: "POST",
                url: "KursTipiSil",
                data: { id: id },
                dataType: "json",
                async: false,
                success: function (data) {

                    if (data.status == "ok") {
                        GeneralObj.DilokuluMessageBox.init({ status: 'ok', message: data.message + '<br/>Liste yüklenirken lütfen bekleyin...' });
                        //alanlariTemizle();
                        closeModal();
                        locationReload(500);
                    } else {
                        GeneralObj.DilokuluMessageBox.init({ status: 'error', message: data.message });
                        closeModal();
                    }
                }
            });
            //Ajax End
        }



    };

    var myTumKursTipiListesiniSil = function () {
        var cnt = confirm('Tüm kayıtlar silinecek!\nOnaylıyor musunuz?');
        if (cnt) {

            $(".tblFiyatListesi").find('.fieldset').each(function (i) {
                var $fieldset = $(this);

                var id = $('input:hidden:eq(0)', $fieldset).val();
                if (id !== '') {
                    Loader();
                    //Ajax Start
                    $.ajax({
                        type: "POST",
                        url: "KursTipiSil",
                        data: { id: id },
                        dataType: "json",
                        async: false,
                        success: function (data) {

                            if (data.status == "ok") {

                            } else {
                                closeModal();
                                GeneralObj.DilokuluMessageBox.init({ status: 'error', message: data.message });
                            }
                        }
                    });
                    //Ajax End
                    closeModal();
                    locationReload(500);
                }
            });
        }

    };

    var locationReload = function (milliseconds) {
        setTimeout(function () {
            location.reload();
        }, milliseconds);
    };

    var promosyonPDFKaydet = function (formId, buttonId, inputId) {
        $('#' + buttonId).on('click', function () {
            $('#' + formId).trigger('submit');
        })

        $('#' + formId).submit(function (event) {
            Loader();

            //Ajax Start

            var fileName = '';
            var input = document.getElementById(inputId);
            var id = $('#hfOkulId').val();
            var tarih = $('#promostonBtsTarihi').val();

            if (tarih.length > 0 && id.length > 0) {
                if (window.FormData !== undefined) {
                    var data = new FormData();
                    for (var x = 0; x < input.files.length; x++) {
                        data.append("file" + x, input.files[x]);
                    }

                    $.ajax({
                        type: "POST",
                        url: 'PromosyonPdfKaydet?okulId=' + id + '&tarih=' + tarih,
                        contentType: false,
                        processData: false,
                        async: false,
                        data: data,
                        success: function (result) {


                            if (result.status == "ok") {


                                fileName = result.fileName.replace(/\"/g, "");

                                $('#divPromosyonPdf').css('display', 'block')
                                $('#anchorPromosyonPdf').attr('href', fileName)
                                $('#promosyonPdfFile').val('');

                                GeneralObj.DilokuluMessageBox.init({ status: 'ok', message: 'pdf işlemleri tamamlandı.' });
                                closeModal();
                            } else {
                                GeneralObj.DilokuluMessageBox.init({ status: 'error', message: result.message });
                                closeModal();
                            }


                        },
                        error: function (xhr, status, p3, p4) {
                            var err = "Error " + " " + status + " " + p3 + " " + p4;
                            if (xhr.responseText && xhr.responseText[0] == "{")
                                err = JSON.parse(xhr.responseText).Message;
                            console.log(err);
                        }
                    });
                } else {
                    alert("This browser doesn't support HTML5 file uploads!");
                }
            } else {
                closeModal();
                GeneralObj.DilokuluMessageBox.init({ status: 'error', message: 'Lütfen tarih ve dosya seçim alanını kontrol ediniz!' });
            }

            //Ajax End
            event.preventDefault();
        })
    };

    var promosyonPDFSil = function (okulId) {

        var cnt = confirm('Dosya silinecek. Emin misiniz?');
        if (cnt) {
            //Ajax Start
            $.ajax({
                type: "GET",
                url: 'PromosyonPdfSil?okulId=' + okulId,
                contentType: false,
                processData: false,
                async: false,

                success: function (result) {


                    if (result.status == "ok") {

                        $('#divPromosyonPdf').css('display', 'none')
                        $('#anchorPromosyonPdf').attr('href', '#')
                        $('#promosyonPdfFile').val('');
                        $('#promostonBtsTarihi').val('');

                        GeneralObj.DilokuluMessageBox.init({ status: 'ok', message: 'pdf silindi.' });
                        closeModal();
                    } else {
                        GeneralObj.DilokuluMessageBox.init({ status: 'error', message: result.message });
                        closeModal();
                    }

                },
                error: function (xhr, status, p3, p4) {
                    var err = "Error " + " " + status + " " + p3 + " " + p4;
                    if (xhr.responseText && xhr.responseText[0] == "{")
                        err = JSON.parse(xhr.responseText).Message;
                    console.log(err);
                }
            });
            //Ajax End
        }
    };

    return {
        tarihGoster: tarihAyarla,
        ozelTarihYazdir: myOzelTarihYazdir,
        kursTipiSil: myKursTipiSil,
        tumKursTipiListesiniSil: myTumKursTipiListesiniSil,
        promosyonPDFForm: promosyonPDFKaydet,
        init: myInit
    };
})();

function OzelTarihSil(element) {
    var arrayTemp = [];
    var tarih = $(element).attr('data-item');
    if (arrayKursTipiOzelTarihler !== null && arrayKursTipiOzelTarihler.length > 0) {
        for (var i = 0; i < arrayKursTipiOzelTarihler.length; i++) {
            if (arrayKursTipiOzelTarihler[i].tarih !== tarih) {
                arrayTemp.push(arrayKursTipiOzelTarihler[i]);
            }
        }

        arrayKursTipiOzelTarihler = arrayTemp;

        arrayTemp = [];

        kursTipi.ozelTarihYazdir();
    }
}
//Kurs Tipi İşlemleri End

//Fiyat Tanimlari Start
var fiyatTanimi = (function () {
    var myInit = function () {

        // standartHaftaGuncelle();

        $('#fiyatlandirmaTipi').on('change', function () {
            var fiyatlandirmaTipiId = $(this).val();
            var kursTipiId = $('#kursTipi').val();
            GeneralObj.DilokuluMessageBox.hide();

            if (fiyatlandirmaTipiId !== '') {

                myFiyatlandirmaListesiniGetir(fiyatlandirmaTipiId, kursTipiId);

            } else {
                $('#fiyatTanimiHaftaTanimlari').html('');
                closeModal();
            }
        });

        //Standart Hafta Ekle
        myStandartHaftaEkle = function () {
            $('#btnStandartHafta_Ekle').on('click', function () {

                var kursTipiId = $('#kursTipi').val();
                var fiyatlandirmaTipiId = $(this).attr('Data-FiyatlandirmaTipiId');

                var hafta = $('#StandartHafta_Hafta').val();
                var fiyat = $('#StandartHafta_Fiyat').val();
                if (hafta !== '' && fiyat !== '' && kursTipiId !== '' && fiyatlandirmaTipiId !== '') {
                    Loader();

                    //Ajax Start
                    $.ajax({
                        type: "POST",
                        url: "KursTipiFiyatiEkle",
                        data: { kursTipiId: kursTipiId, fiyatlandirmaTipiId: fiyatlandirmaTipiId, hafta: hafta, fiyat: fiyat },
                        dataType: "json",
                        success: function (data) {
                            if (data.status == "ok") {
                                GeneralObj.DilokuluMessageBox.init({ status: 'ok', message: 'Kayıt yapıldı.' });
                                myFiyatlandirmaListesiniGetir(fiyatlandirmaTipiId, kursTipiId);
                                $('#StandartHafta_Hafta').val('');
                                $('#StandartHafta_Fiyat').val('');

                                $('.focusField').focus();

                                closeModal();
                            } else {
                                GeneralObj.DilokuluMessageBox.init({ status: 'error', message: data.message });
                                closeModal();
                            }
                        }
                    });
                    //Ajax End
                }
            });
        };

        myStandartHaftaAllDelete = function () {
            $('#btnStandartHafta_AllDelete').on('click', function () {

                var kursTipiId = $('#kursTipi').val();
                var fiyatlandirmaTipiId = $(this).attr('Data-FiyatlandirmaTipiId');

                var cnt = confirm('Kayıtlar silinecek!\nOnaylıyor musunuz?');
                if (cnt) {
                    if (kursTipiId !== '' && fiyatlandirmaTipiId !== '') {
                        Loader();

                        //Ajax Start
                        $.ajax({
                            type: "POST",
                            url: "KursTipiFiyatAllDelete",
                            data: { kursTipiId: kursTipiId },
                            async: false,
                            dataType: "json",
                            success: function (data) {
                                if (data.status == "ok") {
                                    GeneralObj.DilokuluMessageBox.init({ status: 'ok', message: 'Kayıt silindi.' });

                                    fiyatTanimi.fiyatlandirmaListesiniGetir(fiyatlandirmaTipiId, kursTipiId);

                                    closeModal();
                                } else {
                                    GeneralObj.DilokuluMessageBox.init({ status: 'error', message: data.message });
                                    closeModal();
                                }
                            }
                        });
                        //Ajax End
                    }
                }
            });
        };

        //Hafta Bazlı & Birim Fiyatlı Ekle
        myHaftaBazliEkle = function () {
            $('#btnHaftaBazli_Ekle').on('click', function () {

                var kursTipiId = $('#kursTipi').val();
                var fiyatlandirmaTipiId = $(this).attr('Data-FiyatlandirmaTipiId');
                var haftaBaslangic = $('#HaftaBazli_HaftaBaslangic').val();
                var haftaBitis = $('#HaftaBazli_HaftaBitis').val();
                var fiyat = $('#HaftaBazli_Fiyat').val();
                if (haftaBaslangic !== '' && haftaBitis !== '' && fiyat !== '' && kursTipiId !== '' && fiyatlandirmaTipiId !== '') {
                    Loader();

                    //Ajax Start
                    $.ajax({
                        type: "POST",
                        url: "KursTipiFiyatiEkle",
                        data: { kursTipiId: kursTipiId, fiyatlandirmaTipiId: fiyatlandirmaTipiId, haftaBaslangic: haftaBaslangic, haftaBitis: haftaBitis, fiyat: fiyat },
                        dataType: "json",
                        success: function (data) {
                            if (data.status == "ok") {
                                GeneralObj.DilokuluMessageBox.init({ status: 'ok', message: 'Kayıt yapıldı.' });
                                myFiyatlandirmaListesiniGetir(fiyatlandirmaTipiId, kursTipiId);
                                $('#HaftaBazli_HaftaBaslangic').val('');
                                $('#HaftaBazli_HaftaBaslangic').val('');
                                $('#HaftaBazli_Fiyat').val();

                                $('.focusField').focus();

                                closeModal();
                            } else {
                                GeneralObj.DilokuluMessageBox.init({ status: 'error', message: data.message });
                                closeModal();
                            }
                        }
                    });
                    //Ajax End
                }
            });
        };

        //Her Haftaya Özel Fiyat
        myHerHaftayaOzelEkle = function () {
            $('#btnHaftayaOzel_Ekle').on('click', function () {

                var kursTipiId = $('#kursTipi').val();
                var fiyatlandirmaTipiId = $(this).attr('Data-FiyatlandirmaTipiId');
                var hafta = 0;
                var fiyat = 0;

                $('.haftayaOzelInput').each(function () {
                    hafta = $(this).attr('data-hafta');
                    fiyat = $(this).val();
                    if (hafta !== '' && fiyat !== '') {
                        Loader();

                        //Ajax Start
                        $.ajax({
                            type: "POST",
                            url: "KursTipiFiyatiEkle",
                            async: false,
                            data: { kursTipiId: kursTipiId, fiyatlandirmaTipiId: fiyatlandirmaTipiId, hafta: hafta, fiyat: fiyat },
                            dataType: "json",
                            success: function (data) {
                                if (data.status == "ok") {
                                    GeneralObj.DilokuluMessageBox.init({ status: 'ok', message: 'Kayıt yapıldı.' });
                                    myFiyatlandirmaListesiniGetir(fiyatlandirmaTipiId, kursTipiId);

                                    closeModal();
                                } else {
                                    GeneralObj.DilokuluMessageBox.init({ status: 'error', message: data.message });
                                    closeModal();
                                }
                            }
                        });
                        //Ajax End
                    }
                });
            });
        };

        //Fiyat tanımları bölümünde eğer daha önceden fiyatlandırma modeli tanımlanmış ise seçili de getirmek için kullanılan script
        $('#kursTipi').change(function () {

            var kursTipiId = $(this).val();

            Loader();

            $('#fiyatTanimiHaftaTanimlari').html('');

            //Ajax Start
            $.ajax({
                type: "POST",
                url: "KursTipiFiyatModeliBul",
                data: { kursTipiId: kursTipiId },
                dataType: "json",
                asyn: false,
                success: function (data) {
                    if (data.status == "ok") {

                        $('#fiyatlandirmaTipi').val(data.fiyatlandirmaModelId);
                        $('#fiyatlandirmaTipi').trigger('change');
                        closeModal();
                    } else {

                        // Kurs tipi değiştiriğinde liste ekranı temizleniyor
                        $('#fiyatlandirmaTipi').val('');


                        closeModal();
                    }
                }
            });
            //Ajax End
        });

        //Default seçili bir kurstipi var ise
        var kursTipiId = $('#kursTipi').val();
        if (kursTipiId !== '') {

            $('#kursTipi').trigger('change');

        }
    };


    // Standart Hafta Güncelle
    var standartHaftaGuncelle = function () {
        $('#btnStandartHafta_Guncelle').on('click', function () {
            var cnt = confirm('Tüm fiyatlar güncellenecek.\nOnaylıyor musunuz?');
            if (cnt) {
                var kursTipiId = $('#kursTipi').val();
                var fiyatlandirmaTipiId = $(this).attr('data-fiyatlandirmatipiid');

                $("#standartHafta").find('.fieldset').each(function (i) {
                    var $fieldset = $(this);

                    var id = $('input:hidden:eq(0)', $fieldset).val();
                    var hafta = $('input:text:eq(0)', $fieldset).val();
                    var fiyat = $('input:text:eq(1)', $fieldset).val();

                    if (id !== '' && fiyat !== '' && hafta !== '') {


                        //Ajax Start
                        $.ajax({
                            type: "POST",
                            url: "KursTipiFiyatiGuncelle",
                            data: { id: id, fiyatlandirmaTipiId: fiyatlandirmaTipiId, hafta: hafta, fiyat: fiyat },
                            dataType: "json",
                            async: false,
                            success: function (data) {

                            }
                        });
                        //Ajax End


                    }



                });

                myFiyatlandirmaListesiniGetir(fiyatlandirmaTipiId, kursTipiId);
            }
        });
    };

    //Hafta Bazlı & Birim Fiyatlı Ekle
    var haftaBazliGuncelle = function () {
        $('#btnHaftaBazli_Guncelle').on('click', function () {

            var cnt = confirm('Tüm fiyatlar güncellenecek.\nOnaylıyor musunuz?');
            if (cnt) {
                var kursTipiId = $('#kursTipi').val();
                var fiyatlandirmaTipiId = $(this).attr('Data-FiyatlandirmaTipiId');

                $("#haftaBazli").find('.fieldset').each(function (i) {
                    var $fieldset = $(this);

                    var id = $('input:hidden:eq(0)', $fieldset).val();
                    var haftaBaslangic = $('input:text:eq(0)', $fieldset).val();
                    var haftaBitis = $('input:text:eq(1)', $fieldset).val();
                    var fiyat = $('input:text:eq(2)', $fieldset).val();

                    if (id !== '' && haftaBaslangic !== '' && haftaBitis !== '' && fiyat !== '') {
                        //Ajax Start
                        $.ajax({
                            type: "POST",
                            url: "KursTipiFiyatiGuncelle",
                            data: { id: id, fiyatlandirmaTipiId: fiyatlandirmaTipiId, haftaBaslangic: haftaBaslangic, haftaBitis: haftaBitis, fiyat: fiyat },
                            dataType: "json",
                            async: false,
                            success: function (data) {

                            }
                        });
                        //Ajax End
                    }
                });

                myFiyatlandirmaListesiniGetir(fiyatlandirmaTipiId, kursTipiId);
            }
        });
    };

    //Fiyatlandırma Listesini Getir
    var myFiyatlandirmaListesiniGetir = function (fiyatlandirmaTipiId, kursTipiId) {

        Loader();

        $('#fiyatTanimiHaftaTanimlari').html('');
        //Ajax Start
        $.ajax({
            type: "GET",
            url: "FiyatTanimlari_HaftaTanimlari",
            data: { fiyatlandirmaTipiId: fiyatlandirmaTipiId, kursTipiId: kursTipiId },
            //dataType: "json",
            success: function (data) {
                if (data !== undefined && data !== null) {
                    $('#fiyatTanimiHaftaTanimlari').html(data);

                    //Button Eventlarını görebilmesi için yapıldı
                    myStandartHaftaEkle();
                    myStandartHaftaAllDelete();
                    myHaftaBazliEkle();
                    //myHerHaftayaOzelEkle();
                    standartHaftaGuncelle();
                    haftaBazliGuncelle();
                } else {
                    $('#fiyatTanimiHaftaTanimlari').html('');
                }
                closeModal();
            },
            error: function () {
                closeModal();
            }
        });
        //Ajax End
    };
    return {
        fiyatlandirmaListesiniGetir: myFiyatlandirmaListesiniGetir,
        init: myInit
    };
})();

function KursTipiFiyatSil(element) {
    var kursTipiId = $('#kursTipi').val();
    var fiyatlandirmaTipiId = $(element).attr('Data-FiyatlandirmaTipiId');
    var kursTipiFiyatId = $(element).attr('data-fiyatid');

    var cnt = confirm('Kayıt silinecek!\nOnaylıyor musunuz?');
    if (cnt) {

        Loader();

        //Ajax Start
        $.ajax({
            type: "POST",
            url: "KursTipiFiyatSil",
            async: false,
            data: { kursTipiId: kursTipiId, fiyatlandirmaTipiId: fiyatlandirmaTipiId, kursTipiFiyatId: kursTipiFiyatId },
            dataType: "json",
            success: function (data) {
                if (data.status == "ok") {
                    GeneralObj.DilokuluMessageBox.init({ status: 'ok', message: 'Kayıt silindi.' });

                    fiyatTanimi.fiyatlandirmaListesiniGetir(fiyatlandirmaTipiId, kursTipiId);

                    closeModal();
                } else {
                    GeneralObj.DilokuluMessageBox.init({ status: 'error', message: data.message });
                    closeModal();
                }
            }
        });
        //Ajax End
    }

}
//Fiyat Tanimlari End

//Promosyon Tanimlari Start
var promosyonTanimi = (function () {
    var myInit = function () {

        //Ekstra Hafta Promosyonu Ekle
        myExtraHaftaEkle = function () {
            $('#btnExtraHafta_Ekle').on('click', function () {

                var kursTipiId = $('#kursTipi').val();
                var promosyonTipiId = $(this).attr('data-promosyontipiid');

                var extraHafta_MinimumHafta = $('#ExtraHafta_MinimumHafta').val();
                var extraHafta_UcretsizHaftaSayisi = $('#ExtraHafta_UcretsizHaftaSayisi').val();
                var promosyonBitisTarihi = $('#PromosyonBitisTarihi').val();

                if (extraHafta_MinimumHafta !== '' && extraHafta_UcretsizHaftaSayisi !== '' && promosyonBitisTarihi !== '' && kursTipiId !== '' && promosyonTipiId !== '') {
                    Loader();

                    //Ajax Start
                    $.ajax({
                        type: "POST",
                        url: "KursTipiPromosyonEkle",
                        data: { kursTipiId: kursTipiId, promosyonTipiId: promosyonTipiId, extraHafta_MinimumHafta: extraHafta_MinimumHafta, extraHafta_UcretsizHaftaSayisi: extraHafta_UcretsizHaftaSayisi, promosyonBitisTarihi: promosyonBitisTarihi },
                        dataType: "json",
                        success: function (data) {
                            if (data.status == "ok") {
                                GeneralObj.DilokuluMessageBox.init({ status: 'ok', message: 'Kayıt yapıldı.' });
                                myPromosyonListesiniGetir(promosyonTipiId, kursTipiId);
                                $('#ExtraHafta_MinimumHafta').val('');
                                $('#ExtraHafta_UcretsizHaftaSayisi').val('');
                                $('#PromosyonBitisTarihi').val('');

                                closeModal();
                            } else {
                                GeneralObj.DilokuluMessageBox.init({ status: 'error', message: data.message });
                                closeModal();
                            }
                        }
                    });
                    //Ajax End
                } else {
                    alert('Eksik Alanları doldurun!');
                }
            });
        };

        //Yüzdesel İmdirim Promosyonu Ekle
        myIndirimPromosyonuEkle = function () {
            $('#btnIndirimPromosyonu_Ekle').on('click', function () {

                var kursTipiId = $('#kursTipi').val();
                var promosyonTipiId = $(this).attr('data-promosyontipiid');

                var indirimPromosyonu_Hafta = $('#IndirimPromosyonu_Hafta').val();
                var indirimPromosyonu_HaftaMax = $('#IndirimPromosyonu_HaftaMax').val();
                var indirimPromosyonu_Oran = $('#IndirimPromosyonu_Oran').val();
                var promosyonBitisTarihi = $('#PromosyonBitisTarihi').val();
                var kursTarihiBaslangic = $('#KursTarihiBaslangic').val();
                var kursTarihiBitis = $('#KursTarihiBitis').val();

                if (indirimPromosyonu_Hafta !== '' && indirimPromosyonu_HaftaMax !== '' && indirimPromosyonu_Oran !== '' && promosyonBitisTarihi !== '' && kursTipiId !== '' && promosyonTipiId !== '') {
                    Loader();

                    //Ajax Start
                    $.ajax({
                        type: "POST",
                        url: "KursTipiPromosyonEkle",
                        data: { kursTipiId: kursTipiId, promosyonTipiId: promosyonTipiId, indirimPromosyonu_Hafta: indirimPromosyonu_Hafta, indirimPromosyonu_HaftaMax: indirimPromosyonu_HaftaMax, indirimPromosyonu_Oran: indirimPromosyonu_Oran, promosyonBitisTarihi: promosyonBitisTarihi, kursTarihiBaslangic: kursTarihiBaslangic, kursTarihiBitis: kursTarihiBitis },
                        dataType: "json",
                        success: function (data) {
                            if (data.status == "ok") {
                                GeneralObj.DilokuluMessageBox.init({ status: 'ok', message: 'Kayıt yapıldı.' });
                                myPromosyonListesiniGetir(promosyonTipiId, kursTipiId);
                                $('#IndirimPromosyonu_Hafta').val('');
                                $('#IndirimPromosyonu_HaftaMax').val('');
                                $('#IndirimPromosyonu_Oran').val('');
                                $('#PromosyonBitisTarihi').val('');
                                $('#KursTarihiBaslangic').val('');
                                $('#KursTarihiBitis').val('');

                                closeModal();
                            } else {
                                GeneralObj.DilokuluMessageBox.init({ status: 'error', message: data.message });
                                closeModal();
                            }
                        }
                    });
                    //Ajax End
                } else {
                    alert('Eksik Alanları doldurun!');
                }
            });
        };

        //Fitay Bazlı İndirim Promosyon Ekle
        myKursBazliEkle = function () {
            $('#btnKursBazli_Ekle').on('click', function () {

                var kursTipiId = $('#kursTipi').val();
                var promosyonTipiId = $(this).attr('data-promosyontipiid');

                var kursBazli_Hafta = $('#KursBazli_Hafta').val();
                var kursBazli_Fiyat = $('#KursBazli_Fiyat').val();
                var promosyonBitisTarihi = $('#PromosyonBitisTarihi').val();

                if (kursBazli_Hafta !== '' && kursBazli_Fiyat !== '' && promosyonBitisTarihi !== '' && kursTipiId !== '' && promosyonTipiId !== '') {
                    Loader();

                    //Ajax Start
                    $.ajax({
                        type: "POST",
                        url: "KursTipiPromosyonEkle",
                        data: { kursTipiId: kursTipiId, promosyonTipiId: promosyonTipiId, kursBazli_Hafta: kursBazli_Hafta, kursBazli_Fiyat: kursBazli_Fiyat, promosyonBitisTarihi: promosyonBitisTarihi },
                        dataType: "json",
                        success: function (data) {
                            if (data.status == "ok") {
                                GeneralObj.DilokuluMessageBox.init({ status: 'ok', message: 'Kayıt yapıldı.' });
                                myPromosyonListesiniGetir(promosyonTipiId, kursTipiId);
                                $('#KursBazli_Hafta').val('');
                                $('#KursBazli_Fiyat').val('');
                                $('#PromosyonBitisTarihi').val('');

                                closeModal();
                            } else {
                                GeneralObj.DilokuluMessageBox.init({ status: 'error', message: data.message });
                                closeModal();
                            }
                        }
                    });
                    //Ajax End
                } else {
                    alert('Eksik Alanları doldurun!');
                }
            });
        };

        //İndirimli Birim Fiyat Promosyonu Ekle
        myIndirimliBirimFiyatPromosyonuEkle = function () {
            $('#btnIndirimliBirimFiyatPromosyonu_Ekle').on('click', function () {

                var kursTipiId = $('#kursTipi').val();
                var promosyonTipiId = $(this).attr('data-promosyontipiid');

                var indirimPromosyonu_Hafta = $('#IndirimPromosyonu_Hafta').val();
                var indirimPromosyonu_HaftaMax = $('#IndirimPromosyonu_HaftaMax').val();
                var indirimPromosyonu_BirimFiyat = $('#IndirimPromosyonu_BirimFiyat').val();
                var promosyonBitisTarihi = $('#PromosyonBitisTarihi').val();
                var kursTarihiBaslangic = $('#KursTarihiBaslangic').val();
                var kursTarihiBitis = $('#KursTarihiBitis').val();

                if (indirimPromosyonu_Hafta !== '' && indirimPromosyonu_HaftaMax !== '' && indirimPromosyonu_BirimFiyat !== '' && promosyonBitisTarihi !== '' && kursTipiId !== '' && promosyonTipiId !== '') {
                    Loader();

                    //Ajax Start
                    $.ajax({
                        type: "POST",
                        url: "KursTipiPromosyonEkle",
                        data: { kursTipiId: kursTipiId, promosyonTipiId: promosyonTipiId, indirimPromosyonu_Hafta: indirimPromosyonu_Hafta, indirimPromosyonu_HaftaMax: indirimPromosyonu_HaftaMax, indirimPromosyonu_BirimFiyat: indirimPromosyonu_BirimFiyat, promosyonBitisTarihi: promosyonBitisTarihi, kursTarihiBaslangic: kursTarihiBaslangic, kursTarihiBitis: kursTarihiBitis },
                        dataType: "json",
                        success: function (data) {
                            if (data.status == "ok") {
                                GeneralObj.DilokuluMessageBox.init({ status: 'ok', message: 'Kayıt yapıldı.' });
                                myPromosyonListesiniGetir(promosyonTipiId, kursTipiId);
                                $('#IndirimPromosyonu_Hafta').val('');
                                $('#IndirimPromosyonu_Oran').val('');
                                $('#PromosyonBitisTarihi').val('');
                                $('#KursTarihiBaslangic').val('');
                                $('#KursTarihiBitis').val('');

                                closeModal();
                            } else {
                                GeneralObj.DilokuluMessageBox.init({ status: 'error', message: data.message });
                                closeModal();
                            }
                        }
                    });
                    //Ajax End
                } else {
                    alert('Eksik Alanları doldurun!');
                }
            });
        };


        $('#promosyonTipi').change(function () {
            var promosyonTipiId = $(this).val();
            var kursTipiId = $('#kursTipi').val();

            GeneralObj.DilokuluMessageBox.hide();

            if (promosyonTipiId !== '') {

                myPromosyonListesiniGetir(promosyonTipiId, kursTipiId);

            } else {
                $('#promosyonTanimiHaftaTanimlari').html('');
                closeModal();
            }
        });

        //Promosyon tanımları bölümünde eğer daha önceden promosyon modeli tanımlanmış ise seçili getirmek için kullanılan script
        $('#kursTipi').on('change', function () {

            var kursTipiId = $(this).val();

            Loader();

            $('#promosyonTanimiHaftaTanimlari').html('');

            //Ajax Start
            $.ajax({
                type: "POST",
                url: "KursTipiPromosyonModeliBul",
                data: { kursTipiId: kursTipiId },
                dataType: "json",
                asyn: false,
                success: function (data) {
                    if (data.status == "ok") {

                        $('#promosyonTipi').val(data.promosyonTipiId);
                        $('#promosyonTipi').trigger('change');
                        closeModal();
                    } else {

                        // Kurs tipi değiştiriğinde liste ekranı temizleniyor
                        $('#promosyonTipi').val('');


                        closeModal();
                    }
                }
            });
            //Ajax End
        });

        //Extra Hafta Promosyonu kopyala
        extraHaftaPromosyonuKopyala = function () {
            $('#btnExtraHaftaPromosyonu_Kopyala').on('click', function () {
                alert('Adnan');
            });
        };

        //Yüzdesel indirim Promosyonu kopyala
        yuzdeselIndirimPromosyonuKopyala = function () {
            $('#btnIndirimPromosyonu_Kopyala').on('click', function () {
                alert('Adnan');
            });
        };

        //Fiyat Bazlı İndirim Promosyonu Kopyala
        kursBazliPromosyonKopyala = function () {
            $('#btnKursBazliPromosyon_Kopyala').on('click', function () {
                alert('Adnan');
            });

        };

        //Default seçili bir kurstipi var ise
        var kursTipiId = $('#kursTipi').val();
        if (kursTipiId !== '') {
            //alert(kursTipiId);
            $('#kursTipi').trigger('change');

        }
    };


    // Ekstra Hafta Promosyonu Güncelle
    var ekstraHaftaPromosyonuGuncelle = function () {
        $('#btnEkstraHaftaPromosyonu_Guncelle').on('click', function () {
            var cnt = confirm('Tüm promosyon girişleri güncellenecek.\nOnaylıyor musunuz?');
            if (cnt) {

                var kursTipiId = $('#kursTipi').val();
                var promosyonTipiId = $(this).attr('data-promosyontipiid');


                $("#standartHafta").find('.fieldset').each(function (i) {
                    var $fieldset = $(this);

                    var id = $('input:hidden:eq(0)', $fieldset).val();
                    var extraHafta_MinimumHafta = $('input:text:eq(0)', $fieldset).val();
                    var extraHafta_UcretsizHaftaSayisi = $('input:text:eq(1)', $fieldset).val();
                    var promosyonBitisTarihi = $('input:text:eq(2)', $fieldset).val();

                    if (id !== '' && extraHafta_MinimumHafta !== '' && extraHafta_UcretsizHaftaSayisi !== '' && promosyonBitisTarihi !== '') {
                        //Ajax Start
                        $.ajax({
                            type: "POST",
                            url: "PromosyonTipiFiyatiGuncelle",
                            data: {
                                id: id,
                                kursTipiId: kursTipiId,
                                promosyonTipiId: promosyonTipiId,

                                extraHafta_MinimumHafta: extraHafta_MinimumHafta,
                                extraHafta_UcretsizHaftaSayisi: extraHafta_UcretsizHaftaSayisi,
                                promosyonBitisTarihi: promosyonBitisTarihi
                            },
                            dataType: "json",
                            async: false,
                            success: function (data) {
                                GeneralObj.DilokuluMessageBox.init({ status: 'ok', message: 'Kayıt güncellendi.' });
                            }
                        });
                        //Ajax End
                    }
                });

                //myFiyatlandirmaListesiniGetir(fiyatlandirmaTipiId, kursTipiId);
            }
        });
    };

    //Yüzdesel indirim Promosyonu Guncelle
    var yuzdeselIndirimPromosyonGuncelle = function () {
        $('#btnYuzdeselIndirimPromosyon_Guncelle').on('click', function () {

            var cnt = confirm('Tüm promosyon girişleri güncellenecek.\nOnaylıyor musunuz?');
            if (cnt) {

                var kursTipiId = $('#kursTipi').val();
                var promosyonTipiId = $(this).attr('data-promosyontipiid');


                $("#standartHafta").find('.fieldset').each(function (i) {
                    var $fieldset = $(this);

                    var id = $('input:hidden:eq(0)', $fieldset).val();
                    var indirimPromosyonu_Hafta = $('input:text:eq(0)', $fieldset).val();
                    var indirimPromosyonu_HaftaMax = $('input:text:eq(1)', $fieldset).val();
                    var indirimPromosyonu_Oran = $('input:text:eq(2)', $fieldset).val();
                    var promosyonBitisTarihi = $('input:text:eq(3)', $fieldset).val();
                    var kursTarihiBaslangic = $('input:text:eq(4)', $fieldset).val();
                    var kursTarihiBitis = $('input:text:eq(5)', $fieldset).val();

                    if (id !== '' && indirimPromosyonu_Hafta !== '' && indirimPromosyonu_HaftaMax !== '' && indirimPromosyonu_Oran !== '' && promosyonBitisTarihi !== '') {
                        //Ajax Start
                        $.ajax({
                            type: "POST",
                            url: "PromosyonTipiFiyatiGuncelle",
                            data: {
                                id: id,
                                kursTipiId: kursTipiId,
                                promosyonTipiId: promosyonTipiId,
                                indirimPromosyonu_Hafta: indirimPromosyonu_Hafta,
                                indirimPromosyonu_HaftaMax: indirimPromosyonu_HaftaMax,
                                indirimPromosyonu_Oran: indirimPromosyonu_Oran,
                                promosyonBitisTarihi: promosyonBitisTarihi,
                                kursTarihiBaslangic: kursTarihiBaslangic,
                                kursTarihiBitis: kursTarihiBitis
                            },
                            dataType: "json",
                            async: false,
                            success: function (data) {
                                GeneralObj.DilokuluMessageBox.init({ status: 'ok', message: 'Kayıt güncellendi.' });
                            }
                        });
                        //Ajax End
                    }
                });

                //myFiyatlandirmaListesiniGetir(fiyatlandirmaTipiId, kursTipiId);
            }
        });
    };

    // Fiyat Bazli Indirim Promosyonu Güncelle
    var fiyatBazliIndirimPromosyonuGuncelle = function () {
        $('#btnFiyatBazliIndirimPromosyonu_Guncelle').on('click', function () {
            var cnt = confirm('Tüm promosyon girişleri güncellenecek.\nOnaylıyor musunuz?');
            if (cnt) {

                var kursTipiId = $('#kursTipi').val();
                var promosyonTipiId = $(this).attr('data-promosyontipiid');


                $("#standartHafta").find('.fieldset').each(function (i) {
                    var $fieldset = $(this);

                    var id = $('input:hidden:eq(0)', $fieldset).val();
                    var kursBazli_Hafta = $('input:text:eq(0)', $fieldset).val();
                    var kursBazli_Fiyat = $('input:text:eq(1)', $fieldset).val();
                    var promosyonBitisTarihi = $('input:text:eq(2)', $fieldset).val();

                    if (id !== '' && kursBazli_Hafta !== '' && kursBazli_Fiyat !== '' && promosyonBitisTarihi !== '') {
                        //Ajax Start
                        $.ajax({
                            type: "POST",
                            url: "PromosyonTipiFiyatiGuncelle",
                            data: {
                                id: id,
                                kursTipiId: kursTipiId,
                                promosyonTipiId: promosyonTipiId,

                                kursBazli_Hafta: kursBazli_Hafta,
                                kursBazli_Fiyat: kursBazli_Fiyat,
                                promosyonBitisTarihi: promosyonBitisTarihi
                            },
                            dataType: "json",
                            async: false,
                            success: function (data) {
                                GeneralObj.DilokuluMessageBox.init({ status: 'ok', message: 'Kayıt güncellendi.' });
                            }
                        });
                        //Ajax End
                    }
                });

                //myFiyatlandirmaListesiniGetir(fiyatlandirmaTipiId, kursTipiId);
            }
        });
    };

    //Indirimli Birim Fiyat Promosyonu Guncelle
    var indirimliBirimFiyatPromosyonuGuncelle = function () {
        $('#btnIndirimliBirimFiyatPromosyonu_Guncelle').on('click', function () {

            var cnt = confirm('Tüm promosyon girişleri güncellenecek.\nOnaylıyor musunuz?');
            if (cnt) {

                var kursTipiId = $('#kursTipi').val();
                var promosyonTipiId = $(this).attr('data-promosyontipiid');

                $("#standartHafta").find('.fieldset').each(function (i) {
                    var $fieldset = $(this);

                    var id = $('input:hidden:eq(0)', $fieldset).val();
                    var indirimPromosyonu_Hafta = $('input:text:eq(0)', $fieldset).val();
                    var indirimPromosyonu_HaftaMax = $('input:text:eq(1)', $fieldset).val();
                    var indirimPromosyonu_BirimFiyat = $('input:text:eq(2)', $fieldset).val();
                    var promosyonBitisTarihi = $('input:text:eq(3)', $fieldset).val();
                    var kursTarihiBaslangic = $('input:text:eq(4)', $fieldset).val();
                    var kursTarihiBitis = $('input:text:eq(5)', $fieldset).val();

                    if (id !== '' && indirimPromosyonu_Hafta !== '' && indirimPromosyonu_HaftaMax !== '' && indirimPromosyonu_BirimFiyat !== '' && promosyonBitisTarihi !== '') {
                        //Ajax Start
                        $.ajax({
                            type: "POST",
                            url: "PromosyonTipiFiyatiGuncelle",
                            data: {
                                id: id,
                                kursTipiId: kursTipiId,
                                promosyonTipiId: promosyonTipiId,
                                indirimPromosyonu_Hafta: indirimPromosyonu_Hafta,
                                indirimPromosyonu_HaftaMax: indirimPromosyonu_HaftaMax,
                                indirimPromosyonu_BirimFiyat: indirimPromosyonu_BirimFiyat,
                                promosyonBitisTarihi: promosyonBitisTarihi,
                                kursTarihiBaslangic: kursTarihiBaslangic,
                                kursTarihiBitis: kursTarihiBitis
                            },
                            dataType: "json",
                            async: false,
                            success: function (data) {
                                GeneralObj.DilokuluMessageBox.init({ status: 'ok', message: 'Kayıt güncellendi.' });
                            }
                        });
                        //Ajax End
                    }
                });

                //myFiyatlandirmaListesiniGetir(fiyatlandirmaTipiId, kursTipiId);
            }
        });
    };


    //Fiyatlandırma Listesini Getir
    var myPromosyonListesiniGetir = function (promosyonTipiId, kursTipiId) {

        Loader();

        $('#promosyonTanimiHaftaTanimlari').html('');
        //Ajax Start
        $.ajax({
            type: "GET",
            url: "PromosyonTanimlari_PromosyonTanimlari",
            data: { promosyonTipiId: promosyonTipiId, kursTipiId: kursTipiId },
            //dataType: "json",
            success: function (data) {
                if (data !== undefined && data !== null) {
                    $('#promosyonTanimiHaftaTanimlari').html(data);

                    //Button Eventlarını görebilmesi için yapıldı
                    myExtraHaftaEkle();
                    myIndirimPromosyonuEkle();
                    myIndirimliBirimFiyatPromosyonuEkle();
                    myKursBazliEkle();
                    //Güncellemeler
                    ekstraHaftaPromosyonuGuncelle();
                    yuzdeselIndirimPromosyonGuncelle();
                    fiyatBazliIndirimPromosyonuGuncelle();
                    indirimliBirimFiyatPromosyonuGuncelle();

                    tarihAyarla('.promosyonBitisTarihi');

                } else {
                    $('#promosyonTanimiHaftaTanimlari').html('');
                }
                closeModal();
            },
            error: function () {
                closeModal();
            }
        });
        //Ajax End
    };

    var tarihAyarla = function (controller) {


        var cnt = controller;

        $(controller).datepicker({
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 3,
            showWeek: true,
            minDate: 0
        });
    };

    return {
        init: myInit,
        promosyonListesiniGetir: myPromosyonListesiniGetir,
        tarihGoster: tarihAyarla
    };
})();

function KursTipiPromosyonSil(element) {
    var kursTipiId = $('#kursTipi').val();
    var promosyonTipiId = $(element).attr('data-promosyontipiid');
    var kursTipiFiyatId = $(element).attr('data-fiyatid');

    var cnt = confirm('Kayıt silinecek!\nOnaylıyor musunuz?')
    if (cnt) {

        Loader();

        //Ajax Start
        $.ajax({
            type: "POST",
            url: "KursTipiPromosyonSil",
            async: false,
            data: { kursTipiId: kursTipiId, promosyonTipiId: promosyonTipiId, kursTipiFiyatId: kursTipiFiyatId },
            dataType: "json",
            success: function (data) {
                if (data.status == "ok") {
                    GeneralObj.DilokuluMessageBox.init({ status: 'ok', message: 'Kayıt silindi.' });

                    promosyonTanimi.promosyonListesiniGetir(promosyonTipiId, kursTipiId);

                    closeModal();
                } else {
                    GeneralObj.DilokuluMessageBox.init({ status: 'error', message: data.message });
                    closeModal();
                }
            }
        });
        //Ajax End
    }

}
//Promosyon Tanimlari End

//Okul Ek Ücretler

var okulEkUcretHavaAlaniArray = [];
var okulEkUcretOkulKayitUcretiArray = [];
var okulEkUcretOkulKayitEkUcretArray = [];
var okulEkUcretKonaklamaAyarlamaUcretiArray = [];
var okulEkUcretKonaklamaAyarlamaEkUcretArray = [];
var okulEkUcretMateryalUcretiArray = [];
var okulEkUcretSaglikSigortasiUcretiArray = [];
var ucretler_KargoHariciKurslar = [];

var okulEkUcretler = (function () {
    var myInit = function () {
        $('#btnHavaAlaniEkle').click(function () {
            var havaAlani = $('#Ucretler_HavaAlaniAdi').val();
            var ucret = parseFloat($('#Ucretler_HavaAlaniUcreti').val());
            if (havaAlani !== '' && !isNaN(ucret)) {
                okulEkUcretHavaAlaniArray.push({ 'havaAlani': havaAlani, 'ucret': ucret });
                myHavaAlaniListele();

                $('#Ucretler_HavaAlaniAdi').val('');
                $('#Ucretler_HavaAlaniUcreti').val('');
            }

        });

        $('#btnUcretler_OkulKayitUcretiEkle').click(function () {

            var baslangic = $('#Ucretler_OkulKayitUcretiArray_Baslangic').val();
            var bitis = $('#Ucretler_OkulKayitUcretiArray_Bitis').val();
            var fiyat = $('#Ucretler_OkulKayitUcretiArray_Fiyat').val();

            if (baslangic !== '' && !isNaN(baslangic) && bitis !== '' && !isNaN(bitis) && fiyat !== '' && !isNaN(fiyat)) {
                okulEkUcretOkulKayitUcretiArray.push({ 'baslangic': baslangic, 'bitis': bitis, 'fiyat': fiyat });
                myOkulEkUcretListele();

                $('#Ucretler_OkulKayitUcretiArray_Baslangic').val('');
                $('#Ucretler_OkulKayitUcretiArray_Bitis').val('');
                $('#Ucretler_OkulKayitUcretiArray_Fiyat').val('');
            }
        });

        $('#btnUcretler_OkulKayitEkUcretArrayEkle').click(function () {

            var baslangic = $('#Ucretler_OkulKayitEkUcretArray_Baslangic').val();
            var bitis = $('#Ucretler_OkulKayitEkUcretArray_Bitis').val();
            var fiyat = $('#Ucretler_OkulKayitEkUcretArray_Fiyat').val();

            if (baslangic !== '' && bitis !== '' && fiyat !== '' && !isNaN(fiyat)) {
                okulEkUcretOkulKayitEkUcretArray.push({ 'baslangic': baslangic, 'bitis': bitis, 'fiyat': fiyat });
                myOkulKayitEkUcretListele();

                $('#Ucretler_OkulKayitEkUcretArray_Baslangic').val('');
                $('#Ucretler_OkulKayitEkUcretArray_Bitis').val('');
                $('#Ucretler_OkulKayitEkUcretArray_Fiyat').val('');
            }
        });

        $('#btnUcretler_KonaklamaAyarlamaUcretiEkle').click(function () {

            var baslangic = $('#Ucretler_KonaklamaAyarlamaUcretiArray_Baslangic').val();
            var bitis = $('#Ucretler_KonaklamaAyarlamaUcretiArray_Bitis').val();
            var fiyat = $('#Ucretler_KonaklamaAyarlamaUcretiArray_Fiyat').val();

            if (baslangic !== '' && !isNaN(baslangic) && bitis !== '' && !isNaN(bitis) && fiyat !== '' && !isNaN(fiyat)) {
                okulEkUcretKonaklamaAyarlamaUcretiArray.push({ 'baslangic': baslangic, 'bitis': bitis, 'fiyat': fiyat });

                myOkulEkUcretKonaklamaAyarlamaListele();

                $('#Ucretler_KonaklamaAyarlamaUcretiArray_Baslangic').val('');
                $('#Ucretler_KonaklamaAyarlamaUcretiArray_Bitis').val('');
                $('#Ucretler_KonaklamaAyarlamaUcretiArray_Fiyat').val('');
            }
        });

        $('#btnUcretler_KonaklamaAyarlamaEkUcretArrayEkle').click(function () {

            var baslangic = $('#Ucretler_KonaklamaAyarlamaEkUcretArray_Baslangic').val();
            var bitis = $('#Ucretler_KonaklamaAyarlamaEkUcretArray_Bitis').val();
            var fiyat = $('#Ucretler_KonaklamaAyarlamaEkUcretArray_Fiyat').val();

            if (baslangic !== '' && bitis !== '' && fiyat !== '' && !isNaN(fiyat)) {
                okulEkUcretKonaklamaAyarlamaEkUcretArray.push({ 'baslangic': baslangic, 'bitis': bitis, 'fiyat': fiyat });

                myOkulEkUcretKonaklamaAyarlamaEkUcretListele();

                $('#Ucretler_KonaklamaAyarlamaEkUcretArray_Baslangic').val('');
                $('#Ucretler_KonaklamaAyarlamaEkUcretArray_Bitis').val('');
                $('#Ucretler_KonaklamaAyarlamaEkUcretArray_Fiyat').val('');
            }
        });

        $('#btnUcretler_MateryalUcretiEkle').click(function () {

            var baslangic = $('#Ucretler_MateryalUcretiArray_Baslangic').val();
            var bitis = $('#Ucretler_MateryalUcretiArray_Bitis').val();
            var fiyat = $('#Ucretler_MateryalUcretiArray_Fiyat').val();

            if (baslangic !== '' && !isNaN(baslangic) && bitis !== '' && !isNaN(bitis) && fiyat !== '' && !isNaN(fiyat)) {
                okulEkUcretMateryalUcretiArray.push({ 'baslangic': baslangic, 'bitis': bitis, 'fiyat': fiyat });

                myOkulEkUcretMateryalUcretleri();

                $('#Ucretler_MateryalUcretiArray_Baslangic').val('');
                $('#Ucretler_MateryalUcretiArray_Bitis').val('');
                $('#Ucretler_MateryalUcretiArray_Fiyat').val('');
            }
        });

        $('#Ucretler_MateryalTahsilatTipi').change(function () {
            var val = $(this).val();
            if (val == 'HaftaAralikli') {
                $('#trMateryalUcretiTablo').css('display', '');
                $('#Ucretler_MateryalUcreti').attr('disabled', 'disabled');
            } else {
                $('#trMateryalUcretiTablo').css('display', 'none');
                $('#Ucretler_MateryalUcreti').removeAttr('disabled');
            }

        });

        $('#btnUcretler_SaglikSigortasiUcretiEkle').click(function () {

            var baslangic = $('#Ucretler_SaglikSigortasiArray_Baslangic').val();
            var bitis = $('#Ucretler_SaglikSigortasiArray_Bitis').val();
            var fiyat = $('#Ucretler_SaglikSigortasiArray_Fiyat').val();

            if (baslangic !== '' && !isNaN(baslangic) && bitis !== '' && !isNaN(bitis) && fiyat !== '' && !isNaN(fiyat)) {
                okulEkUcretSaglikSigortasiUcretiArray.push({ 'baslangic': baslangic, 'bitis': bitis, 'fiyat': fiyat });

                myOkulEkUcretSaglikSigortasiUcretleri();

                $('#Ucretler_SaglikSigortasiArray_Baslangic').val('');
                $('#Ucretler_SaglikSigortasiArray_Bitis').val('');
                $('#Ucretler_SaglikSigortasiArray_Fiyat').val('');
            }
        });

        $('#Ucretler_SaglikSigortasiTahsilatTipi').change(function () {
            var val = $(this).val();
            if (val == 'HaftaAralikli') {
                $('#trSaglikSigortasiUcretleriTablo').css('display', '');
                $('#Ucretler_SaglikSigortasi').attr('disabled', 'disabled');
            } else {
                $('#trSaglikSigortasiUcretleriTablo').css('display', 'none');
                $('#Ucretler_SaglikSigortasi').removeAttr('disabled');
            }

        });

        ucretler_KargoHariciKurslar = JSON.parse($("#Ucretler_KargoHariciKurslar").val());

        //Checkbox lar seçili hale getiriliyor
        if (ucretler_KargoHariciKurslar !== null && ucretler_KargoHariciKurslar.length > 0) {
            $.each(ucretler_KargoHariciKurslar, function () {
                $('#cbxKursTipi_' + this.kursTipiId).attr("checked", "checked");
            })

        }


        $('.kargoHaricKurslar').click(function () {

            var val = $(this).val();
            var okulId = $(this).attr("data-okulid");
            var kursTipiId = $(this).attr("data-id");
            var isChecked = $(this).is(':checked');

            if (ucretler_KargoHariciKurslar.length > 0) {
                var isAvailable = false;

                for (var i = 0; i < ucretler_KargoHariciKurslar.length; i++) {
                    if (ucretler_KargoHariciKurslar[i].kursTipiId == kursTipiId) {
                        isAvailable = true;
                        ucretler_KargoHariciKurslar[i].selected = isChecked;
                        break;
                    }
                }

                if (!isAvailable) {
                    ucretler_KargoHariciKurslar.push({ 'okulId': okulId, 'kursTipiId': kursTipiId, 'selected': isChecked });
                }


            } else {
                ucretler_KargoHariciKurslar.push({ 'okulId': okulId, 'kursTipiId': kursTipiId, 'selected': isChecked });
            }

            $("#Ucretler_KargoHariciKurslar").val(JSON.stringify(ucretler_KargoHariciKurslar));

        });


    };

    //Ek Ücret Havaalanı işlemleri
    var myHavaAlaniKarsilamaAta = function (msg) {
        var havaAlaniListe = $('#Ucretler_HavaAlaniKarsilama').val();
        if (havaAlaniListe.length > 0) {
            var listeArray = havaAlaniListe.split("|");
            for (var i = 0; i < listeArray.length; i++) {

                okulEkUcretHavaAlaniArray.push({ 'havaAlani': listeArray[i].split(";")[0], 'ucret': listeArray[i].split(";")[1] });
            }

            myHavaAlaniListele();
        }
    };

    var myHavaAlaniListele = function () {
        if (okulEkUcretHavaAlaniArray !== null && okulEkUcretHavaAlaniArray.length > 0) {

            $('.tblHavaAlanlari').find("thead").remove();
            $('.tblHavaAlanlari').find("tbody").remove();


            var liste = '';
            var strHavaAlanlari = '';

            var tblHeader = '<thead><tr><th>HAVA ALANI</th><th>FİYATI</th><th>#</th></tr></thead>';
            tblHeader += '<tbody>';
            $.each(okulEkUcretHavaAlaniArray, function () {
                liste += '<tr><td class="baslik">' + this.havaAlani + '</td><td>' + this.ucret + '</td><td><a href="javascript:;" onclick="okulEkUcretler.havaAlaniCikart(\'' + this.havaAlani + '\',\'' + this.ucret + '\');" >Sil</a></td></tr>';

                //Ucretler_HavaAlaniKarsilama field için
                strHavaAlanlari += this.havaAlani + ';' + this.ucret + '|';
                //Ucretler_HavaAlaniKarsilama field için
            });
            tblHeader += '</tbody>';
            if (liste !== '') {
                $('.tblHavaAlanlari').append(tblHeader);
                $('.tblHavaAlanlari').append(liste);

                //Ucretler_HavaAlaniKarsilama doldur

                $('#Ucretler_HavaAlaniKarsilama').val(strHavaAlanlari);

                //Ucretler_HavaAlaniKarsilama doldur

            }
        } else {

            $('.tblHavaAlanlari').find("thead").remove();
            $('.tblHavaAlanlari').find("tbody").remove();

            $('#Ucretler_HavaAlaniKarsilama').val('');
        }
    };

    var myHavaAlaniCikart = function (havaAlani, ucret) {
        var cnt = confirm('Hava alanı listeden çıkartılacak!\nOnaylıyor musunuz?')
        if (cnt) {
            if (okulEkUcretHavaAlaniArray !== null && okulEkUcretHavaAlaniArray.length > 0) {

                okulEkUcretHavaAlaniArray = okulEkUcretHavaAlaniArray.filter(function (el) {
                    return el.havaAlani !== havaAlani;
                });

                myHavaAlaniListele();
            }

        }
    };
    //Ek Ücret Havaalanı işlemleri

    //Ek Ücret Okul Kayıt Ücreti işlemleri
    var myOkulEkUcretAta = function () {
        var fiyatListe = $('#Ucretler_OkulKayitUcretiArray').val();
        if (fiyatListe.length > 0) {
            var listeArray = fiyatListe.split("|");
            for (var i = 0; i < listeArray.length; i++) {

                okulEkUcretOkulKayitUcretiArray.push({ 'baslangic': listeArray[i].split(";")[0], 'bitis': listeArray[i].split(";")[1], 'fiyat': listeArray[i].split(";")[2] });
            }

            myOkulEkUcretListele();
        }
    };

    var myOkulEkUcretCikart = function (baslangic, bitis, fiyat) {
        var cnt = confirm('Okul kayıt ücretini listeden çıkartılacak!\nOnaylıyor musunuz?')
        if (cnt) {
            if (okulEkUcretOkulKayitUcretiArray !== null && okulEkUcretOkulKayitUcretiArray.length > 0) {

                okulEkUcretOkulKayitUcretiArray = okulEkUcretOkulKayitUcretiArray.filter(function (el) {
                    return el.fiyat !== fiyat;
                });

                myOkulEkUcretListele();
            }

        }
    };

    var myOkulEkUcretListele = function () {

        if (okulEkUcretOkulKayitUcretiArray !== null && okulEkUcretOkulKayitUcretiArray.length > 0) {

            $('.tblOkulKayitUcretleri').find("tbody").remove();
            var liste = '';
            var strfiyatListesi = '';

            var tblHeader = '';
            tblHeader += '<tbody>';
            $.each(okulEkUcretOkulKayitUcretiArray, function () {
                liste += '<tr><td style="padding:0px;"><span class="TextBox" style="width:46px;display:block;">' + this.baslangic + '</span></td>';
                liste += '<td style="padding:0px;"><span class="TextBox" style="width:46px;display:block;">' + this.bitis + '</span></td>';
                liste += '<td style="padding:0px;"><span class="TextBox" style="width:46px;display:block;">' + this.fiyat + '</span></td>';
                liste += '<td style="padding:0px;"><a href="javascript:;" onclick="okulEkUcretler.okulEkUcretCikart(\'' + this.baslangic + '\',\'' + this.bitis + '\',\'' + this.fiyat + '\');">Sil</a></td></tr>';
                //Ucretler_OkulKayitUcretiArray field için
                strfiyatListesi += this.baslangic + ';' + this.bitis + ';' + this.fiyat + '|';
                //Ucretler_OkulKayitUcretiArray field için
            });
            tblHeader += '</tbody>';
            if (liste !== '') {
                $('.tblOkulKayitUcretleri').append(liste);

                //Ucretler_OkulKayitUcretiArray doldur

                $('#Ucretler_OkulKayitUcretiArray').val(strfiyatListesi);

                //Ucretler_OkulKayitUcretiArray doldur

            }
        } else {

            $('.tblOkulKayitUcretleri').find("tbody").remove();

            $('#Ucretler_OkulKayitUcretiArray').val('');
        }
    };
    //Ek Ücret Okul Kayıt Ücreti işlemleri

    //Ek Ücret Okul Kayıt Ek Ücret işlemleri
    var myOkulKayitEkUcretAta = function () {
        var fiyatListe = $('#Ucretler_OkulKayitEkUcretArray').val();
        if (fiyatListe.length > 0) {
            var listeArray = fiyatListe.split("|");
            for (var i = 0; i < listeArray.length; i++) {

                okulEkUcretOkulKayitEkUcretArray.push({ 'baslangic': listeArray[i].split(";")[0], 'bitis': listeArray[i].split(";")[1], 'fiyat': listeArray[i].split(";")[2] });
            }

            myOkulKayitEkUcretListele();
        }
    };

    var myOkulKayitEkUcretCikart = function (baslangic, bitis, fiyat) {
        var cnt = confirm('Okul kayıt ek ücretini listeden çıkartılacak!\nOnaylıyor musunuz?')
        if (cnt) {
            if (okulEkUcretOkulKayitEkUcretArray !== null && okulEkUcretOkulKayitEkUcretArray.length > 0) {

                okulEkUcretOkulKayitEkUcretArray = okulEkUcretOkulKayitEkUcretArray.filter(function (el) {
                    return el.fiyat !== fiyat;
                });

                myOkulKayitEkUcretListele();
            }

        }
    };

    var myOkulKayitEkUcretListele = function () {

        if (okulEkUcretOkulKayitEkUcretArray !== null && okulEkUcretOkulKayitEkUcretArray.length > 0) {

            $('.tblOkulKayitEkUcretler').find("tbody").remove();
            var liste = '';
            var strfiyatListesi = '';

            var tblHeader = '';
            tblHeader += '<tbody>';
            $.each(okulEkUcretOkulKayitEkUcretArray, function () {
                liste += '<tr><td style="padding:0px;"><span class="TextBox" style="width:66px;display:block;">' + this.baslangic + '</span></td>';
                liste += '<td style="padding:0px;"><span class="TextBox" style="width:66px;display:block;">' + this.bitis + '</span></td>';
                liste += '<td style="padding:0px;"><span class="TextBox" style="width:46px;display:block;">' + this.fiyat + '</span></td>';
                liste += '<td style="padding:0px;"><a href="javascript:;" onclick="okulEkUcretler.okulKayitEkUcretCikart(\'' + this.baslangic + '\',\'' + this.bitis + '\',\'' + this.fiyat + '\');">Sil</a></td></tr>';
                //Ucretler_OkulKayitUcretiArray field için
                strfiyatListesi += this.baslangic + ';' + this.bitis + ';' + this.fiyat + '|';
                //Ucretler_OkulKayitUcretiArray field için
            });
            tblHeader += '</tbody>';
            if (liste !== '') {
                $('.tblOkulKayitEkUcretler').append(liste);

                //Ucretler_OkulKayitEkUcretArray doldur

                $('#Ucretler_OkulKayitEkUcretArray').val(strfiyatListesi);

                //Ucretler_OkulKayitEkUcretArray doldur

            }
        } else {

            $('.tblOkulKayitEkUcretler').find("tbody").remove();

            $('#Ucretler_OkulKayitEkUcretArray').val('');
        }
    };
    //Ek Ücret Okul Kayıt Ek Ücret işlemleri

    //Ek Ücret Konaklama Ayarlama Ücreti işlemleri
    var myOkulEkUcretKonaklamaAta = function () {
        var fiyatListe = $('#Ucretler_KonaklamaAyarlamaUcretiArray').val();
        if (fiyatListe.length > 0) {
            var listeArray = fiyatListe.split("|");
            for (var i = 0; i < listeArray.length; i++) {

                okulEkUcretKonaklamaAyarlamaUcretiArray.push({ 'baslangic': listeArray[i].split(";")[0], 'bitis': listeArray[i].split(";")[1], 'fiyat': listeArray[i].split(";")[2] });
            }

            myOkulEkUcretKonaklamaAyarlamaListele();
        }
    };

    var myOkulEkUcretKonaklamaCikart = function (baslangic, bitis, fiyat) {
        var cnt = confirm('Konaklama ücreti listeden çıkartılacak!\nOnaylıyor musunuz?')
        if (cnt) {
            if (okulEkUcretKonaklamaAyarlamaUcretiArray !== null && okulEkUcretKonaklamaAyarlamaUcretiArray.length > 0) {

                okulEkUcretKonaklamaAyarlamaUcretiArray = okulEkUcretKonaklamaAyarlamaUcretiArray.filter(function (el) {
                    return el.fiyat !== fiyat;
                });

                myOkulEkUcretKonaklamaAyarlamaListele();
            }

        }
    };

    var myOkulEkUcretKonaklamaAyarlamaListele = function () {

        if (okulEkUcretKonaklamaAyarlamaUcretiArray !== null && okulEkUcretKonaklamaAyarlamaUcretiArray.length > 0) {

            $('.tblKonaklamaAyarlamaUcretleri').find("tbody").remove();
            var liste = '';
            var strfiyatListesi = '';

            var tblHeader = '';
            tblHeader += '<tbody>';
            $.each(okulEkUcretKonaklamaAyarlamaUcretiArray, function () {
                liste += '<tr><td style="padding:0px;"><span class="TextBox" style="width:46px;display:block;">' + this.baslangic + '</span></td>';
                liste += '<td style="padding:0px;"><span class="TextBox" style="width:46px;display:block;">' + this.bitis + '</span></td>';
                liste += '<td style="padding:0px;"><span class="TextBox" style="width:46px;display:block;">' + this.fiyat + '</span></td>';
                liste += '<td style="padding:0px;"><a href="javascript:;" onclick="okulEkUcretler.okulEkUcretKonaklamaCikart(\'' + this.baslangic + '\',\'' + this.bitis + '\',\'' + this.fiyat + '\');">Sil</a></td></tr>';
                //Ucretler_KonaklamaAyarlamaUcretiArray field için
                strfiyatListesi += this.baslangic + ';' + this.bitis + ';' + this.fiyat + '|';
                //Ucretler_KonaklamaAyarlamaUcretiArray field için
            });
            tblHeader += '</tbody>';
            if (liste !== '') {
                $('.tblKonaklamaAyarlamaUcretleri').append(liste);

                //Ucretler_OkulKayitUcretiArray doldur
                $('#Ucretler_KonaklamaAyarlamaUcretiArray').val(strfiyatListesi);
                //Ucretler_OkulKayitUcretiArray doldur

            }
        } else {

            $('.tblKonaklamaAyarlamaUcretleri').find("tbody").remove();
            $('#Ucretler_KonaklamaAyarlamaUcretiArray').val('');
        }
    };
    //Ek Ücret Konaklama Ayarlama Ücreti işlemleri

    //Ek Ücret Konaklama Ayarlama Ek Ücret işlemleri
    var myOkulEkUcretKonaklamaEkUcretAta = function () {
        var fiyatListe = $('#Ucretler_KonaklamaAyarlamaEkUcretArray').val();
        if (fiyatListe.length > 0) {
            var listeArray = fiyatListe.split("|");
            for (var i = 0; i < listeArray.length; i++) {

                okulEkUcretKonaklamaAyarlamaEkUcretArray.push({ 'baslangic': listeArray[i].split(";")[0], 'bitis': listeArray[i].split(";")[1], 'fiyat': listeArray[i].split(";")[2] });
            }

            myOkulEkUcretKonaklamaAyarlamaEkUcretListele();
        }
    };

    var myOkulEkUcretKonaklamaEkUcretCikart = function (baslangic, bitis, fiyat) {
        var cnt = confirm('Konaklama ayarlama ek ücreti listeden çıkartılacak!\nOnaylıyor musunuz?')
        if (cnt) {
            if (okulEkUcretKonaklamaAyarlamaEkUcretArray !== null && okulEkUcretKonaklamaAyarlamaEkUcretArray.length > 0) {

                okulEkUcretKonaklamaAyarlamaEkUcretArray = okulEkUcretKonaklamaAyarlamaEkUcretArray.filter(function (el) {
                    return el.fiyat !== fiyat;
                });

                myOkulEkUcretKonaklamaAyarlamaEkUcretListele();
            }

        }
    };

    var myOkulEkUcretKonaklamaAyarlamaEkUcretListele = function () {

        if (okulEkUcretKonaklamaAyarlamaEkUcretArray !== null && okulEkUcretKonaklamaAyarlamaEkUcretArray.length > 0) {

            $('.tblKonaklamaAyarlamaEkUcretler').find("tbody").remove();
            var liste = '';
            var strfiyatListesi = '';

            var tblHeader = '';
            tblHeader += '<tbody>';
            $.each(okulEkUcretKonaklamaAyarlamaEkUcretArray, function () {
                liste += '<tr><td style="padding:0px;"><span class="TextBox" style="width:66px;display:block;">' + this.baslangic + '</span></td>';
                liste += '<td style="padding:0px;"><span class="TextBox" style="width:66px;display:block;">' + this.bitis + '</span></td>';
                liste += '<td style="padding:0px;"><span class="TextBox" style="width:46px;display:block;">' + this.fiyat + '</span></td>';
                liste += '<td style="padding:0px;"><a href="javascript:;" onclick="okulEkUcretler.okulEkUcretKonaklamaEkUcretCikart(\'' + this.baslangic + '\',\'' + this.bitis + '\',\'' + this.fiyat + '\');">Sil</a></td></tr>';
                //Ucretler_KonaklamaAyarlamaEkUcretArray field için
                strfiyatListesi += this.baslangic + ';' + this.bitis + ';' + this.fiyat + '|';
                //Ucretler_KonaklamaAyarlamaEkUcretArray field için
            });
            tblHeader += '</tbody>';
            if (liste !== '') {
                $('.tblKonaklamaAyarlamaEkUcretler').append(liste);

                //Ucretler_OkulKayitUcretiArray doldur
                $('#Ucretler_KonaklamaAyarlamaEkUcretArray').val(strfiyatListesi);
                //Ucretler_OkulKayitUcretiArray doldur

            }
        } else {

            $('.tblKonaklamaAyarlamaEkUcretler').find("tbody").remove();
            $('#Ucretler_KonaklamaAyarlamaEkUcretArray').val('');
        }
    };
    //Ek Ücret Konaklama Ayarlama Ek Ücret işlemleri

    //Ek Ücret Materyal Ücreti işlemleri
    var myOkulEkUcretMateryalUcretleriniAta = function () {

        var tahsilatTipi = $('#Ucretler_MateryalTahsilatTipi').val();

        if (tahsilatTipi == 'HaftaAralikli') {

            $('#trMateryalUcretiTablo').css('display', '');
            $('#Ucretler_MateryalUcreti').attr('disabled', 'disabled');

            var fiyatListe = $('#Ucretler_MateryalUcretiArray').val();
            if (fiyatListe.length > 0) {
                var listeArray = fiyatListe.split("|");
                for (var i = 0; i < listeArray.length; i++) {

                    okulEkUcretMateryalUcretiArray.push({ 'baslangic': listeArray[i].split(";")[0], 'bitis': listeArray[i].split(";")[1], 'fiyat': listeArray[i].split(";")[2] });
                }

                myOkulEkUcretMateryalUcretleri();
            }
        } else {
            $('#trMateryalUcretiTablo').css('display', 'none');
            $('#Ucretler_MateryalUcreti').removeAttr('disabled');
        }
    };

    var myOkulEkUcretMateryalCikart = function (baslangic, bitis, fiyat) {
        var cnt = confirm('Materyal ücreti listeden çıkartılacak!\nOnaylıyor musunuz?')
        if (cnt) {
            if (okulEkUcretMateryalUcretiArray !== null && okulEkUcretMateryalUcretiArray.length > 0) {

                okulEkUcretMateryalUcretiArray = okulEkUcretMateryalUcretiArray.filter(function (el) {
                    return el.fiyat !== fiyat;
                });

                myOkulEkUcretMateryalUcretleri();
            }

        }
    };

    var myOkulEkUcretMateryalUcretleri = function () {

        if (okulEkUcretMateryalUcretiArray !== null && okulEkUcretMateryalUcretiArray.length > 0) {

            $('.tblMateryalUcretleri').find("tbody").remove();
            var liste = '';
            var strfiyatListesi = '';

            var tblHeader = '';
            tblHeader += '<tbody>';
            $.each(okulEkUcretMateryalUcretiArray, function () {
                liste += '<tr><td style="padding:0px;"><span class="TextBox" style="width:46px;display:block;">' + this.baslangic + '</span></td>';
                liste += '<td style="padding:0px;"><span class="TextBox" style="width:46px;display:block;">' + this.bitis + '</span></td>';
                liste += '<td style="padding:0px;"><span class="TextBox" style="width:46px;display:block;">' + this.fiyat + '</span></td>';
                liste += '<td style="padding:0px;"><a href="javascript:;" onclick="okulEkUcretler.okulEkUcretMateryalCikart(\'' + this.baslangic + '\',\'' + this.bitis + '\',\'' + this.fiyat + '\');">Sil</a></td></tr>';
                //Ucretler_KonaklamaAyarlamaUcretiArray field için
                strfiyatListesi += this.baslangic + ';' + this.bitis + ';' + this.fiyat + '|';
                //Ucretler_KonaklamaAyarlamaUcretiArray field için
            });
            tblHeader += '</tbody>';
            if (liste !== '') {
                $('.tblMateryalUcretleri').append(liste);

                //Ucretler_OkulKayitUcretiArray doldur
                $('#Ucretler_MateryalUcretiArray').val(strfiyatListesi);
                //Ucretler_OkulKayitUcretiArray doldur

            }
        } else {

            $('.tblMateryalUcretleri').find("tbody").remove();
            $('#Ucretler_MateryalUcretiArray').val('');
        }
    };
    //Ek Ücret Materyal Ücreti işlemleri

    //Ek Ücret Sağlık Sigortası Ücreti işlemleri
    var myOkulEkUcretSaglikSigortasiUcretleriniAta = function () {

        var tahsilatTipi = $('#Ucretler_SaglikSigortasiTahsilatTipi').val();

        if (tahsilatTipi == 'HaftaAralikli') {

            $('#trSaglikSigortasiUcretleriTablo').css('display', '');
            $('#Ucretler_SaglikSigortasi').attr('disabled', 'disabled');

            var fiyatListe = $('#Ucretler_SaglikSigortasiArray').val();
            if (fiyatListe.length > 0) {
                var listeArray = fiyatListe.split("|");
                for (var i = 0; i < listeArray.length; i++) {

                    okulEkUcretSaglikSigortasiUcretiArray.push({ 'baslangic': listeArray[i].split(";")[0], 'bitis': listeArray[i].split(";")[1], 'fiyat': listeArray[i].split(";")[2] });
                }

                myOkulEkUcretSaglikSigortasiUcretleri();
            }
        } else {
            $('#trSaglikSigortasiUcretleriTablo').css('display', 'none');
            $('#Ucretler_SaglikSigortasi').removeAttr('disabled');
        }
    };

    var myOkulEkUcretSaglikSigortasiCikart = function (baslangic, bitis, fiyat) {
        var cnt = confirm('Sağlık sigortası ücreti listeden çıkartılacak!\nOnaylıyor musunuz?')
        if (cnt) {
            if (okulEkUcretSaglikSigortasiUcretiArray !== null && okulEkUcretSaglikSigortasiUcretiArray.length > 0) {

                okulEkUcretSaglikSigortasiUcretiArray = okulEkUcretSaglikSigortasiUcretiArray.filter(function (el) {
                    return el.fiyat !== fiyat;
                });

                myOkulEkUcretSaglikSigortasiUcretleri();
            }

        }
    };

    var myOkulEkUcretSaglikSigortasiUcretleri = function () {

        if (okulEkUcretSaglikSigortasiUcretiArray !== null && okulEkUcretSaglikSigortasiUcretiArray.length > 0) {

            $('.tblSaglikSigortasiUcretleri').find("tbody").remove();
            var liste = '';
            var strfiyatListesi = '';

            var tblHeader = '';
            tblHeader += '<tbody>';
            $.each(okulEkUcretSaglikSigortasiUcretiArray, function () {
                liste += '<tr><td style="padding:0px;"><span class="TextBox" style="width:46px;display:block;">' + this.baslangic + '</span></td>';
                liste += '<td style="padding:0px;"><span class="TextBox" style="width:46px;display:block;">' + this.bitis + '</span></td>';
                liste += '<td style="padding:0px;"><span class="TextBox" style="width:46px;display:block;">' + this.fiyat + '</span></td>';
                liste += '<td style="padding:0px;"><a href="javascript:;" onclick="okulEkUcretler.okulEkUcretSaglikSigortasiCikart(\'' + this.baslangic + '\',\'' + this.bitis + '\',\'' + this.fiyat + '\');">Sil</a></td></tr>';
                //Ucretler_KonaklamaAyarlamaUcretiArray field için
                strfiyatListesi += this.baslangic + ';' + this.bitis + ';' + this.fiyat + '|';
                //Ucretler_KonaklamaAyarlamaUcretiArray field için
            });
            tblHeader += '</tbody>';
            if (liste !== '') {
                $('.tblSaglikSigortasiUcretleri').append(liste);

                //Ucretler_OkulKayitUcretiArray doldur
                $('#Ucretler_SaglikSigortasiArray').val(strfiyatListesi);
                //Ucretler_OkulKayitUcretiArray doldur

            }
        } else {

            $('.tblSaglikSigortasiUcretleri').find("tbody").remove();
            $('#Ucretler_SaglikSigortasiArray').val('');
        }
    };
    //Ek Ücret Sağlık Sigortası Ücreti işlemleri


    return {
        init: myInit,

        okulEkUcretAta: myOkulEkUcretAta,
        okulEkUcretCikart: myOkulEkUcretCikart,

        okulKayitEkUcretAta: myOkulKayitEkUcretAta,
        okulKayitEkUcretCikart: myOkulKayitEkUcretCikart,

        okulEkUcretKonaklamaAta: myOkulEkUcretKonaklamaAta,
        okulEkUcretKonaklamaCikart: myOkulEkUcretKonaklamaCikart,

        okulEkUcretKonaklamaEkUcretAta: myOkulEkUcretKonaklamaEkUcretAta,
        okulEkUcretKonaklamaEkUcretCikart: myOkulEkUcretKonaklamaEkUcretCikart,

        okulEkUcretMateryalUcretleriniAta: myOkulEkUcretMateryalUcretleriniAta,
        okulEkUcretMateryalCikart: myOkulEkUcretMateryalCikart,

        okulEkUcretSaglikSigortasiUcretleriniAta: myOkulEkUcretSaglikSigortasiUcretleriniAta,
        okulEkUcretSaglikSigortasiCikart: myOkulEkUcretSaglikSigortasiCikart,

        havaAlaniKarsilamaAta: myHavaAlaniKarsilamaAta,
        havaAlaniCikart: myHavaAlaniCikart
    }
})();
//Okul Ek Ücretler

//Summer Supplement
var summerSupplement = (function () {

    var myInit = function () {

    };

    var myKursTipiChange = function (url) {

        $('#kursTipi').change(function () {
            var kursTipiId = $(this).val();
            if (kursTipiId !== '') {
                window.location = url + '&kursTipiId=' + $(this).val();
            }
        });
    };

    var myUcretSil = function (id) {
        var cnt = confirm('Ücret girişi silinecek!\nOnaylıyor musunuz?');
        if (cnt) {

            //Ajax Start
            $.ajax({
                type: "POST",
                url: "KursEkUcretSil",
                async: false,
                data: { id: id },
                dataType: "json",
                success: function (data) {
                    if (data.status == "ok") {
                        window.location.reload();
                    } else {
                        GeneralObj.DilokuluMessageBox.init({ status: 'error', message: data.message });
                    }
                }
            });
            //Ajax End
        }
    };

    return {
        kursTipiChange: myKursTipiChange,
        ucretSil: myUcretSil
    }

})();
//Konaklama Ek Ücretleri

//Kopyalama
var kurstipiKopyalama = (function () {
    var myInit = function () {
        $('#mainLogo').hide();
        $('#mainMenu').hide();
        $('.MasterTable').css('width', '600px');

        $('.islemBaslik').text('Kurs Tipi Fiyat Kopyalama');

        $('#merkez').change(function () {
            var merkezId = $(this).val();

            //Ajax Start
            $.ajax({
                type: "POST",
                url: "Kopyala_OkullariGetir",
                async: false,
                data: { merkezId: merkezId },
                dataType: "json",
                success: function (data) {
                    if (data !== null && data !== undefined) {
                        $("#okul").html('');
                        $("#okul").append('<option value="0">Seçin...</option>');

                        $.each(data, function () {
                            $('#okul').append($('<option/>', {
                                value: this.Id,
                                text: this.Baslik
                            }));
                        })
                    }
                }
            });
            //Ajax End
        });

        $('#btnKopyala').click(function () {
            var kopyalanacakOkulAdi = $('#spanOzelBilgiOkul').text();
            var cnt = confirm(kopyalanacakOkulAdi + ' okuluna ait tüm kurs girişleri kopyalanacaktır!\nEmin misiniz?');
            if (cnt) {

                Loader();

                var kopyalanacakOkulId = $('#OkulId').val();
                var kaydedilecekOkulId = $('#okul').val();

                //Ajax Start
                $.ajax({
                    type: "POST",
                    url: "Kopyala_Process",
                    async: false,
                    data: { kopyalanacakOkulId: kopyalanacakOkulId, kaydedilecekOkulId: kaydedilecekOkulId },
                    dataType: "json",
                    success: function (data) {
                        if (data !== null && data !== undefined) {
                            if (data.status !== 'error') {
                                parent.$.fancybox.close();
                            } else {
                                closeModal();
                                alert(data.message);
                            }

                        }
                    }
                });
                //Ajax End
            }
        })
    };

    return {
        init: myInit
    };
})();
//Kopyalama

//Promosyon Kopyalama
var promosyonKopyalama = (function () {
    var myInit = function () {
        $('#mainLogo').hide();
        $('#mainMenu').hide();
        $('.MasterTable').css('width', '400px');

        $('#btnPromosyonKopyala').click(function () {

            var cnt = confirm('Seçili tüm alanlara ait kopyalama işlemi başlayacak!\nEmin misiniz?');
            if (cnt) {

                Loader();
                var secilenKursTipleri = $("input:checked").length;
                if (secilenKursTipleri > 0) {
                    $('#btnSubmit').trigger('click');

                } else {
                    alert('Hata:Lütfen kopyalanacak kurs tip(ler)i seçin!');
                }


                ////Ajax Start
                //$.ajax({
                //    type: "POST",
                //    url: "Kopyala_Process",
                //    async: false,
                //    data: { kopyalanacakOkulId: kopyalanacakOkulId, kaydedilecekOkulId: kaydedilecekOkulId },
                //    dataType: "json",
                //    success: function (data) {
                //        if (data !== null && data !== undefined) {
                //            if (data.status !== 'error') {
                //                parent.$.fancybox.close();
                //            } else {
                //                closeModal();
                //                alert(data.message);
                //            }

                //        }
                //    }
                //});
                ////Ajax End
            }
            closeModal();
            return false;
        })
    };

    return {
        init: myInit
    };
})();
//Promosyon Kopyalama

//Supplement Kopyalama
var summerSupplementKopyalama = (function () {
    var myInit = function () {
        $('#mainLogo').hide();
        $('#mainMenu').hide();
        $('.MasterTable').css('width', '400px');

        $('#btnsummerSupplimentKopyala').click(function () {

            var cnt = confirm('Seçili tüm alanlara ait kopyalama işlemi başlayacak!\nEmin misiniz?');
            if (cnt) {

                Loader();
                var secilenKursTipleri = $("input:checked").length;
                if (secilenKursTipleri > 0) {
                    $('#btnSubmit').trigger('click');

                } else {
                    alert('Hata:Lütfen kopyalanacak kurs tip(ler)i seçin!');
                }


                ////Ajax Start
                //$.ajax({
                //    type: "POST",
                //    url: "Kopyala_Process",
                //    async: false,
                //    data: { kopyalanacakOkulId: kopyalanacakOkulId, kaydedilecekOkulId: kaydedilecekOkulId },
                //    dataType: "json",
                //    success: function (data) {
                //        if (data !== null && data !== undefined) {
                //            if (data.status !== 'error') {
                //                parent.$.fancybox.close();
                //            } else {
                //                closeModal();
                //                alert(data.message);
                //            }

                //        }
                //    }
                //});
                ////Ajax End
            }
            closeModal();
            return false;
        })
    };

    return {
        init: myInit
    };
})();

var paketPromosyonPDFKopyalama = (function () {
    var myInit = function () {
        $('#mainLogo').hide();
        $('#mainMenu').hide();
        $('.MasterTable').css('width', '400px');

        $('#btnPromosyoPDFKopyala').click(function () {

            var cnt = confirm('Seçili tüm alanlara ait kopyalama işlemi başlayacak!\nEmin misiniz?');
            if (cnt) {

                Loader();
                var secilenOkullar = $("input:checked").length;
                if (secilenKursTipleri > 0) {
                    $('#btnSubmit').trigger('click');

                } else {
                    alert('Hata:Lütfen kopyalanacak kurs tip(ler)i seçin!');
                }


            }
            closeModal();
            return false;
        })
    };

    return {
        init: myInit
    };
})();
//Supplement Kopyalama