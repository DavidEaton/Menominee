using MenomineePlayWASM.Server.Repository.RepairOrders;
using MenomineePlayWASM.Shared.Dtos.RepairOrders;
using MenomineePlayWASM.Shared.Entities.RepairOrders;
using MenomineePlayWASM.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Server.Controllers.RepairOrders
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
            return Ok(results);
        }

        // api/repairorders
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<RepairOrderToRead>>> GetRepairOrdersAsync()
        {
            var result = await repository.GetRepairOrdersAsync();
            return Ok(result);
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
        public async Task<IActionResult> UpdateRepairOrderAsync(long id, RepairOrderToWrite roUpdateDto)
        {
            if (!await repository.RepairOrderExistsAsync(id))
                return NotFound($"Could not find RO # {roUpdateDto.Id} to update.");

            //1) Get domain entity from repository
            var ro = repository.GetRepairOrderEntityAsync(id).Result;

            // 2) Update domain entity with data in data transfer object(DTO)
            ro.RepairOrderNumber = roUpdateDto.RepairOrderNumber;
            ro.InvoiceNumber = roUpdateDto.InvoiceNumber;
            ro.CustomerName = roUpdateDto.CustomerName;
            ro.Vehicle = roUpdateDto.Vehicle;
            ro.Total = roUpdateDto.Total;

            // Update the objects ObjectState and synch the EF Change Tracker
            // 3) Set entity's TrackingState to Modified
            ro.SetTrackingState(TrackingState.Modified);

            // 4) FixTrackingState: moves entity state tracking into the context
            repository.FixTrackingState();

            repository.UpdateRepairOrderAsync(ro);

            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<RepairOrderToRead>> AddRepairOrderAsync(RepairOrderToWrite roDto)
        {
            // 1. Convert dto to domain entity
            var ro = new RepairOrder()
            {
                RepairOrderNumber = roDto.RepairOrderNumber,
                InvoiceNumber = roDto.InvoiceNumber,
                CustomerName = roDto.CustomerName,
                Vehicle = roDto.Vehicle,
                Total = roDto.Total
            };

            // 2. Add domain entity to repository
            await repository.AddRepairOrderAsync(ro);

            // 3. Save changes on repository
            await repository.SaveChangesAsync();

            // 4. Return new Id from database to consumer after save
            return Created(new Uri($"{BasePath}/{ro.Id}", UriKind.Relative), new { id = ro.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteRepairOrderAsync(long id)
        {
            var roFromRepository = await repository.GetRepairOrderAsync(id);
            if (roFromRepository == null)
                return NotFound($"Could not find RO in the database to delete with id of {id}.");

            await repository.DeleteRepairOrderAsync(id);

            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
