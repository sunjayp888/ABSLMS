using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using ABS_LMS.Service;
using ABS_LMS.Service.Model;

namespace ABS_LMS.Helper
{
    public static class SmtpHelper
    {
        public static void Send(string to,string subject,string body)
        {
            try
            {
                var mail = new MailMessage();
                var smtpServer = new SmtpClient(ConfigHelper.SmtpServer);
                mail.From = new MailAddress(ConfigHelper.FromAddress);
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                smtpServer.Port = Convert.ToInt32(ConfigHelper.SmtpPort);
                smtpServer.Credentials = new System.Net.NetworkCredential(ConfigHelper.SmtpUsername, ConfigHelper.SmtpPassword);
                smtpServer.EnableSsl = true;
                smtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

        public static void SendMailOnCreate(string employeeName, string leavetype, string to, EmployeeLeave employee)
        {
            var leaveTemplate = Template.CreateLeaveTemplate(employeeName,
                                   employee.Employee.FirstName + " " + employee.Employee.LastName, leavetype,
                                   employee.NoOfDays.ToString(),
                                   employee.LeaveStartDate.ToString(),
                                   employee.LeaveEndDate.ToString(),
                                   employee.Reason, LeaveStatus.Submit.ToString());

            Send(to, "Leave Application", leaveTemplate);
        }
    }
}