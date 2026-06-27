using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.User.Interfaces.Session.CreateSession
{
    public  interface ICreateSessionRepository
    {
         Task<DiningTable> GetTable(string qrToken);
        Task CreateSession(TableSession session);


    }
}
