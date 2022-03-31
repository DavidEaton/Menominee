using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using CustomerVehicleManagement.Shared.Models.RepairOrders;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var repairOrderFromRepository = repository.GetRepairOrderEntityAsync(id).Result;
            if (repairOrderFromRepository is null)
                return NotFound($"Could not find Repair Order #{id} to update.");

            UpdateRepairOrder(repairOrder, repairOrderFromRepository);

            List<RepairOrderService> services = new();
            if (repairOrder?.Services.Count > 0)
            {
                services.AddRange(repairOrder.Services.Select(service => new RepairOrderService()
                {
                    DiscountTotal = service.DiscountTotal,
                    IsCounterSale = service.IsCounterSale,
                    IsDeclined = service.IsDeclined,
                    LaborTotal = service.LaborTotal,
                    PartsTotal = service.PartsTotal,
                    RepairOrderId = id,
                    SaleCode = service.SaleCode,
                    ServiceName = service.ServiceName,
                    ShopSuppliesTotal = service.ShopSuppliesTotal,
                    TaxTotal = service.TaxTotal,
                    Total = service.Total,
                    Items = RepairOrderHelper.CreateServiceItems(service.Items),
                    Taxes = RepairOrderHelper.CreateServiceTaxes(service.Taxes),
                    Techs = RepairOrderHelper.CreateTechnicians(service.Techs)
                }));
            }

            List<RepairOrderPayment> payments = new();
            if (repairOrder?.Payments.Count > 0)
            {
                payments.AddRange(repairOrder.Payments.Select(payment => new RepairOrderPayment()
                {
                    Amount = payment.Amount,
                    PaymentMethod = payment.PaymentMethod,
                    RepairOrderId = payment.RepairOrderId
                }));
            }

            List<RepairOrderTax> taxes = new();
            if (repairOrder?.Taxes.Count > 0)
            {
                taxes.AddRange(repairOrder.Taxes.Select(payment => new RepairOrderTax()
                {
                    LaborTax = payment.LaborTax,
                    LaborTaxRate = payment.LaborTaxRate,
                    PartTax = payment.PartTax,
                    PartTaxRate = payment.PartTaxRate,
                    RepairOrderId = payment.RepairOrderId,
                    TaxId = payment.TaxId
                }));
            }

            repairOrderFromRepository.SetServices(services);
            repairOrderFromRepository.SetPayments(payments);
            repairOrderFromRepository.SetTaxes(taxes);

            repairOrderFromRepository.SetTrackingState(TrackingState.Modified);
            repository.FixTrackingState();

            repository.UpdateRepairOrderAsync(repairOrderFromRepository);
            await repository.SaveChangesAsync();

            return NoContent();
        }

        private static void UpdateRepairOrder(RepairOrderToWrite repairOrder, RepairOrder repairOrderFromRepository)
        {
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
            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
