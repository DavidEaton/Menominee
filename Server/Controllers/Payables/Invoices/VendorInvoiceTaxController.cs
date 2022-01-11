using MenomineePlayWASM.Shared.Dtos.Payables.Invoices.Taxes;
using MenomineePlayWASM.Shared.Entities.Payables.Invoices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Server.Controllers.Payables.Invoices
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendorInvoiceTaxController : Controller
    {
        //private readonly ApplicationDbContext context;

        //public VendorInvoiceTaxController(ApplicationDbContext context)
        //{
        //    this.context = context;
        //}

        //[HttpGet]
        //public async Task<ActionResult<List<VendorInvoiceTax>>> Get()
        //{
        //    return await context.VendorInvoiceTaxes.ToListAsync();
        //}

        //[HttpGet("{id:int}")]
        //public async Task<ActionResult<VendorInvoiceTaxDto>> Get(int id)
        //{
        //    var item = await context.VendorInvoiceTaxes.FirstOrDefaultAsync(x => x.Id == id);
        //    if (item == null)
        //    {
        //        return NotFound();
        //    }

        //    var model = new VendorInvoiceTaxDto();
        //    model.Id = item.Id;
        //    model.InvoiceId = item.InvoiceId;
        //    model.Order = item.Order;
        //    model.TaxId = item.TaxId;
        //    model.TaxName = "State Tax";
        //    model.Amount = item.Amount;
        //    return model;
        //}

        //[HttpGet("InvoiceId/{invoiceId:int}")]
        //public async Task<ActionResult<List<VendorInvoiceTax>>> GetTaxes(int invoiceId)
        //{
        //    return await context.VendorInvoiceTaxes.Where(x => x.InvoiceId == invoiceId).ToListAsync();
        //}

        //[HttpPost]
        //public async Task<ActionResult<int>> Post(VendorInvoiceTax tax)
        //{
        //    context.Add(tax);
        //    await context.SaveChangesAsync();
        //    return tax.Id;
        //}

        //[HttpPut]
        //public async Task<ActionResult> Put(VendorInvoiceTax tax)
        //{
        //    context.Attach(tax).State = EntityState.Modified;
        //    await context.SaveChangesAsync();
        //    return NoContent();
        //}
    }
}
