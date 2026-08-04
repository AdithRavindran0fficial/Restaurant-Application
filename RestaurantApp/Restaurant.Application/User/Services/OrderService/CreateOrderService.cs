using Restaurant.Application.Common;
using Restaurant.Application.Common.Interface;
using Restaurant.Application.User.DTOs.OrderDTOs;
using Restaurant.Application.User.Interfaces.Order;
using Restaurant.Application.User.Interfaces.Orders;
using Restaurant.Application.User.Interfaces.Otp.OtpVerify;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.User.Services.OrderService
{
    public class CreateOrderService : ICreateOrderService
    {
        private readonly ICreateOrderRepo createOrderRepo;
        private readonly ITwillioOtpService _service;

        public CreateOrderService(ICreateOrderRepo createOrder,ITwillioOtpService _service)
        {
            createOrderRepo = createOrder;
            this._service = _service;
        }

        public async Task<ApiResponse<OrderItemResponseDTO>> CreateOrderAsync(CreateOrderDTO createOrderDTO)
        {
            var validationErrors = new List<string>();

            if (createOrderDTO == null)
                return ApiResponse<OrderItemResponseDTO>.ValidationErrorResponse("Request cannot be null.");

            if (string.IsNullOrWhiteSpace(createOrderDTO.Phone))
                validationErrors.Add("Phone number is required.");

            if (string.IsNullOrWhiteSpace(createOrderDTO.Otp))
                validationErrors.Add("OTP is required.");

            if (string.IsNullOrWhiteSpace(createOrderDTO.SessionToken))
                validationErrors.Add("Session token is required.");

            if (createOrderDTO.Items == null || !createOrderDTO.Items.Any())
                validationErrors.Add("Please select at least one item.");

            if (validationErrors.Any())
            {
                return ApiResponse<OrderItemResponseDTO>.ValidationErrorResponse(
                    "Validation failed",
                    validationErrors);
            }

            // Validate session
            var session = await createOrderRepo.GetTableSession(createOrderDTO.SessionToken);

            if (session == null)
            {
                return ApiResponse<OrderItemResponseDTO>.NotFoundResponse(
                    "Session not found. Please rescan the QR.");
            }

            if (session.CreatedAt.AddHours(4) < DateTime.UtcNow)
            {
                return ApiResponse<OrderItemResponseDTO>.FailureResponse(
                    "Session has expired. Please rescan the QR.");
            }

            // Verify OTP
            var otpVerified = await _service.VerifyOtpAsync(
                createOrderDTO.Phone,
                createOrderDTO.Otp,
                CancellationToken.None);

            if (!otpVerified)
            {
                return ApiResponse<OrderItemResponseDTO>.FailureResponse(
                    "OTP verification failed.");
            }

            // Find or create customer
            Customer customer;

            var existingCustomer = await createOrderRepo.CheckCustomer(
                createOrderDTO.Phone,
                session.TenantId);

            if (existingCustomer != null)
            {
                customer = existingCustomer;
            }
            else
            {
                customer = new Customer
                {
                    Name = createOrderDTO.Name,
                    PhoneNumber = createOrderDTO.Phone,
                    TenantId = session.TenantId
                };

                await createOrderRepo.CreateCustomer(customer);
            }

            // Get menu items
            var itemIds = createOrderDTO.Items
                .Select(x => x.ProductID)
                .Distinct()
                .ToList();

            var menuItems = await createOrderRepo.GetItems(
                itemIds,
                session.TenantId);

            if (menuItems.Count != itemIds.Count)
            {
                return ApiResponse<OrderItemResponseDTO>.ValidationErrorResponse(
                    "One or more products are invalid.");
            }

            decimal totalAmount = 0;

            var orderItems = new List<OrderItem>();

            foreach (var menuItem in menuItems)
            {
                var requestItem = createOrderDTO.Items
                    .First(x => x.ProductID == menuItem.Id);

                var quantity = requestItem.Quantity;

                totalAmount += menuItem.Price * quantity;

                orderItems.Add(new OrderItem
                {
                    MenuItemId = menuItem.Id,
                    TenantId = menuItem.TenantId,
                    Quantity = quantity,
                    Price = menuItem.Price,
                    TotalPrice = menuItem.Price * quantity,
                    Notes = requestItem.Note
                });
            }

            var order = new Order
            {
                TenantId = session.TenantId,
                TableId = session.TableId,
                TableSessionId = session.Id,
                CustomerId = customer.Id,

                OrderNumber = $"ORD-{Guid.NewGuid():N}"[..12],

                Notes = createOrderDTO.Notes,
                TotalAmount = totalAmount,

                OrderItems = orderItems
            };

            await createOrderRepo.CreateOrderAsync(order);

            return ApiResponse<OrderItemResponseDTO>.SuccessResponse(
                new OrderItemResponseDTO
                {
                    OrderId = order.Id.ToString(),
                    OrderNumber = order.OrderNumber,
                    TotalPrice = order.TotalAmount
                },
                "Order placed successfully.");
        }
    }
}
