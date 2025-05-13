var ErrorMessage = "";
var ErrorCount = 0;

var RegisterVariables = {};

RegisterVariables.NewUser =
{
    init: function () {
        $('#buttonGonder').click(function () {
            var Name = $('#Name').val();
            var Surname = $('#Surname').val();
            var Email = $('#EMail').val();
            var Username = $('#Username').val();
            var Password = $('#Password').val();
            var RetryPassword = $('#RetryPassword').val();


            ErrorMessage = "";
            ErrorCount = 0;

            if (Username == '') {
                ErrorCount++;
                ErrorMessage += "Lütfen kullanıcı adı giriniz!\n";
            }

            if (Password == '') {
                ErrorCount++;
                ErrorMessage += "Lütfen şifre giriniz!\n";
            } else if (Password != RetryPassword) {
                ErrorCount++;
                ErrorMessage += "Lütfen şifre alanlarını kontrol ediniz!\n";
            }

            if (Name == '') {
                ErrorCount++;
                ErrorMessage += "Lütfen adınızı giriniz!\n";
            }

            if (Surname == '') {
                ErrorCount++;
                ErrorMessage += "Lütfen soyadınızı giriniz!\n";
            }

            if (Email == '') {
                ErrorCount++;
                ErrorMessage += "Lütfen e-posta giriniz!\n";
            } else if (!validateMail(Email)) {
                ErrorCount++;
                ErrorMessage += "Lütfen geçerli bir e-posta giriniz!\n";
            }

            if (ErrorCount > 0) {
                alert(ErrorMessage);
                return false;
              
            } else {
                return true;
            }
        });
    }
};

RegisterVariables.UpdateUser =
{
    init: function () {
        $('#buttonGonder').click(function () {
            var Name = $('#Name').val();
            var Surname = $('#Surname').val();
            var Email = $('#EMail').val();
            var Username = $('#Username').val();
            var Password = $('#Password').val();
            var RetryPassword = $('#RetryPassword').val();


            ErrorMessage = "";
            ErrorCount = 0;

            if (Username == '') {
                ErrorCount++;
                ErrorMessage += "Lütfen kullanıcı adı giriniz!\n";
            }

            if (Password != '') {
                if (Password != RetryPassword) {
                    ErrorCount++;
                    ErrorMessage += "Lütfen şifre alanlarını kontrol ediniz!\n";
                }                
            } 

            if (Name == '') {
                ErrorCount++;
                ErrorMessage += "Lütfen adınızı giriniz!\n";
            }

            if (Surname == '') {
                ErrorCount++;
                ErrorMessage += "Lütfen soyadınızı giriniz!\n";
            }

            if (Email == '') {
                ErrorCount++;
                ErrorMessage += "Lütfen e-posta giriniz!\n";
            } else if (!validateMail(Email)) {
                ErrorCount++;
                ErrorMessage += "Lütfen geçerli bir e-posta giriniz!\n";
            }

            if (ErrorCount > 0) {
                alert(ErrorMessage);
                return false;

            } else {
                return true;
            }
        });
    }
};

function validateMail(email) {
    var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
    return reg.test(email);
}
