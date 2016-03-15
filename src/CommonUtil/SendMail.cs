using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Configuration;
using System.IO;

namespace CommonUtil
{
    /// <summary>
    /// 发送邮件
    /// </summary>
    public class SendMail
    {
        /// <summary>
        /// 发送普通邮件
        /// </summary>
        /// <param name="sendto">收件人</param>
        /// <param name="stitle">标题</param>
        /// <param name="scontent">内容</param>
        /// <returns></returns>
        public static bool SendEmail(string sendto, string title, string content)
        {
            MailMessage msg = new MailMessage();
            msg.To.Add(sendto);
            string email = ConfigurationManager.AppSettings["EmailUser"];
            string displayname = ConfigurationManager.AppSettings["EmailDisplay"];
            msg.From = new MailAddress(email, displayname, System.Text.Encoding.UTF8);

            msg.Subject = title;//邮件标题 
            msg.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码 
            msg.Body = content;//邮件内容 
            msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码 
            msg.IsBodyHtml = false;//是否是HTML邮件 
            msg.Priority = MailPriority.High;//邮件优先级

            SmtpClient client = new SmtpClient();
            string pass = ConfigurationManager.AppSettings["EmailPass"];
            string host = ConfigurationManager.AppSettings["SmtpHost"];
            string port = ConfigurationManager.AppSettings["SmtpPort"];
            client.Credentials = new System.Net.NetworkCredential(email, pass);//发件的邮箱和密码
            client.Host = host;
            client.Port = Convert.ToInt32(port);
            object userState = msg;
            try
            {
                client.SendAsync(msg, userState);
                //client.Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteException("邮件发送失败", ex);
                return false;
            }
        }

        /// <summary>
        /// 发送带附件的邮件
        /// </summary>
        /// <param name="sendto">收件人</param>
        /// <param name="stitle">标题</param>
        /// <param name="scontent">内容</param>
        /// <param name="fileUrls">附件路径</param>
        /// <returns></returns>
        public static bool SendEmail(string sendto, string title, string content,params string[] fileUrls)
        {
            MailMessage msg = new MailMessage();
            msg.To.Add(sendto);
            string email = ConfigurationManager.AppSettings["EmailUser"];
            string displayname = ConfigurationManager.AppSettings["EmailDisplay"];
            msg.From = new MailAddress(email, displayname, System.Text.Encoding.UTF8);

            //附件
            if (fileUrls != null)
            {
                foreach (string file in fileUrls)
                {
                    if (File.Exists(file))
                    {
                        msg.Attachments.Add(new Attachment(file));
                    }
                }
            }

            msg.Subject = title;//邮件标题 
            msg.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码 
            msg.Body = content;//邮件内容 
            msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码 
            msg.IsBodyHtml = false;//是否是HTML邮件 
            msg.Priority = MailPriority.High;//邮件优先级

            SmtpClient client = new SmtpClient();
            string pass = ConfigurationManager.AppSettings["EmailPass"];
            client.Credentials = new System.Net.NetworkCredential(email, pass);
            string host = ConfigurationManager.AppSettings["SmtpHost"];
            string port = ConfigurationManager.AppSettings["SmtpPort"];
            client.Host = host;
            client.Port = Convert.ToInt32(port);

            object userState = msg;
            try
            {
                client.SendAsync(msg, userState);
                //client.Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteException("邮件发送失败", ex);
                return false;
            }
        }

        /// <summary>
        /// 发送正文为html格式的邮件
        /// </summary>
        /// <param name="sendto">收件人</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="fileUrls">附件</param>
        /// <returns></returns>
        public static bool SendHtmlEmail(string sendto, string title, string content, params string[] fileUrls)
        {
            MailMessage msg = new MailMessage();
            msg.To.Add(sendto);
            string email = ConfigurationManager.AppSettings["EmailUser"];
            string displayname = ConfigurationManager.AppSettings["EmailDisplay"];
            msg.From = new MailAddress(email, displayname, System.Text.Encoding.UTF8);

            //附件
            if (fileUrls != null)
            {
                foreach (string file in fileUrls)
                {
                    if (File.Exists(file))
                    {
                        msg.Attachments.Add(new Attachment(file));
                    }
                }
            }

            msg.Subject = title;//邮件标题 
            msg.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码 
            msg.Body = content;//邮件内容 
            msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码 
            msg.IsBodyHtml = true;//是否是HTML邮件 
            msg.Priority = MailPriority.High;//邮件优先级

            SmtpClient client = new SmtpClient();
            string pass = ConfigurationManager.AppSettings["EmailPass"];
            client.Credentials = new System.Net.NetworkCredential(email, pass);
            string host = ConfigurationManager.AppSettings["SmtpHost"];
            string port = ConfigurationManager.AppSettings["SmtpPort"];
            client.Host = host;
            client.Port = Convert.ToInt32(port);

            object userState = msg;
            try
            {
                client.SendAsync(msg, userState);
                //client.Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteException("邮件发送失败", ex);
                return false;
            }
        }
    }
}
