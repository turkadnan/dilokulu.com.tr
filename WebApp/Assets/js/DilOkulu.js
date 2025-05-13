var globalParameters = {};


globalParameters.language =
{
    init: function (options) {

        $(options.controlName).on('change', function () {
            var dilId = $(this).val();
            if (dilId != '') {

                $.ajax({
                    type: "GET",
                    url: '/ajax/UlkeListesi?dilId=' + dilId,
                    dataType: "json",
                    success: function (data) {
                        if (data != null) {

                            $(options.controlName1 + ' option').remove();
                            $(options.controlName1).append('<option value="">Ülke Seçin</option>');
                            $(options.controlName1).parent().children('span').text('Ülke Seçin');

                            $(options.controlName2 + ' option').remove();
                            $(options.controlName2).append('<option value="">Şehir Seçin</option>');
                            $(options.controlName2).parent().children('span').text('Şehir Seçin');

                            $.each(data.ulkeler, function (key, obj) {

                                $(options.controlName1).append('<option value="' + obj.Key + '">' + obj.Value + '</option>');
                            });

                        }
                        else {
                            $(options.controlName1 + ' option').remove();
                            $(options.controlName1).append('<option value="">Ülke Seçin</option>');
                            $(options.controlName1).parent().children('span').text('Ülke Seçin');
                        }
                    }
                });

            } else {

                $(options.controlName1 + ' option').remove();
                $(options.controlName1).append('<option value="">Ülke Seçin</option>');
                $(options.controlName1).parent().children('span').text('Ülke Seçin');

                $(options.controlName2 + ' option').remove();
                $(options.controlName2).append('<option value="">Şehir Seçin</option>');
                $(options.controlName2).parent().children('span').text('Şehir Seçin');
            }
        });
    }
}

globalParameters.country =
{
    init: function (options) {

        $(options.controlName).on('change', function () {

            var ulkeId = $(this).val();
            if (ulkeId != '') {

                $.ajax({
                    type: "GET",
                    url: '/ajax/SehirlerListesi?ulkeId=' + ulkeId,
                    dataType: "json",
                    success: function (data) {
                        if (data != null) {

                            $(options.controlName1 + ' option').remove();
                            $(options.controlName1).append('<option value="">Şehir Seçin</option>');
                            $(options.controlName1).parent().children('span').text('Şehir Seçin');

                            $.each(data.sehirler, function (key, obj) {

                                $(options.controlName1).append('<option value="' + obj.Key + '">' + obj.Value + '</option>');
                            });

                        }
                        else {
                            $(options.controlName1 + ' option').remove();
                            $(options.controlName1).append('<option value="">Şehir Seçin</option>');
                            $(options.controlName1).parent().children('span').text('Şehir Seçin');
                        }
                    }
                });

            } else {
                $(options.controlName1 + ' option').remove();
                $(options.controlName1).append('<option value="">Şehir Seçin</option>');
                $(options.controlName1).parent().children('span').text('Şehir Seçin');
            }
        });
    }
}
