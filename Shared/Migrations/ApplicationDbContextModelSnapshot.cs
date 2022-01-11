﻿// <auto-generated />
using System;
using MenomineePlayWASM.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MenomineePlayWASM.Shared.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MenomineePlayWASM.Shared.Entities.Customers.Customer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street2")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("MenomineePlayWASM.Shared.Entities.Inventory.InventoryItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Core")
                        .HasColumnType("float");

                    b.Property<double>("Cost")
                        .HasColumnType("float");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<double>("Labor")
                        .HasColumnType("float");

                    b.Property<long?>("ManufacturerId")
                        .HasColumnType("bigint");

                    b.Property<double>("OnHand")
                        .HasColumnType("float");

                    b.Property<string>("PartNumber")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("PartType")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<double>("Retail")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ManufacturerId");

                    b.ToTable("InventoryItems", "dbo");
                });

            modelBuilder.Entity("MenomineePlayWASM.Shared.Entities.Inventory.Manufacturer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("MfrId")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Prefix")
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.HasKey("Id");

                    b.ToTable("Manufacturers", "dbo");
                });

            modelBuilder.Entity("MenomineePlayWASM.Shared.Entities.Payables.CreditReturns.CreditReturn", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("InvoiceNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Total")
                        .HasColumnType("float");

                    b.Property<long>("VendorId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("CreditReturns");
                });

            modelBuilder.Entity("MenomineePlayWASM.Shared.Entities.Payables.CreditReturns.CreditReturnItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Core")
                        .HasColumnType("float");

                    b.Property<double>("Cost")
                        .HasColumnType("float");

                    b.Property<long?>("CreditReturnId")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InvoiceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InvoiceNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MfrId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PONumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Quantity")
                        .HasColumnType("float");

                    b.Property<DateTime?>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreditReturnId");

                    b.ToTable("CreditReturnItems");
                });

            modelBuilder.Entity("MenomineePlayWASM.Shared.Entities.Payables.Invoices.VendorInvoice", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DatePosted")
                        .HasColumnType("datetime2");

                    b.Property<string>("InvoiceNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<double>("Total")
                        .HasColumnType("float");

                    b.Property<long?>("VendorId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("VendorId");

                    b.ToTable("VendorInvoices", "dbo");
                });

            modelBuilder.Entity("MenomineePlayWASM.Shared.Entities.Payables.Invoices.VendorInvoiceItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Core")
                        .HasColumnType("float");

                    b.Property<double>("Cost")
                        .HasColumnType("float");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<long>("InvoiceId")
                        .HasColumnType("bigint");

                    b.Property<string>("InvoiceNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MfrId")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("PONumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartNumber")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<double>("Quantity")
                        .HasColumnType("float");

                    b.Property<DateTime?>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.HasKey("Id");

                    b.HasIndex("InvoiceId");

                    b.ToTable("VendorInvoiceItems", "dbo");
                });

            modelBuilder.Entity("MenomineePlayWASM.Shared.Entities.Payables.Invoices.VendorInvoicePayment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<long>("InvoiceId")
                        .HasColumnType("bigint");

                    b.Property<int>("PaymentMethod")
                        .HasColumnType("int");

                    b.Property<long?>("VendorInvoiceId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("VendorInvoiceId");

                    b.ToTable("VendorInvoicePayments", "dbo");
                });

            modelBuilder.Entity("MenomineePlayWASM.Shared.Entities.Payables.Invoices.VendorInvoicePaymentMethod", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("PaymentName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("VendorInvoicePaymentMethods");
                });

            modelBuilder.Entity("MenomineePlayWASM.Shared.Entities.Payables.Invoices.VendorInvoiceTax", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<long>("InvoiceId")
                        .HasColumnType("bigint");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<int>("TaxId")
                        .HasColumnType("int");

                    b.Property<long?>("VendorInvoiceId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("VendorInvoiceId");

                    b.ToTable("VendorInvoiceTaxes", "dbo");
                });

            modelBuilder.Entity("MenomineePlayWASM.Shared.Entities.Payables.Vendors.Vendor", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("VendorCode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.ToTable("Vendors", "dbo");
                });

            modelBuilder.Entity("MenomineePlayWASM.Shared.Entities.RepairOrders.RepairOrder", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CustomerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("InvoiceNumber")
                        .HasColumnType("bigint");

                    b.Property<long>("RepairOrderNumber")
                        .HasColumnType("bigint");

                    b.Property<double>("Total")
                        .HasColumnType("float");

                    b.Property<string>("Vehicle")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RepairOrders", "dbo");
                });

            modelBuilder.Entity("MenomineePlayWASM.Shared.Entities.Inventory.InventoryItem", b =>
                {
                    b.HasOne("MenomineePlayWASM.Shared.Entities.Inventory.Manufacturer", "Manufacturer")
                        .WithMany()
                        .HasForeignKey("ManufacturerId");

                    b.Navigation("Manufacturer");
                });

            modelBuilder.Entity("MenomineePlayWASM.Shared.Entities.Payables.CreditReturns.CreditReturnItem", b =>
                {
                    b.HasOne("MenomineePlayWASM.Shared.Entities.Payables.CreditReturns.CreditReturn", null)
                        .WithMany("LineItems")
                        .HasForeignKey("CreditReturnId");
                });

            modelBuilder.Entity("MenomineePlayWASM.Shared.Entities.Payables.Invoices.VendorInvoice", b =>
                {
                    b.HasOne("MenomineePlayWASM.Shared.Entities.Payables.Vendors.Vendor", "Vendor")
                        .WithMany()
                        .HasForeignKey("VendorId");

                    b.Navigation("Vendor");
                });

            modelBuilder.Entity("MenomineePlayWASM.Shared.Entities.Payables.Invoices.VendorInvoiceItem", b =>
                {
                    b.HasOne("MenomineePlayWASM.Shared.Entities.Payables.Invoices.VendorInvoice", "Invoice")
                        .WithMany("LineItems")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Invoice");
                });

            modelBuilder.Entity("MenomineePlayWASM.Shared.Entities.Payables.Invoices.VendorInvoicePayment", b =>
                {
                    b.HasOne("MenomineePlayWASM.Shared.Entities.Payables.Invoices.VendorInvoice", null)
                        .WithMany("Payments")
                        .HasForeignKey("VendorInvoiceId");
                });

            modelBuilder.Entity("MenomineePlayWASM.Shared.Entities.Payables.Invoices.VendorInvoiceTax", b =>
                {
                    b.HasOne("MenomineePlayWASM.Shared.Entities.Payables.Invoices.VendorInvoice", null)
                        .WithMany("Taxes")
                        .HasForeignKey("VendorInvoiceId");
                });

            modelBuilder.Entity("MenomineePlayWASM.Shared.Entities.Payables.CreditReturns.CreditReturn", b =>
                {
                    b.Navigation("LineItems");
                });

            modelBuilder.Entity("MenomineePlayWASM.Shared.Entities.Payables.Invoices.VendorInvoice", b =>
                {
                    b.Navigation("LineItems");

                    b.Navigation("Payments");

                    b.Navigation("Taxes");
                });
#pragma warning restore 612, 618
        }
    }
}
