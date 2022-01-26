using MenomineePlayWASM.Shared;
using MenomineePlayWASM.Shared.Dtos.RepairOrders;
using MenomineePlayWASM.Shared.Entities.RepairOrders;
using MenomineePlayWASM.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Server.Repository.RepairOrders
{
    public class RepairOrderRepository : IRepairOrderRepository
    {
        private readonly ApplicationDbContext context;

        public RepairOrderRepository(ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task AddRepairOrderAsync(RepairOrder repairOrder)
        {
            if (repairOrder != null)
                await context.AddAsync(repairOrder);
        }

        public async Task DeleteRepairOrderAsync(long id)
        {
            var roFromContext = await context.RepairOrders.FindAsync(id);
            if (roFromContext != null)
                context.Remove(roFromContext);
        }

        public void FixTrackingState()
        {
            context.FixState();
        }

        public async Task<RepairOrderToRead> GetRepairOrderAsync(long id)
        {
            var roFromContext = await context.RepairOrders
                .Include(ro => ro.Services)
                    .ThenInclude(i => i.Items)
                        .ThenInclude(t => t.Taxes)
                .Include(ro => ro.Services)
                    .ThenInclude(i => i.Items)
                        .ThenInclude(s => s.SerialNumbers)
                .Include(ro => ro.Services)
                    .ThenInclude(i => i.Items)
                        .ThenInclude(w => w.Warranties)
                .Include(ro => ro.Services)
                    .ThenInclude(t => t.Techs)
                .Include(ro => ro.Services)
                    .ThenInclude(t => t.Taxes)
                .Include(ro => ro.Taxes)
                .Include(ro => ro.Payments)
                .FirstOrDefaultAsync(ro => ro.Id == id);

            return RepairOrderToRead.ConvertToDto(roFromContext);
        }

        public async Task<RepairOrder> GetRepairOrderEntityAsync(long id)
        {
            var roFromContext = await context.RepairOrders
                .Include(ro => ro.Services)
                    .ThenInclude(i => i.Items)
                        .ThenInclude(t => t.Taxes)
                .Include(ro => ro.Services)
                    .ThenInclude(i => i.Items)
                        .ThenInclude(s => s.SerialNumbers)
                .Include(ro => ro.Services)
                    .ThenInclude(i => i.Items)
                        .ThenInclude(w => w.Warranties)
                .Include(ro => ro.Services)
                    .ThenInclude(t => t.Techs)
                .Include(ro => ro.Services)
                    .ThenInclude(t => t.Taxes)
                .Include(ro => ro.Taxes)
                .Include(ro => ro.Payments)
                .FirstOrDefaultAsync(ro => ro.Id == id);

            return roFromContext;
        }

        public async Task<IReadOnlyList<RepairOrderToReadInList>> GetRepairOrderListAsync()
        {
            IReadOnlyList<RepairOrder> repairOrders = await context.RepairOrders.ToListAsync();

            return repairOrders.
                Select(repairOrder =>
                       RepairOrderToReadInList.ConvertToDto(repairOrder))
                .ToList();
        }

        //public async Task<IReadOnlyList<RepairOrderToRead>> GetRepairOrdersAsync()
        //{
        //    IReadOnlyList<RepairOrder> ros = await context.RepairOrders.ToListAsync();

        //    return ros.Select(ro => new RepairOrderToRead()
        //    {
        //        Id = ro.Id,
        //        RepairOrderNumber = ro.RepairOrderNumber,
        //        InvoiceNumber = ro.InvoiceNumber,
        //        CustomerName = ro.CustomerName,
        //        Vehicle = ro.Vehicle,
        //        Total = ro.Total
        //    }).ToList();
        //}

        public async Task<bool> RepairOrderExistsAsync(long id)
        {
            return await context.RepairOrders.AnyAsync(ro => ro.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void UpdateRepairOrderAsync(RepairOrder repairOrder)
        {
            // No code in this implementation
        }
    }
}
