using CustomerVehicleManagement.Shared.Models.RepairOrders;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.RepairOrders
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepairOrdersController : ControllerBase
    {
        private readonly IRepairOrderRepository repository;
        private readonly string BasePath = "/api/repairorders";

        public RepairOrdersController(IRepairOrderRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // api/repairorders/listing
        [Route("listing")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<RepairOrderToReadInList>>> GetRepairOrderListAsync()
        {
            var results = await repository.GetRepairOrderListAsync();

            if (results == null)
                return NotFound();

            return Ok(results);
        }

        // api/repairorders
        //[HttpGet]
        //public async Task<ActionResult<IReadOnlyList<RepairOrderToRead>>> GetRepairOrdersAsync()
        //{
        //    var result = await repository.GetRepairOrdersAsync();
        //    return Ok(result);
        //}

        // api/repairorders/1
        [HttpGet("{id:long}")]
        public async Task<ActionResult<RepairOrderToRead>> GetRepairOrderAsync(long id)
        {
            var result = await repository.GetRepairOrderAsync(id);

            if (result == null)
                return NotFound();

            return result;
        }

        // api/repairorders/1
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateRepairOrderAsync(long id, RepairOrderToWrite repairOrder)
        {
            if (!await repository.RepairOrderExistsAsync(id))
                return NotFound($"Could not find Repair Order #{id} to update.");

            var repairOrderFromRepository = repository.GetRepairOrderEntityAsync(id).Result;

            repairOrderFromRepository.CustomerName = repairOrder.CustomerName;
            repairOrderFromRepository.DateModified = DateTime.Today;
            repairOrderFromRepository.DiscountTotal = repairOrder.DiscountTotal;
            repairOrderFromRepository.HazMatTotal = repairOrder.HazMatTotal;
            repairOrderFromRepository.InvoiceNumber = repairOrder.InvoiceNumber;
            repairOrderFromRepository.LaborTotal = repairOrder.LaborTotal;
            repairOrderFromRepository.PartsTotal = repairOrder.PartsTotal;
            repairOrderFromRepository.RepairOrderNumber = repairOrder.RepairOrderNumber;
            repairOrderFromRepository.ShopSuppliesTotal = repairOrder.ShopSuppliesTotal;
            repairOrderFromRepository.TaxTotal = repairOrder.TaxTotal;
            repairOrderFromRepository.Total = repairOrder.Total;
            repairOrderFromRepository.Vehicle = repairOrder.Vehicle;

            if (repairOrder.DateInvoiced.HasValue)
                repairOrderFromRepository.DateInvoiced = (DateTime)repairOrder.DateInvoiced;

            repairOrderFromRepository.SetPayments(RepairOrderHelper.CreatePayments(repairOrder?.Payments));
            repairOrderFromRepository.SetServices(RepairOrderHelper.CreateServices(repairOrder?.Services));
            repairOrderFromRepository.SetTaxes(RepairOrderHelper.CreateTaxes(repairOrder?.Taxes));

            repairOrderFromRepository.SetTrackingState(TrackingState.Modified);
            repository.FixTrackingState();

            repository.UpdateRepairOrderAsync(repairOrderFromRepository);
            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> AddRepairOrderAsync(RepairOrderToWrite repairOrderToAdd)
        {
            var repairOrder = RepairOrderHelper.CreateRepairOrder(repairOrderToAdd);

            await repository.AddRepairOrderAsync(repairOrder);
            await repository.SaveChangesAsync();

            return Created(new Uri($"{BasePath}/{repairOrder.Id}",
                               UriKind.Relative),
                               new { id = repairOrder.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteRepairOrderAsync(long id)
        {
            var roFromRepository = await repository.GetRepairOrderAsync(id);
            if (roFromRepository == null)
                return NotFound($"Could not find Repair Order in the database to delete with id of {id}.");

            await repository.DeleteRepairOrderAsync(id);

            if (await repository.SaveChangesAsync())
                return NoContent();

            return BadRequest($"Failed to delete Repair Order with Id of {id}.");
        }
    }
}
