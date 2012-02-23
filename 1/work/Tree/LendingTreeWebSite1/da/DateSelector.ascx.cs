using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DateSelector : System.Web.UI.UserControl
{
    //-------------------------------------------------------------------------
    #region Event Handlers
    //-------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void Page_Init()
    {
        this.DateFormat = "yyyy/MM/dd";
        this.DisplayFormat = "MM/dd/yyyy";
    }

    protected void DateSelectorTextBox_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox)
        {
            string selectedDate = (sender as TextBox).Text;
            if (!string.IsNullOrWhiteSpace(selectedDate))
            {
                DateTime parsedDate;
                bool successfullyParsedDate = DateTime.TryParse(selectedDate, out parsedDate);
                if (successfullyParsedDate)
                {
                    SelectedDate = parsedDate;
                }
            }
        }
    }
    #endregion
    //-------------------------------------------------------------------------

    //-------------------------------------------------------------------------
    #region Private Methods
    //-------------------------------------------------------------------------
    private void SetSelectedDate(DateTime selectedDate)
    {
        PutSelectedDateInSession(selectedDate);
        DateSelectorTextBox.Text = selectedDate.ToString(this.DisplayFormat);
    }

    private void PutSelectedDateInSession(DateTime selectedDate)
    {
        if (GetSelectedDateFromSession() != selectedDate)
        {
            Session[this.SelectedDateSessionKey] = selectedDate;
        }
    }

    private DateTime GetSelectedDate()
    {
        return GetSelectedDateFromSession();
    }

    private DateTime GetSelectedDateFromSession()
    {
        DateTime? dateInSession = Session[this.SelectedDateSessionKey] as DateTime?;
        if (dateInSession != null)
        {
            return dateInSession.Value;
        }
        else
        {
            return DateTime.MinValue;
        }
    }
    #endregion
    //-------------------------------------------------------------------------

    //-------------------------------------------------------------------------
    #region Public Methods and Properties
    //-------------------------------------------------------------------------
    public DateTime SelectedDate
    {
        get { return GetSelectedDate(); }
        set { SetSelectedDate(value); }
    }

    public string SelectedDateSessionKey
    {
        get;
        set;
    }

    public string DateFormat
    {
        get;
        set;
    }

    public string DisplayFormat
    {
        get { return DateSelectorTextBoxCalendarExtender.Format; }
        set { DateSelectorTextBoxCalendarExtender.Format = value; }
    } 
    #endregion
    //-------------------------------------------------------------------------
}
