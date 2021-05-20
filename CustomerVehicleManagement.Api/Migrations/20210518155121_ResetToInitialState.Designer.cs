﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CustomerVehicleManagement.Api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20210518155121_ResetToInitialState")]
    partial class ResetToInitialState
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CustomerVehicleManagement.Domain.Entities.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int>("CustomerType")
                        .HasColumnType("int");

                    b.Property<int>("EntityId")
                        .HasColumnType("int");

                    b.Property<int>("EntityType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Customer", "dbo");
                });

            modelBuilder.Entity("CustomerVehicleManagement.Domain.Entities.Email", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(254)
                        .HasColumnType("nvarchar(254)");

                    b.Property<bool>("IsPrimary")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int?>("OrganizationId")
                        .HasColumnType("int");

                    b.Property<int?>("PersonId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.HasIndex("PersonId");

                    b.ToTable("Email", "dbo");
                });

            modelBuilder.Entity("CustomerVehicleManagement.Domain.Entities.Organization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ContactId")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .HasMaxLength(10000)
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ContactId");

                    b.ToTable("Organization", "dbo");
                });

            modelBuilder.Entity("CustomerVehicleManagement.Domain.Entities.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("datetime2");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Person", "dbo");
                });

            modelBuilder.Entity("CustomerVehicleManagement.Domain.Entities.Phone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsPrimary")
                        .HasColumnType("bit");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("OrganizationId")
                        .HasColumnType("int");

                    b.Property<int?>("PersonId")
                        .HasColumnType("int");

                    b.Property<string>("PhoneType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.HasIndex("PersonId");

                    b.ToTable("Phone", "dbo");
                });

            modelBuilder.Entity("CustomerVehicleManagement.Domain.Entities.Vehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("Make")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VIN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Vehicle", "dbo");
                });

            modelBuilder.Entity("CustomerVehicleManagement.Domain.Entities.Customer", b =>
                {
                    b.OwnsOne("SharedKernel.ValueObjects.ContactPreferences", "ContactPreferences", b1 =>
                        {
                            b1.Property<int>("CustomerId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<bool>("AllowEmail")
                                .HasColumnType("bit")
                                .HasColumnName("AllowEmail");

                            b1.Property<bool>("AllowMail")
                                .HasColumnType("bit")
                                .HasColumnName("AllowMail");

                            b1.Property<bool>("AllowSms")
                                .HasColumnType("bit")
                                .HasColumnName("AllowSms");

                            b1.HasKey("CustomerId");

                            b1.ToTable("Customer");

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");
                        });

                    b.Navigation("ContactPreferences");
                });

            modelBuilder.Entity("CustomerVehicleManagement.Domain.Entities.Email", b =>
                {
                    b.HasOne("CustomerVehicleManagement.Domain.Entities.Organization", null)
                        .WithMany("Emails")
                        .HasForeignKey("OrganizationId");

                    b.HasOne("CustomerVehicleManagement.Domain.Entities.Person", null)
                        .WithMany("Emails")
                        .HasForeignKey("PersonId");
                });

            modelBuilder.Entity("CustomerVehicleManagement.Domain.Entities.Organization", b =>
                {
                    b.HasOne("CustomerVehicleManagement.Domain.Entities.Person", "Contact")
                        .WithMany()
                        .HasForeignKey("ContactId");

                    b.OwnsOne("SharedKernel.ValueObjects.Address", "Address", b1 =>
                        {
                            b1.Property<int>("OrganizationId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("AddressLine")
                                .HasMaxLength(255)
                                .HasColumnType("nvarchar(255)")
                                .HasColumnName("AddressLine");

                            b1.Property<string>("City")
                                .HasMaxLength(255)
                                .HasColumnType("nvarchar(255)")
                                .HasColumnName("AddressCity");

                            b1.Property<string>("PostalCode")
                                .HasMaxLength(15)
                                .HasColumnType("nvarchar(15)")
                                .HasColumnName("AddressPostalCode");

                            b1.Property<string>("State")
                                .HasMaxLength(255)
                                .HasColumnType("nvarchar(255)")
                                .HasColumnName("AddressState");

                            b1.HasKey("OrganizationId");

                            b1.ToTable("Organization");

                            b1.WithOwner()
                                .HasForeignKey("OrganizationId");
                        });

                    b.OwnsOne("SharedKernel.ValueObjects.OrganizationName", "Name", b1 =>
                        {
                            b1.Property<int>("OrganizationId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("Value")
                                .HasMaxLength(255)
                                .HasColumnType("nvarchar(255)")
                                .HasColumnName("Name");

                            b1.HasKey("OrganizationId");

                            b1.ToTable("Organization");

                            b1.WithOwner()
                                .HasForeignKey("OrganizationId");
                        });

                    b.Navigation("Address");

                    b.Navigation("Contact");

                    b.Navigation("Name");
                });

            modelBuilder.Entity("CustomerVehicleManagement.Domain.Entities.Person", b =>
                {
                    b.OwnsOne("SharedKernel.ValueObjects.Address", "Address", b1 =>
                        {
                            b1.Property<int>("PersonId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("AddressLine")
                                .HasMaxLength(255)
                                .HasColumnType("nvarchar(255)")
                                .HasColumnName("AddressLine");

                            b1.Property<string>("City")
                                .HasMaxLength(255)
                                .HasColumnType("nvarchar(255)")
                                .HasColumnName("AddressCity");

                            b1.Property<string>("PostalCode")
                                .HasMaxLength(15)
                                .HasColumnType("nvarchar(15)")
                                .HasColumnName("AddressPostalCode");

                            b1.Property<string>("State")
                                .HasMaxLength(255)
                                .HasColumnType("nvarchar(255)")
                                .HasColumnName("AddressState");

                            b1.HasKey("PersonId");

                            b1.ToTable("Person");

                            b1.WithOwner()
                                .HasForeignKey("PersonId");
                        });

                    b.OwnsOne("SharedKernel.ValueObjects.DriversLicense", "DriversLicense", b1 =>
                        {
                            b1.Property<int>("PersonId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("Number")
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("DriversLicenseNumber");

                            b1.Property<string>("State")
                                .HasMaxLength(2)
                                .HasColumnType("nvarchar(2)")
                                .HasColumnName("DriversLicenseState");

                            b1.HasKey("PersonId");

                            b1.ToTable("Person");

                            b1.WithOwner()
                                .HasForeignKey("PersonId");

                            b1.OwnsOne("SharedKernel.ValueObjects.DateTimeRange", "ValidRange", b2 =>
                                {
                                    b2.Property<int>("DriversLicensePersonId")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("int")
                                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                                    b2.Property<DateTime>("End")
                                        .HasColumnType("datetime2")
                                        .HasColumnName("DriversLicenseExpiry");

                                    b2.Property<DateTime>("Start")
                                        .HasColumnType("datetime2")
                                        .HasColumnName("DriversLicenseIssued");

                                    b2.HasKey("DriversLicensePersonId");

                                    b2.ToTable("Person");

                                    b2.WithOwner()
                                        .HasForeignKey("DriversLicensePersonId");
                                });

                            b1.Navigation("ValidRange");
                        });

                    b.OwnsOne("SharedKernel.ValueObjects.PersonName", "Name", b1 =>
                        {
                            b1.Property<int>("PersonId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasMaxLength(255)
                                .HasColumnType("nvarchar(255)")
                                .HasColumnName("FirstName");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasMaxLength(255)
                                .HasColumnType("nvarchar(255)")
                                .HasColumnName("LastName");

                            b1.Property<string>("MiddleName")
                                .HasMaxLength(255)
                                .HasColumnType("nvarchar(255)")
                                .HasColumnName("MiddleName");

                            b1.HasKey("PersonId");

                            b1.ToTable("Person");

                            b1.WithOwner()
                                .HasForeignKey("PersonId");
                        });

                    b.Navigation("Address");

                    b.Navigation("DriversLicense");

                    b.Navigation("Name");
                });

            modelBuilder.Entity("CustomerVehicleManagement.Domain.Entities.Phone", b =>
                {
                    b.HasOne("CustomerVehicleManagement.Domain.Entities.Organization", null)
                        .WithMany("Phones")
                        .HasForeignKey("OrganizationId");

                    b.HasOne("CustomerVehicleManagement.Domain.Entities.Person", null)
                        .WithMany("Phones")
                        .HasForeignKey("PersonId");
                });

            modelBuilder.Entity("CustomerVehicleManagement.Domain.Entities.Vehicle", b =>
                {
                    b.HasOne("CustomerVehicleManagement.Domain.Entities.Customer", "Customer")
                        .WithMany("Vehicles")
                        .HasForeignKey("CustomerId");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("CustomerVehicleManagement.Domain.Entities.Customer", b =>
                {
                    b.Navigation("Vehicles");
                });

            modelBuilder.Entity("CustomerVehicleManagement.Domain.Entities.Organization", b =>
                {
                    b.Navigation("Emails");

                    b.Navigation("Phones");
                });

            modelBuilder.Entity("CustomerVehicleManagement.Domain.Entities.Person", b =>
                {
                    b.Navigation("Emails");

                    b.Navigation("Phones");
                });
#pragma warning restore 612, 618
        }
    }
}
