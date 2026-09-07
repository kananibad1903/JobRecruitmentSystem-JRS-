using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.BLL.Services.Interfaces
{
    public interface IAiChatService
    {
        Task<string> SendMessageAsync(string message);
    }
}
