using MenomineePlayWASM.Shared.Dtos.Payables.Invoices.Items;
using MenomineePlayWASM.Shared.Entities.Payables.Invoices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Server.Controllers.Payables.Invoices
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendorInvoiceItemController : ControllerBase
    {
        //private readonly IVendorInvoiceItemRepository repository;

        //public VendorInvoiceItemController(IVendorInvoiceItemRepository repository)
        //{
        //    this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        //}


        //// GET: api/vendorinvoiceitem/list
        //[Route("list")]
        //[HttpGet]
        //public async Task<ActionResult<IReadOnlyList<VendorInvoiceInListDto>>> GetInvoiceListAsync()
        //{
        //    var invoices = await repository.GetInvoiceListAsync();

        //    if (invoices == null)
        //        return NotFound();

        //    return Ok(invoices);
        //}
        //[HttpGet]
        //public async Task<ActionResult<List<VendorInvoiceItem>>> Get()
        //{
        //    return await context.VendorInvoiceItems.ToListAsync();
        //}

        //[HttpGet("{id:int}")]
        //public async Task<ActionResult<VendorInvoiceItemReadDto>> Get(int id)
        //{
        //    var item = await context.VendorInvoiceItems.FirstOrDefaultAsync(item => item.Id == id);
        //    if (item == null)
        //    {
        //        return NotFound();
        //    }

        //    var model = new VendorInvoiceItemReadDto();
        //    model.Id = item.Id;
        //    model.InvoiceId = item.InvoiceId;
        //    model.Type = item.Type;
        //    model.PartNumber = item.PartNumber;
        //    model.MfrId = item.MfrId;
        //    model.Description = item.Description;
        //    model.Quantity = item.Quantity;
        //    model.Cost = item.Cost;
        //    model.Core = item.Core;
        //    model.PONumber = item.PONumber;
        //    model.InvoiceNumber = item.InvoiceNumber;
        //    model.TransactionDate = item.TransactionDate;

        //    return model;
        //}

        //[HttpGet("InvoiceId/{invoiceId:int}")]
        //public async Task<ActionResult<List<VendorInvoiceItem>>> GetItems(int invoiceId)
        //{
        //    return await context.VendorInvoiceItems.Where(item => item.InvoiceId == invoiceId).ToListAsync();
        //}

        //[HttpPost]
        //public async Task<ActionResult<int>> Post(VendorInvoiceItem item)
        //{
        //    context.Add(item);
        //    await context.SaveChangesAsync();
        //    return item.Id;
        //}

        //[HttpPut]
        //public async Task<ActionResult> Put(VendorInvoiceItem item)
        //{
        //    context.Attach(item).State = EntityState.Modified;
        //    await context.SaveChangesAsync();
        //    return NoContent();
        //}
    }
}
