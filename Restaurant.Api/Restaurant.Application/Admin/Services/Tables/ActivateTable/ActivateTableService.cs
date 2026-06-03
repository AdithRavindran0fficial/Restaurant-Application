using Restaurant.Application.Admin.Interfaces.Tables.ActivateTable;
using Restaurant.Application.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Services.Tables.ActivateTable
{
    public class ActivateTableService : IActivateTableService
    {
        private readonly IActivateTableRepository _repository;

        public ActivateTableService(IActivateTableRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<bool>> ActivateTableAsync(int tenantId, int tableId)
        {
            try
            {
                if (tenantId <= 0)
                {
                    return ApiResponse<bool>.ValidationErrorResponse(
                        "Invalid tenant ID",
                        new List<string> { "Tenant ID must be greater than 0" });
                }

                if (tableId <= 0)
                {
                    return ApiResponse<bool>.ValidationErrorResponse(
                        "Invalid table ID",
                        new List<string> { "Table ID must be greater than 0" });
                }

                var table = await _repository.GetTableByIdAsync(tenantId, tableId);

                if (table == null)
                {
                    return ApiResponse<bool>.NotFoundResponse($"Table with ID {tableId} not found");
                }

                if (table.IsDeleted)
                {
                    return ApiResponse<bool>.ValidationErrorResponse(
                        "Cannot activate deleted table",
                        new List<string> { $"Table with ID {tableId} is marked as deleted" });
                }

                if (table.IsActive)
                {
                    return ApiResponse<bool>.ValidationErrorResponse(
                        "Table already active",
                        new List<string> { $"Table with ID {tableId} is already active" });
                }

                var result = await _repository.ActivateTableAsync(table);

                if (!result)
                {
                    return ApiResponse<bool>.ServerErrorResponse("Failed to activate table. Please try again later.");
                }

                return ApiResponse<bool>.SuccessResponse(true, "Table activated successfully");
            }
            catch
            {
                return ApiResponse<bool>.ServerErrorResponse(
                    "An error occurred while activating the table. Please try again later.");
            }
        }
    }
}
