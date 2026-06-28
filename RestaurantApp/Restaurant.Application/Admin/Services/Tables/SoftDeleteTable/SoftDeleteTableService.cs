using Restaurant.Application.Admin.Interfaces.Tables.SoftDeleteTable;
using Restaurant.Application.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Services.Tables.SoftDeleteTable
{
    public class SoftDeleteTableService : ISoftDeleteTableService
    {
        private readonly ISoftDeleteTableRepository _repository;

        public SoftDeleteTableService(ISoftDeleteTableRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<bool>> SoftDeleteTableAsync(int tenantId, int tableId)
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
                        "Table already deleted",
                        new List<string> { $"Table with ID {tableId} is already marked as deleted" });
                }

                var result = await _repository.SoftDeleteTableAsync(table);

                if (!result)
                {
                    return ApiResponse<bool>.ServerErrorResponse("Failed to delete table. Please try again later.");
                }

                return ApiResponse<bool>.SuccessResponse(true, "Table deleted successfully");
            }
            catch
            {
                return ApiResponse<bool>.ServerErrorResponse(
                    "An error occurred while deleting the table. Please try again later.");
            }
        }
    }
}
