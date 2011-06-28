﻿using System;
using System.ComponentModel;
using LendingTreeLib.Schemas;
using Microsoft.Practices.Unity;
using System.Linq;

namespace LendingTreeLib
{
    public class LendingTreeModel : INotifyPropertyChanged, ILendingTreeModel
    {
        LendingTreeAffiliateRequest _data = null;

        [Dependency]
        public LendingTreeConfig LendingTreeConfig { get; set; }

        [Dependency("StatesExcludedFromDisclosure")]
        public string StatesExcludedFromDisclosure { get; set; }

        /// <summary>
        /// LendingTreeAffiliateRequest holds the information that translates to an XML POST.
        /// It is lazily initialized so Page_Load has a chance to restore it from Session.
        /// </summary>
        public LendingTreeAffiliateRequest Data
        {
            get
            {
                if (_data == null)
                {
                    _data = new LendingTreeAffiliateRequest();
                    _data.Request.SourceOfRequest = LendingTreeConfig.SourceOfRequest;
                    OnPropertyChanged(this, DataPropertyName);
                }
                return _data;
            }
            set
            {
                if (_data != null)
                {
                    throw new Exception("model already exists");
                }
                _data = value;
            }
        }

        /// <summary>
        /// This initializes the model and should be called before accessing other members.
        /// </summary>
        void ILendingTreeModel.Initialize()
        {
            if (this.AppID == Guid.Empty.ToString())
            {
                this.AppID = Guid.NewGuid().ToString();
            }
        }

        /// <summary>
        /// This is used by the client to determine if Initialize needs to be called.
        /// </summary>
        /// <returns></returns>
        bool ILendingTreeModel.RequiresInitialization()
        {
            return this.AppID == Guid.Empty.ToString();
        }

        public string DataPropertyName { get { return "Data"; } }

        public string GetXMLForPost()
        {
            string xml = Data.ToString();
            return xml;
        }

        public object this[string propertyName]
        {
            get
            {
                return GetType().GetProperty(propertyName).GetValue(this, null);
            }
        }

        #region Events/Notification

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// When a property changes, if that property is copied to the underlying LendingTreeAffiliateRequest
        /// (a.k.a. the "Data" property), then we send one notification for the Data property followed by the 
        /// another for the actual property.
        /// </summary>
        /// <param name="propertyName"></param>
        void OnDataChanged(string propertyName)
        {
            OnPropertyChanged(this, DataPropertyName);
            OnPropertyChanged(this, propertyName);
        }

        void OnPropertyChanged(object target, string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Model Properties

        public string AppID
        {
            get
            {
                return Data.Request.AppID;
            }
            set
            {
                Data.Request.AppID = value;
                OnDataChanged("AppID");
            }
        }

        public string LoanType
        {
            get
            {
                return GetEnumName<ELoanType>(Data.Request.LoanType);
            }
            set
            {
                Data.Request.LoanType = ParseEnum<ELoanType>(value);
                OnDataChanged("LoanType");
            }
        }

        public string PropertyState
        {
            get
            {
                return GetEnumName<StateType>(Data.Request.TheApplicant.State);
            }
            set
            {
                Data.Request.TheApplicant.State = ParseEnum<StateType>(value);
                OnDataChanged("PropertyState");
            }
        }

        public string CreditRating
        {
            get
            {
                return GetEnumName<CreditHistoryTypeCreditSelfRating>(Data.Request.TheApplicant.CreditHistory.CreditSelfRating);
            }
            set
            {
                Data.Request.TheApplicant.CreditHistory.CreditSelfRating = ParseEnum<CreditHistoryTypeCreditSelfRating>(value);
                OnDataChanged("CreditRating");
            }
        }

        public string BankruptcyDischarged
        {
            get
            {
                return GetEnumName<CreditHistoryTypeBankruptcyDischarged>(Data.Request.TheApplicant.CreditHistory.BankruptcyDischarged);
            }
            set
            {
                Data.Request.TheApplicant.CreditHistory.BankruptcyDischarged = ParseEnum<CreditHistoryTypeBankruptcyDischarged>(value);
                OnDataChanged("BankruptcyDischarged");
            }
        }

        public string ForeclosureDischarged
        {
            get
            {
                return GetEnumName<CreditHistoryTypeForeclosureDischarged>(Data.Request.TheApplicant.CreditHistory.ForeclosureDischarged);
            }
            set
            {
                Data.Request.TheApplicant.CreditHistory.ForeclosureDischarged = ParseEnum<CreditHistoryTypeForeclosureDischarged>(value);
                OnDataChanged("ForeclosureDischarged");
            }
        }

        public string PropertyType
        {
            get
            {
                return GetEnumName<PropertyType>(GetSubjectPropertyProperty<PropertyType>("PropertyType"));
            }
            set
            {
                SetSubjectPropertyProperty("PropertyType", ParseEnum<PropertyType>(value));
                OnDataChanged("PropertyType");
            }
        }

        public string PropertyUse
        {
            get
            {
                return GetEnumName<PropertyUseType>(GetSubjectPropertyProperty<PropertyUseType>("PropertyUse"));
            }
            set
            {
                SetSubjectPropertyProperty("PropertyUse", ParseEnum<PropertyUseType>(value));
                OnDataChanged("PropertyUse");
            }
        }

        public string PropertyZip
        {
            get
            {
                return GetSubjectPropertyProperty<string>("PropertyZip");
            }
            set
            {
                SetSubjectPropertyProperty("PropertyZip", value);
                OnDataChanged("PropertyZip");

                if (string.IsNullOrEmpty(ApplicantZipCode))
                {
                    ApplicantZipCode = value;
                }
            }
        }

        public decimal PropertyApproximateValue
        {
            get
            {
                AssertRefi();
                return (Data.Request.HomeLoanProduct.Item as RefinanceType).PropertyEstimatedValue;
            }
            set
            {
                AssertRefi();
                (Data.Request.HomeLoanProduct.Item as RefinanceType).PropertyEstimatedValue = value;
                OnDataChanged("PropertyApproximateValue");
            }
        }

        public decimal EstimatedMortgageBalance
        {
            get
            {
                AssertRefi();
                return (Data.Request.HomeLoanProduct.Item as RefinanceType).EstimatedMortgageBalance;
            }
            set
            {
                AssertRefi();
                (Data.Request.HomeLoanProduct.Item as RefinanceType).EstimatedMortgageBalance = value;
                OnDataChanged("EstimatedMortgageBalance");
            }
        }

        public decimal CashOut
        {
            get
            {
                AssertRefi();
                return (Data.Request.HomeLoanProduct.Item as RefinanceType).CashOut;
            }
            set
            {
                AssertRefi();
                (Data.Request.HomeLoanProduct.Item as RefinanceType).CashOut = value;
                OnDataChanged("CashOut");
            }
        }

        public decimal MonthlyPayment
        {
            get
            {
                AssertRefi();
                return (Data.Request.HomeLoanProduct.Item as RefinanceType).MonthlyPayment;
            }
            set
            {
                AssertRefi();
                (Data.Request.HomeLoanProduct.Item as RefinanceType).MonthlyPayment = value;
                OnDataChanged("MonthlyPayment");
            }
        }

        public decimal PurchasePrice
        {
            get
            {
                return GetPurchase().PropertyPurchasePrice;
            }
            set
            {
                GetPurchase().PropertyPurchasePrice = value;
                OnDataChanged("PurchasePrice");
            }
        }

        public decimal DownPayment
        {
            get
            {
                return GetPurchase().DownPayment;
            }
            set
            {

                GetPurchase().DownPayment = value;
                OnDataChanged("DownPayment");
            }
        }

        public string PropertyCity
        {
            get
            {
                return GetPurchase().SubjectProperty.PropertyCity;
            }
            set
            {
                GetPurchase().SubjectProperty.PropertyCity = value;
                OnDataChanged("PropertyCity");
            }
        }

        public string DOB
        {
            get
            {
                return Data.Request.TheApplicant.DateOfBirth;
            }
            set
            {
                Data.Request.TheApplicant.DateOfBirth = value;
                OnDataChanged("DOB");
            }
        }

        public string Email
        {
            get
            {
                return Data.Request.TheApplicant.EmailAddress;
            }
            set
            {
                Data.Request.TheApplicant.EmailAddress = value;
                OnDataChanged("Email");
            }
        }

        public string FirstName
        {
            get
            {
                return Data.Request.TheApplicant.FirstName;
            }
            set
            {
                Data.Request.TheApplicant.FirstName = value;
                OnDataChanged("FirstName");
            }
        }

        public string HomePhone
        {
            get
            {
                return Data.Request.TheApplicant.HomePhone;
            }
            set
            {
                Data.Request.TheApplicant.HomePhone = FixPhoneNum(value);
                OnDataChanged("Email");
            }
        }

        public string LastName
        {
            get
            {
                return Data.Request.TheApplicant.LastName;
            }
            set
            {
                Data.Request.TheApplicant.LastName = value;
                OnDataChanged("LastName");
            }
        }

        public string SSN
        {
            get
            {
                return Data.Request.TheApplicant.SSN;
            }
            set
            {
                Data.Request.TheApplicant.SSN = value;
                OnDataChanged("SSN");
            }
        }

        public string Address
        {
            get
            {
                return Data.Request.TheApplicant.Street;
            }
            set
            {
                Data.Request.TheApplicant.Street = value;
                OnDataChanged("Address");
            }
        }

        public string WorkPhone
        {
            get
            {
                return Data.Request.TheApplicant.WorkPhone;
            }
            set
            {
                Data.Request.TheApplicant.WorkPhone = FixPhoneNum(value);
                OnDataChanged("WorkPhone");
            }
        }

        public string ApplicantZipCode
        {
            get
            {
                return Data.Request.TheApplicant.Zip;
            }
            set
            {
                Data.Request.TheApplicant.Zip = value;
                OnDataChanged("ApplicantZipCode");
            }
        }

        public bool LendingTreeLoansOptIn
        {
            get
            {
                return Data.Request.SourceOfRequest.LTLOptin == YesNoType.Y;
            }
            set
            {
                Data.Request.SourceOfRequest.LTLOptin = value ? YesNoType.Y : YesNoType.N;
                OnDataChanged("IsVetran");
            }
        }

        public bool IsVetran
        {
            get
            {
                return Data.Request.TheApplicant.IsVeteran == YesNoType.Y;
            }
            set
            {
                Data.Request.TheApplicant.IsVeteran = value ? YesNoType.Y : YesNoType.N;
                OnDataChanged("IsVetran");
            }
        }

        #endregion

        #region Private Helpers

        string FixPhoneNum(string value)
        {
            //var a = new[] { "-", " ", "(", ")" };
            //a.Aggregate((i, c) => { return i.Replace(c, string.Empty); });
            return value
                .Replace("-", String.Empty)
                .Replace(" ", String.Empty)
                .Replace("(", String.Empty)
                .Replace(")", String.Empty);
        }

        PurchaseType GetPurchase()
        {
            AssertPurchase();
            return (Data.Request.HomeLoanProduct.Item as PurchaseType);
        }

        void Assert(bool b, string errorMessage)
        {
            if (!b) throw new Exception(errorMessage);
        }

        void AssertRefi()
        {
            Assert(Data.Request.LoanType == ELoanType.REFINANCE, Messages.InvalidLoanType);
        }

        void AssertPurchase()
        {
            Assert(Data.Request.LoanType == ELoanType.PURCHASE, Messages.InvalidLoanType);
        }

        static string GetEnumName<T>(T v)
        {
            return Enum.GetName(typeof(T), v);
        }

        static T ParseEnum<T>(string v) where T : struct
        {
            T o;
            return Enum.TryParse<T>(v, out o) ? (T)o : (T)Enum.Parse(typeof(T), "Item" + v.Replace("-", String.Empty));
        }

        object GetHomeLoanProductItem()
        {
            object result = null;
            switch (Data.Request.LoanType)
            {
                case ELoanType.REFINANCE:
                    result = (RefinanceType)Data.Request.HomeLoanProduct.Item;
                    break;
                case ELoanType.PURCHASE:
                    result = (PurchaseType)Data.Request.HomeLoanProduct.Item;
                    break;
                default:
                    throw new Exception("invalid loan type");
            }
            return result;
        }

        void SetSubjectPropertyProperty(string s, object v)
        {
            object hlpi = GetHomeLoanProductItem();
            object sp = hlpi.GetType().GetProperty("SubjectProperty").GetValue(hlpi, null);
            sp.GetType().GetProperty(s).SetValue(sp, v, null);
        }

        T GetSubjectPropertyProperty<T>(string s)
        {
            object hlpi = GetHomeLoanProductItem();
            object sp = hlpi.GetType().GetProperty("SubjectProperty").GetValue(hlpi, null);
            return (T)sp.GetType().GetProperty(s).GetValue(sp, null);
        }

        #endregion

        LendingTreeAffiliateRequest _responseData;
        public System.Xml.Linq.XElement ResonseXml
        {
            set
            {
                LendingTreeAffiliateRequest.Create(value, out _responseData);
            }
        }

        public bool ResponseValidForPixelFire
        {
            get
            {
                return _data.Request.LoanType == ELoanType.REFINANCE;
            }
        }

        public string VisitorIPAddress 
        { 
            set
            {
                _data.Request.SourceOfRequest.VisitorIPAddress = value;
            }
        }

        public string VisitorURL
        {
            set
            {
                _data.Request.SourceOfRequest.VisitorURL = value;
            }
        }

        public bool RequiresDisclosure
        {
            get
            {
                return !StatesExcludedFromDisclosure.Contains(this.PropertyState);
            }
        }
    }
}