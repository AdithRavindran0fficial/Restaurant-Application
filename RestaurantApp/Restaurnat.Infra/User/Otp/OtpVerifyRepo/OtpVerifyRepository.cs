using Microsoft.EntityFrameworkCore;
using Restaurant.Application.User.Interfaces.Otp.OtpVerify;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurnat.Infra.User.Otp.OtpVerifyRepo
{
    public class OtpVerifyRepository : IOtpverifyRepository
    {
        private readonly MasterDbContext masterDbContext;
        public OtpVerifyRepository(MasterDbContext masterDbContext)
        {
            this.masterDbContext = masterDbContext;
        }
        public async Task<TableSession> GetTableSession(string Session)
        {
            var session = await masterDbContext.TableSessions.FirstOrDefaultAsync(x => x.SessionToken == Session && x.IsActive);
            return session; 
        }
    }
}
