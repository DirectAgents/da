//-----------------------------------------------
// global context
//-----------------------------------------------
var g = {};
g.MaxLTV = 0.85;
g.Val_DDL = '#ApproximatePropertyValue1_DropDownList1';
g.Val = 0;
g.Amt_MortBal = 0;
g.Amt_MortBal_DDL = '#MortgageBalance1_DropDownList1';
g.Amt_CashOut = 0;
g.Amt_CashOut_DDL = '#CashOut1_DropDownList1';

//-----------------------------------------------
// drop down change event handlers
//-----------------------------------------------
$(document).ready(function () {
    $(g.Val_DDL).change(function () {
        g.Val = parseInt($(this).val());
        FixAvailableMortgageBalances();
        FixAvailableCashOutOptions();
        SelectLastOption(g.Amt_CashOut_DDL);
    });
    $(g.Amt_MortBal_DDL).change(function () {
        g.Amt_MortBal = parseInt($(this).val());
        FixAvailableCashOutOptions();
        SelectLastOption(g.Amt_CashOut_DDL);
    });
    $(g.Amt_CashOut_DDL).change(function () {
        g.Amt_CashOut = parseInt($(this).val());
    });
});
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
// Drop down adjustments
//-----------------------------------------------
function FixAvailableMortgageBalances() {
    if (g.Val > 0) {
        if ($(g.Amt_MortBal_DDL + ' option').length < MortgageBalanceLength) {
            ResetChoices(g.Amt_MortBal_DDL, MortgageBalanceOptions);
            g.Amt_MortBal = 0;
        }
        LimitChoices(g.Amt_MortBal_DDL, MaxLTV_MortBal);
    }
}
function FixAvailableCashOutOptions() {
    if (g.Val > 0) {
        if ($(g.Amt_CashOut_DDL + ' option').length < CashOutLength) {
            ResetChoices(g.Amt_CashOut_DDL, CashOutOptions);
            g.Amt_CashOut = 0;
        }
        LimitChoices(g.Amt_CashOut_DDL, MaxLTV_CashOut);
    }
}
//-----------------------------------------------
// Drop down adjustment helpers
//-----------------------------------------------
function LimitChoices(ddl, f) {
    var max = 0;
    $(ddl).each(function () {
        $('option', this).each(function (i) {
            var amt = parseInt($(this).val());
            if (!f(amt)) {
                return false;
            }
            else {
                max = i;
            }
        });
    });
    $(ddl + ' option:gt(' + max + ')').remove();
}
function ResetChoices(ddl, options) {
    $(ddl).find('option').remove();
    $.each(options, function (val, text) {
        $(ddl).append(
            $('<option></option>').val(val).html(text)
        );
    });
}
function SelectLastOption(ddl) {
    $(ddl + ' option:last').attr('selected', 'selected');
}

// problem domain
// - D, dropdowns with increasing values
// - V, subset of DD
// - X, decimal 0<X<=1
// - D-V constrained such that SUM(D-V)<X*SUM(V)