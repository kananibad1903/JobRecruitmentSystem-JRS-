using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using JobRecruitmentSystem.BLL.Services.Interfaces;

namespace JobRecruitmentSystem.BLL.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendConfirmationCodeAsync(string toEmail, string code)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(
                _configuration["Email:SenderName"],
                _configuration["Email:SenderEmail"]));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = "Email Təsdiq Kodu - Job Recruitment System";

            message.Body = new TextPart("html")
            {
                Text = $@"
                    <h2>Xoş gəlmisiniz!</h2>
                    <p>Qeydiyyatınızı tamamlamaq üçün aşağıdakı kodu daxil edin:</p>
                    <h1 style='letter-spacing: 5px;'>{code}</h1>
                    <p>Bu kod 15 dəqiqə etibarlıdır.</p>
                "
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(
                _configuration["Email:SmtpServer"],
                int.Parse(_configuration["Email:SmtpPort"]),
                MailKit.Security.SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(
                _configuration["Email:SenderEmail"],
                _configuration["Email:SenderPassword"]);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}