
function STMRCWindow(page, sHeight, swidth) {
    if (swidth && sHeight) {
        window.open(page, "CtrlWindow", ",toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,dependent=no,directories=no,width=" + swidth + ",height=" + sHeight + ",x=50,y=50");
    }
    else {
        window.open(page, "CtrlWindow", "toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,dependent=no,directories=no,width=500,height=540,x=50,y=50");
    }
}

var defaultPromoID = '00001';

function prepopPromoObject(obj) {
    if (obj != null) {
        $("#topheader").css({ 'height': '130px' });
        //hide the tagline
        $("#tagline").hide();
        $("#promoHeader").removeClass('hide');
        if (obj.Templates[0].Header) {
            $("#promoHeader").html(obj.Templates[0].Header);
            $("#promoSubHeader").html(stripUnicode(obj.Templates[0].SubHeader));
            $("#promoDisclosure a").attr("href", "javascript:STMRCWindow('http://www.lendingtree.com/stmrc/promotional_disclosures.asp?disclosures=" + obj.DisclosureIDs.join() + "')");
            $("#promoDisclosure a").text("Advertising Disclosures");
        }
        
    }
}

function prepopPromoInfo(promoID) {
    if (parseInt(promoID) > -1) {
        Tree.API.LendingTree.getPromoInfo(promoID, function (promoInfo) {
            if (promoInfo == null || promoInfo.Status != "1") { // invalid promo resort to default
                Tree.API.LendingTree.getPromoInfo(defaultPromoID, function (promoInfo) {
                    prepopPromoObject(promoInfo)
                });
            } else {
                prepopPromoObject(promoInfo);
            }
        });
    }
}

function clearTextBoxHints() {
    $("input.hint_text").val("").removeClass("hint_text");
};

function applyTextBoxHints () {
    $("input[type=text]").each(function () {
        //Ensure fields that are hidden are not applied with the 'hint_text'
        // Ideal is to use HiddenValiable for hidden fields
        if ($(this).css("display") != "none") {
            if (($(this).val() == "" && $(this).attr("title") != "") || ($(this).val() == $(this).attr("title"))) {
                $(this).val($(this).attr("title")).addClass("hint_text");
            }
        }
    });
}

positionModal = function(){
	 var wWidth = window.innerWidth;
	 var wHeight = window.innerHeight;
	 if (wWidth==undefined) {
		 wWidth = document.documentElement.clientWidth;
		 wHeight = document.documentElement.clientHeight;
	 }
	 var boxLeft = parseInt((wWidth / 2) - ( $(".errorNotification").width() / 2 ));
	 var boxTop = parseInt((wHeight / 2) - ( $(".errorNotification").height() / 2 ));
	 if (boxTop<0){
		boxTop = 0; 
	}
	 // position modal
	 $(".errorNotification").css('margin', boxTop + 'px 0 0 ' + boxLeft + 'px');		
}

loadNotice = function() {
    $("body").append('<div id="modalBackground"></div>');
    $("#modalBackground").css("opacity", 0).fadeTo("slow", "0.85");
    $("body").append('<div id="modalWrapper"></div>')

    $('.errorNotification').remove();
    var $err = $('<div>').addClass('errorNotification').css({ 'top': 10 });
    $err.html("<div class=\"content\"><h3>Please wait while LendingTree processes your request</h3><img src=\"images/loading-bar.gif\" alt=\"loading...\" width=\"500\" height=\"21\" /></div>");
    $("#modalWrapper").append($err);
    $err.fadeIn('slow');
    return false;
}


var clearErrors = function () {
    $(".error-icon").remove();
    $(".error-text").remove();
    $(".dropdown-item-error").removeClass("dropdown-item-error");
    $(".enter-oneline-item-error").removeClass("enter-oneline-item-error");
    $(".enter-item-error").removeClass("enter-item-error");
    $("#ErrorCorrection").hide();
}

var clearError = function (target) {
    var $target = $(target);
    var $item;
    if ($target.closest("div.oneline-item").length == 1) {
        $item = $target.closest("div.oneline-item").removeClass("enter-oneline-item-error").removeClass("enter-item-error");
        if ($target.attr("tagName") == "SELECT") $item.removeClass("dropdown-item-error");
    } else if ($target.attr("tagName") == "INPUT") {
            $item = $target.closest("div.enter-item").removeClass("enter-item-error");
    } else {
        $item = $target.closest("div.dropdown-item").removeClass("dropdown-item-error");
    }

    $(".error-icon", $item).remove();
    $(".error-text", $item).remove();
    return true;
}

var showError = function ($target, message) {
    var $item;
    $item = $target.closest("div.answer").addClass("enter-item-error");
    var clear = $("div:last-child", $item).hasClass("clear");
    if (clear) $("div.clear", $item).remove();
    if ($("div:last-child", $item).hasClass("smallLabel")) {
        $item = $("div:last-child", $item);
        $item.before($("<span>&nbsp;</span>").addClass("error-icon"));
        $item.after($("<div></div>").addClass("error-text").text(message));
    }
    else {
        $item.append().append($("<span></span>").addClass("error-icon")).append($("<div></div>").addClass("error-text").text(message));
    }
    if (clear) $item.append($("<div></div>").addClass("clear"));
}



function showOrHideImage($target, bShow, src) {
    if (bShow) {
        $target.css("display", "inline");
        $target.attr("src", src);
    }
    else {
        $target.css("display", "none");
    }
}


