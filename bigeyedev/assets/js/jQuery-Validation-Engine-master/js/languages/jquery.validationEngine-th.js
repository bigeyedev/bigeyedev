(function($) {
        $.fn.validationEngineLanguage = function() {};
        $.validationEngineLanguage = {
                newLang: function() {
                        $.validationEngineLanguage.allRules =   {"required":{                           // Add your regex rules here, you can take telephone as an example
                                                "regex":"none",
                                                "alertText":"* กรุณากรอกข้อมูล",
                                                "alertTextCheckboxMultiple":"* โปรดเลือกตัวเลือก",
                                                "alertTextCheckboxe":"* โปรดเลือก 1 ตัวเลือก"},
                                        "length":{
                                                "regex":"none",
                                                "alertText":"*ระหว่าง ",
                                                "alertText2":" และ ",
                                                "alertText3": " ตัวอักษรเท่านั้น"},
                                        "maxCheckbox":{
                                                "regex":"none",
                                                "alertText":"* เลือกเยอะเกินไป"},       
                                        "minCheckbox":{
                                                "regex":"none",
                                                "alertText":"* โปรดเลือก ",
                                                "alertText2":" ตัวเลือก"},                                        
                                        "minSize": {
                                            "regex": "none",
                                            "alertText": "* ต้องมีอย่างน้อย ",
                                            "alertText2": " ตัวอักษร"
                                        },
                                        "maxSize": {
                                            "regex": "none",
                                            "alertText": "* Maximum ",
                                            "alertText2": " characters allowed"
                                        },  
                                        "confirm":{
                                                "regex":"none",
                                                "alertText":"* ข้อมูลไม่ตรงกัน"},               
                                        "phone":{
                                                "regex":"/^[0-9\-\(\)\ ]+$/",
                                                "alertText":"* เบอร์โทรศัพท์ไม่ถูกต้อง"},  
                                        "email":{
                                                "regex":"/^[a-zA-Z0-9_\.\-]+\@([a-zA-Z0-9\-]+\.)+[a-zA-Z0-9]{2,4}$/",
                                                "alertText":"* อีเมลไม่ถูกต้อง"},   
										"number": {
                                            // Number, including positive, negative, and floating decimal. credit: orefalo
                                            "regex": /^[\-\+]?((([0-9]{1,3})([,][0-9]{3})*)|([0-9]+))?([\.]([0-9]+))?$/,
                                            "alertText": "* ตัวเลขหรือจุดทศนิยมเท่านั้น"
                                        },
                                        "equals": {
                                            "regex": "none",
                                            "alertText": "* รหัสผ่านไม่ตรงกัน"
                                        },
                                        "date":{
                                                 "regex":"/^[0-9]{4}\-\[0-9]{1,2}\-\[0-9]{1,2}$/",
                                                 "alertText":"* Invalid date, must be in YYYY-MM-DD format"},
                                        "onlyNumber":{
                                                "regex":"/^[0-9\ ]+$/",
                                                "alertText":"* ตัวเลขเท่านั้น"},        
                                        "noSpecialCaracters":{
                                                "regex":"/^[0-9a-zA-Z]+$/",
                                                "alertText":"* ใช้อักษรพิเศษไม่ได้"},   
                                        "ajaxUser":{
                                                "file":"validateUser.php",
                                                "extraData":"name=eric",
                                                "alertTextOk":"* user นี้สามารถใช้งานได้",      
                                                "alertTextLoad":"* Loading, please wait",
                                                "alertText":"* user นี้มีคนใช้งานแล้ว"},        
                                        "ajaxName":{
                                                "file":"validateUser.php",
                                                "alertText":"* This name is already taken",
                                                "alertTextOk":"* This name is available",       
                                                "alertTextLoad":"* Loading, please wait"},              
                                        "onlyLetter":{
                                                "regex":"/^[a-zA-Z\ \']+$/",
                                                "alertText":"* ตัวอักษรเท่านั้น"},
                                        "validate2fields":{
                                        "nname":"validate2fields",
                                        "alertText":"* You must have a firstname and a lastname"}       
                                        }       
                                        
                }
        }
})(jQuery);

$(document).ready(function() {  
        $.validationEngineLanguage.newLang()
});