using MenomineePlayWASM.Shared.Dtos.Payables.Invoices.Payments;
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
    public class VendorInvoicePaymentController : Controller
    {
        //private readonly ApplicationDbContext context;

        //public VendorInvoicePaymentController(ApplicationDbContext context)
        //{
        //    this.context = context;
        //}

        //[HttpGet]
        //public async Task<ActionResult<List<VendorInvoicePayment>>> Get()
        //{
        //    return await context.VendorInvoicePayments.ToListAsync();
        //}

        //[HttpGet("{id:int}")]
        //public async Task<ActionResult<VendorInvoicePaymentReadDto>> Get(int id)
        //{
        //    var item = await context.VendorInvoicePayments.FirstOrDefaultAsync(x => x.Id == id);
        //    if (item == null)
        //    {
        //        return NotFound();
        //    }

        //    var model = new VendorInvoicePaymentReadDto();
        //    model.Id = item.Id;
        //    model.InvoiceId = item.InvoiceId;
        //    model.PaymentMethod = item.PaymentMethod;
        //    model.PaymentMethodName = "VISA";
        //    model.Amount = item.Amount;
        //    return model;
        //}

        //[HttpGet("InvoiceId/{invoiceId:int}")]
        //public async Task<ActionResult<List<VendorInvoicePayment>>> GetPayments(int invoiceId)
        //{
        //    return await context.VendorInvoicePayments.Where(x => x.InvoiceId == invoiceId).ToListAsync();
        //}

        //[HttpPost]
        //public async Task<ActionResult<int>> Post(VendorInvoicePayment payment)
        //{
        //    context.Add(payment);
        //    await context.SaveChangesAsync();
        //    return payment.Id;
        //}

        //[HttpPut]
        //public async Task<ActionResult> Put(VendorInvoicePayment payment)
        //{
        //    context.Attach(payment).State = EntityState.Modified;
        //    await context.SaveChangesAsync();
        //    return NoContent();
        //}
    }
}
