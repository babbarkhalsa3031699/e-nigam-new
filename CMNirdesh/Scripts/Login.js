

    $('#LoginForm input').keydown(function (e) {
            if (e.keyCode == 13) {
        SubmitForm();
            }
        });

        $(document).ready(function () {

            //var attr = $("#saveUserNamehidden").attr('value');
            //alert(attr);
            $("#saveUserNamehidden").val("False");
        //    if (attr == 'value') {
        //document.getElementById("saveUserNameCheck").checked = true;
        //        $("#saveUserNamehidden").val("True");
        //    }
        //    else {

        //$("#saveUserNameCheck").removeAttr('checked');
        //        $("#saveUserNamehidden").val("False");
        //    }

            var sub = '@TempData["submitted"]';
			var suc = '@TempData["Success"]';

		    if (sub == "Submitted") {
        alert("\t\t\t      @Resources.UserRegistration.SuccessMessage.\n\n" + "Your CMNirdesh Id and password have been sent on registered mobile number.");
			}
			else if (suc == "Success") {
        alert("Your CMNirdesh password has been sent on registered mobile number.");
			}
		});

function FormSubmit() {

    SubmitForm();
}
$("#saveUserNamehidden").val("true");

        //$("#saveUserNameCheck").click(function () {
        //    if ($('#saveUserNameCheck').is(':checked')) {
        //$("#saveUserNamehidden").val("true");
        //    }
        //    else {
        //$("#saveUserNamehidden").val("false");
        //    }
        //});

        function OnChangeTextBox(obj) {
            var value = $(obj).val();
            var id = $(obj).attr('id');
            if (value != "") {
                if (id == "UserName") {
        $('#UsernameValidation').html('');
                }
                if (id == "Password") {
        $('#PasswordValidation').html('');
                }
                if (id == "CaptchaText") {
        $('#CaptchaValidation').html('');
                    $('#CaptchaServerError').html('');

                }

            }
        }

     
//function HashKey(salt) {
//    alert(salt);
//    debugger;
//    var str = encodeURI($("#Password").val());
//    var strCode1 = toHex(salt);
//    var strCode2 = toHex(CryptoJS.enc.Latin1.parse(str).toString(Crypto.enc.Base64));
//    var key = encodeCodes(strCode1, strCode2);
//    alert(key + ",         key");
//    //var Key = CryptoJS.PBKDF2(str, salt, { keySize: 256 / 24, iterations: 1000 });
//    //Key = Key.toString(CryptoJS.enc.Base64);
//    return key;
//}

//function toHex(str) {
//    var hex = '';
//    for (var i = 0; i < str.length; i++) {
//        hex += '' + str.charCodeAt(i).toString(16);
//    }
//    return hex;
//}

//function encodeCodes(str1, str2) {
//    var strEncoded = '';
//    var limit = str2.length;
//    if (str2.length > str1.length) limit = str1.length;
//    for (var i = 0; i < str2.length; i++) {
//        if (i > str1.length)
//            strEncoded += '' + Math.floor(Math.random() * (9 - 1 + 1) + 1).toString() + str2.charAt(i);
//        else
//            strEncoded += '' + str1.charAt(i) + str2.charAt(i);
//    }
//    return strEncoded;
//}

        function searchKeyPress(e) {
            // look for window.event in case event isn't passed in
            if (typeof e == 'undefined' && window.event) {e = window.event; }
            if (e.keyCode == 13) {
        FormSubmit();
            }
        }
 
        function DisableBackButton() {
            window.history.forward()
        }
        DisableBackButton();
        window.onload = DisableBackButton;
        window.onpageshow = function (evt) { if (evt.persisted) DisableBackButton() }
        window.onunload = function () {void (0)}
 