using Restaurant.Application.Common;
using Restaurant.Application.User.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.User.Interfaces.Session.CreateSession
{
    public interface ICreateSessionService
    {
        Task<ApiResponse<SessionDTO>> CreateSessionAsync(string  qrToken);    
    }
}
