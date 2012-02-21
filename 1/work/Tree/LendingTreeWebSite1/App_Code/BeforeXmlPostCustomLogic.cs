using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

public class BeforeXmlPostCustomLogic
{    
    public BeforeXmlPostCustomLogic(System.Xml.Linq.XDocument xDoc)
    {
        this._xDoc = xDoc;
    }

    public System.Xml.Linq.XDocument XDoc
    {
        get { return _xDoc; }
        set { _xDoc = value; }
    }

    public string ReferringCdNumber
    {
        set
        {
            _xDoc.Root.SetAttributeValue("affid", value);
        }
    }

    public void Check()
    {
        if (ApplicantHasBadCredit && OneChanceIn(3))
        {
            CreditHistory.Value = "MAJORCREDITPROBLEMS";
        }
    }

    XElement CreditHistory
    {
        get
        {
            return this.XDoc.Root.Element("Request").Element("Applicant").Element("CreditHistory").Element("CreditSelfRating");
        }
    }

    bool ApplicantHasBadCredit
    {
        get
        {
            return CreditHistory.Value == "LITTLEORNOCREDITHISTORY";
        }
    }

    static bool OneChanceIn(int num)
    {
        return new Random().Next(num) == num - 1;
    }

    System.Xml.Linq.XDocument _xDoc;
}
