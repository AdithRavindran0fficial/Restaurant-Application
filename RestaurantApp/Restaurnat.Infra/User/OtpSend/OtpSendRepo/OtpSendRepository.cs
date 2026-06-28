using Microsoft.EntityFrameworkCore;
using Restaurant.Application.User.Interfaces.Otp.OtpSend;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurnat.Infra.User.OtpSend.OtpSendRepo
{
    public class OtpSendRepository : IOtpSendRepository
    {
        private readonly MasterDbContext context;
        public OtpSendRepository(MasterDbContext masterDbContext)
        {
            context = masterDbContext;
        }
        public async Task AddOtpVerification(OtpVerification otpVerification)
        {
            await context.OtpVerifications.AddAsync(otpVerification);
            await context.SaveChangesAsync();
            
        }

        public async Task<TableSession?> GetTableSession(string sessionToken)
        {
            var session = await context.TableSessions.FirstOrDefaultAsync(s => s.SessionToken == sessionToken);
            return session;
        }

        public async Task CloseTableSession(TableSession session)
        {
            context.TableSessions.Update(session);
            await context.SaveChangesAsync();
        }
    }
}
