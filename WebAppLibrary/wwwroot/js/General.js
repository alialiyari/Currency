//1400/11/26 ModalMode add for Datebox
//1401/01/21 file input max size added

function Toast(options) {
    var bgColor = 'bg-info';
    var title = 'اعلام';
    var message = 'بدون پیام';
    var timeout = 10
    if (options) {
        if (options.title) title = options.title;
        if (options.message) message = options.message;
        if (options.timeout) timeout = options.timeout;
        if (options.bgColor) bgColor = options.bgColor;
    }

    //<small class="text-muted">just now</small>
    var toastTempate = `
<div class="toast ${bgColor} show mt-2" role="alert" aria-atomic="true">
  <div class="toast-header">
    <strong class="me-auto"><i class='bi-alarm'></i> ${title}</strong>
    <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
  </div>
  <div class="toast-body">${message}</div>
</div>`;
    $(toastTempate).appendTo($(".toast-container"));
    $(".toast-container").find('.toast').last().show();

    var toast = $(".toast-container").find('.toast').last();
    toast.find('.btn-close').click(function () {
        toast.remove();
    });
    setTimeout(function () {
        toast.remove();
    }, timeout * 1000)
}



//#region Alert
var SuccessIcon = '<i class="fa fa-2x mx-2 fa-check-circle" style="color: #1ea076"></i>';
var WarnningIcon = '<i class="fa fa-2x mx-2  fa-exclamation-triangle " style="color: #ffd43b"></i>';

var AlertBodyScrollStatus = false;
function AlertLarg(message, callback) {
    AlertBodyScrollStatus = $("body").hasClass('modal-open');
    bootbox.alert({ message: message, backdrop: true, closeButton: false, size: 'large', callback: callback });
    $("body").find(".modal-backdrop").last().css('zIndex', 9999);
    $(".bootbox").last().css('zIndex', 99999);
}

function Confirm(message, callback) {
    AlertBodyScrollStatus = $("body").hasClass('modal-open');
    bootbox.confirm({ message: message, backdrop: true, closeButton: false, size: 'large', callback: callback });
    $("body").find(".modal-backdrop").last().css('zIndex', 9999);
    $(".bootbox").last().css('zIndex', 99999);
}

function Alert(message, callback) {
    bootbox.alert({ message: message, backdrop: true, closeButton: false, callback: callback });
}
$(document).on("hidden.bs.modal", ".bootbox.modal", function (e) {
    if (AlertBodyScrollStatus == true) { $("body").addClass('modal-open'); }
});

//#endregion Alert

//#region FileUploader

function toBase64(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => {
            resolve({
                Content: reader.result.split(";")[1].split(",")[1],
                Name: file.name,
                Type: file.type,
                Size: file.size,
                Status: 'Added',
                Header: reader.result.split(";")[0] + ';' + reader.result.split(";")[1].split(",")[0]
            });
        };
        reader.onerror = error => reject(error);
    });
}

async function getBase64FileSelector(fileSelectorId) {
    var files = document.querySelector('#' + fileSelectorId).files;
    if (files.length == 0) return null

    if ($("#" + fileSelectorId).hasAttr('multiple')) {
        var result = [];

        for (var i = 0; i < files.length; i++) {
            var ttt = await toBase64(files[i]);
            result.push(ttt);
        }
        return result;
    }
    else {
        return await toBase64(files[0]);
    }
}

//#endregion FileUploader

//#region Extention

$.fn.hasAttr = function (name) {
    return this.attr(name) !== undefined;
};

$.fn.digits = function () {
    $(this).val($(this).val().split(",").join(""));
    return this.each(function () {
        $(this).val($(this).val().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    })
}


$.UrlParam = function (name) {
    var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href);
    if (results == null) return ''; return results[1] || '';
}

function replaceAll(str, term, replacement) {
    return str.split(term).join(replacement);
}

String.prototype.replaceAll = function (search, replace) {
    return this.split(search).join(replace);
};

String.prototype.digitSeprate = function () {
    return this.split(",").join("").replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
};


//#endregion Extention


function CollectformData(Parent, AddedIdForRemove, DeptDetect = false) {
    if (!AddedIdForRemove) AddedIdForRemove = '';

    var toReturn = {}
    if (Parent == null) Parent = $("body");


    function ValueSetByNamspace(Id, Value) {
        if (!Id) return;
        Id = Id.replace(AddedIdForRemove, '');
        var ref = toReturn;

        if (DeptDetect == true) {
            var IdSeprated = Id.split('_')

            $(IdSeprated).each(function (index, key) {
                if (index == IdSeprated.length - 1) { ref[key] = Value; } else { ref[key] = ref[key] || {}; ref = ref[key]; }
            })
        } else {
            toReturn[Id] = Value;
        }

    }


    $(Parent).find("input[type=text],input[type=hidden],input[type=number],input[type=time]").each(function () {
        if ($(this).attr('id') == false || $(this).attr('id') == '') return;

        var t = $(this).val();
        var tor = "۰۱۲۳۴۵۶۷۸۹";
        $.each(tor.split(''), function (index, value) {
            t = replaceAll(t, value, index)
        });
        if ($(this).hasAttr('data-type') && $(this).attr('data-type') == 'Number') { t = t.replaceAll(',', ''); }
        ValueSetByNamspace($(this).attr('id'), t)
        // toReturn[$(this).attr('id')] = t;
    })

    $(Parent).find("input[type=file]").each(function () {
        if ($(this).attr('id') == false || $(this).attr('id') == '') return;

        if ($(this).attr('data-json') == null) {
            $(this).attr('data-json', JSON.stringify({ Status: 'Unchanged' }))
        }

        ValueSetByNamspace($(this).attr('id'), JSON.parse($(this).attr('data-json')))
    })

    $(Parent).find("input[type=checkbox]").each(function () {
        if ($(this).attr('id') == false || $(this).attr('id') == '') return;
        ValueSetByNamspace($(this).attr('id'), $(this).is(":checked"))
        //toReturn[$(this).attr('id')] = $(this).is(":checked");
    })

    $(Parent).find("input[type=date]").each(function () {
        if ($(this).attr('id') == false || $(this).attr('id') == '') return;

        ValueSetByNamspace($(this).attr('id'), $(this).val())
        //toReturn[$(this).attr('id')] = $(this).val();
    })

    $(Parent).find("select").each(function () {
        if ($(this).attr('id') == false || $(this).attr('id') == '') return;
        //toReturn[$(this).attr('id')] = $(this).val();
        ValueSetByNamspace($(this).attr('id'), $(this).val())
    })

    $(Parent).find("textarea").each(function () {
        if ($(this).attr('id') == false || $(this).attr('id') == '') return;
        if ($(this).hasAttr('data-type') && $(this).attr('data-type') == 'ckeditor') {

            ValueSetByNamspace($(this).attr('id'), CKEDITOR.instances[$(this).attr('id')].getData())
            //toReturn[$(this).attr('id')] = CKEDITOR.instances[$(this).attr('id')].getData()
        } else {
            ValueSetByNamspace($(this).attr('id'), $(this).val())
            //toReturn[$(this).attr('id')] = $(this).val();
        }
    })

    return toReturn;
}


function SelectOptionsRefresh(Selector, Parameters) {
    $(Selector).find("option").remove();
    $.post($(Selector).attr('data-cascaderefreshurl'), Parameters, function (Result) {
        $.each(Result, function (index, item) {
            $(Selector).append("<option value='" + item.Value + "'>" + item.Text + "</option>")
        })
    })
}


//#region Modal


var ModalIndex = 1065;

function Modal5Show(options) {
    ModalIndex += 2;
    var Url = options.Url;
    var OnClose = options.OnClose;

    var ModalId = options.ModalId;


    if (options.backdrop == null) { options.backdrop = true; options.keyboard = true; }
    if (options.backdrop == false) options.keyboard = false;


    var AddModalOpenClass = $("body").hasClass('modal-open');


    if (ModalId) {
        var modalSize = 'modal-lg';
        if (options.modalSize) { modalSize = options.modalSize }

        var modal = $("<div></div>").attr('id', 'Modal' + ModalIndex).attr('data-id', ModalId).appendTo("body");

        var modalTemplate = `
        <div class="modal" tabindex="-1">
            <div class="modal-dialog ${modalSize} modal-dialog-centered modal-dialog-scrollable">
                <div class="modal-content"></div>
            </div>
        </div>
        `
        $(modalTemplate).appendTo(modal);
        $("#" + ModalId).removeClass('d-none').appendTo(modal.find('.modal-content'));

        var myModal = new bootstrap.Modal($('#Modal' + ModalIndex).find(".modal").get(0), {});

        $('#Modal' + ModalIndex).find(".modal").get(0).addEventListener('hidden.bs.modal', function (event) {
            if (AddModalOpenClass == true) { $("body").addClass('modal-open'); }
            $("#" + ModalId).appendTo($("body")).addClass("d-none");


            myModal.dispose();
            $('#Modal' + ModalIndex).remove();

            if (typeof OnClose === "function") { OnClose(); }
        })


        myModal.show();
        $(".modal-backdrop ").last().css('z-index', ModalIndex - 1)
        $('#Modal' + ModalIndex).find(".modal").css('z-index', ModalIndex);

    } else {
        $.get(Url, {}, function (result) {
            if (result.Status == 0) {
                if (result.Errors) {

                    var Messages = '<ul>';
                    $(result.Errors).each(function (key, value) {
                        Messages += '<li>' + value.ErrorMessage + '</li>';
                    });
                    Messages += '</ul>';
                    AlertLarg(Messages); return;
                }
                if (result.Message) { AlertLarg(WarnningIcon + result.Message); return; }
            }


            $("<div></div>").attr('id', 'Modal' + ModalIndex).attr('data-url', Url).appendTo("body").html(result);

            var myModal = new bootstrap.Modal($('#Modal' + ModalIndex).find(".modal").get(0), {});
            $('#Modal' + ModalIndex).find(".modal").get(0).addEventListener('hidden.bs.modal', function (event) {
                if (AddModalOpenClass == true) { $("body").addClass('modal-open'); }
                myModal.dispose();

                $(this).parents('*[data-url]').remove();
                if (typeof OnClose === "function") { OnClose(); }
            })
            myModal.show();
            $(".modal-backdrop ").last().css('z-index', ModalIndex - 1)
            $('#Modal' + ModalIndex).find(".modal").css('z-index', ModalIndex);
        });
    }


}




function DynamicModalShow(Url, OnClose, Width = 'max', Height = 'max') {
    ModalShow({ Url: Url, OnClose: OnClose, Width: Width, Height: Height });
}


function ModalShow(options) {
    ModalIndex += 1;
    var Url = options.Url;
    var Width = options.Width;
    var Height = options.Height;
    var OnClose = options.OnClose;

    if (Width == null) Width = 'max';
    if (Height == null) Height = 'max';

    if (options.backdrop == null) { options.backdrop = true; options.keyboard = true; }
    if (options.backdrop == false) options.keyboard = false;


    if (Width.toString().toLowerCase() == 'max') Width = ($(window).width() - 100);
    if (Height.toString().toLowerCase() == 'max') Height = ($(window).height() - 50);

    if (Width > ($(window).width() - 100)) { Width = ($(window).width() - 100) };

    $.get(Url, {}, function (result) {
        if (result.Status == 0) {
            if (result.Errors) {

                var Messages = '<ul>';
                $(result.Errors).each(function (key, value) {
                    Messages += '<li>' + value.ErrorMessage + '</li>';
                });
                Messages += '</ul>';
                AlertLarg(Messages); return;
            }
            if (result.Message) { AlertLarg(WarnningIcon + result.Message); return; }
        }
        if (options.GenerateDialog == true) {
            var dialogTemplate = `
<div class="modal fade" tabindex="-1" role="dialog">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">@@Title</h5>
            </div>
            <div class="modal-body">
                 @@Body
            </div> 
        </div>
    </div>
</div>
            `
            result = dialogTemplate.replace("@@Body", result).replace("@@Title", options.Title);
        }


        $("<div></div>").attr('id', 'Modal' + ModalIndex).attr('data-url', Url).appendTo("body").html(result);


        var AddModalOpenClass = $("body").hasClass('modal-open');
        $("#Modal" + ModalIndex).find('.modal-dialog').css('maxWidth', Width + 'px');

        $("#Modal" + ModalIndex).find(".modal").css({ 'zIndex': ModalIndex, 'background': 'rgba(0, 123, 255, .25)' }).on('hidden.bs.modal', function () {
            if (AddModalOpenClass == true) { $("body").addClass('modal-open'); }
            $(this).parents('*[data-url]').remove(); if (typeof OnClose === "function") { OnClose(); }
        }).modal({ backdrop: options.backdrop, keyboard: options.keyboard });

    });
}

//#endregion 

//#region Validation

function isValidNationalCode(input) {
    if (!/^\d{10}$/.test(input)) return false;

    var check = parseInt(input[9]);
    var sum = 0; var i;
    for (i = 0; i < 9; ++i) {
        sum += parseInt(input[i]) * (10 - i);
    }
    sum %= 11;

    return (sum < 2 && check == sum) || (sum >= 2 && check + sum == 11);
}

function ErrorDisplay(Errors) {
    var allError = '';
    $.each(Errors, function (key, Error) {

        var parent = $("#" + Error.PropertyName).parent().addClass('has-error');

        if (parent.find('.ErrorList').length == 0) {
            parent.append('<ul class="ErrorList"></ul>')
        }
        allError += '<li>' + Error.ErrorMessage + '</li>';
        parent.find('.ErrorList').append('<li>' + Error.ErrorMessage + '</li>')
    })

    return "<ul class='ErrorList'>" + allError + "</ul>";
}

function ResetGroup(GroupName) {
    var selector = 'input[data-groupname="' + GroupName + '"]';
    if (GroupName == null) selector = 'input:not([data-groupname])';

    $(selector).each(function () {
        if ($(this).attr('data-type') == 'Date') $('#' + $(this).attr('id') + 'Text').val('');
        if ($(this).prop('type') == 'file') {

            $(this).removeData('hasfile');
            window[$(this).prop('id') + 'Uploader'].clearPreviewPanel()
        }

        $(this).val('').parent().removeClass('has-error validated').find('ul').remove();
    })

    selector = 'select[data-groupname="' + GroupName + '"]';
    if (GroupName == null) selector = 'select:not([data-groupname]), select[data-groupname="All"]';

    $(selector).each(function () {
        $(this).val(null).trigger('change');
        $(this).parent().removeClass('has-error validated bounceIn').find('ul').remove();
    })
}

function ValidateGroup(GroupName) {
    var toReturn = [];

    var selector = 'input[data-groupname="' + GroupName + '"]';
    if (GroupName == null) selector = 'input:not([data-groupname])';

    $(selector).each(function () {
        toReturn = toReturn.concat(ValidateObject($(this)));
    })

    selector = 'select[data-groupname="' + GroupName + '"]';
    if (GroupName == null) selector = 'select:not([data-groupname])';

    $(selector).each(function () {
        toReturn = toReturn.concat(ValidateObject($(this)));
    })

    if (toReturn.length == 0) return null;

    var message = '<ul>';
    $(toReturn).each(function (index, value) {
        message += '<li>' + value + '</li>';
    });
    message += '</ul>';
    return message;
}

function ValidateObject(Obj) {
    var toReturn = [];

    if ($(Obj).prop("tagName").toLowerCase() == 'select') {
        // is required
        if ($(Obj).hasAttr('data-isrequired') && $(Obj).attr('data-isrequired').toLowerCase() == 'true') {
            var message = $(Obj).attr('data-isrequiredmessage');


            if (!message) {
                message = $(Obj).parent().find('label').first().text();
                if ($("body").hasClass("ltr")) { message += " Is Required." } else { message += ' الزامی می باشد '; }
            }

            if ($(Obj).val() == 0 || $(Obj).val() == '' || $(Obj).val() == null) {
                toReturn.push(message);
                ValidationErrorStatusSet($(Obj), [message]);
            }
        }

        if (toReturn.length != 0) return toReturn;

        ValidationValidStatusSet($(Obj));
        return toReturn;
    }

    if ($(Obj).attr('type') == 'radio') return toReturn;

    // file
    if ($(Obj).prop('type') == 'file') {
        if ($(Obj).hasAttr('data-isrequired')) {

            var message = $(Obj).attr('data-isrequiredmessage');
            if (!message) {
                message = $(Obj).parent().find('label').first().text()
                if ($("body").hasClass("ltr")) { message += " Is Required." } else { message += ' الزامی می باشد '; }
            }

            if (window[$(Obj).prop('id') + 'Uploader'].currentFileCount == 0) {
                toReturn.push(message);
            }
        }
        if ($(Obj).hasAttr('data-maxsize')) {
            var maxSize = parseInt($(Obj).attr('data-maxsize'));

            var message = $(Obj).attr('data-maxsizemessage');
            if (!message) {
                message = $(Obj).parent().find('label').first().text()
                if ($("body").hasClass("ltr")) {
                    message += " Max size limitation is: " + (maxSize / 1024) + ' kb'
                } else {
                    message += ' محدودیت حجم ارسالی ' + (maxSize / 1024) + ' kb';
                }
            }

            var firstFile = window[$(Obj).prop('id') + 'Uploader'].cachedFileArray[0];
            if (firstFile != null) {
                if (firstFile.size > maxSize) {
                    toReturn.push(message)
                }
            }
        }


    }

    // min len
    if ($(Obj).hasAttr('data-minlen')) {
        if ($(Obj).val() == null || $(Obj).val().length < $(Obj).data('minlen')) {
            var message = $(Obj).data('minlenmessage');
            if (!message) {
                message = $(Obj).parent().find('label').first().text();
                if ($("body").hasClass("ltr")) { message += ' must be at least ' + $(Obj).data('minlen') + ' character ' } else { message += ' حداقل ' + $(Obj).data('minlen') + ' کاراکتر باشد '; }
            }

            toReturn.push(message);
        }
    }

    // max len
    if ($(Obj).hasAttr('data-maxlen')) {
        if ($(Obj).val() == null || $(Obj).val().length > $(Obj).data('maxlen')) {
            var message = $(Obj).data('maxlenmessage');
            if (!message) {
                message = $(Obj).parent().find('label').first().text();
                if ($("body").hasClass("ltr")) { message += ' must be maximum ' + $(Obj).data('minlen') + ' character ' } else { message += ' حداکثر ' + $(Obj).data('minlen') + ' کاراکتر باشد '; }
            }

            toReturn.push(message);
        }
    }


    // min 
    if ($(Obj).hasAttr('data-min')) {
        if ($(Obj).val() == null || $(Obj).val() < $(Obj).data('min')) {
            var message = $(Obj).data('minmessage');
            if (!message) {
                message = $(Obj).parent().find('label').first().text();
                if ($("body").hasClass("ltr")) { message += ' must be at least ' + $(Obj).data('min') } else { message += ' حداقل باید ' + $(Obj).data('min') + ' باشد '; }
            }

            toReturn.push(message);
        }
    }

    // max 
    if ($(Obj).hasAttr('data-max')) {
        if ($(Obj).val() == null || $(Obj).val() > $(Obj).data('max')) {
            var message = $(Obj).data('maxmessage');
            if (!message) {
                message = $(Obj).parent().find('label').first().text();
                if ($("body").hasClass("ltr")) { message += ' must be maximum ' + $(Obj).data('max') } else { message += ' حداکثر ' + $(Obj).data('min') + ' باشد '; }
            }

            toReturn.push(message);
        }
    }

    // is required
    if ($(Obj).hasAttr('data-isrequired')) {
        var message = $(Obj).attr('data-isrequiredmessage');
        if (!message) {
            message = $(Obj).parent().find('label').first().text()
            if ($("body").hasClass("ltr")) { message += " Is Required." } else { message += ' الزامی می باشد '; }
        }

        if ($(Obj).prop('type') == 'file') {
            if (window[$(Obj).prop('id') + 'Uploader'].currentFileCount == 0) {
                toReturn.push(message);
            }
        }
        else {
            if ($(Obj).val() == null || $(Obj).val().length == 0) {
                toReturn.push(message);
            }
        }
    }

    // todo جای تکمیل کردن داره
    // is data-validatetype
    //if ($(Obj).hasAttr('data-validatetype')) {
    //    if ($(Obj).attr('data-validationtype') == 'date') {
    //        if (!Date.parse($(Obj).val())) { ValidationErrorStatusSet($(Obj), $(Obj).attr('data-validationtypemessage')); return false; }
    //    }
    //    else if ($(Obj).attr('data-validationtype') == 'number') {
    //        if (!isNumeric($(Obj).val())) { ValidationErrorStatusSet($(Obj), $(Obj).attr('data-validationtypemessage')); return false; }
    //    }
    //}

    var TrueNumberCharecter = "0123456789۰۱۲۳۴۵۶۷۸۹,";


    if ($(Obj).data('type') == "Number") {
        $($(Obj).val().split('')).each(function (index, value) {
            if (TrueNumberCharecter.indexOf(value) == -1) toReturn.push('لطفا عدد وارد کنید')
        });
    }

    var AlphabetType = $(Obj).attr('data-alphabettype');
    if (AlphabetType == "PersianAlphabet") {
        var TruePersianCharecter = "ا آ ب پ ت ث ج چ ح خ د ذ ر ز ژ س ش ص ض ط ظ ع غ ف ق ک گ ل م ن و ه ی ي" + TrueNumberCharecter;
        $($(Obj).val().split('')).each(function (index, value) {
            if (TruePersianCharecter.indexOf(value) == -1) { toReturn.push('لطفا حروف فارسی استفاده کنید'); }
        });
    }
    else if (AlphabetType == "EnglishAlphabet") {
        var TrueEnglishAlphabet = "abcdefghijgklmnopqrstuvwxyz " + TrueNumberCharecter;

        $($(Obj).val().split('')).each(function (index, value) {
            if (TrueEnglishAlphabet.indexOf(value.toLowerCase()) == -1) {
                if ($("body").hasClass('ltr')) { toReturn.push('please USE english alphabet'); } else { toReturn.push('لطفا از حروف انگلیسی استفاده کنید'); }

            }
        });
    }



    if (toReturn.length > 0) {
        ValidationErrorStatusSet(Obj, toReturn); return toReturn
    };

    ValidationValidStatusSet($(Obj));
    return [];
}

function ValidationValidStatusSet(Obj) {
    var col = $(Obj).parent();

    if (col.hasClass('form-check')) {
        col = col.parents('.Checkbox');
    } else if ($(Obj).prop('type') == 'file') {
        col = $(Obj).parents('.custom-file-container').parent();
    }

    col.removeClass('has-error').addClass('validated').find(".ErrorList").remove();
}

function ValidationErrorStatusSet(Obj, ErrorMessages) {
    var col = $(Obj).parent();

    if (col.hasClass('form-check')) {
        col = col.parents('.Checkbox');
    } else if ($(Obj).prop('type') == 'file') {
        col = $(Obj).parents('.custom-file-container').parent();
    }

    col.removeClass('validated').addClass('has-error').find(".ErrorList").remove();


    col.append('<ul class="ErrorList"></ul>');


    $(ErrorMessages).each(function (index, value) {
        if (col.find('.ErrorList li:contains(' + value + ')').length > 0) return;
        col.find('.ErrorList').append('<li>' + value + '</li>')
    })
}

function InputKeyup(Input) {
    var result = ValidateObject(Input);
    if (result.length > 0) return;

    if ($(Input).attr('data-type') == 'Number') {
        $(Input).digits();
    }
}

function ValidationEventBind(Obj) {
    $(Obj).change(function () {
        InputKeyup($(this));
    })

    if ($(Obj).prop('tagName').toLowerCase() == 'input') {
        if ($(Obj).attr('type') == 'file') {
            var Id = $(Obj).attr('id');

            var maxFileCount = $(Obj).data('maxfilecount');
            if (maxFileCount == null) maxFileCount = 10;


            var option = {
                maxFileCount: maxFileCount,
                text: {
                    chooseFile: '',
                    browse: 'انتخاب',
                    selectedCount: 'فایل انتخاب شده'
                }
            };
            if ($(Obj).hasAttr('data-presetfiles')) {
                option.presetFiles = JSON.parse($(Obj).attr('data-presetfiles'))
            }

            window[Id + 'Uploader'] = new FileUploadWithPreview(Id, option);

            $(Obj).parents('.custom-file-container').find(".custom-file-container__image-clear").click(function () {
                $(Obj).attr('data-json', JSON.stringify({ Status: 'Deleted' }));
            });

            $(Obj).change(async function () {

                var info = await getBase64FileSelector(Id);
                $(Obj).attr('data-json', JSON.stringify(info));

                ValidateObject(Obj);
            })

        }


        if ($(Obj).data('type') == 'Date') {

            DatePickerInit(Obj);
        }
        else if ($(Obj).data('type') == 'radio') {

            var Id = $(Obj).attr('id');
            $("input[name='" + Id + "Name'").change(function () {
                if ($(this).is(":checked")) $(Obj).val($(this).val())
            })
        }
        else if ($(Obj).data('type') == 'Number') {
            $(Obj).digits();
        }



        $(Obj).keyup(function (e) {
            if ($(Obj).hasAttr('data-onenter')) {
                if (e.keyCode == 13) { eval($(Obj).attr('data-onenter')) }
            }

            InputKeyup($(this))
        })
    }

    else if ($(Obj).prop('tagName').toLowerCase() == 'select') {

        if ($(Obj).hasClass('Select2')) {
            Select2Init(Obj)
        }
    }
}

function DatePickerInit(Obj) {
    var Id = $(Obj).attr('id');
    try {
        var options = {
            dateFormat: 'yyyy-MM-dd hh:mm:ss',
            targetDateSelector: '#' + Id,
            targetTextSelector: '#' + Id + 'Text',

            modalMode: $(Obj).data("modalmode"),
            isGregorian: $(Obj).data("isgregorian"),

            enableTimePicker: $(Obj).data("enabletimepicker"),
            disableAfterToday: $(Obj).data("disableaftertoday"),
            disableBeforeToday: $(Obj).data("disablebeforetoday"),
        };
        console.log(options);
        if ($(Obj).hasAttr('data-disableAfterDate')) { options.disableAfterDate = new Date($(Obj).data('disableafterdate')) }
        if ($(Obj).hasAttr('data-disableBeforeDate')) { options.disableBeforeDate = new Date($(Obj).data('disablebeforedate')) }

        window['DatePicker' + Id] = new mds.MdsPersianDateTimePicker(document.getElementById(Id + 'Icon'), options);
        if ($('#' + Id).val() != '') {
            window['DatePicker' + Id].setDate(new Date($('#' + Id).val()));
            window['DatePicker' + Id].updateSelectedDateText();
        }

    } catch (e) {
        console.log(e);
        console.log('Id:' + Id);
    }
}

function Select2Init(Obj) {
    var responseUrl = $(Obj).data('responseurl');
    var OnPostData = $(Obj).attr('id') + '_OnPostData';

    var options = { dir: 'rtl', language: 'fa' }
    if (responseUrl) {
        options.ajax = {
            url: responseUrl, quietMillis: 900, method: 'POST',
            data: function (params, page) {
                var data = { Query: params.term, PageNumber: page }
                if (typeof window[OnPostData] == 'function') { window[OnPostData](data); }
                return data;
            },
            processResults: function (data, params) { return { results: data.items, pagination: { more: (params.page * 10) < data.ItemsNumber } } },
            cache: true
        };
        options.templateResult = function (Item) { return $(Item.viewTemplate); };
        options.templateSelection = function (Item) { return Item.selectionTemplate; };
    }

    var parent = $(Obj).parents('.modal').first();
    if (parent.length > 0) {
        options.dropdownParent = parent;
    }
    $(Obj).select2(options);

    $(document).on("select2:open", () => {
        let allFound = document.querySelectorAll(
            ".select2-container--open .select2-search__field"
        );
        allFound[allFound.length - 1].focus();
    });

    var selecteditems = $(Obj).data("selecteditems");
    if (selecteditems) {
        $(selecteditems).each(function (index, item) {
            var data = { id: item.value, viewTemplate: item.text, selectionTemplate: item.text }
            $(Obj).select2('trigger', 'select', { data: data })
        })

    }
}

$("body").ready(function () {

    $("input,select").each(function () {
        ValidationEventBind($(this))
    })
})

//#endregion 

//#region GV
var GVs = {};
function gv(Id) {
    if (GVs[Id] != null) {
        return GVs[Id]
    };

    var GV = { Id: Id, Selector: '#' + Id };

    GV.PageGo = function (PageNumber) {

        if (PageNumber == 0) return;
        if (PageNumber > $("#PageCount").val() - 1) { PageNumber = 1; }

        $(GV.Selector).find('ul .active').removeClass("active").find("a").removeClass("bg-primary");
        $(GV.Selector).find('ul li[data-index=' + PageNumber + "]").addClass("active").find("a").addClass("bg-primary");

        GV.Refresh();
    }

    GV.AfterRefresh = function () { }

    GV.GetSubPanelStatus = function (Id) {
        var status = $("#" + GV.Id + " tr[data-id='" + Id + "']").attr('data-panelstatus');
        if (status == '' || status == null) return 'collapsed';
        return status;
    }

    GV.CollapseSubPanel = function (Id) {
        var tr = $("#" + GV.Id + " tr[data-id='" + Id + "']");
        $("#" + GV.Id + " tr[data-subpanel='" + Id + "']").remove();
        tr.attr('data-panelstatus', 'collapsed');
    }

    GV.ExpandSubPanel = function (Id, Content) {
        var tr = $("#" + GV.Id + " tr[data-id='" + Id + "']");
        if (tr.attr('data-panelstatus') == 'expanded') {
            $("#" + GV.Id + " tr[data-subpanel='" + Id + "']").remove();
        }
        tr.attr('data-panelstatus', 'expanded');
        $("<tr data-subpanel='" + Id + "' ><td colspan='99' class='bg-white p2-3'>" + Content + "</td></tr>").insertAfter(tr);
    }

    GV.ChangeViewType = function (ViewType) {
        $(GV.Selector).attr("data-viewtype", ViewType);
        return GV;
    }

    GV.TdGet = function (RowId, ColName) {
        var ColIndex = $(GV.Selector).find("th[data-columnname=" + ColName + "]").index();
        return $(GV.Selector).find("tr[data-id=" + RowId + "]").find("td").eq(ColIndex);
    }

    GV.DataSouceRequestGenerate = function () {
        var PageNumber = 1;
        PageNumber = $(GV.Selector).find("ul").find(".active").find("a").text();
        if (!$.isNumeric(PageNumber)) { PageNumber = 1; }

        var Filters = [];

        $(GV.Selector).find("th[data-columnname]").each(function (index, th) {
            var ColumnName = $(th).attr('data-columnname')
            var filterInput = $("#" + ColumnName + "ColumnFilters");
            if (filterInput.length == 0) return; if (filterInput.val() == '') return;


            var filters = jQuery.parseJSON(filterInput.val());

            $(filters).each(function (i, f) {
                Filters.push({
                    Field: ColumnName,
                    Operator: f.Operator,
                    Value: f.Value,
                    ValueTitle: f.ValueTitle
                });
            })
        })

        var DataSouceRequest = {};
        DataSouceRequest.Filters = Filters;
        DataSouceRequest.PageNumber = PageNumber;
        DataSouceRequest.PageSize = $(GV.Selector).attr("data-pagesize");
        return DataSouceRequest;
    }


    GV.Refresh = function () {

        var data = { Id: Id };
        data.DataSouceRequest = GV.DataSouceRequestGenerate();


        data["ColumnsSerialazed"] = $(GV.Selector).attr("data-columns");
        data["ActionsSerialazed"] = $(GV.Selector).attr("data-actions");

        data["ColumnKeyName"] = $(GV.Selector).attr("data-columnkeyname");
        data["ExcelRowColumns"] = $(GV.Selector).attr("data-excelrowcolumns");

        data["MenuGenerationIsInCustom"] = $(GV.Selector).attr("data-menugenerationisinCustom");
        data["CustomizationPartialViewAddress"] = $(GV.Selector).attr("data-customizationpartialviewaddress");

        data["ViewType"] = $(GV.Selector).attr("data-viewtype");

        data = GV.OnDataSendToServer(data);
        $.post($(GV.Selector).attr("data-responseurl"), data, function (Result) {
            if (Result.Status == 0) {
                if (Result.Errors) {
                    var allError = '';
                    $.each(Result.Errors, function (key, Error) {
                        allError += '<li>' + Error.ErrorMessage + '</li>';
                    })

                    Alert("<ul class='ErrorList'>" + allError + "</ul>"); return;
                }
                if (Result.Message) { Toast({ message: Result.Message }); return; }
            }
            if (data["ViewType"] == 'ExcelExport') {
                var json = JSON.parse(Result);

                var str = '';
                for (var i = 0; i < json.length; i++) {
                    var line = '';
                    for (var index in json[i]) {
                        if (line != '') line += ','

                        var value = json[i][index];
                        if (value == null) value = '';
                        if (value.indexOf(',') > 0) value = `"${value}"`;
                        line += (value === null ? '' : value);
                    }

                    str += line + '\r\n';
                }


                var universalBOM = "\uFEFF";
                var a = window.document.createElement('a');
                a.setAttribute('href', 'data:text/csv; charset=utf-8,' + encodeURIComponent(universalBOM + str));
                a.setAttribute('download', 'data.csv');
                window.document.body.appendChild(a);
                a.click();



                //var blob = new Blob([str], { type: 'text/csv;charset=utf-8;' });
                //if (navigator.msSaveBlob) { // IE 10+
                //    navigator.msSaveBlob(blob, exportedFilenmae);
                //} else {
                //    var link = document.createElement("a");
                //    if (link.download !== undefined) { // feature detection
                //        // Browsers that support HTML5 download attribute
                //        var url = URL.createObjectURL(blob);
                //        link.setAttribute("href", url);
                //        link.setAttribute("download", "data.csv");
                //        link.style.visibility = 'hidden';
                //        document.body.appendChild(link);
                //        link.click();
                //        document.body.removeChild(link);
                //    }
                //}

            } else {
                $(GV.Selector).html(Result);

                $(GV.Selector).find('tr[data-id*="details-collapse"]').click(function () {
                    $(this).toggleClass('active').next('.row-detail').toggleClass('d-none d-table-row');
                });

            }

            GV.AfterRefresh();
        })

    }

    GV.OnDataSendToServer = function (data) {
        return data
    }

    GV.ColumnFilterApply = function (ColumnName) {
        var Filters = [];
        $("#" + ColumnName + "Manage").find(".row").not(".d-none").each(function () {
            var Filter = {
                Field: ColumnName,
                Operator: $(this).find(".Operator").find("select").val()
            };

            var ValueCol = $(this).find(".Value");



            if (ValueCol.find('input').length == 0) {
                var select = $(this).find(".Value").find("select");
                Filter.Value = select.val();

                if (select.hasClass('Select2')) {
                    if (select.select2('data').length == 0) return;
                    Filter.ValueTitle = select.select2('data')[0].selectionTemplate;
                }
            }
            else if (ValueCol.find('input').length == 1) { // textbox
                var input = ValueCol.find("input");

                Filter.Value = input.val();
            } else {
                var label = ValueCol.find('label');
                var Id = label.attr('for');


                var input = ValueCol.find("input");
                Filter.Value = $("#" + Id).val();
            }

            Filters.push(Filter);
        });

        $("#" + ColumnName + "ColumnFilters").val(JSON.stringify(Filters));
        $("#" + ColumnName + "Manage").modal('hide');
        GV.Refresh();
    }

    GV.FilterPanelShow = function (ColumnName) {
        $("#" + ColumnName + "Manage").modal('show');
        var filters = jQuery.parseJSON($("#" + ColumnName + "ColumnFilters").val());

        $(filters).each(function (i, f) {
            GV.FilterAdd(ColumnName, f.Operator, f.Value, f.ValueTitle)
        })

        $("#" + ColumnName + "ColumnFilters").val('');
        if (filters.length == 0) GV.FilterAdd(ColumnName);
    }

    GV.FilterAdd = function (ColumnName, Operator = '3', Value = '', ValueTitle = '') {
        var th = $("#" + ColumnName + "Manage");
        var rows = th.find(".row");

        var newRow = rows.first().clone().removeClass('d-none');
        newRow.appendTo(rows.first().parent());

        var Operator = newRow.find(".Operator").find('select').val(Operator);
        Operator.attr('id', Operator.attr('id') + rows.length);

        var ValueCol = newRow.find(".Value");


        if (ValueCol.find('input').length == 0) {
            var select = ValueCol.find('select');
            select.attr('id', select.attr('id') + rows.length);

            if (select.hasClass('Select2')) {
                select.attr('data-selectedvalue', Value);
                select.attr('data-selectedtext', ValueTitle);
                Select2Init(select);
            } else {
                select.val(Value);
            }

        }
        else if (ValueCol.find('input').length == 1) { // textbox
            var input = ValueCol.find('input');
            input.val(Value);
            input.attr('id', input.attr('id') + rows.length);
        }
        else { // datebox

            var label = ValueCol.find('label');
            var Id = label.attr('for');
            label.attr('for', Id + rows.length);


            var InputText = ValueCol.find("input").eq(0);
            InputText.attr('id', Id + rows.length + 'Text');

            var InputValue = ValueCol.find("input").eq(1);
            InputValue.attr('id', Id + rows.length);
            InputValue.val(Value);



            var span = ValueCol.find('span');
            span.attr('id', Id + rows.length + 'Icon');

            DatePickerInit(InputValue.get(0))
        }



        if (rows.length > 2) { newRow.find(".Condition").parents('.d-none').removeClass('d-none'); }
    }

    GV.FilterRemove = function (Obj) {
        $(Obj).parents('.row').first().remove();
    }

    GV.Init = function () {
        if ($(GV.Selector).attr('data-autoFillOnClientSide') == 'true') {
            GV.Refresh();
        }
    }

    GVs[GV.Id] = GV;
    return GVs[GV.Id];
}

function GVPageGo(Obj, PageNumber) {
    var GV = $(Obj).parents("*[data-responseurl]");
    gv(GV.attr("id")).PageGo(PageNumber);
}

//#endregion