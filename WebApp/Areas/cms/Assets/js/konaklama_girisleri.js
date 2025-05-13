
//Konaklama Tipi İşlemleri Start
var arrayKonaklamaTipiOzelTarihler = []; //Array

var konaklamaTipi = (function () {

    var myInit = function () {
        $('.konaklamaTipiTarih').on('click', function () {
            var value = $(this).val();

            if (value == 'herPazartesi') {
                if (arrayKonaklamaTipiOzelTarihler.length > 0) {
                    if (confirm('Tanımlı özel tarihler var. Liste silinecek!\nOnaylıyor musunuz?')) {
                        arrayKonaklamaTipiOzelTarihler = [];
                        myOzelTarihYazdir();

                        $('#divKonaklamaTipiOzelTarih').hide();

                    } else {

                        //radio gruptaki checked class ismi ile bulunup siliniyor. Sonra özel tarih radyo buttonu yeniden seçili hale getiriliyor.
                        $('.konaklamaTipiTarih').removeAttr('checked');
                        $('input:radio[name=konaklamaTipiTarih]')[1].checked = true;
                    }
                } else {
                    $('#divKonaklamaTipiOzelTarih').hide();
                }

            }
            if (value == 'ozel') {
                $('#divKonaklamaTipiOzelTarih').show();
            }
        });

        $('#btnTarihEkle').on('click', function () {
            var ozelTarih1 = $('#konaklamaTipiOzelTarihGiris');
            var ozelTarih2 = $('#konaklamaTipiOzelTarihBitis');
            myOzelTarihPushObject(ozelTarih1, ozelTarih2)
        });

        $('#btnKaydet').on('click', function () {
            /* Variables */
            var errCount = 0;
            var errMessage = '';
            var okulId = $('#okulId').val();
            var baslik = $('#baslik').val();
            var konaklamaKategoriId = $('#konaklamaKategorisi').val();
            var aciklama = $('#konaklamaTipiAciklama').val();
            var basTarihiStatus = $('.konaklamaTipiTarih:checked').val();
            var ozelTarihListesi = $('#ozelTarihListesi').val();
            /* Variables End */

            /* Error Control */
            if ($.trim(konaklamaKategoriId).length == 0) {
                errCount++;
                errMessage += "Konaklama kategorisi seçin!</br>";
            }


            if ($.trim(baslik).length == 0) {
                errCount++;
                errMessage += "Konaklama tipi girişi yapın!</br>";
            }

            if (basTarihiStatus == 'ozel' && arrayKonaklamaTipiOzelTarihler.length == 0) {
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
                    url: "KonaklamaTipiEkle",
                    data: { okulId: okulId, baslik: baslik, konaklamaKategoriId: konaklamaKategoriId, basTarihiStatus: basTarihiStatus, ozelTarihListesi: ozelTarihListesi, aciklama: encodeURIComponent(aciklama) },
                    dataType: "json",
                    success: function (data) {

                        if (data.status == "ok") {
                            GeneralObj.DilokuluMessageBox.init({ status: 'ok', message: 'Kayıt yapıldı.' });
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

    var myOzelTarihPushObject = function (element1, element2) {
        if ($.trim(element1.val()).length > 0 && $.trim(element2.val()).length > 0) {

            arrayKonaklamaTipiOzelTarihler.push({ 'tarih1': element1.val(), 'tarih2': element2.val() });
            element1.val('');
            element2.val('');
            myOzelTarihYazdir();
        }

    };

    var myOzelTarihYazdir = function () {

        $('#divOzelTarihListesi').html('');
        $('#ozelTarihListesi').val('');

        if (arrayKonaklamaTipiOzelTarihler != null && arrayKonaklamaTipiOzelTarihler.length > 0) {
            var display = '';
            var hiddenField = '';

            for (var i = 0; i < arrayKonaklamaTipiOzelTarihler.length; i++) {

                display += '<div style="float: left; padding: 5px; margin-right: 5px;font-weight:bold;">' + arrayKonaklamaTipiOzelTarihler[i].tarih1 + '-' + arrayKonaklamaTipiOzelTarihler[i].tarih2 + '<a href="javascript:void(0)" onclick="OzelTarihSil(this)" data-item="' + arrayKonaklamaTipiOzelTarihler[i].tarih1 + '-' + arrayKonaklamaTipiOzelTarihler[i].tarih2 + '">[X]</a></div>';
                hiddenField += arrayKonaklamaTipiOzelTarihler[i].tarih1 + '-' + arrayKonaklamaTipiOzelTarihler[i].tarih2 + '|';

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
        $('#konaklamaKategorisi').val('');
        $('#konaklamaTipiAciklama').val('');

        //Array temizleniyor
        arrayKonaklamaTipiOzelTarihler = [];

        //Eklenerek gösterilen tarihler siliniyor
        $('#divOzelTarihListesi').html('');

        //Seçilen tarihin database e gönderimi işini yapan hidden field temizleniyor
        $('#ozelTarihListesi').val('');

        //radio butonlar default değerine alınıyor
        $('.konaklamaTipiTarih').removeAttr('checked');
        $('input:radio[name=konaklamaTipiTarih]')[0].checked = true;

        //Tarih giriş textbox ı kapatılıyor
        $('#divKonaklamaTipiOzelTarih').hide();
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

    var tarihAyarlaRange = function (control1, control2) {
        $("#" + control1).datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            numberOfMonths: 3,
            onClose: function (selectedDate) {
                $("#" + control2).datepicker("option", "minDate", selectedDate);
            }
        });
        $("#" + control2).datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            numberOfMonths: 3,
            onClose: function (selectedDate) {
                $("#" + control1).datepicker("option", "maxDate", selectedDate);
            }
        });
    };

    var myKonaklamaTipiSil = function (id) {

        var cnt = confirm('Kayıt silinecek!\nOnaylıyor musunuz?');
        if (cnt) {
            Loader();
            //Ajax Start
            $.ajax({
                type: "POST",
                url: "KonaklamaTipiSil",
                data: { id: id },
                dataType: "json",
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

    var myTumKonaklamaTipiListesiniSil = function () {
        var cnt = confirm('Tüm kayıtlar silinecek!\nOnaylıyor musunuz?');
        if (cnt) {

            $(".tblFiyatListesi").find('.fieldset').each(function (i) {
                var $fieldset = $(this);

                var id = $('input:hidden:eq(0)', $fieldset).val();
                
                if (id != '') {
                    Loader();
                    //Ajax Start
                    $.ajax({
                        type: "POST",
                        url: "KonaklamaTipiSil",
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

    return {
        tarihGoster: tarihAyarla,
        tarihGosterRange: tarihAyarlaRange,
        ozelTarihYazdir: myOzelTarihYazdir,
        konaklamaTipiSil: myKonaklamaTipiSil,
        tumKonaklamaTipiListesiniSil: myTumKonaklamaTipiListesiniSil,
        init: myInit
    };
})();

function OzelTarihSil(element) {
    var arrayTemp = [];
    var tarih = $(element).attr('data-item');
    if (arrayKonaklamaTipiOzelTarihler != null && arrayKonaklamaTipiOzelTarihler.length > 0) {
        for (var i = 0; i < arrayKonaklamaTipiOzelTarihler.length; i++) {
            if (arrayKonaklamaTipiOzelTarihler[i].tarih1 + '-' + arrayKonaklamaTipiOzelTarihler[i].tarih2 != tarih) {
                arrayTemp.push(arrayKonaklamaTipiOzelTarihler[i]);
            }
        }

        arrayKonaklamaTipiOzelTarihler = arrayTemp;

        arrayTemp = [];

        konaklamaTipi.ozelTarihYazdir();
    }
}
//Konaklama Tipi İşlemleri End

//Fiyat Tanimlari Start
var fiyatTanimi = (function () {
    var myInit = function () {
        $('#fiyatlandirmaTipi').on('change', function () {
            var fiyatlandirmaTipiId = $(this).val();
            var konaklamaTipiId = $('#konaklamaTipi').val();

            GeneralObj.DilokuluMessageBox.hide();

            if (fiyatlandirmaTipiId != '') {

                myFiyatlandirmaListesiniGetir(fiyatlandirmaTipiId, konaklamaTipiId);

            } else {
                $('#fiyatTanimiHaftaTanimlari').html('');
                closeModal();
            }
        });

        //Standart Hafta Ekle
        myStandartHaftaEkle = function () {
            $('#btnStandartHafta_Ekle').on('click', function () {

                var konaklamaTipiId = $('#konaklamaTipi').val();
                var fiyatlandirmaTipiId = $(this).attr('Data-FiyatlandirmaTipiId');

                var hafta = $('#StandartHafta_Hafta').val();
                var fiyat = $('#StandartHafta_Fiyat').val();
                if (hafta != '' && fiyat != '' && konaklamaTipiId != '' && fiyatlandirmaTipiId != '') {
                    Loader();

                    //Ajax Start
                    $.ajax({
                        type: "POST",
                        url: "KonaklamaTipiFiyatiEkle",
                        data: { konaklamaTipiId: konaklamaTipiId, fiyatlandirmaTipiId: fiyatlandirmaTipiId, hafta: hafta, fiyat: fiyat },
                        dataType: "json",
                        success: function (data) {
                            if (data.status == "ok") {
                                GeneralObj.DilokuluMessageBox.init({ status: 'ok', message: 'Kayıt yapıldı.' });
                                myFiyatlandirmaListesiniGetir(fiyatlandirmaTipiId, konaklamaTipiId);
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

        //Hafta Bazlı & Birim Fiyatlı Ekle
        myHaftaBazliEkle = function () {
            $('#btnHaftaBazli_Ekle').on('click', function () {

                var konaklamaTipiId = $('#konaklamaTipi').val();
                var fiyatlandirmaTipiId = $(this).attr('Data-FiyatlandirmaTipiId');
                var haftaBaslangic = $('#HaftaBazli_HaftaBaslangic').val();
                var haftaBitis = $('#HaftaBazli_HaftaBitis').val();
                var fiyat = $('#HaftaBazli_Fiyat').val();
                if (haftaBaslangic != '' && haftaBitis != '' && fiyat != '' && konaklamaTipiId != '' && fiyatlandirmaTipiId != '') {
                    Loader();

                    //Ajax Start
                    $.ajax({
                        type: "POST",
                        url: "KonaklamaTipiFiyatiEkle",
                        data: { konaklamaTipiId: konaklamaTipiId, fiyatlandirmaTipiId: fiyatlandirmaTipiId, haftaBaslangic: haftaBaslangic, haftaBitis: haftaBitis, fiyat: fiyat },
                        dataType: "json",
                        success: function (data) {
                            if (data.status == "ok") {
                                GeneralObj.DilokuluMessageBox.init({ status: 'ok', message: 'Kayıt yapıldı.' });
                                myFiyatlandirmaListesiniGetir(fiyatlandirmaTipiId, konaklamaTipiId);
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

                var konaklamaTipiId = $('#konaklamaTipi').val();
                var fiyatlandirmaTipiId = $(this).attr('Data-FiyatlandirmaTipiId');
                var hafta = 0;
                var fiyat = 0;

                $('.haftayaOzelInput').each(function () {
                    hafta = $(this).attr('data-hafta');
                    fiyat = $(this).val();
                    if (hafta != '' && fiyat != '') {
                        Loader();

                        //Ajax Start
                        $.ajax({
                            type: "POST",
                            url: "KonaklamaTipiFiyatiEkle",
                            async: false,
                            data: { konaklamaTipiId: konaklamaTipiId, fiyatlandirmaTipiId: fiyatlandirmaTipiId, hafta: hafta, fiyat: fiyat },
                            dataType: "json",
                            success: function (data) {
                                if (data.status == "ok") {
                                    GeneralObj.DilokuluMessageBox.init({ status: 'ok', message: 'Kayıt yapıldı.' });
                                    myFiyatlandirmaListesiniGetir(fiyatlandirmaTipiId, konaklamaTipiId);

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

        ////Fiyat tanımları bölümünde eğer daha önceden fiyatlandırma modeli tanımlanmış ise seçili getirmek için de kullanılan script
        $('#konaklamaTipi').change(function () {
            var konaklamaTipiId = $(this).val();

            Loader();

            $('#fiyatTanimiHaftaTanimlari').html('');

            //Ajax Start
            $.ajax({
                type: "POST",
                url: "KonaklamaTipiFiyatModeliBul",
                data: { konaklamaTipiId: konaklamaTipiId },
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

        //Default seçili bir konaklama var ise
        var konaklamaTipiId = $('#konaklamaTipi').val();
        if (konaklamaTipiId != '') {

            $('#konaklamaTipi').trigger('change');

        }
    };

    // Standart Hafta Güncelle
    var standartHaftaGuncelle = function () {
        $('#btnStandartHafta_Guncelle').on('click', function () {
            var cnt = confirm('Tüm fiyatlar güncellenecek.\nOnaylıyor musunuz?');
            if (cnt) {
                var konaklamaTipiId = $('#konaklamaTipi').val();
                var fiyatlandirmaTipiId = $(this).attr('data-fiyatlandirmatipiid');

                $("#standartHafta").find('.fieldset').each(function (i) {
                    var $fieldset = $(this);

                    var id = $('input:hidden:eq(0)', $fieldset).val();
                    var hafta = $('input:text:eq(0)', $fieldset).val();
                    var fiyat = $('input:text:eq(1)', $fieldset).val();

                    if (id != '' && fiyat != '' && hafta != '') {


                        //Ajax Start
                        $.ajax({
                            type: "POST",
                            url: "KonaklamaTipiFiyatiGuncelle",
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

                var konaklamaTipiId = $('#konaklamaTipi').val();

                var fiyatlandirmaTipiId = $(this).attr('Data-FiyatlandirmaTipiId');

                $("#haftaBazli").find('.fieldset').each(function (i) {
                    var $fieldset = $(this);

                    var id = $('input:hidden:eq(0)', $fieldset).val();
                    var haftaBaslangic = $('input:text:eq(0)', $fieldset).val();
                    var haftaBitis = $('input:text:eq(1)', $fieldset).val();
                    var fiyat = $('input:text:eq(2)', $fieldset).val();

                    if (id != '' && haftaBaslangic != '' && haftaBitis != '' && fiyat != '') {
                        //Ajax Start
                        $.ajax({
                            type: "POST",
                            url: "KonaklamaTipiFiyatiGuncelle",
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
    var myFiyatlandirmaListesiniGetir = function (fiyatlandirmaTipiId, konaklamaTipiId) {

        Loader();

        $('#fiyatTanimiHaftaTanimlari').html('');
        //Ajax Start
        $.ajax({
            type: "GET",
            url: "FiyatTanimlari_HaftaTanimlari",
            data: { fiyatlandirmaTipiId: fiyatlandirmaTipiId, konaklamaTipiId: konaklamaTipiId },
            //dataType: "json",
            success: function (data) {
                if (data != undefined && data != null) {
                    $('#fiyatTanimiHaftaTanimlari').html(data);

                    //Button Eventlarını görebilmesi için yapıldı
                    myStandartHaftaEkle();
                    myHaftaBazliEkle();
                    myHerHaftayaOzelEkle();

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
function KonaklamaTipiFiyatSil(element) {
    var konaklamaTipiId = $('#konaklamaTipi').val();
    var fiyatlandirmaTipiId = $(element).attr('Data-FiyatlandirmaTipiId');
    var konaklamaTipiFiyatId = $(element).attr('data-fiyatid');

    var cnt = confirm('Kayıt silinecek!\nOnaylıyor musunuz?')
    if (cnt) {

        Loader();

        //Ajax Start
        $.ajax({
            type: "POST",
            url: "KonaklamaTipiFiyatSil",
            async: false,
            data: { konaklamaTipiId: konaklamaTipiId, fiyatlandirmaTipiId: fiyatlandirmaTipiId, konaklamaTipiFiyatId: konaklamaTipiFiyatId },
            dataType: "json",
            success: function (data) {
                if (data.status == "ok") {
                    GeneralObj.DilokuluMessageBox.init({ status: 'ok', message: 'Kayıt silindi.' });

                    fiyatTanimi.fiyatlandirmaListesiniGetir(fiyatlandirmaTipiId, konaklamaTipiId);

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
        $('#promosyonTipi').on('change', function () {
            var promosyonTipiId = $(this).val();
            var konaklamaTipiId = $('#konaklamaTipi').val();

            GeneralObj.DilokuluMessageBox.hide();

            if (promosyonTipiId != '') {

                myPromosyonListesiniGetir(promosyonTipiId, konaklamaTipiId);

            } else {
                $('#promosyonTanimiHaftaTanimlari').html('');
                closeModal();
            }
        });

        //Ekstra Hafta Promosyonu Ekle
        myExtraHaftaEkle = function () {
            $('#btnExtraHafta_Ekle').on('click', function () {

                var konaklamaTipiId = $('#konaklamaTipi').val();
                var promosyonTipiId = $(this).attr('data-promosyontipiid');

                var extraHafta_MinimumHafta = $('#ExtraHafta_MinimumHafta').val();
                var extraHafta_UcretsizHaftaSayisi = $('#ExtraHafta_UcretsizHaftaSayisi').val();
                var promosyonBitisTarihi = $('#PromosyonBitisTarihi').val();
                var kalisTarihiBaslangic = $('#KalisTarihiBaslangic').val();
                var kalisTarihiBitis = $('#KalisTarihiBitis').val();

                if (extraHafta_MinimumHafta != '' && extraHafta_UcretsizHaftaSayisi != '' && promosyonBitisTarihi != '' && konaklamaTipiId != '' && promosyonTipiId != '') {
                    Loader();

                    //Ajax Start
                    $.ajax({
                        type: "POST",
                        url: "KonaklamaTipiPromosyonEkle",
                        data: { konaklamaTipiId: konaklamaTipiId, promosyonTipiId: promosyonTipiId, extraHafta_MinimumHafta: extraHafta_MinimumHafta, extraHafta_UcretsizHaftaSayisi: extraHafta_UcretsizHaftaSayisi, promosyonBitisTarihi: promosyonBitisTarihi, kalisTarihiBaslangic: kalisTarihiBaslangic, kalisTarihiBitis: kalisTarihiBitis },
                        dataType: "json",
                        success: function (data) {
                            if (data.status == "ok") {
                                GeneralObj.DilokuluMessageBox.init({ status: 'ok', message: 'Kayıt yapıldı.' });
                                myPromosyonListesiniGetir(promosyonTipiId, konaklamaTipiId);
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

        //Yüzdesel İndirim Promosyonu Ekle
        myIndirimPromosyonuEkle = function () {
            $('#btnIndirimPromosyonu_Ekle').on('click', function () {

                var konaklamaTipiId = $('#konaklamaTipi').val();
                var promosyonTipiId = $(this).attr('data-promosyontipiid');

                var indirimPromosyonu_Hafta = $('#IndirimPromosyonu_Hafta').val();
                var indirimPromosyonu_HaftaMax = $('#IndirimPromosyonu_HaftaMax').val();
                var indirimPromosyonu_Oran = $('#IndirimPromosyonu_Oran').val();
                var promosyonBitisTarihi = $('#PromosyonBitisTarihi').val();
                var kalisTarihiBaslangic = $('#KalisTarihiBaslangic').val();
                var kalisTarihiBitis = $('#KalisTarihiBitis').val();

                if (indirimPromosyonu_Hafta != '' && indirimPromosyonu_HaftaMax != '' && indirimPromosyonu_Oran != '' && promosyonBitisTarihi != '' && konaklamaTipiId != '' && promosyonTipiId != '') {
                    Loader();

                    //Ajax Start
                    $.ajax({
                        type: "POST",
                        url: "KonaklamaTipiPromosyonEkle",
                        data: { konaklamaTipiId: konaklamaTipiId, promosyonTipiId: promosyonTipiId, indirimPromosyonu_Hafta: indirimPromosyonu_Hafta, indirimPromosyonu_HaftaMax: indirimPromosyonu_HaftaMax, indirimPromosyonu_Oran: indirimPromosyonu_Oran, promosyonBitisTarihi: promosyonBitisTarihi, kalisTarihiBaslangic: kalisTarihiBaslangic, kalisTarihiBitis: kalisTarihiBitis },
                        dataType: "json",
                        success: function (data) {
                            if (data.status == "ok") {
                                GeneralObj.DilokuluMessageBox.init({ status: 'ok', message: 'Kayıt yapıldı.' });
                                myPromosyonListesiniGetir(promosyonTipiId, konaklamaTipiId);
                                $('#IndirimPromosyonu_Hafta').val('');
                                $('#IndirimPromosyonu_HaftaMax').val('');
                                $('#IndirimPromosyonu_Oran').val('');
                                $('#PromosyonBitisTarihi').val('');
                                $('#KalisTarihiBaslangic').val('');
                                $('#KalisTarihiBitis').val('');

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

        //Fiyat Bazlı Promosyon Ekle
        myKonaklamaBazliEkle = function () {
            $('#btnKonaklamaBazli_Ekle').on('click', function () {

                var konaklamaTipiId = $('#konaklamaTipi').val();
                var promosyonTipiId = $(this).attr('data-promosyontipiid');

                var konaklamaBazli_Hafta = $('#KonaklamaBazli_Hafta').val();
                var konaklamaBazli_Fiyat = $('#KonaklamaBazli_Fiyat').val();
                var promosyonBitisTarihi = $('#PromosyonBitisTarihi').val();
                var kalisTarihiBaslangic = $('#KalisTarihiBaslangic').val();
                var kalisTarihiBitis = $('#KalisTarihiBitis').val();

                if (konaklamaBazli_Hafta != '' && konaklamaBazli_Fiyat != '' && promosyonBitisTarihi != '' && konaklamaTipiId != '' && promosyonTipiId != '') {
                    Loader();

                    //Ajax Start
                    $.ajax({
                        type: "POST",
                        url: "KonaklamaTipiPromosyonEkle",
                        data: { konaklamaTipiId: konaklamaTipiId, promosyonTipiId: promosyonTipiId, konaklamaBazli_Hafta: konaklamaBazli_Hafta, konaklamaBazli_Fiyat: konaklamaBazli_Fiyat, promosyonBitisTarihi: promosyonBitisTarihi, kalisTarihiBaslangic: kalisTarihiBaslangic, kalisTarihiBitis: kalisTarihiBitis },
                        dataType: "json",
                        success: function (data) {
                            if (data.status == "ok") {
                                GeneralObj.DilokuluMessageBox.init({ status: 'ok', message: 'Kayıt yapıldı.' });
                                myPromosyonListesiniGetir(promosyonTipiId, konaklamaTipiId);
                                $('#KonaklamaBazli_Hafta').val('');
                                $('#KonaklamaBazli_Fiyat').val('');
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

                var konaklamaTipiId = $('#konaklamaTipi').val();
                var promosyonTipiId = $(this).attr('data-promosyontipiid');

                var indirimPromosyonu_Hafta = $('#IndirimPromosyonu_Hafta').val();
                var indirimPromosyonu_HaftaMax = $('#IndirimPromosyonu_HaftaMax').val();
                var indirimPromosyonu_BirimFiyat = $('#IndirimPromosyonu_BirimFiyat').val();
                var promosyonBitisTarihi = $('#PromosyonBitisTarihi').val();
                var kalisTarihiBaslangic = $('#KalisTarihiBaslangic').val();
                var kalisTarihiBitis = $('#KalisTarihiBitis').val();

                if (indirimPromosyonu_Hafta != '' && indirimPromosyonu_HaftaMax != '' && indirimPromosyonu_BirimFiyat != '' && promosyonBitisTarihi != '' && konaklamaTipiId != '' && promosyonTipiId != '') {
                    Loader();

                    //Ajax Start
                    $.ajax({
                        type: "POST",
                        url: "KonaklamaTipiPromosyonEkle",
                        data: { konaklamaTipiId: konaklamaTipiId, promosyonTipiId: promosyonTipiId, indirimPromosyonu_Hafta: indirimPromosyonu_Hafta, indirimPromosyonu_HaftaMax: indirimPromosyonu_HaftaMax, indirimPromosyonu_BirimFiyat: indirimPromosyonu_BirimFiyat, promosyonBitisTarihi: promosyonBitisTarihi, kalisTarihiBaslangic: kalisTarihiBaslangic, kalisTarihiBitis: kalisTarihiBitis },
                        dataType: "json",
                        success: function (data) {
                            if (data.status == "ok") {
                                GeneralObj.DilokuluMessageBox.init({ status: 'ok', message: 'Kayıt yapıldı.' });
                                myPromosyonListesiniGetir(promosyonTipiId, konaklamaTipiId);
                                $('#IndirimPromosyonu_Hafta').val('');
                                $('#IndirimPromosyonu_Oran').val('');
                                $('#PromosyonBitisTarihi').val('');
                                $('#KalisTarihiBaslangic').val('');
                                $('#KalisTarihiBitis').val('');

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

        //Promosyon tanımları bölümünde eğer daha önceden promosyon modeli tanımlanmış ise seçili getirmek için kullanılan script
        $('#konaklamaTipi').on('change', function () {

            var konaklamaTipiId = $(this).val();

            Loader();

            $('#promosyonTanimiHaftaTanimlari').html('');

            //Ajax Start
            $.ajax({
                type: "POST",
                url: "KonaklamaTipiPromosyonModeliBul",
                data: { konaklamaTipiId: konaklamaTipiId },
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

        //Default seçili bir konaklama var ise
        var konaklamaTipiId = $('#konaklamaTipi').val();
        if (konaklamaTipiId != '') {

            $('#konaklamaTipi').trigger('change');

        }

    };

    //Fiyatlandırma Listesini Getir
    var myPromosyonListesiniGetir = function (promosyonTipiId, konaklamaTipiId) {

        Loader();

        $('#promosyonTanimiHaftaTanimlari').html('');
        //Ajax Start
        $.ajax({
            type: "GET",
            url: "PromosyonTanimlari_PromosyonTanimlari",
            data: { promosyonTipiId: promosyonTipiId, konaklamaTipiId: konaklamaTipiId },
            //dataType: "json",
            success: function (data) {
                if (data != undefined && data != null) {
                    $('#promosyonTanimiHaftaTanimlari').html(data);

                    //Button Eventlarını görebilmesi için yapıldı
                    myExtraHaftaEkle();
                    myIndirimPromosyonuEkle();
                    myKonaklamaBazliEkle();
                    myIndirimliBirimFiyatPromosyonuEkle();
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
        $(controller).datepicker({
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 3,
            showWeek: true,
            minDate: 0
        });
    };

    var tarihAyarlaRange = function (control1, control2) {
        $(control1).datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            numberOfMonths: 3,
            onClose: function (selectedDate) {
                $(control2).datepicker("option", "minDate", selectedDate);
            }
        });
        $(control2).datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            numberOfMonths: 3,
            onClose: function (selectedDate) {
                $(control1).datepicker("option", "maxDate", selectedDate);
            }
        });
    };

    return {
        init: myInit,
        promosyonListesiniGetir: myPromosyonListesiniGetir,
        tarihGoster: tarihAyarla,
        tarihGosterRange: tarihAyarlaRange,
    };
})();

function KonaklamaTipiPromosyonSil(element) {
    var konaklamaTipiId = $('#konaklamaTipi').val();
    var promosyonTipiId = $(element).attr('data-promosyontipiid');
    var konaklamaTipiFiyatId = $(element).attr('data-fiyatid');

    var cnt = confirm('Kayıt silinecek!\nOnaylıyor musunuz?')
    if (cnt) {

        Loader();

        //Ajax Start
        $.ajax({
            type: "POST",
            url: "KonaklamaTipiPromosyonSil",
            async: false,
            data: { konaklamaTipiId: konaklamaTipiId, promosyonTipiId: promosyonTipiId, konaklamaTipiFiyatId: konaklamaTipiFiyatId },
            dataType: "json",
            success: function (data) {
                if (data.status == "ok") {
                    GeneralObj.DilokuluMessageBox.init({ status: 'ok', message: 'Kayıt silindi.' });

                    promosyonTanimi.promosyonListesiniGetir(promosyonTipiId, konaklamaTipiId);

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

//Konaklama Ek Ücretleri
var konaklamaEkUcret = (function () {

    var myInit = function () {

    };

    var myKonaklamaTipiChange = function (url) {
        $('#konaklamaTipi').change(function () {
            var konaklamaTipiId = $(this).val();
            if (konaklamaTipiId != '') {
                window.location = url + '&konaklamaTipiId=' + $(this).val();
            }
        });
    };

    var myUcretSil = function (id) {
        var cnt = confirm('Ücret girişi silinecek!\nOnaylıyor musunuz?');
        if (cnt) {
            //Ajax Start
            $.ajax({
                type: "POST",
                url: "KonaklamaEkUcretSil",
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
        konaklamaTipiChange: myKonaklamaTipiChange,
        ucretSil: myUcretSil
    }

})();
//Konaklama Ek Ücretleri

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
                    alert('Hata:Lütfen kopyalanacak konaklama tip(ler)i seçin!');
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
//Promosyon Kopyalama

//Konaklama Ek Kopyalama
var konaklamaEkUcretKopyalama = (function () {
    var myInit = function () {
        $('#mainLogo').hide();
        $('#mainMenu').hide();
        $('.MasterTable').css('width', '400px');

        $('#btnKonaklamaEkUcretKopyala').click(function () {

            var cnt = confirm('Seçili tüm alanlara ait kopyalama işlemi başlayacak!\nEmin misiniz?');
            if (cnt) {

                Loader();
                var secilenKursTipleri = $("input:checked").length;
                if (secilenKursTipleri > 0) {
                    $('#btnSubmit').trigger('click');

                } else {
                    alert('Hata:Lütfen kopyalanacak konaklama tip(ler)i seçin!');
                }


                ////Ajax Start
                //$.ajax({
                //    type: "POST",
                //    url: "Kopyala_Process",
                //    async: false,
                //    data: { kopyalanacakOkulId: kopyalanacakOkulId, kaydedilecekOkulId: kaydedilecekOkulId },
                //    dataType: "json",
                //    success: function (data) {
                //        if (data != null && data != undefined) {
                //            if (data.status != 'error') {
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
//Konaklama Ek Kopyalama