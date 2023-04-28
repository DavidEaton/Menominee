using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using CustomerVehicleManagement.Shared.Models.RepairOrders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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

        public async Task AddRepairOrderAsync(RepairOrder repairOrder)
        {
            if (repairOrder != null)
                await context.AddAsync(repairOrder);
        }

        public async Task DeleteRepairOrderAsync(long id)
        {
            var repairOrderFromContext = await context.RepairOrders.FindAsync(id);
            if (repairOrderFromContext != null)
                context.Remove(repairOrderFromContext);
        }

        public async Task<RepairOrderToRead> GetRepairOrderAsync(long id)
        {
            var repairOrderFromContext = await context.RepairOrders
                                             .Include(repairOrder => repairOrder.Services)
                                                 .ThenInclude(service => service.Items)
                                                     .ThenInclude(repairOrderItem => repairOrderItem.Manufacturer)
                                             .Include(repairOrder => repairOrder.Services)
                                                 .ThenInclude(service => service.Items)
                                                     .ThenInclude(repairOrderItem => repairOrderItem.ProductCode)
                                             .Include(repairOrder => repairOrder.Services)
                                                 .ThenInclude(service => service.Items)
                                                     .ThenInclude(repairOrderItem => repairOrderItem.SaleCode)
                                             .Include(repairOrder => repairOrder.Services)
                                                 .ThenInclude(service => service.Items)
                                                     .ThenInclude(repairOrderItem => repairOrderItem.Taxes)
                                             .Include(repairOrder => repairOrder.Services)
                                                 .ThenInclude(service => service.Items)
                                                     .ThenInclude(repairOrderItem => repairOrderItem.SerialNumbers)
                                             .Include(repairOrder => repairOrder.Services)
                                                 .ThenInclude(repairOrderItem => repairOrderItem.Items)
                                                     .ThenInclude(repairOrderItem => repairOrderItem.Warranties)
                                             .Include(repairOrder => repairOrder.Services)
                                                 .ThenInclude(repairOrderItem => repairOrderItem.Items)
                                                     .ThenInclude(repairOrderItem => repairOrderItem.Purchases)
                                             .Include(repairOrder => repairOrder.Services)
                                                 .ThenInclude(service => service.Techs)
                                             .Include(repairOrder => repairOrder.Services)
                                                 .ThenInclude(service => service.Taxes)
                                             .Include(repairOrder => repairOrder.Taxes)
                                             .Include(repairOrder => repairOrder.Payments)
                                             .AsSplitQuery()
                                             .AsNoTracking()
                                             .FirstOrDefaultAsync(repairOrder => repairOrder.Id == id);

            return RepairOrderHelper.Transform(repairOrderFromContext);
        }

        public async Task<RepairOrder> GetRepairOrderEntityAsync(long id)
        {
            var repairOrderFromContext = await context.RepairOrders
                .Include(repairOrder => repairOrder.Services)
                    .ThenInclude(i => i.Items)
                        .ThenInclude(m => m.Manufacturer)
                .Include(repairOrder => repairOrder.Services)
                    .ThenInclude(i => i.Items)
                        .ThenInclude(p => p.ProductCode)
                .Include(repairOrder => repairOrder.Services)
                    .ThenInclude(i => i.Items)
                        .ThenInclude(s => s.SaleCode)
                .Include(repairOrder => repairOrder.Services)
                    .ThenInclude(i => i.Items)
                        .ThenInclude(t => t.Taxes)
                .Include(repairOrder => repairOrder.Services)
                    .ThenInclude(i => i.Items)
                        .ThenInclude(s => s.SerialNumbers)
                .Include(repairOrder => repairOrder.Services)
                    .ThenInclude(i => i.Items)
                        .ThenInclude(w => w.Warranties)
                .Include(repairOrder => repairOrder.Services)
                    .ThenInclude(t => t.Techs)
                .Include(repairOrder => repairOrder.Services)
                    .ThenInclude(t => t.Taxes)
                .Include(repairOrder => repairOrder.Taxes)
                .Include(repairOrder => repairOrder.Payments)
                .FirstOrDefaultAsync(repairOrder => repairOrder.Id == id);

            return repairOrderFromContext;
        }

        public async Task<IReadOnlyList<RepairOrderToReadInList>> GetRepairOrderListAsync()
        {
            _logger.LogInformation("GetRepairOrderListAsync");
            IReadOnlyList<RepairOrder> repairOrders = await context.RepairOrders.ToListAsync();

            return repairOrders
                .Select(repairOrder =>
                       RepairOrderHelper.TransformInList(repairOrder))
                .ToList();
        }

        public async Task<bool> RepairOrderExistsAsync(long id)
        {
            return await context.RepairOrders.AnyAsync(repairOrder => repairOrder.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                var moops = await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                // TODO: log exception
                throw;
            }
        }
    }
}
