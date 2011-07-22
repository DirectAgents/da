//-----------------------------------------------
// global
//-----------------------------------------------
function Dropdown(ddlname) {
    this.Value = 0;
    this.Selected = false;
    this.DDL = ddlname;
    this.Limited = false;
    var that = this;
    this.SelectLast = function () {
        $(that.DDL + ' option:last').attr('selected', 'selected');
    }
    $(ddlname).change(function () {
        var v = $(this).val();
        if (typeof v == 'number') {
            that.Value = v;
            that.Selected = true;
        }
    });
    return true;
};

var Val = new Dropdown('#ApproximatePropertyValue1_DropDownList1');
var MortBal = new Dropdown('#ApproximatePropertyValue1_DropDownList1');
var CashOut = new Dropdown('#AmountDesiredAtClosing1_DropDownList1');

var g = {};
g.MaxLTV = 0.85;

var Logger = {
    Log: function (message) {
        return true;
    }
};

//-----------------------------------------------
// drop down change event handlers
//-----------------------------------------------
$(document).ready(function () {
    $(Val.DDL).change(function () {
        var v = $(this).val();
        if (typeof v == 'number') {
            Val.Value = v;
            Val.Selected = true;
        }
        FixAvailableMortgageBalances();
        FixAvailableCashOutOptions();
        CashOut.SelectLast();
    });
    $(MortBal.DDL).change(function () {
        var v = $(this).val();
        if (typeof v == 'number') {
            Val.Value = v;
            Val.Selected = true;
        }
        FixAvailableCashOutOptions();
        CashOut.SelectLast();
    });
    $(g.Amt_CashOut_DDL).change(function () {
        var v = $(this).val();
        if (typeof v == 'number') {
            Val.Value = v;
            Val.Selected = true;
        }
    });
});

//-----------------------------------------------
// LTV calculators
//-----------------------------------------------
function MaxLTV_MortBal(amt) {
    return (amt) < (Val.Value * g.MaxLTV);
}
function MaxLTV_CashOut(amt) {
    return (amt + g.Amt_MortBal) < (Val.Value * g.MaxLTV);
}

//-----------------------------------------------
// mortgage rate balance adjustments
//-----------------------------------------------
function FixAvailableMortgageBalances() {
    if (Val.Selected && Val.Value > 0) {
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
    if (Val.Selected && Val.Value > 0) {
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
    //log.debug('limiting choices for ' + ddl);
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
        //log.debug('removing choices with index > ' + max);
        $(ddl + ' option:gt(' + max + ')').remove();
    }
    else {
        //log.debug('NOT removing choices');
    }
}
function ResetChoices(ddl, options) {
    //log.debug('resetting choices for ' + ddl);
    $(ddl).find('option').remove();
    $.each(options, function (val, text) {
        $(ddl).append(
            $('<option></option>').val(val).html(text)
        );
    });
}
