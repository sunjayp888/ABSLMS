Use [ABS_LMS]

Go

update 
 e
set 
e.Address1 = ea.Address1,
e.Address2= ea.address2,
e.Address3=ea.address3,
e.Address4= ea.address4,
e.City=ea.city,
e.Postcode= ea.postcode,
e.state = 'Maharashtra',
e.Country='India'
from
employeeaddress ea inner join employee e 
on ea.employeecode = e.employeecode

--------------Birthdate
Go

update 
 e
set 
e.DOB = b.Dob
from
[dbo].[Birthdate] b inner join employee e 
on b.EmployeeCode = e.EmployeeCode

