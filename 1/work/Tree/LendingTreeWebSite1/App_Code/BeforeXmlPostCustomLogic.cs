using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

public class BeforeXmlPostCustomLogic
{
    private System.Xml.Linq.XDocument _xDoc;

    public System.Xml.Linq.XDocument XDoc
    {
        get { return _xDoc; }
        set { _xDoc = value; }
    }

    public BeforeXmlPostCustomLogic(System.Xml.Linq.XDocument xDoc)
    {
        this._xDoc = xDoc;
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
        if (ApplicantHasBadCredit && FifityFiftyChance)
        {
            CreditHistory.Value = "MAJORCREDITPROBLEMS";
        }
    }

    XElement CreditHistory
    {
        get
        {
            return this.XDoc.Root
                        .Element("Applicant")
                        .Element("CreditHistory")
                        .Element("CreditSelfRating");
        }
    }

    bool ApplicantHasBadCredit
    {
        get
        {
            return CreditHistory.Value == "LITTLEORNOCREDITHISTORY";
        }
    }

    private static bool FifityFiftyChance
    {
        get
        {
            return new Random().Next(2) == 1;
        }
    }
}
