using Restaurant.Application.Admin.Interfaces.Tables.DeactivateTable;
using Restaurant.Application.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Services.Tables.DeactivateTable
{
    public class DeactivateTableService : IDeactivateTableService
    {
        private readonly IDeactivateTableRepository _repository;

        public DeactivateTableService(IDeactivateTableRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<bool>> DeactivateTableAsync(int tenantId, int tableId)
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
                        "Cannot deactivate deleted table",
                        new List<string> { $"Table with ID {tableId} is marked as deleted" });
                }

                if (!table.IsActive)
                {
                    return ApiResponse<bool>.ValidationErrorResponse(
                        "Table already inactive",
                        new List<string> { $"Table with ID {tableId} is already inactive" });
                }

                var result = await _repository.DeactivateTableAsync(table);

                if (!result)
                {
                    return ApiResponse<bool>.ServerErrorResponse("Failed to deactivate table. Please try again later.");
                }

                return ApiResponse<bool>.SuccessResponse(true, "Table deactivated successfully");
            }
            catch
            {
                return ApiResponse<bool>.ServerErrorResponse(
                    "An error occurred while deactivating the table. Please try again later.");
            }
        }
    }
}
