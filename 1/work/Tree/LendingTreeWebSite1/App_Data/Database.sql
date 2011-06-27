--
-- TABLE : zips
--
CREATE TABLE [dbo].[zips](
	[ZIP Code] [varchar](5) NULL,
	[City] [varchar](100) NULL,
	[State Abbreviation] [varchar](2) NULL
) ON [PRIMARY]
GO
--
-- TABLE : EventType
--
CREATE TABLE [dbo].[EventType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_EventType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
--
-- TABLE : EventData
--
CREATE TABLE [dbo].[EventData](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Data] [xml] NOT NULL,
 CONSTRAINT [PK_EventData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
--
-- SCHEMA COLLECTION : LendingTreeAffiliateSchemaCollection
--
CREATE XML SCHEMA COLLECTION [dbo].[LendingTreeAffiliateSchemaCollection] AS N'<xsd:schema xmlns:xsd="http://www.w3.org/2001/XMLSchema" attributeFormDefault="qualified" elementFormDefault="qualified"><xsd:element name="LendingTreeAffiliateRequest"><xsd:complexType><xsd:complexContent><xsd:restriction base="xsd:anyType"><xsd:sequence><xsd:element name="Request" type="RequestType" minOccurs="0" /><xsd:element name="Response" type="ResponseType" minOccurs="0" /></xsd:sequence></xsd:restriction></xsd:complexContent></xsd:complexType></xsd:element><xsd:complexType name="Address"><xsd:complexContent><xsd:restriction base="xsd:anyType"><xsd:sequence><xsd:element name="Address1" type="xsd:string" /><xsd:element name="Address2" type="xsd:string" minOccurs="0" /><xsd:element name="City" type="xsd:string" /><xsd:element name="County" type="xsd:string" /><xsd:element name="State" type="xsd:string" /><xsd:element name="Zip" type="ZipCodeType" /></xsd:sequence></xsd:restriction></xsd:complexContent></xsd:complexType><xsd:complexType name="ApplicantType"><xsd:complexContent><xsd:restriction base="xsd:anyType"><xsd:sequence><xsd:element name="FirstName" type="NameType" /><xsd:element name="MiddleName" type="NameType" minOccurs="0" /><xsd:element name="LastName" type="NameType" /><xsd:element name="NameSuffix" type="NameSuffixType" minOccurs="0" /><xsd:element name="Street" type="NameType" /><xsd:element name="City" type="NameType" minOccurs="0" /><xsd:element name="County" type="NameType" minOccurs="0" /><xsd:element name="State" type="StateType" minOccurs="0" /><xsd:element name="Zip"><xsd:simpleType><xsd:restriction base="ZipCodeType" /></xsd:simpleType></xsd:element><xsd:element name="DateOfBirth" type="xsd:string" /><xsd:element name="HomePhone" type="PhoneType" /><xsd:element name="MobilePhone" type="PhoneType" minOccurs="0" /><xsd:element name="WorkPhone" type="PhoneType" minOccurs="0" /><xsd:element name="WorkPhoneExt" type="WorkPhoneExtensionType" minOccurs="0" /><xsd:element name="EmailAddress" type="EmailType" /><xsd:element name="Password" type="xsd:string" minOccurs="0" /><xsd:element name="SSN" type="SSNType" minOccurs="0" /><xsd:element name="IsVeteran" type="YesNoType" default="N" /><xsd:element name="MaritalStatus" type="MaritalStatusType" minOccurs="0" /><xsd:element name="IsUSCitizen" type="CitizenshipStatusType" minOccurs="0" /><xsd:element name="RelationshipToApplicant" type="RelationshipToApplicantType" minOccurs="0" /><xsd:element name="ContactPreference" type="ContactPreferenceType" minOccurs="0" /><xsd:element name="Employment" type="EmploymentType" minOccurs="0" /><xsd:element name="CreditHistory" type="CreditHistoryType" /></xsd:sequence><xsd:attribute name="Primary" use="required"><xsd:simpleType><xsd:restriction base="xsd:string"><xsd:enumeration value="Y" /><xsd:enumeration value="N" /></xsd:restriction></xsd:simpleType></xsd:attribute><xsd:attribute name="PrimaryContact" use="required"><xsd:simpleType><xsd:restriction base="xsd:string"><xsd:enumeration value="Y" /><xsd:enumeration value="N" /></xsd:restriction></xsd:simpleType></xsd:attribute></xsd:restriction></xsd:complexContent></xsd:complexType><xsd:complexType name="ContactPreferenceType"><xsd:complexContent><xsd:restriction base="xsd:anyType"><xsd:sequence /><xsd:attribute name="PreferredContactPlace"><xsd:simpleType><xsd:restriction base="xsd:string"><xsd:enumeration value="Home" /><xsd:enumeration value="Work" /></xsd:restriction></xsd:simpleType></xsd:attribute><xsd:attribute name="PreferredContactMethod"><xsd:simpleType><xsd:restriction base="xsd:string"><xsd:enumeration value="Any" /><xsd:enumeration value="Email" /><xsd:enumeration value="Fax" /><xsd:enumeration value="Phone" /></xsd:restriction></xsd:simpleType></xsd:attribute><xsd:attribute name="PreferredContactTime"><xsd:simpleType><xsd:restriction base="xsd:string"><xsd:enumeration value="Anytime" /><xsd:enumeration value="Morning" /><xsd:enumeration value="Afternoon" /><xsd:enumeration value="Evening" /></xsd:restriction></xsd:simpleType></xsd:attribute></xsd:restriction></xsd:complexContent></xsd:complexType><xsd:complexType name="CreditHistoryType"><xsd:complexContent><xsd:restriction base="xsd:anyType"><xsd:sequence><xsd:element name="CreditSelfRating"><xsd:simpleType><xsd:restriction base="xsd:string"><xsd:enumeration value="EXCELLENT" /><xsd:enumeration value="LITTLEORNOCREDITHISTORY" /><xsd:enumeration value="MAJORCREDITPROBLEMS" /><xsd:enumeration value="SOMECREDITPROBLEMS" /></xsd:restriction></xsd:simpleType></xsd:element><xsd:element name="DeclaredBankruptcy" type="YesNoType" minOccurs="0" /><xsd:element name="DeclaredForeclosure" type="YesNoType" minOccurs="0" /><xsd:element name="BankruptcyDischarged"><xsd:simpleType><xsd:restriction base="xsd:string"><xsd:enumeration value="NOT_YET_DISCHARGED" /><xsd:enumeration value="01-12_MONTHS" /><xsd:enumeration value="25-36_MONTHS" /><xsd:enumeration value="37-48_MONTHS" /><xsd:enumeration value="49-60_MONTHS" /><xsd:enumeration value="61-72_MONTHS" /><xsd:enumeration value="OVER_72_MONTHS" /><xsd:enumeration value="OVER_84_MONTHS" /><xsd:enumeration value="NEVER" /></xsd:restriction></xsd:simpleType></xsd:element><xsd:element name="ForeclosureDischarged"><xsd:simpleType><xsd:restriction base="xsd:string"><xsd:enumeration value="CURRENTLY_IN_FORECLOSURE" /><xsd:enumeration value="01-12_MONTHS" /><xsd:enumeration value="25-36_MONTHS" /><xsd:enumeration value="37-48_MONTHS" /><xsd:enumeration value="49-60_MONTHS" /><xsd:enumeration value="61-72_MONTHS" /><xsd:enumeration value="OVER_72_MONTHS" /><xsd:enumeration value="OVER_84_MONTHS" /><xsd:enumeration value="NEVER" /></xsd:restriction></xsd:simpleType></xsd:element></xsd:sequence></xsd:restriction></xsd:complexContent></xsd:complexType><xsd:complexType name="EmploymentType"><xsd:complexContent><xsd:restriction base="xsd:anyType"><xsd:sequence><xsd:element name="EmployerName" type="xsd:string" minOccurs="0" /><xsd:element name="EmployeeTitle" type="xsd:string" minOccurs="0" /><xsd:element name="EmploymentYears" type="xsd:string" minOccurs="0" /><xsd:element name="EmploymentStatus" type="EmploymentStausType" minOccurs="0" /><xsd:element name="EmploymentIncome" type="AmountType" minOccurs="0" /></xsd:sequence><xsd:attribute name="EmploymentIndicator" type="xsd:string" /></xsd:restriction></xsd:complexContent></xsd:complexType><xsd:complexType name="ErrorsType"><xsd:complexContent><xsd:restriction base="xsd:anyType"><xsd:sequence><xsd:element name="Error" type="xsd:string" /></xsd:sequence></xsd:restriction></xsd:complexContent></xsd:complexType><xsd:complexType name="HomeEquityPurposeType"><xsd:complexContent><xsd:restriction base="xsd:anyType"><xsd:sequence><xsd:element name="Purpose" type="HomeEquityPurposesType" maxOccurs="unbounded" default="DEBTCONSOLIDATION" /></xsd:sequence></xsd:restriction></xsd:complexContent></xsd:complexType><xsd:complexType name="HomeEquitySubjectPropertyType"><xsd:complexContent><xsd:restriction base="xsd:anyType"><xsd:sequence><xsd:element name="PropertyType" type="PropertyType" default="SINGLEFAMDET" /><xsd:element name="PropertyUse" type="PropertyUseType" default="OWNEROCCUPIED" /><xsd:element name="PropertyAddress" type="xsd:string" minOccurs="0" /><xsd:element name="PropertyState" type="StateType" minOccurs="0" /><xsd:element name="PropertyCounty" type="xsd:string" minOccurs="0" /><xsd:element name="PropertyCity" type="xsd:string" minOccurs="0" /><xsd:element name="PropertyZip" type="ZipCodeType" /><xsd:element name="Units" type="UnitsType" minOccurs="0" /></xsd:sequence></xsd:restriction></xsd:complexContent></xsd:complexType><xsd:complexType name="HomeEquityType"><xsd:complexContent><xsd:restriction base="xsd:anyType"><xsd:sequence><xsd:element name="FirstHome" type="YesNoType" minOccurs="0" /><xsd:element name="PropertyEstimatedValue" type="AmountType" /><xsd:element name="MonthlyPayment" type="AmountType" minOccurs="0" /><xsd:element name="CashOut" type="AmountType" /><xsd:element name="RequestedProducts" type="RequestedProductsType" minOccurs="0" /><xsd:element name="HomeEquityPurpose" type="HomeEquityPurposeType" minOccurs="0" /><xsd:element name="SubjectProperty" type="HomeEquitySubjectPropertyType" /></xsd:sequence></xsd:restriction></xsd:complexContent></xsd:complexType><xsd:complexType name="HomeLoanProductType"><xsd:complexContent><xsd:restriction base="xsd:anyType"><xsd:choice><xsd:element name="Purchase" type="PurchaseType" /><xsd:element name="Refinance" type="RefinanceType" /><xsd:element name="HomeEquity" type="HomeEquityType" /></xsd:choice></xsd:restriction></xsd:complexContent></xsd:complexType><xsd:complexType name="PurchaseSubjectPropertyType"><xsd:complexContent><xsd:restriction base="xsd:anyType"><xsd:sequence><xsd:element name="PropertyType" type="PropertyType" default="SINGLEFAMDET" /><xsd:element name="PropertyUse" type="PropertyUseType" default="OWNEROCCUPIED" /><xsd:element name="PropertyAddress" type="xsd:string" minOccurs="0" /><xsd:element name="PropertyState" type="StateType" /><xsd:element name="PropertyCounty" type="xsd:string" minOccurs="0" /><xsd:element name="PropertyCity" type="xsd:string" /><xsd:element name="PropertyZip" type="ZipCodeType" minOccurs="0" /><xsd:element name="Units" type="UnitsType" minOccurs="0" /></xsd:sequence></xsd:restriction></xsd:complexContent></xsd:complexType><xsd:complexType name="PurchaseType"><xsd:complexContent><xsd:restriction base="xsd:anyType"><xsd:sequence><xsd:element name="RequestedLoanAmount" type="AmountType" minOccurs="0" /><xsd:element name="PropertyPurchasePrice" type="AmountType" /><xsd:element name="FirstTimeHomeBuyer" type="YesNoType" minOccurs="0" /><xsd:element name="FoundAHome" type="YesNoType" /><xsd:element name="SignedSalesContract" type="YesNoType" minOccurs="0" /><xsd:element name="NeedRealtor" type="YesNoType" default="Y" /><xsd:element name="DownPayment" type="AmountType" /><xsd:element name="RequestedProducts" type="RequestedProductsType" minOccurs="0" /><xsd:element name="SubjectProperty" type="PurchaseSubjectPropertyType" /></xsd:sequence></xsd:restriction></xsd:complexContent></xsd:complexType><xsd:complexType name="RefinancePurposeType"><xsd:complexContent><xsd:restriction base="xsd:anyType"><xsd:sequence><xsd:element name="Purpose" type="RefinancePurposesType" maxOccurs="unbounded" default="REFIPRIMARY" /></xsd:sequence></xsd:restriction></xsd:complexContent></xsd:complexType><xsd:complexType name="RefinanceSubjectPropertyType"><xsd:complexContent><xsd:restriction base="xsd:anyType"><xsd:sequence><xsd:element name="PropertyType" type="PropertyType" default="SINGLEFAMDET" /><xsd:element name="PropertyUse" type="PropertyUseType" default="OWNEROCCUPIED" /><xsd:element name="PropertyAddress" type="xsd:string" minOccurs="0" /><xsd:element name="PropertyState" type="StateType" minOccurs="0" /><xsd:element name="PropertyCounty" type="xsd:string" minOccurs="0" /><xsd:element name="PropertyCity" type="xsd:string" minOccurs="0" /><xsd:element name="PropertyZip" type="ZipCodeType" /><xsd:element name="Units" type="UnitsType" minOccurs="0" /></xsd:sequence></xsd:restriction></xsd:complexContent></xsd:complexType><xsd:complexType name="RefinanceType"><xsd:complexContent><xsd:restriction base="xsd:anyType"><xsd:sequence><xsd:element name="RequestedLoanAmount" type="AmountType" minOccurs="0" /><xsd:element name="PropertyEstimatedValue" type="AmountType" /><xsd:element name="MonthlyPayment" type="AmountType" /><xsd:element name="EstimatedMortgageBalance" type="AmountType" /><xsd:element name="FirstMortgageInterestRate" type="AmountType" minOccurs="0" /><xsd:element name="HaveMultipleMortages" type="YesNoType" default="N" /><xsd:element name="SecondMortgageBalance" type="AmountType" minOccurs="0" /><xsd:element name="SecondMortgageMonthlyPayment" type="AmountType" minOccurs="0" /><xsd:element name="SecondMortgageInterestRate" type="AmountType" minOccurs="0" /><xsd:element name="RequestedProducts" type="RequestedProductsType" minOccurs="0" /><xsd:element name="RefinancePurpose" type="RefinancePurposeType" minOccurs="0" /><xsd:element name="CashOut" type="AmountType" default="0" /><xsd:element name="SubjectProperty" type="RefinanceSubjectPropertyType" /></xsd:sequence></xsd:restriction></xsd:complexContent></xsd:complexType><xsd:complexType name="RequestType"><xsd:complexContent><xsd:restriction base="xsd:anyType"><xsd:sequence><xsd:element name="SourceOfRequest" type="SourceOfRequestType" /><xsd:element name="HomeLoanProduct" type="HomeLoanProductType" /><xsd:element name="Applicant" type="ApplicantType" maxOccurs="unbounded" /></xsd:sequence><xsd:attribute name="type" type="xsd:string" /><xsd:attribute name="created" type="xsd:string" use="required" /><xsd:attribute name="updated" type="xsd:string" use="required" /><xsd:attribute name="VisitorSessionID" type="xsd:string" /><xsd:attribute name="AppID" type="xsd:string" use="required" /><xsd:attribute name="ElectronicDisclosureConsent" type="xsd:string" /></xsd:restriction></xsd:complexContent></xsd:complexType><xsd:complexType name="RequestedProductsType"><xsd:complexContent><xsd:restriction base="xsd:anyType"><xsd:sequence><xsd:element name="Product" type="ProductType" maxOccurs="unbounded" default="30YRF" /></xsd:sequence></xsd:restriction></xsd:complexContent></xsd:complexType><xsd:complexType name="ResponseType"><xsd:complexContent><xsd:restriction base="xsd:anyType"><xsd:sequence><xsd:element name="ReturnURL" type="xsd:string" minOccurs="0" /><xsd:element name="Errors" type="ErrorsType" minOccurs="0" /></xsd:sequence></xsd:restriction></xsd:complexContent></xsd:complexType><xsd:complexType name="SourceOfRequestType"><xsd:complexContent><xsd:restriction base="xsd:anyType"><xsd:sequence><xsd:element name="LendingTreeAffiliatePartnerCode" type="xsd:string" /><xsd:element name="LendingTreeAffiliateUserName" type="xsd:string" /><xsd:element name="LendingTreeAffiliatePassword" type="xsd:string" /><xsd:element name="LendingTreeAffiliateEsourceID" type="xsd:string" /><xsd:element name="LendingTreeAffiliateBrand" type="xsd:string" minOccurs="0" /><xsd:element name="LendingTreeAffiliateFormVersion" type="xsd:string" minOccurs="0" /><xsd:element name="VisitorIPAddress" type="xsd:string" /><xsd:element name="VisitorURL" type="xsd:string" /><xsd:element name="TreeSessionID" type="xsd:string" minOccurs="0" /><xsd:element name="TreeComputerID" type="xsd:string" minOccurs="0" /><xsd:element name="V1stCookie" type="xsd:string" minOccurs="0" /><xsd:element name="LTLOptin" type="YesNoType" /></xsd:sequence></xsd:restriction></xsd:complexContent></xsd:complexType><xsd:simpleType name="AmountType"><xsd:restriction base="xsd:decimal"><xsd:fractionDigits value="2" /><xsd:totalDigits value="8" /></xsd:restriction></xsd:simpleType><xsd:simpleType name="CitizenshipStatusType"><xsd:restriction base="xsd:string"><xsd:enumeration value="NONPERMANENTRESIDENTALIEN" /><xsd:enumeration value="NONRESIDENTALIEN" /><xsd:enumeration value="PERMANENTRESIDENTALIEN" /><xsd:enumeration value="USCITIZEN" /></xsd:restriction></xsd:simpleType><xsd:simpleType name="EmailType"><xsd:restriction base="xsd:string"><xsd:maxLength value="64" /><xsd:pattern value="[a-zA-Z0-9_\.\-]+@[a-zA-Z0-9\-\.]+\.[a-zA-Z0-9]{2,4}" /></xsd:restriction></xsd:simpleType><xsd:simpleType name="EmploymentStausType"><xsd:restriction base="xsd:string"><xsd:enumeration value="FULLTIME" /><xsd:enumeration value="HOMEMAKER" /><xsd:enumeration value="PARTTIME" /><xsd:enumeration value="RETIRED" /><xsd:enumeration value="SELFEMPLOYED" /><xsd:enumeration value="STUDENT" /></xsd:restriction></xsd:simpleType><xsd:simpleType name="HomeEquityPurposesType"><xsd:restriction base="xsd:string"><xsd:enumeration value="PAYOFFHELOAN" /><xsd:enumeration value="PAYOFFHELOC" /><xsd:enumeration value="HOMEIMP" /><xsd:enumeration value="DEBTCONSOLIDATION" /><xsd:enumeration value="AUTOPURCHASE" /><xsd:enumeration value="BOATPURCHASE" /><xsd:enumeration value="RVPURCHASE" /><xsd:enumeration value="MOTORCYCLEPURCHASE" /><xsd:enumeration value="OTHER" /></xsd:restriction></xsd:simpleType><xsd:simpleType name="MaritalStatusType"><xsd:restriction base="xsd:string"><xsd:enumeration value="MARRIED" /><xsd:enumeration value="UNMARRIED" /><xsd:enumeration value="SEPARATED" /></xsd:restriction></xsd:simpleType><xsd:simpleType name="NameSuffixType"><xsd:restriction base="xsd:string"><xsd:enumeration value="I" /><xsd:enumeration value="II" /><xsd:enumeration value="III" /><xsd:enumeration value="IV" /><xsd:enumeration value="JR" /><xsd:enumeration value="SR" /><xsd:enumeration value="V" /></xsd:restriction></xsd:simpleType><xsd:simpleType name="NameType"><xsd:restriction base="xsd:normalizedString"><xsd:maxLength value="128" /><xsd:minLength value="1" /></xsd:restriction></xsd:simpleType><xsd:simpleType name="PhoneType"><xsd:restriction base="xsd:string"><xsd:maxLength value="10" /><xsd:pattern value="[0-9]{10}" /></xsd:restriction></xsd:simpleType><xsd:simpleType name="ProductType"><xsd:restriction base="xsd:string"><xsd:enumeration value="5YRFHELOAN" /><xsd:enumeration value="5YRVHELOAN" /><xsd:enumeration value="5YRFHELOC" /><xsd:enumeration value="5YRVHELOC" /><xsd:enumeration value="7YRFHELOAN" /><xsd:enumeration value="7YRVHELOAN" /><xsd:enumeration value="7YRFHELOC" /><xsd:enumeration value="7YRVHELOC" /><xsd:enumeration value="10YRFHELOAN" /><xsd:enumeration value="10YRVHELOAN" /><xsd:enumeration value="10YRFHELOC" /><xsd:enumeration value="10YRVHELOC" /><xsd:enumeration value="15YRFHELOAN" /><xsd:enumeration value="15YRVHELOAN" /><xsd:enumeration value="15YRFHELOC" /><xsd:enumeration value="15YRVHELOC" /><xsd:enumeration value="20YRFHELOAN" /><xsd:enumeration value="20YRVHELOAN" /><xsd:enumeration value="20YRFHELOC" /><xsd:enumeration value="20YRVHELOC" /><xsd:enumeration value="25YRFHELOAN" /><xsd:enumeration value="25YRVHELOAN" /><xsd:enumeration value="25YRFHELOC" /><xsd:enumeration value="25YRVHELOC" /><xsd:enumeration value="30YRFHELOAN" /><xsd:enumeration value="30YRVHELOAN" /><xsd:enumeration value="30YRFHELOC" /><xsd:enumeration value="30YRVHELOC" /><xsd:enumeration value="OTHERINNOVATIVEHEPRODUCTS" /><xsd:enumeration value="MORTGAGEOTHER" /><xsd:enumeration value="10YRF" /><xsd:enumeration value="15YRF" /><xsd:enumeration value="20YRF" /><xsd:enumeration value="25YRF" /><xsd:enumeration value="30YRF" /><xsd:enumeration value="40YRF" /><xsd:enumeration value="5YRB" /><xsd:enumeration value="7YRB" /><xsd:enumeration value="10YRB" /><xsd:enumeration value="6MONTHARM" /><xsd:enumeration value="1YRARM" /><xsd:enumeration value="2YRARM" /><xsd:enumeration value="3YRARM" /><xsd:enumeration value="5YRARM" /><xsd:enumeration value="7YRARM" /><xsd:enumeration value="10YRARM" /><xsd:enumeration value="5YRARM-INTONLY" /><xsd:enumeration value="10YRARM-INTONLY" /><xsd:enumeration value="15YRARM-INTONLY" /><xsd:enumeration value="20YRARM-INTONLY" /><xsd:enumeration value="25YRARM-INTONLY" /><xsd:enumeration value="30YRARM-INTONLY" /></xsd:restriction></xsd:simpleType><xsd:simpleType name="PropertyType"><xsd:restriction base="xsd:string"><xsd:enumeration value="SINGLEFAMDET" /><xsd:enumeration value="SINGLEFAMATT" /><xsd:enumeration value="LOWRISECONDO" /><xsd:enumeration value="HIGHRISECONDO" /><xsd:enumeration value="2TO4UNITFAM" /><xsd:enumeration value="COOP" /><xsd:enumeration value="MODULAR" /><xsd:enumeration value="MOBILEPERMANENT" /><xsd:enumeration value="MOBILEMOVEABLE" /><xsd:enumeration value="MANUFACTURED" /></xsd:restriction></xsd:simpleType><xsd:simpleType name="PropertyUseType"><xsd:restriction base="xsd:string"><xsd:enumeration value="OWNEROCCUPIED" /><xsd:enumeration value="SECONDHOME" /><xsd:enumeration value="INVESTMENTPROPERTY" /></xsd:restriction></xsd:simpleType><xsd:simpleType name="RefinancePurposesType"><xsd:restriction base="xsd:string"><xsd:enumeration value="REFIPRIMARY" /><xsd:enumeration value="PAYOFFSECOND" /><xsd:enumeration value="PAYOFFHELOC" /><xsd:enumeration value="CASHOUT" /><xsd:enumeration value="PAYOFFSPOUSE" /><xsd:enumeration value="HOMEIMP" /></xsd:restriction></xsd:simpleType><xsd:simpleType name="RelationshipToApplicantType"><xsd:restriction base="xsd:string"><xsd:enumeration value="SPOUSE" /><xsd:enumeration value="PARENT" /><xsd:enumeration value="SELF" /><xsd:enumeration value="OTHER" /></xsd:restriction></xsd:simpleType><xsd:simpleType name="SSNType"><xsd:restriction base="xsd:string"><xsd:pattern value="[0-9]{3}-[0-9]{2}-[0-9]{4}" /></xsd:restriction></xsd:simpleType><xsd:simpleType name="StateType"><xsd:restriction base="xsd:string"><xsd:enumeration value="AK" /><xsd:enumeration value="AL" /><xsd:enumeration value="AR" /><xsd:enumeration value="AZ" /><xsd:enumeration value="CA" /><xsd:enumeration value="CO" /><xsd:enumeration value="CT" /><xsd:enumeration value="DE" /><xsd:enumeration value="DC" /><xsd:enumeration value="FL" /><xsd:enumeration value="GA" /><xsd:enumeration value="HI" /><xsd:enumeration value="ID" /><xsd:enumeration value="IL" /><xsd:enumeration value="IN" /><xsd:enumeration value="IA" /><xsd:enumeration value="KS" /><xsd:enumeration value="KY" /><xsd:enumeration value="LA" /><xsd:enumeration value="ME" /><xsd:enumeration value="MD" /><xsd:enumeration value="MA" /><xsd:enumeration value="MI" /><xsd:enumeration value="MN" /><xsd:enumeration value="MS" /><xsd:enumeration value="MO" /><xsd:enumeration value="MT" /><xsd:enumeration value="NE" /><xsd:enumeration value="NV" /><xsd:enumeration value="NH" /><xsd:enumeration value="NJ" /><xsd:enumeration value="NM" /><xsd:enumeration value="NY" /><xsd:enumeration value="NC" /><xsd:enumeration value="ND" /><xsd:enumeration value="OH" /><xsd:enumeration value="OK" /><xsd:enumeration value="OR" /><xsd:enumeration value="PA" /><xsd:enumeration value="RI" /><xsd:enumeration value="SC" /><xsd:enumeration value="SD" /><xsd:enumeration value="TN" /><xsd:enumeration value="TX" /><xsd:enumeration value="UT" /><xsd:enumeration value="VT" /><xsd:enumeration value="VA" /><xsd:enumeration value="WA" /><xsd:enumeration value="WV" /><xsd:enumeration value="WI" /><xsd:enumeration value="WY" /></xsd:restriction></xsd:simpleType><xsd:simpleType name="UnitsType"><xsd:restriction base="xsd:string"><xsd:enumeration value="1UNIT" /><xsd:enumeration value="2UNITS" /><xsd:enumeration value="3UNITS" /><xsd:enumeration value="4UNITS" /><xsd:enumeration value="5ORMOREUNITS" /></xsd:restriction></xsd:simpleType><xsd:simpleType name="WorkPhoneExtensionType"><xsd:restriction base="xsd:int"><xsd:totalDigits value="4" /></xsd:restriction></xsd:simpleType><xsd:simpleType name="YesNoType"><xsd:restriction base="xsd:string"><xsd:enumeration value="Y" /><xsd:enumeration value="N" /></xsd:restriction></xsd:simpleType><xsd:simpleType name="ZipCodeType"><xsd:restriction base="xsd:string"><xsd:length value="5" /><xsd:pattern value="\d{5}" /></xsd:restriction></xsd:simpleType></xsd:schema>'
GO
--
-- VIEW : Posted
--
create view [dbo].[Posted] as
select 
	  Data.value('(Fragment/@AppID)[1]','char(36)') [AppID]
	, Data.value('(/Fragment/LendingTreeAffiliateRequest/Response/ReturnURL)[1]', 'varchar(max)') [ReturnURL]
	, Data [Data]
from EventData
where
	Data.exist('/Fragment/LendingTreeAffiliateRequest/Response/ReturnURL')=1
GO
--
-- VIEW : Errors
--
create view [dbo].[Errors] as
select 
	  Data.value('(Fragment/@AppID)[1]','char(36)') [AppID]
	, Data.value('(/Fragment/LendingTreeAffiliateRequest/Response/Errors/Error)[1]', 'varchar(max)') [Error]
	, Data [Data]
from EventData
where
	Data.exist('/Fragment/LendingTreeAffiliateRequest/Response/Errors/Error')=1
GO
--
-- TABLE : WebLogEntry
--
CREATE TABLE [dbo].[WebLogEntry](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[EventTypeId] [int] NOT NULL,
	[EventDataId] [int] NOT NULL,
 CONSTRAINT [PK_WebLogEntry] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
--
-- VIEW : WebLogEntryItem
--
CREATE view [dbo].[WebLogEntryItem] as
select 
	AppId =
		case Name
			when 'ApplicationSubmitting' then Data.value('(/LendingTreeAffiliateRequest/Request/@AppID)[1]', 'char(36)')
			else Data.value('(/Fragment/@AppID)[1]', 'char(36)')
		end,
	*
from (
	select 
		WE.[Timestamp], ET.Name, ED.Data
	from 
		WebLogEntry WE
		inner join EventType ET on WE.EventTypeId=ET.Id
		inner join [EventData] ED on WE.EventDataId=ED.Id
) a
GO
--
-- VIEW : ApplicationLog
--
CREATE view [dbo].[ApplicationLog] as
select
	A.AppId, 
	A.[Timestamp] SubmitTimestamp, 
	A.Data SubmitData, 
	B.[Timestamp] ResponseTimestamp, 
	B.Data ResponseData,
	Latency =
		case when B.[Timestamp] is null then null
		else CAST(DATEDIFF(ms, A.[Timestamp], B.[Timestamp])/1000 AS dec(12,2))
		end
from 
	(select * from WebLogEntryItem where Name='ApplicationSubmitting') A left outer join 
	(select * from WebLogEntryItem where Name='ApplicationCompleted') B on A.AppId=B.AppId
GO
--
-- VIEW : Leads
--
CREATE view [dbo].[Leads] as
with T as (
select 
	[Type] =
		case Name
			when 'ApplicationSubmitting' then Data.value('(/LendingTreeAffiliateRequest/Request/@type)[1]', 'char(50)')
			else null
		end
	,Data.value('(/LendingTreeAffiliateRequest/@affid)[1]', 'char(50)') [CDNumber]
	,Data.value('(/LendingTreeAffiliateRequest/Request/SourceOfRequest/VisitorIPAddress)[1]', 'char(50)') [IP]
	,Data.value('(/LendingTreeAffiliateRequest/Request/Applicant/EmailAddress)[1]', 'char(255)') [Email]
	,[Timestamp]
	,Data.value('(/LendingTreeAffiliateRequest/Request/@AppID)[1]', 'char(50)') [AppID]
	,[Data]
from 
	WebLogEntryItem
) 
select * 
from T where not T.[Type] is null
GO
--
-- FUNCTION : GetLeads
--
CREATE function [dbo].[GetLeads]()
returns @res table
(
	  AppID char(36)
	, [Type] char(50)
	, CDNumber char(50)
	, Email char(255)
	, IP char(50)
	, [Timestamp] datetime
	, [Error] varchar(max)
)
as
begin
	declare @T1 table (
		AppID char(36), 
		[Err] varchar(max)
	);
	insert into @T1 (AppID, Err)
		select AppID, [Error] [Err] 
		from Errors;
	insert @res
		select 
			  Leads.AppID
			, [Type]
			, CDNumber
			, Email
			, IP
			, [Timestamp]
			, Err
		from Leads left outer join @T1 A on Leads.AppID=A.AppID
	return;
end
GO
--
-- FOREIGN KEY CONSTRAINT: FK_WebLogEntry_EventData
--
ALTER TABLE [dbo].[WebLogEntry]  WITH CHECK ADD  CONSTRAINT [FK_WebLogEntry_EventData] FOREIGN KEY([EventDataId])
REFERENCES [dbo].[EventData] ([Id])
GO
--
-- CHECK CONSTRAINT : FK_WebLogEntry_EventData
--
ALTER TABLE [dbo].[WebLogEntry] CHECK CONSTRAINT [FK_WebLogEntry_EventData]
GO
--
-- FOREIGN KEY CONSTRAINT: FK_WebLogEntry_EventType
--
ALTER TABLE [dbo].[WebLogEntry]  WITH CHECK ADD  CONSTRAINT [FK_WebLogEntry_EventType] FOREIGN KEY([EventTypeId])
REFERENCES [dbo].[EventType] ([Id])
GO
--
-- CHECK CONSTRAINT : FK_WebLogEntry_EventType
--
ALTER TABLE [dbo].[WebLogEntry] CHECK CONSTRAINT [FK_WebLogEntry_EventType]
GO
