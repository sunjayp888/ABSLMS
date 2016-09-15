using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using ABS_LMS.Service;

namespace ABS_LMS.Helper
{
    public static class Template
    {
        public static string CreateLeaveTemplate(string to, string regards, string leaveType, string days, string fromDate, string toDate, string reason, string status)
        {
            var reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Template/CreateLeave.html"));
            var readFile = reader.ReadToEnd();
            var strContent = "";
            strContent = readFile;
            strContent = strContent.Replace("[To]", to);
            strContent = strContent.Replace("[HolidayType]", leaveType);
            strContent = strContent.Replace("[FromDate]", fromDate);
            strContent = strContent.Replace("[Todate]", toDate);
            strContent = strContent.Replace("[Days]", days);
            strContent = strContent.Replace("[Reason]", reason);
            strContent = strContent.Replace("[Regards]", regards);
            strContent = strContent.Replace("[Status]", status);
            return strContent;
        }

        public static string PortalAccount(string employeeName, string username, string password)
        {
            var reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Template/PortalAccount.html"));
            var readFile = reader.ReadToEnd();
            var strContent = "";
            strContent = readFile;
            strContent = strContent.Replace("[URL]", ConfigHelper.PortalUrl);
            strContent = strContent.Replace("[EmployeeName]", employeeName);
            strContent = strContent.Replace("[Username]", username);
            strContent = strContent.Replace("[Password]", password);
            return strContent;
        }

        public static string ForgotPassword(string username, string forgetPasswordLink)
        {
            var reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Template/ForgotPassword.html"));
            var readFile = reader.ReadToEnd();
            var strContent = "";
            strContent = readFile;
            strContent = strContent.Replace("[Username]", username);
            strContent = strContent.Replace("[ForgetPasswordLink]", forgetPasswordLink);
            strContent = strContent.Replace("[CompanyName]", CompanyContainer.CompanyName);
            return strContent;
        }

    }
}