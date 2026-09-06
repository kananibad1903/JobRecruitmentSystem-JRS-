using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.BLL.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendConfirmationCodeAsync(string toEmail, string code);
        Task SendPasswordResetCodeAsync(string toEmail, string code);
    }
}
