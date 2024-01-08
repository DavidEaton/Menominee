using Menominee.Api.Data;
using Menominee.Api.Features.Customers;
using Menominee.Domain.Entities.RepairOrders;
using Menominee.Shared.Models.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Features.RepairOrders
{
    public class RepairOrderRepository : IRepairOrderRepository
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<RepairOrderRepository?> _logger;

        public RepairOrderRepository(ApplicationDbContext context, ILogger<RepairOrderRepository?> logger)
        {
            this.context = context ??
                           throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public void Add(RepairOrder repairOrder)
        {
            if (repairOrder is not null)
                context.Attach(repairOrder);
        }

        public void Delete(RepairOrder repairOrder)
        {
            if (repairOrder is not null)
                context.Remove(repairOrder);
        }

        public async Task<RepairOrderToRead?> GetAsync(long id)
        {
            var repairOrderFromContext = await context.RepairOrders
                .Include(repairOrder => repairOrder.Customer)
                .Include(repairOrder => repairOrder.Vehicle)
                .Include(repairOrder => repairOrder.Services)
                    .ThenInclude(service => service.LineItems)
                        .ThenInclude(lineItem => lineItem.Item.Manufacturer)
                .Include(repairOrder => repairOrder.Services)
                    .ThenInclude(service => service.LineItems)
                        .ThenInclude(lineItem => lineItem.Item.ProductCode)
                .Include(repairOrder => repairOrder.Services)
                    .ThenInclude(service => service.LineItems)
                        .ThenInclude(lineItem => lineItem.Item.SaleCode)
                .Include(repairOrder => repairOrder.Services)
                    .ThenInclude(service => service.LineItems)
                        .ThenInclude(repairOrderItem => repairOrderItem.Taxes)
                .Include(repairOrder => repairOrder.Services)
                    .ThenInclude(service => service.LineItems)
                        .ThenInclude(lineItem => lineItem.SerialNumbers)
                .Include(repairOrder => repairOrder.Services)
                    .ThenInclude(lineItem => lineItem.LineItems)
                        .ThenInclude(service => service.Warranties)
                .Include(repairOrder => repairOrder.Services)
                    .ThenInclude(service => service.LineItems)
                        .ThenInclude(lineItem => lineItem.Purchases)
                .Include(repairOrder => repairOrder.Services)
                    .ThenInclude(service => service.Technicians)
                .Include(repairOrder => repairOrder.Services)
                    .ThenInclude(service => service.Taxes)
                .Include(repairOrder => repairOrder.Services)
                    .ThenInclude(service => service.SaleCode)
                .Include(repairOrder => repairOrder.Statuses)
                .Include(repairOrder => repairOrder.Taxes)
                .Include(repairOrder => repairOrder.Payments)
                .AsSplitQuery()
            //.AsNoTracking() // must enable tracking to load related etities
            .FirstOrDefaultAsync(repairOrder => repairOrder.Id == id);

            if (repairOrderFromContext is null)
            {
                return null;
            }

            await Helper.LoadCustomerEntity(repairOrderFromContext.Customer, context);
            await Helper.LoadVehiclesAsync(repairOrderFromContext.Customer, context);
            await Helper.LoadContactDetailsAsync(repairOrderFromContext.Customer, context);

            return RepairOrderHelper.ConvertToReadDto(repairOrderFromContext);
        }


        public async Task<RepairOrder?> GetEntityAsync(long id)
        {
            return await context.RepairOrders
                .Include(repairOrder => repairOrder.Services)
                    .ThenInclude(service => service.LineItems)
                        .ThenInclude(lineItem => lineItem.Item.Manufacturer)
                .Include(repairOrder => repairOrder.Services)
                    .ThenInclude(service => service.LineItems)
                        .ThenInclude(lineItem => lineItem.Item.ProductCode)
                .Include(repairOrder => repairOrder.Services)
                    .ThenInclude(service => service.LineItems)
                        .ThenInclude(lineItem => lineItem.Item.SaleCode)
                .Include(repairOrder => repairOrder.Services)
                    .ThenInclude(service => service.LineItems)
                        .ThenInclude(lineItem => lineItem.Taxes)
                .Include(repairOrder => repairOrder.Services)
                    .ThenInclude(service => service.LineItems)
                        .ThenInclude(lineItem => lineItem.SerialNumbers)
                .Include(repairOrder => repairOrder.Services)
                    .ThenInclude(service => service.LineItems)
                        .ThenInclude(lineItem => lineItem.Warranties)
                .Include(repairOrder => repairOrder.Services)
                    .ThenInclude(service => service.Technicians)
                .Include(repairOrder => repairOrder.Services)
                    .ThenInclude(service => service.Taxes)
                .Include(repairOrder => repairOrder.Taxes)
                .Include(repairOrder => repairOrder.Payments)
                .FirstOrDefaultAsync(repairOrder => repairOrder.Id == id);
        }

        public async Task<IReadOnlyList<RepairOrderToReadInList?>> GetListAsync()
        {
            _logger.LogInformation("GetRepairOrderListAsync");
            var repairOrders = await context.RepairOrders
                .Include(repairOrder => repairOrder.Customer)
                .Include(repairOrder => repairOrder.Vehicle)
                .ToListAsync();

            foreach (var repairOrder in repairOrders)
            {
                await Helper.LoadCustomerEntity(repairOrder.Customer, context);
            }

            return repairOrders
                .Select(repairOrder =>
                       RepairOrderHelper.ConvertToReadInListDto(repairOrder))
                .ToList();
        }

        public async Task<List<long>> GetTodaysRepairOrderNumbersAsync()
        {
            var repairOrders = await context.RepairOrders.ToListAsync();
            var todayDatePart = long.Parse(DateTime.Today.ToString("yyyyMMdd")) * 1000;

            return repairOrders
                .Select(repairOrder => repairOrder.RepairOrderNumber)
                .Where(number => number >= todayDatePart && number < todayDatePart + 1000)
                .ToList();
        }

        public async Task SaveChangesAsync() =>
            await context.SaveChangesAsync();

        public long GetLastInvoiceNumberOrSeed()
        {
            var invoiceNumbers = context.RepairOrders
                .Select(repairOrder => repairOrder.InvoiceNumber)
                .DefaultIfEmpty()
                .Max();

            return invoiceNumbers;
        }

    }
}
