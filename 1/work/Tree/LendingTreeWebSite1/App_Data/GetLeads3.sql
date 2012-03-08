-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE GetLeads3
	@FromTimestamp datetime,
	@ToTimestamp datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @to datetime = DATEADD(dd, 1, @ToTimestamp)
SELECT
	 [Timestamp]
	,lds.AppID
	,[Type]
	,CDNumber
	,IP	
	,ESourceID
	,Email
	,Credit
	,[State]
	,DateOfBirth
	,IsVeteran
	,ers.[Error]
FROM 
	-- 
	-- Leads (lds)
	--
	(
		SELECT
			Data.value('(/LendingTreeAffiliateRequest/Request/@AppID)[1]', 'varchar(50)') AS AppID,
			Data.value('(/LendingTreeAffiliateRequest/Request/@type)[1]', 'varchar(50)') AS [Type], 
			[Timestamp], 
			Data.value('(/LendingTreeAffiliateRequest/@affid)[1]', 'varchar(50)') AS CDNumber, 
			Data.value('(/LendingTreeAffiliateRequest/Request/SourceOfRequest/VisitorIPAddress)[1]', 'varchar(50)') AS IP, 
			Data.value('(/LendingTreeAffiliateRequest/Request/SourceOfRequest/LendingTreeAffiliateEsourceID)[1]', 'varchar(50)') AS ESourceID,
	        Data.value('(/LendingTreeAffiliateRequest/Request/Applicant/State)[1]', 'char(2)') AS [State],
			Data.value('(/LendingTreeAffiliateRequest/Request/Applicant/DateOfBirth)[1]', 'varchar(20)') AS DateOfBirth,
			Data.value('(/LendingTreeAffiliateRequest/Request/Applicant/EmailAddress)[1]', 'varchar(255)') AS Email,
			Data.value('(/LendingTreeAffiliateRequest/Request/Applicant/IsVeteran)[1]', 'char(1)') AS IsVeteran,
			Data.value('(/LendingTreeAffiliateRequest/Request/Applicant/CreditHistory/CreditSelfRating)[1]', 'varchar(50)') AS Credit
		FROM 
			WebLogEntry wle
			INNER JOIN EventType et on wle.EventTypeId=et.Id
			INNER JOIN [EventData] ed on wle.EventDataId=ed.Id
		WHERE
			(et.Id=2)
				AND ([Timestamp] BETWEEN @FromTimestamp	AND @to)
	) lds 
	LEFT OUTER JOIN 
		-- 
		-- Errors (ers)
		--
		(
			SELECT 
				 Data.value('(Fragment/@AppID)[1]','char(36)') [AppID]
				,Data.value('(/Fragment/LendingTreeAffiliateRequest/Response/Errors/Error)[1]', 'varchar(255)') [Error]
			FROM 
				WebLogEntry wle
				INNER JOIN EventType et on wle.EventTypeId=et.Id
				INNER JOIN [EventData] ed on wle.EventDataId=ed.Id
			WHERE
				(et.Id=1)
					AND (Data.exist('/Fragment/LendingTreeAffiliateRequest/Response/Errors/Error')=1)
					AND ([Timestamp] BETWEEN @FromTimestamp AND @to)
		) 
		ers ON lds.AppID=ers.AppID
END
GO
