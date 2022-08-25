<h3 style="color:#00bfff">Implementation Checklist</h3>

#### Please use this guidance when implementing features.

<h3 style="color:#00bfff">Gather requirements of the feature</h3>

Al knows all requirements and defines the business rules for each.
Define the domain classes, informed by the requirements definitions Al provides (e.g. ExciseFee domain class property types and  constraints).

<h3 style="color:#00bfff">Encapsulate the Domain Model Classes</h3>

For each domain class, encapsulate its data with private setters on all properties. For example, ExciseFee properties:

        public string Description { get; private set; }
        public ExciseFeeType FeeType { get; private set; }
        public double Amount { get; private set; }

   
Expose a public **Create** factory method that defines the domain class invariants, and validates them. If all invariants are satisfied, method calls its private constructor, and then returns a success result instance conataining a valid object, (or a failure result if validation fails):


        public static Result<ExciseFee> Create(
            string description,
            ExciseFeeType feeType,
            double amount)
        {
            if (description is null)
                return Result.Failure<ExciseFee>(RequiredMessage);

            if (!Enum.IsDefined(typeof(ExciseFeeType), feeType))
                return Result.Failure<ExciseFee>(InvalidMessage);

            if (amount < MinimumValue)
                return Result.Failure<ExciseFee>(MinimumValueMessage);

            description = (description ?? string.Empty).Trim();

            if (description.Length > DescriptionMaximumLength)
                return Result.Failure<ExciseFee>
                    (DescriptionMaximumLengthMessage);

            return Result.Success(
                new ExciseFee(description, feeType, amount));
        }  

Define public setter methods that validate the incoming data for each property. If data fails the validation check inside the Set method, throw an exception, signaling a bug; we somehow  failed to filter out invalid data in our Validator class (more on Validation classes later in this document): 

        public void SetFeeType(ExciseFeeType feeType)
        {
            if (Enum.IsDefined(typeof(ExciseFeeType), feeType))
                throw new ArgumentOutOfRangeException(RequiredMessage);

            FeeType = feeType;
        }

        public void SetAmount(double amount)
        {
            if (amount < MinimumValue)
                throw new ArgumentOutOfRangeException(MinimumValueMessage);

            Amount = amount;
        }
        ...

The outcome is an encapsulated class with properties that can only be modified thru its public Set methods, and can only be instantiated when all of its invariants are satisfied. Objects of the class will always be valid when created or modified; no caller can mutate the object without those changes being validated with the domain class invariants. We have therefore greatly reduced the surface area where bugs can slip through. 

<h3 style="color:#00bfff">Create unit tests for each domain class</h3>

Test class name + test name should tell a story. For example, ExciseFee domain class unit test class is named ExciseFeeShould.cs.  

ExciseFeeShould.cs tests:  
	CreateExciseFee  
	Not_Create_ExciseFee_With_Invalid_Description  
	Not_Create_ExciseFee_With_Null_Description  
	Not_Create_ExciseFee_With_Invalid_Order  
	Not_Create_ExciseFee_With_Invalid_TaxIdNumber  
	Not_Create_ExciseFee_With_Invalid_PartTaxRate  
	Not_Create_ExciseFee_With_Invalid_LaborTaxRate  
	Not_Create_ExciseFee_With_Invalid_SalesTaxes  

So one story from the ExciseFee unit tests: ExciseFeeShould.Not_Create_ExciseFee_With_Invalid_Description.  

Create a test for every possible condition for each domain class invariant.


<h3 style="color:#00bfff">Create an EntityConfiguration class for each domain class</h3>

...to define the database constraints, using the domain class invariants to inform those constraints. For example, ExciseFeeConfiguration inherits from

	EntityConfiguration<T>

Build the database constraintes to use the domain class invariant rules. For example, ExciseFee.cs defines the following invariants:  

            if (description is null)
                return ...

            if (!Enum.IsDefined(typeof(ExciseFeeType), feeType))
                return...

            if (amount < MinimumValue)
                return...

            if (description.Length > DescriptionMaximumLength)
                return...

...so the configration file tells the database to create those contraints:

            base.Configure(builder);
            builder.ToTable("ExciseFee", "dbo");

            builder.Property(fee => fee.Description)
                .IsRequired()
                .HasMaxLength(10000);
            builder.Property(fee => fee.FeeType)
                .IsRequired()
                .HasDefaultValue(ExciseFeeType.Flat);
            builder.Property(fee => fee.Amount)
                .HasDefaultValue(0);

<h3 style="color:#00bfff">Create a database migration</h3>

...if any domain class refactorings require a migration.

#### TODO: Unify database migrations with TenantManager sql scripts

Update API Startup.cs ConfigureServices to use the migrations database connection:


            if (HostEnvironment.IsDevelopment())
            {
                AddControllersWithOptions(services, false);

                services.AddDbContext<ApplicationDbContext>
                    (options => options
                        .UseSqlServer(
                            Configuration[$"DatabaseSettings:MigrationsConnection"]));


In ApplicationDbContext.OnConfiguring(), comment all but last line:  

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //if (UserContext != null) // Unit tests do not yet inject UserContext
            //    Connection = GetTenantConnection();

            //if (!options.IsConfigured) // Unit tests will configure context with test provider
            //{
            //    if (Environment?.EnvironmentName != "Production")
            //    {
            //        options.UseLoggerFactory(CreateLoggerFactory());
            //        options.EnableSensitiveDataLogging(true);
            //    }

            //    options.UseSqlServer(Connection);
            //}

            base.OnConfiguring(options);

        }

Run in Package Manager console:  

            add - migration Initial - Context CustomerVehicleManagement.Api.Data.ApplicationDbContext
            update - database - Context CustomerVehicleManagement.Api.Data.ApplicationDbContext

If updating the Users database in IdentityServer4:

            Script - Migration - Idempotent - Context Janco.Idp.Data.Contexts.UserDbContext

Create idempotent script for tenants/Tenant Manager:  

            Script - Migration - Idempotent - Context CustomerVehicleManagement
                .Api.Data.ApplicationDbContext


<h3 style="color:#00bfff">Create data contracts, aka Data Transfer Objects (DTOs) for each domain class</h3>

With Data Contracts we can refactor a domain class without breaking clients that rely on the existing api, and provide the correct data shape for incoming and outgoing requests, which have different requirements, neither of which is correctly represented by the domain class.  
Data contracts can hide sensitive information, and they contain only data, no logic, validations or methods. For incoming (write) requests, we name our contracts using "[ClassName]ToWrite", and for outgoing (reads) it's "[ClassName]ToRead". For example, ExciseFee domain class DTOs:

ExciseFeeToWrite  
ExciseFeeToRead  
ExciseFeeToReadInList  

"InList" contracts are used for lookups/dropdowns, and omit child collections, which would make no sense in a dropdown, but damage performance.

<h3 style="color:#00bfff">Create a Validator for each Data Contract</h3>

Validator classes inherit from AbstractValidator in the FluentValidation package. Create a validator class for each domain class "ToWrite" data contract. Reads do not require validation; values from the database were validated before ingress. 
	
We define our property constraints inside each domain class private constructor. That's where the transformation (e.g., name.Trim()) and parsing (e.g., if (name is null)) of values from the outside world happens. 

FluentValidation classes invoke domain class Create factory methods to validate those values from the outside world, within the ASP.NET request pipeline, before a request even hits a controller endpoint:  

    public class VendorValidator : AbstractValidator<VendorToWrite>
    {
        public VendorValidator()
        {
            RuleFor(vendor => vendor)
                .MustBeEntity(
                    vendor => Vendor.Create(
                        vendor.Name,
                        vendor.VendorCode));
        }
    }

So no need to duplicate those rules inside controllers because the domain class invariants already get validated when the Create method is invoked by its data contract data Validator class. Therefore all (more likely all but a few edge cases, practically speaking) of our business rules can live in the domain classes where they belong (accoring to DDD guidance).

Controllers look like they aren't checking much because the ASP.NET request pipeline has already invoked the domain class validators.

The only other place me must define constraints is in our persistence layer: the EntityConfiguration<T> classes in the API project. Our domain class invariants are the single source of truth containing our business rules. So our domain class invariants must be used to inform the EntityConfiguration<T> classes when we create them. 

<h3 style="color:#00bfff">Create an API Repository Interface for each domain class</h3>

...named I[DomainClass]Repository. For example, IExciseFeeRepository.  

Define interface method signatures/contract api. 
For example: 

    public interface IExciseFeeRepository
    {
        Task AddExciseFeeAsync(ExciseFee entity);
        Task<ExciseFee> GetExciseFeeEntityAsync(long id);
        Task<ExciseFeeToRead> GetExciseFeeAsync(long id);
        Task<IReadOnlyList<ExciseFeeToReadInList>> GetExciseFeeListAsync();
        void DeleteExciseFee(ExciseFee entity);
        Task<bool> ExciseFeeExistsAsync(long id);
        Task SaveChangesAsync();
        void FixTrackingState();
    }

Implement repository in concrete class:  

    public class ExciseFeeRepository : IExciseFeeRepository
    {
        private readonly ApplicationDbContext context;
        ...

Implement methods to Add, Delete, Exists, Get, Update, SaveChanges, FixTrackingState. In this disconnected web application, all reads via EF Core should disable the change tracker, except for reads of objects inside controller Update methods, which DO need the change tracker.  
Reads with child collections must include .AsSplitQuery() to prevent performance degradation; without .AsSplitQuery(), emitted sql from Entity Framework selects a cartesian product for each child type when we only need to add a single sql result for each child type. See OrganizationsController for implementation guidance.

<h3 style="color:#00bfff">Create helper methods to convert domain class data contract classes from each contract type</h3>

...named after/prefixed by the domain class. For example, ExciseFee**Helper**.cs. Helper methods unclutter our controllers and are reused by integration tests, reducing code duplication and maintenance.

<h4 style="color:#00bfff">Data Contract Helper Method Naming</h4>

|Entity|Read|Write|Method|Inverted|
|---:|---:| ---:|
|0|1|1|CovertReadToWriteDto|CoverWriteToReadDto|
|1|0|1|ConvertEntityToWriteDto|ConvertWriteDtoToEntity|
|1|1|0|ConvertEntityToReadDto|ConvertReadDtoToEntity|

Results in six possible methods named:  
CovertReadToWriteDto  
CoverWriteToReadDto  
ConvertEntityToWriteDto  
ConvertWriteDtoToEntity  
ConvertEntityToReadDto  
ConvertReadDtoToEntity  

When dto convert methods in a helper class convert child classes, and no helper class exists for those child classes, methods names of varying types may collide. If so, the fix is to include the Entity type name. For example:  
Convert**Payment**ReadToWriteDto()  
Convert**Purchase**ReadToWriteDto()  

For methods that convert collections, same naming convention but pluralize using "Dtos" and "Entities" (if it appears):  

ConvertReadToWriteDto**s**  
ConvertEntit**ies**ToWriteDto**s**  
ConvertPurchase**s**ReadToWriteDto**s**  


#### Controllers
 Refer to OrganizationsController.cs for implementation guidance.  
#### Register interface and repository with API dependency injection (DI) container.
#### Add DBSets to AppDbContext
#### Test API with Postman  
#### Test validators with Postman  
#### Create integration tests
