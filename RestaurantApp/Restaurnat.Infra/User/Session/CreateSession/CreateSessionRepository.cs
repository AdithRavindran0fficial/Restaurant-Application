using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Restaurant.Application.User.Interfaces.Session.CreateSession;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurnat.Infra.User.Session.CreateSession
{
    public  class CreateSessionRepository : ICreateSessionRepository
    {
        private readonly MasterDbContext _context;
        public CreateSessionRepository(MasterDbContext masterDbContext)
        {
            _context = masterDbContext;
        }

        public async Task<DiningTable> GetTable(string qrToken)
        {
            var table = await _context.Tables.FirstOrDefaultAsync(t => t.QrToken == qrToken && t.IsActive && !t.IsDeleted);
            return table; 
        }
        public async Task CreateSession(TableSession session)
        {

            await _context.TableSessions.AddAsync(session);
            await _context.SaveChangesAsync();
        }
    }
}
