using MenomineePlayWASM.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MenomineePlayWASM.Server.Repository.Inventory;
using MenomineePlayWASM.Server.Repository.Payables;
using MenomineePlayWASM.Server.Repository.Customers;
using System.Linq;
using MenomineePlayWASM.Server.Repository.RepairOrders;

namespace MenomineePlayWASM.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddAutoMapper(typeof(Startup));
            //services.AddControllersWithViews();
            services.AddMvc()
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddRazorPages();

            // Repair Orders
            services.AddScoped<IRepairOrderRepository, RepairOrderRepository>();

            // Inventory
            services.AddScoped<IInventoryItemRepository, InventoryItemRepository>();

            // Payables
            services.AddScoped<IVendorRepository, VendorRepository>();
            //services.AddScoped<IVendorDataService, VendorDataService>();
            services.AddScoped<IVendorInvoiceRepository, VendorInvoiceRepository>();
            services.AddScoped<IVendorInvoiceItemRepository, VendorInvoiceItemRepository>();
            services.AddScoped<IVendorInvoicePaymentRepository, VendorInvoicePaymentRepository>();
            services.AddScoped<IVendorInvoicePaymentMethodRepository, VendorInvoicePaymentMethodRepository>();
            services.AddScoped<IVendorInvoiceTaxRepository, VendorInvoiceTaxRepository>();
            services.AddScoped<ICreditReturnRepository, CreditReturnRepository>();

            // Customers
            services.AddScoped<ICustomerRepository, CustomerRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
