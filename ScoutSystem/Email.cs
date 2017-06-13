using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ScoutSystem
{
    static public class Email
    {
        /// <summary>
        /// Uses "From" as Application Email.
        /// </summary>
        /// <param name="To"></param>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>

        public static void Send(string To, string From, string Subject, string Body)
        {
            Send(To, From, Subject, Body, new Attachment[] { });
        }
        public static void Send(string To, string From, string Subject, string Body, Attachment[] Attachments)
        {
#if (!DEBUG)
            string FROM = From;
            string HOST = Properties.Settings.Default.SMTP_Host;
            int PORT = Properties.Settings.Default.SMTP_Port;
            string SMTP_USERNAME = Properties.Settings.Default.SMTP_Username;
            string SMTP_PASSWORD = Properties.Settings.Default.SMTP_Password;

            MailMessage message = new MailMessage(FROM, To, Subject, Body);
            message.IsBodyHtml = true;

            foreach(var file in Attachments)
                message.Attachments.Add(file);
            
            using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(HOST, PORT))
            {
                client.Credentials = new System.Net.NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);
                client.EnableSsl = true;
                client.Send(message);
            }
#endif
        }

        /// <summary>
        /// Use this function to render an CSHTML with an MVC Controller.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string RenderMvcHTML(this Controller controller, string viewName, object model)
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

        /// <summary>
        /// Use this function to render an CSHTML with an API Controller.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string RenderApiHTML(string ThisControllerName, string viewName, object viewData)
        {
            using (var writer = new StringWriter())
            {
                var routeData = new RouteData();
                routeData.Values.Add("controller", ThisControllerName);
                var fakeControllerContext = new ControllerContext(new HttpContextWrapper(new HttpContext(new HttpRequest(null, "http://google.com", null), new HttpResponse(null))), routeData, new FakeController());
                var razorViewEngine = new RazorViewEngine();
                var razorViewResult = razorViewEngine.FindView(fakeControllerContext, viewName, "", false);

                var viewContext = new ViewContext(fakeControllerContext, razorViewResult.View, new ViewDataDictionary(viewData), new TempDataDictionary(), writer);
                razorViewResult.View.Render(viewContext, writer);
                return writer.ToString();
            }
        }
        public class FakeController : ControllerBase { protected override void ExecuteCore() { } }
    }
}