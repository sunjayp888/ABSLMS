USE [ABS_LMS_Testing]
Go
Alter table [dbo].[Employee] Add Address1 varchar(2000)
Alter table [dbo].[Employee] Add Address2 varchar(2000)
Alter table [dbo].[Employee] Add Address3 varchar(2000)
Alter table [dbo].[Employee] Add Address4 varchar(2000)
Alter table [dbo].[Employee] Add PostCode varchar(100)
Alter table [dbo].[Employee] Add City varchar(500)
Alter table [dbo].[Employee] Add [State] varchar(500)
Alter table [dbo].[Employee] Add Country varchar(200)
Alter table [dbo].[Employee] Add EmployeeImage binary(500)
ALTER TABLE [dbo].[Employee] ADD HalfDayDateUTC Datetime