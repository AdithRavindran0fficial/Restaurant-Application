using Restaurant.Application.Common;
using Restaurant.Application.Common.Interface;
using Restaurant.Application.Common.Options;
using Restaurant.Application.User.DTOs.OrderDTOs;
using Restaurant.Application.User.Interfaces.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.User.Services.OrderService
{
    public class ActiveOrdersService : IActiveOrdersService
    {
        private readonly IActiveOrdersRepo activeOrdersRepo;
        private readonly ITwillioOtpService twillioOtpService;
        public ActiveOrdersService(IActiveOrdersRepo activeOrdersRepo,ITwillioOtpService twillioOtp)
        {
            this.activeOrdersRepo = activeOrdersRepo;
            twillioOtpService = twillioOtp;
        }
        public async Task<ApiResponse<ActiveOrdersResponseDTO>> GetActiveOrders(ActiveOrderRequestDTO activeOrderRequestDTO)
        {
            var Validations = new List<string>();
            if (string.IsNullOrEmpty(activeOrderRequestDTO.Otp))
            {
                Validations.Add("OTP is empty");
            }
            if (string.IsNullOrEmpty(activeOrderRequestDTO.PhoneNumber))
            {
                Validations.Add("Phone Number is empty");
            }
            if (string.IsNullOrEmpty(activeOrderRequestDTO.SessionToken))
            {
                Validations.Add("Session Token is empty");
            }
            if (Validations.Any())
            {
                return ApiResponse<ActiveOrdersResponseDTO>.ValidationErrorResponse("Validation Failed",Validations);
            }

            var otpResponse = await twillioOtpService.VerifyOtpAsync(activeOrderRequestDTO.PhoneNumber,activeOrderRequestDTO.Otp,CancellationToken.None);
            if (!otpResponse)
            {
                return ApiResponse<ActiveOrdersResponseDTO>.FailureResponse("Wrong Otp");
            }

            // GET SESSION
            var session = await activeOrdersRepo.GetTableSession(activeOrderRequestDTO.SessionToken);

            if (session == null || session.CreatedAt.AddHours(4)<DateTime.UtcNow)
            {
                return ApiResponse<ActiveOrdersResponseDTO>.NotFoundResponse("Session not found please Rescan the QR");
            }

            //Check User Exist 
            var user = await activeOrdersRepo.GetCustomer(activeOrderRequestDTO.PhoneNumber, session.TenantId);
            if (user == null)
            {
                return ApiResponse<ActiveOrdersResponseDTO>.NotFoundResponse("No Order found for this user");
            }

            //Fetch Orders
            var orders = await activeOrdersRepo.GetActiveOrder(user.Id, session.TenantId);
            if (orders == null || orders.Count==0)
            {
                return ApiResponse<ActiveOrdersResponseDTO>.NotFoundResponse("No Active Orders found for this user");

            }
            var orderDTOResponse = new ActiveOrdersResponseDTO
            {
                Orders = orders.Select(o => new OrderResponseDTO
                {
                    CreatedAt = o.CreatedAt,
                    OrderNumber = o.OrderNumber,
                    OrderId = o.Id,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount,
                    Items = o.OrderItems.Select(ot => new OrderItemResponseDTO
                    {
                        MenuItemId = ot.MenuItemId,
                        Name = ot.MenuItem.Name,
                        Notes = ot.Notes,
                        OrderId = ot.OrderId.ToString(),
                        OrderNumber = o.OrderNumber,
                        Price = ot.Price,
                        Quantity = ot.Quantity,
                        TotalPrice = ot.TotalPrice
                    }).ToList()
                }).ToList()
            };

            return ApiResponse<ActiveOrdersResponseDTO>.SuccessResponse(orderDTOResponse);

        }
    }
}
