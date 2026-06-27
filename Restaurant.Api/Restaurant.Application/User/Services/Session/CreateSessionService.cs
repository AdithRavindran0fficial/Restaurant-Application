using Restaurant.Application.Common;
using Restaurant.Application.User.DTOs;
using Restaurant.Application.User.Interfaces.Session.CreateSession;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.User.Services.Session
{
    public class CreateSessionService : ICreateSessionService
    {
        private readonly ICreateSessionRepository _repository;
        public CreateSessionService(ICreateSessionRepository repository )
        {
            _repository = repository;
        }
        public async Task<ApiResponse<SessionDTO>> CreateSessionAsync(string qrToken)
        {
            if (string.IsNullOrEmpty(qrToken))
            {

                return ApiResponse<SessionDTO>.ValidationErrorResponse("QR token is required");
            }

            var table = await _repository.GetTable(qrToken);
            if (table == null)
            {
                return ApiResponse<SessionDTO>.NotFoundResponse("Table not found");
            }

            var tableSession = new TableSession
            {
                TenantId = table.TenantId,
                TableId = table.Id,
                IsActive = true,
                SessionToken = Guid.NewGuid().ToString("N"),
                CreatedAt = DateTime.UtcNow,
                ClosedAt = null,

            };
            await _repository.CreateSession(tableSession);

            var tableSessioDto = new SessionDTO
            {
                SessionToken = tableSession.SessionToken,
                CreatedAt = tableSession.CreatedAt,
                ClosedAt = tableSession.ClosedAt,
                TableNumber = table.TableNumber,
                TenantId = table.TenantId,
                
            };

            return ApiResponse<SessionDTO>.SuccessResponse(tableSessioDto, "Success", 200);

        }
    }
}
