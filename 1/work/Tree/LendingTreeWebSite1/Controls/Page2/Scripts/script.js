var g = {};
g.MaxLTV = 0.97;
g.Val_DDL = '#ApproximatePropertyValue1_DropDownList1';
g.Val_Selected = false;
g.Val = 0;
g.Amt_MortBal_Selected = false;
g.Amt_MortBal_Limited = false;
g.Amt_MortBal = 0;
g.Amt_MortBal_DDL = '#MortgageBalance1_DropDownList1';

$(document).ready(function () {
    $(g.Val_DDL).change(function () {
        log.debug('Val_DDL changed');
        try {
            g.Val = parseInt($(this).val());
            log.debug('Val=' + g.Val);
            g.Val_Selected = true;
            log.debug('Val_Selected=' + g.Val_Selected);
        }
        catch (e) {
            log.debug('exception caught');
            g.Val_Selected = false;
            log.debug('Val_Selected=' + g.Val_Selected);
            g.Val = 0;
            log.debug('Val=' + g.Val);
        }
        FixAvailableMortgageBalances();
        //FixAvailableCashOutOptions();
    });
});

function MaxLTV_MortBal(amt) {
    return (amt) < (g.Val * g.MaxLTV);
}

function FixAvailableMortgageBalances() {
    if (g.Val_Selected && g.Val > 0) {

        log.debug('fixing available mortgage balances');

        ResetAvailableMortgageBalances();

        log.debug('limiting available mortgage balances');
        LimitChoices(g.Amt_MortBal_DDL, MaxLTV_MortBal);
        g.Amt_MortBal_Limited = true;

    } else {
        log.debug('NOT fixing available mortgage balances');
    }
}

function ResetAvailableMortgageBalances() {
    if (g.Amt_MortBal_Limited) {
        log.debug('resetting available mortgage balances');
        ResetChoices(g.Amt_MortBal_DDL, MortgageBalanceOptions);
        g.Amt_MortBal = 0;
        g.Amt_MortBal_Selected = false;
    }
    else {
        log.debug('NOT resetting available mortgage balances');
    }
}

function LimitChoices(ddl, f) {
    log.debug('limiting choices for ' + ddl);
    var max = 0;
    $(ddl).each(function () {
        $('option', this).each(function (i) {
            if (i == 0) return true;
            var amt = 0;
            try {
                amt = parseInt($(this).val());
            } catch (e) {
                return true;
            }
            if (f(amt)) {
                max = i;
            }
            else {
                return false;
            }
        });
    });
    if (max > 0) {
        log.debug('removing choices with index > ' + max);
        $(ddl + ' option:gt(' + max + ')').remove();
    }
    else {
        log.debug('NOT removing choices');
    }
}

function ResetChoices(ddl, options) {
    log.debug('resetting choices for ' + ddl);
    $(ddl).find('option').remove();
    log.debug('options removed');
    $.each(options, function (val, text) {
        $(ddl).append(
            $('<option></option>').val(val).html(text)
        );
    });
    log.debug('original options restored');
}

//    $(Amt_MortBal_DDL).change(function () {
//        try {
//            Amt_MortBal = parseInt($(this).val());
//            Amt_MortBal_Selected = true;

//            //            if (Val > 0) {
//            //                FixAvailableCashOutOptions();
//            //            }
//            //            LimitChoices('#AmountDesiredAtClosing1_DropDownList1', MaxLTV);
//        }
//        catch (e) {
//            Amt_MortBal = 0;
//            Amt_MortBal_Selected = false;
//        }
//    });

//var CashOutSelected = false;
//var CashOutAmount = 0;
//var CashOutAmountDDL = '#MortgageBalance1_DropDownList1';

//function FixAvailableCashOutOptions() {
//    if (CashOutSelected) {
//        ResetChoices('#AmountDesiredAtClosing1_DropDownList1', AmountDesiredAtClosingOptions);
//    }
//    LimitChoices('#AmountDesiredAtClosing1_DropDownList1', MaxLTV);
//}

//function ResetAvailableCashOutOptions() {
//    ResetChoices('#AmountDesiredAtClosing1_DropDownList1', MortgageBalanceOptions);
//    CashOutAmount = 0;
//    CashOutSelected = false;
//}

//$(document).ready(function () {

//    $("#MortgageBalance1_DropDownList1").change(function () {
//        try {
//            Amt_MortBal = parseInt($(this).val());
//            Amt_MortBal_Selected = true;
//            if (Val > 0) {
//                FixAvailableCashOutOptions();
//            }
//            LimitChoices('#AmountDesiredAtClosing1_DropDownList1', MaxLTV);
//        }
//        catch (e) {
//            Amt_MortBal = 0;
//            Amt_MortBal_Selected = false;
//        }
//    });

//    $("#AmountDesiredAtClosing1_DropDownList1").change(function () {
//        try {
//            CashOutAmount = parseInt($(this).val());
//            CashOutSelected = true;
//        }
//        catch (e) {
//            CashOutAmount = 0;
//            CashOutSelected = false;
//            return;
//        }
//    });
//});
