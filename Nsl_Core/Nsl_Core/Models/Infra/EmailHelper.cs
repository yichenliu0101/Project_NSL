using Nsl_Core.Models.Dtos.Teacher.TeacherApply;
using Nsl_Core.Models.EFModels;
using Nsl_Core.Models.ViewModels;
using System.Diagnostics.Metrics;
using System.Net.Mail;

namespace Nsl_Core.Models.Infra
{
    public class EmailHelper
	{
		public static void VerifyTeacher(TeacherVerifyDto dto)
		{
			MailMessage msg = new MailMessage();
			msg.To.Add(dto.Email);
			msg.From = new MailAddress("NeverStopLearning@gmail.com", "NSL系統管理員", System.Text.Encoding.UTF8);
			msg.Subject = "【NSL老師申請結果】";
			msg.SubjectEncoding = System.Text.Encoding.UTF8;

            //這個方法在驗證已經做過性別判斷
            msg.Body = $"{dto.Name} {dto.Gender} 您好，恭喜您成為NSL的教師！\r\n現在起可以使用老師專區的功能，麻煩您再填寫相關的基本資料，謝謝。";
			msg.BodyEncoding = System.Text.Encoding.UTF8;
			msg.IsBodyHtml = true;

			SmtpClient client = new SmtpClient();
			client.Credentials = new System.Net.NetworkCredential("08p268955@gmail.com", "jtnhllczbslabezl");
			client.Host = "smtp.gmail.com";
			client.Port = 25;
			client.EnableSsl = true;
			client.Send(msg);
			client.Dispose();
			msg.Dispose();
		}

        public static void VerifyMember(Members member)
        {
            MailMessage msg = new MailMessage();
            msg.To.Add(member.Email);
            msg.From = new MailAddress("NeverStopLearning@gmail.com", "NSL系統管理員", System.Text.Encoding.UTF8);
            msg.Subject = "【NSL會員註冊驗證信】";
            msg.SubjectEncoding = System.Text.Encoding.UTF8;

            string genderText = member.Gender.HasValue ? (member.Gender.Value ? "先生" : "女士") : "會員";
            msg.Body = $"{member.Name} {genderText} 您好，感謝您註冊NSL會員！\r\n請點擊以下連結，並完成Eamil驗證，感謝您的配合。\r\nhttps://localhost:7217/NSL/Verify?token={member.EmailToken}";
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("08p268955@gmail.com", "jtnhllczbslabezl");
            client.Host = "smtp.gmail.com";
            client.Port = 25;
            client.EnableSsl = true;
            client.Send(msg);
            client.Dispose();
            msg.Dispose();
        }

        public static void ForgetPassword(Members member)
        {
            MailMessage msg = new MailMessage();
            msg.To.Add(member.Email);
            msg.From = new MailAddress("NeverStopLearning@gmail.com", "NSL系統管理員", System.Text.Encoding.UTF8);
            msg.Subject = "【NSL忘記密碼驗證信】";
            msg.SubjectEncoding = System.Text.Encoding.UTF8;

            string genderText = member.Gender.HasValue ? (member.Gender.Value ? "先生" : "女士") : "會員";
            msg.Body = $"{member.Name} {genderText} 您好，感謝您使用NSL平台！\r\n如果不是您申請忘記密碼，請忽略此信。\r\n若您忘記密碼，請點選以下連結並完成新密碼的設置，感謝您的配合。\n\thttps://localhost:7217/NSL/VerifyPassword?token={member.EmailToken}";
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("08p268955@gmail.com", "jtnhllczbslabezl");
            client.Host = "smtp.gmail.com";
            client.Port = 25;
            client.EnableSsl = true;
            client.Send(msg);
            client.Dispose();
            msg.Dispose();
        }
    }
}
