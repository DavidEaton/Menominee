### Implementation Checklist

Here's some guidance on how we implement features, what happens when and how. I think to formalize the process, we start with what we've been doing that works:

#### Gather requirements of the feature. 
Al already knows requirements and defines the business rules for each.
Define the domain classes, informed by the requirements definitions Al provides (e.g. ExciseFee domain class property constraints).

For each domain class:
	Encapsulate its data with private setters on all properties, private constructor.
	Define its invariants inside a private constructor.
	Expose a Create factory method that calls its private constructor.
	Define public setter methods for each property.
	The result is an encapsulated class with properties that can only be modified thru its public Set methods, and it can only be instantiated when all of its invariants are satisfied. Objects created from the class will always be valid when created or modified; no caller can change the object without those changes being validated wit hthe domain class invariants. 

Create unit tests for each domain class.
For example, ExciseFee domain class unit test class: ExciseFeeShould.
ExciseFeeShould.cs tests:
	CreateExciseFee
	NotCreateExciseFeeWithInvalidDescription
	NotCreateExciseFeeWithNullDescription
	NotCreateExciseFeeWithInvalidOrder
	NotCreateExciseFeeWithInvalidTaxIdNumber
	NotCreateExciseFeeWithInvalidPartTaxRate
	NotCreateExciseFeeWithInvalidLaborTaxRate
	NotCreateExciseFeeWithInvalidSalesTaxes

Create data contracts, aka Data Transfer Objects (DTOs). For example, ExciseFee domain class DTOs:
	ExciseFeeToRead
	ExciseFeeToReadInList (if used for lookups/dropdowns)
	ExciseFeeToWrite

Create helper methods to create domain class data contract classes from each contract type:
ExciseFeeHelper
	ExciseFee CreateExciseFee(ExciseFeeToWrite exciseFee)
	ExciseFeeToWrite CreateExciseFee(ExciseFeeToRead exciseFee)
	EciseFeeToRead CreateExciseFee(ExciseFee exciseFee)
	ExciseFeeToReadInList CreateExciseFeeInList(ExciseFee excisefee)
	List<ExciseFeeToRead> CreateExciseFees(List<ExciseFee> excisefees)
	List<ExciseFee> CreateExciseFees(List<ExciseFeeToWrite> exciseFees)

	Create an EntityConfiguration<T> class for each domain class to define the database constraints using the domain class invariants to inform those constraints.
	Validator classes inherit from AbstractValidator<T>, ONE FOR EACH DATA CONTRACT.
	
We define our property constraints inside each domain class private constructor. That's where the transformation (e.g., name.Trim()) parsing (e.g., if (name is null)) of values from the outside world happens. 

FluentValidation classes invoke domain class Create factory methods to validate those values from the outside world, within the ASP.NET request pipeline, before a request even hits a controller endpoint.

So no need to duplicate those rules inside controllers because the domain class invariants already get validated when the Create method gets invoked by its Validator class. Therefore all (more likely all but a few edge cases, practically speaking) of our business rules can live in the domain classes where they belong.

Controllers look like they aren't checking much because the ASP.NET request pipeline has already invoked the domain class validators.

The only other place me must define constraints is in our persistence layer: the EntityConfiguration<T> classes in the API project. Our domain class invariants are the single source of truth containing our business rules. So our domain class invariants should inform the EntityConfiguration<T> classes when we create them. 

Create IExciseFeeRepository interface for domain aggregate root: I{DomainClass}Repository.
Define interface method signatures/contract api
Example: IExciseFeeRepository 
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

Implement repository in concrete class.
Example: ExciseFeeRepository
	public class ExciseFeeRepository : IExciseFeeRepository
	Implement methods to Add, Delete, Exists, Get, Update, SaveChanges, FixTrackingState.

Register interface and repository with API dependency injection (DI) container.

Add DBSets to AppDbContext

Test API with Postman
Test validators with Postman
Create integration tests
