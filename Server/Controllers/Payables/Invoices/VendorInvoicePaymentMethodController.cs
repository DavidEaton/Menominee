using MenomineePlayWASM.Shared.Dtos.Payables.Invoices.Payments;
using MenomineePlayWASM.Shared.Entities.Payables.Invoices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Server.Controllers.Payables.Invoices
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendorInvoicePaymentMethodController : Controller
    {
        //private readonly ApplicationDbContext context;

        //public VendorInvoicePaymentMethodController(ApplicationDbContext context)
        //{
        //    this.context = context;
        //}

        //[HttpGet]
        //public async Task<ActionResult<List<VendorInvoicePaymentMethod>>> Get()
        //{
        //    return await context.VendorInvoicePaymentMethods.ToListAsync();
        //}

        //[HttpGet("{id:int}")]
        //public async Task<ActionResult<VendorInvoicePaymentMethodDto>> Get(int id)
        //{
        //    var item = await context.VendorInvoicePaymentMethods.FirstOrDefaultAsync(x => x.Id == id);
        //    if (item == null)
        //    {
        //        return NotFound();
        //    }

        //    var model = new VendorInvoicePaymentMethodDto();
        //    model.Id = item.Id;
        //    model.PaymentName = item.PaymentName;
        //    return model;
        //}

        //[HttpPost]
        //public async Task<ActionResult<int>> Post(VendorInvoicePaymentMethod method)
        //{
        //    context.Add(method);
        //    await context.SaveChangesAsync();
        //    return method.Id;
        //}

        //[HttpPut]
        //public async Task<ActionResult> Put(VendorInvoicePaymentMethod method)
        //{
        //    context.Attach(method).State = EntityState.Modified;
        //    await context.SaveChangesAsync();
        //    return NoContent();
        //}
    }
}
