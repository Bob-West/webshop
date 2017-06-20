using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Runtime.Remoting.Messaging;
using System.Net.Mime;
using System.IO;
using Webshop.Models;
using System.Web.Mvc;
using System.Web.Routing;

namespace Webshop.Models
{
    public class clsEMail
    {

        //class for e-mail composition & sending

        public enum RecipientType { To, CC, BCC }

        private delegate void delegateSendEmailAsync(string vTemplateName, Dictionary<RecipientType, string> vAddresses, string vSubject, Dictionary<String, Object> vValues); //this is a generic pointer construction, it can point to any function using the given signature. it can execute asynchronously, that's why I use it.

        public void SendEmailAsync(string vTemplateName, Dictionary<RecipientType, string> vAddresses, string vSubject, Dictionary<String, Object> vValues)
        {
            delegateSendEmailAsync vCallSendEmail = new delegateSendEmailAsync(SendEmail); //this uses the generic pointer to point to the function SendEmail
            vCallSendEmail.BeginInvoke(vTemplateName, vAddresses, vSubject, vValues, new AsyncCallback(SendEmailAsyncFinish), null); //the execution of function SendEmail is started asynchronously, when it finishes it calls SendEmailAsyncFinish
        }

        private void SendEmailAsyncFinish(IAsyncResult vResult) //this function is called upon finish of the async operation. its only purpose is to close the generic pointer construction which is still using memory.
        {
            AsyncResult vResultTemp = vResult as AsyncResult; //cast vResult to AsyncResult
            delegateSendEmailAsync vDelegate = vResultTemp.AsyncDelegate as delegateSendEmailAsync; //receive the generic pointer construction
            try
            {
                vDelegate.EndInvoke(vResult); //wait for the async operation to finish and close it
            }
            catch (Exception vError)
            {
                //log error
            }
        }

        public void SendEmail(string vTemplateName, Dictionary<RecipientType, string> vAddresses, string vSubject, Dictionary<String, Object> vValues)
        {

            //composes an email and returns the HTML code
            //vTemplateName = file name of the template, without ending >> File test.html would have vTemplateName = test
            //vValues contains the variables to fill into the eMail template, as dictionary, e.g.: <"surname", "Myers"> >> the system adds {{ and }} >> searches for {{surname}} in the template

            String vEmailContent;
            MailMessage vMessage = new MailMessage();
            SmtpClient vClient;
            ContentDisposition vContentDispo;

            //check for mandatory values
            if (vAddresses.ContainsKey(RecipientType.To) == false || vAddresses[RecipientType.To].Length == 0)
            {
                //log error
            }

            //load email template
            if (File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("/App_Data/" + vTemplateName + ".cshtml")))
            {
                //vEmailContent = RenderRazorViewToString((Controller)ControllerBuilder.Current.GetControllerFactory().CreateController(HttpContext.Current.Request.RequestContext, "Home"), System.Web.Hosting.HostingEnvironment.MapPath("/App_Data/" + vTemplateName + ".cshtml"), null);
                vEmailContent = this.RenderRazorViewToString(System.Web.Hosting.HostingEnvironment.MapPath("/App_Data/" + vTemplateName + ".cshtml"), null);
            }
            else
            {
                //log error
                return;
            }

            //fill in values
            if (vValues != null && vValues.Count > 0)
            {
                foreach (KeyValuePair<String, Object> vValue in vValues)
                {
                    vEmailContent = vEmailContent.Replace("{{" + vValue.Key + "}}", vValue.Value.ToString());
                }
            }

            //send email
            vMessage.From = new MailAddress("office@yourdomain.com");
            vMessage.To.Add(vAddresses[RecipientType.To]);
            if (vAddresses.ContainsKey(RecipientType.CC) == true && vAddresses[RecipientType.CC].Length > 0) { vMessage.CC.Add(vAddresses[RecipientType.CC]); }
            if (vAddresses.ContainsKey(RecipientType.BCC) == true && vAddresses[RecipientType.BCC].Length > 0) { vMessage.Bcc.Add(vAddresses[RecipientType.BCC]); }

            vMessage.Subject = vSubject;
            vMessage.IsBodyHtml = true;
            vMessage.Body = vEmailContent;

            vClient = new SmtpClient();
            vClient.Host = "smtp.gmail.com";
            vClient.Credentials = new System.Net.NetworkCredential("smtpdummycweb", "ilovecweb");
            vClient.EnableSsl = true;
            vClient.Send(vMessage);
        }

        public static string RenderRazorViewToString(this Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}