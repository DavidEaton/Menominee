using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using CustomerVehicleManagement.Shared.Models.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.RepairOrders
{
    public class RepairOrderRepository : IRepairOrderRepository
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<RepairOrderRepository> _logger;

        public RepairOrderRepository(ApplicationDbContext context, ILogger<RepairOrderRepository> logger)
        {
            this.context = context ??
                           throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task Add(RepairOrder repairOrder)
        {
            if (repairOrder != null)
                await context.AddAsync(repairOrder);
        }

        public async Task Delete(long id)
        {
            var repairOrderFromContext = await context.RepairOrders.FindAsync(id);
            if (repairOrderFromContext != null)
                context.Remove(repairOrderFromContext);
        }

        public async Task<RepairOrderToRead> Get(long id)
        {
            var repairOrderFromContext = await context.RepairOrders
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
                                             .Include(repairOrder => repairOrder.Taxes)
                                             .Include(repairOrder => repairOrder.Payments)
                                             .AsSplitQuery()
                                             .AsNoTracking()
                                             .FirstOrDefaultAsync(repairOrder => repairOrder.Id == id);

            return RepairOrderHelper.ConvertToReadDto(repairOrderFromContext);
        }

        public async Task<RepairOrder> GetEntity(long id)
        {
            var repairOrderFromContext = await context.RepairOrders
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

            return repairOrderFromContext;
        }

        public async Task<IReadOnlyList<RepairOrderToReadInList>> Get()
        {
            _logger.LogInformation("GetRepairOrderListAsync");
            IReadOnlyList<RepairOrder> repairOrders = await context.RepairOrders.ToListAsync();

            return repairOrders
                .Select(repairOrder =>
                       RepairOrderHelper.ConvertToReadInListDto(repairOrder))
                .ToList();
        }

        public async Task<List<long>> GetTodaysRepairOrderNumbers()
        {
            var repairOrders = await context.RepairOrders.ToListAsync();
            var todayDatePart = long.Parse(DateTime.Today.ToString("yyyyMMdd")) * 1000;

            return repairOrders
                .Select(repairOrder => repairOrder.RepairOrderNumber)
                .Where(number => number >= todayDatePart && number < todayDatePart + 1000)
                .ToList();
        }

        public async Task<bool> Exists(long id)
        {
            return await context.RepairOrders.AnyAsync(repairOrder => repairOrder.Id == id);
        }

        public async Task SaveChanges() =>
            await context.SaveChangesAsync();

        public long GetLastInvoiceNumberOrSeed()
        {
            var invoiceNumbers = context.RepairOrders
                .Select(repairOrder => repairOrder.InvoiceNumber)
                .Max();

            return invoiceNumbers;
        }
    }
}
