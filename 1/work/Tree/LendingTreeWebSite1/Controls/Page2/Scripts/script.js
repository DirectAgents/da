//-----------------------------------------------
// global context
//-----------------------------------------------
var g = {};

g.MaxLTV = 0.90;
g.Val_DDL = '#ApproximatePropertyValue1_DropDownList1';
g.Val_Selected = false;
g.Val = 0;

g.Amt_MortBal_Selected = false;
g.Amt_MortBal_Limited = false;
g.Amt_MortBal = 0;
g.Amt_MortBal_DDL = '#MortgageBalance1_DropDownList1';

g.Amt_CashOut_Selected = false;
g.Amt_CashOut_Limited = false;
g.Amt_CashOut = 0;
g.Amt_CashOut_DDL = '#AmountDesiredAtClosing1_DropDownList1';

//-----------------------------------------------
// drop down change event handlers
//-----------------------------------------------
$(document).ready(function () {
    $(g.Val_DDL).change(function () {
        log.debug('Val_DDL changed');
        log.debug("Max LTV is " + g.MaxLTV);
        try {
            g.Val = parseInt($(this).val());
            log.debug('Val=' + g.Val);
            g.Val_Selected = true;
        }
        catch (e) {
            log.debug('exception caught');
            g.Val_Selected = false;
            log.debug('Val_Selected=' + g.Val_Selected);
            g.Val = 0;
            log.debug('Val=' + g.Val);
        }
        FixAvailableMortgageBalances();
        FixAvailableCashOutOptions();
        UpdateLTVMon()
    });
    $(g.Amt_MortBal_DDL).change(function () {
        log.debug('Amt_MortBal_DDL changed');
        try {
            g.Amt_MortBal = parseInt($(this).val());
            log.debug('Amt_MortBal=' + g.Amt_MortBal);
            g.Amt_MortBal_Selected = true;
        }
        catch (e) {
            log.debug('exception caught');
            g.Amt_MortBal_Selected = false;
            g.Amt_MortBal_Selected = 0;
        }
        FixAvailableCashOutOptions();
        UpdateLTVMon()
    });
    $(g.Amt_CashOut_DDL).change(function () {
        try {
            g.Amt_CashOut = parseInt($(this).val());
            log.debug('Amt_CashOut=' + g.Amt_CashOut);
            g.Amt_CashOut_Selected = true;
        }
        catch (e) {
            log.debug('exception caught');
            g.Amt_CashOut_Selected = false;
            g.Amt_CashOut = 0;
        }
        UpdateLTVMon()
    });
});

//-----------------------------------------------
// LTV monitor (debug)
//-----------------------------------------------
function UpdateLTVMon() {
    try {
        var ltv = (parseFloat(g.Amt_MortBal) + parseFloat(g.Amt_CashOut)) / parseFloat(g.Val);
        $("#ltvmon_val").text(toFixed(ltv, 2) + "%");
    }
    catch (e) {
        $("#ltvmon_val").text('n/a');
    }
}

function toFixed(value, precision) {
    var precision = precision || 0,
    neg = value < 0, power = Math.pow(10, precision),
    value = Math.round(value * power),
    integral = String((neg ? Math.ceil : Math.floor)(value / power)),
    fraction = String((neg ? -value : value) % power),
    padding = new Array(Math.max(precision - fraction.length, 0) + 1).join('0');
    return precision ? integral + '.' + padding + fraction : integral;
}

//-----------------------------------------------
// LTV calculators
//-----------------------------------------------
function MaxLTV_MortBal(amt) {
    return (amt) < (g.Val * g.MaxLTV);
}
function MaxLTV_CashOut(amt) {
    return (amt + g.Amt_MortBal) < (g.Val * g.MaxLTV);
}

//-----------------------------------------------
// mortgage rate balance adjustments
//-----------------------------------------------
function FixAvailableMortgageBalances() {
    if (g.Val_Selected && g.Val > 0) {
        ResetAvailableMortgageBalances();
        LimitChoices(g.Amt_MortBal_DDL, MaxLTV_MortBal);
        g.Amt_MortBal_Limited = true;
    }
}
function ResetAvailableMortgageBalances() {
    if (g.Amt_MortBal_Limited) {
        ResetChoices(g.Amt_MortBal_DDL, MortgageBalanceOptions);
        g.Amt_MortBal = 0;
        g.Amt_MortBal_Selected = false;
    }
}

//-----------------------------------------------
// cash out option adjustments
//-----------------------------------------------
function FixAvailableCashOutOptions() {
    if (g.Val_Selected && g.Val > 0) {
        ResetAvailableCashOutOptions();
        LimitChoices(g.Amt_CashOut_DDL, MaxLTV_CashOut);
        g.Amt_CashOut_Limited = true;

    }
}
function ResetAvailableCashOutOptions() {
    if (g.Amt_CashOut_Limited) {
        ResetChoices(g.Amt_CashOut_DDL, AmountDesiredAtClosingOptions);
        g.Amt_CashOut = 0;
        g.Amt_CashOut_Selected = false;
    }
}

//-----------------------------------------------
// drop down list helpers
//-----------------------------------------------
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
    $.each(options, function (val, text) {
        $(ddl).append(
            $('<option></option>').val(val).html(text)
        );
    });
}
