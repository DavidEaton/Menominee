# Coding Standards & Guidelines

- [HOME](ReadMe.md)

Our Development Process policies are described here, and will act as standards for our application design guidance.

The benefits of versioned policies are:

- All policy changes will go through code review so everyone will be on board with the baseline, and when policy changes happen, they can be systematically communicated to all affected stakeholders.

- Reduce the waste of making and recalling process decisions over and over again.  We can recall a decision by consulting the documents rather than interrupting someone with a question.

- Reading the process documents will be a much faster way to get new hires up to speed than transmission by tribal knowledge.

- Continual refinement of the policy documents will help us become more conscious of the development process and help us think of ways it can be optimized and automated.

## <a href="https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/naming-guidelines">Naming Standards</a>
This application follows <a href="https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/">Microsoft Framework Design Guidelines for .NET</a>

To differentiate words in an identifier, capitalize the first letter of each word in the identifier. Do not use underscores to differentiate words, or for that matter, anywhere in identifiers. There are two appropriate ways to capitalize identifiers, depending on the use of the identifier:

PascalCasing

camelCasing

The PascalCasing convention, used for all identifiers except parameter names, capitalizes the first character of each word (including acronyms over two letters in length), as shown in the following examples:

PropertyDescriptor HtmlTag

A special case is made for two-letter acronyms in which both letters are capitalized, as shown in the following identifier:

IOStream

The camelCasing convention, used only for parameter names, capitalizes the first character of each word except the first word, as shown in the following examples. As the example also shows, two-letter acronyms that begin a camel-cased identifier are both lowercase.

propertyDescriptor ioStream htmlTag

✔️ DO use PascalCasing for all public member, type, and namespace names consisting of multiple words.

✔️ DO use camelCasing for parameter names and private members.

### <a href="https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/general-naming-conventions">General Naming Conventions</a>
#### Word Choice
✔️ DO choose easily readable identifier names.

For example, a property named `HorizontalAlignment` is more English-readable than `AlignmentHorizontal`.

✔️ DO favor readability over brevity.

The property name `CanScrollHorizontally` is better than `ScrollableX` (an obscure reference to the X-axis).

❌ DO NOT use underscores, hyphens, or any other nonalphanumeric characters.

❌ DO NOT use <a href="https://en.wikipedia.org/wiki/Hungarian_notation">Hungarian notation</a>.

❌ AVOID using identifiers that conflict with keywords of widely used programming languages.

#### Using Abbreviations and Acronyms
❌ DO NOT use abbreviations or contractions as part of identifier names.

For example, use `GetWindow` rather than `GetWin`.

❌ DO NOT use any acronyms that are not widely accepted, and even if they are, only when necessary. This application uses the widely accepted abbreviation `Dto` for `Data Transfer Object`.

#### Avoiding Language-Specific Names
✔️ DO use semantically interesting names rather than language-specific keywords for type names.

For example, `GetLength` is a better name than `GetInt`.

✔️ DO use a generic CLR type name, rather than a language-specific name, in the rare cases when an identifier has no semantic meaning beyond its type.

For example, a method converting to Int64 should be named `ToInt64`, not `ToLong` (because Int64 is a CLR name for the C#-specific alias `long`).

✔️ DO use a common name, such as `value` or `item`, rather than repeating the type name, in the rare cases when an identifier has no semantic meaning and the type of the parameter is not important.

## This application design is guided by <a href="https://martinfowler.com/tags/domain%20driven%20design.html">Domain Driven Design (DDD)</a> principles.

The Domain Model is the heart of our software, the place for all domain logic and knowledge that make up the competitive advantage of our company. This is where we focus most of our efforts, keeping it fully encapsulated, covered well by tests, and refactored as often as needed to adapt to changing requirements. It’s the space where we are sure that all data remains consistent and no invariants are violated. We adhere to the Always-Valid Domain Model philosophy. 
## <a href="https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/domain-model-layer-validations">Design validation into the domain model layer</a>
In DDD, validation rules can be thought as invariants. The main responsibility of an aggregate is to enforce invariants across state changes for all the entities within that aggregate. For example, the Business domain aggregate class enforces the business rule, "Business must have a Name", via the Business.Create factory method. Each domain class enforces its invariants in its Create factory method.
### Implement validations in the domain model layer
Validations are implemented in domain entity constructors or in methods that can update the entity. 

Domain model methods are kept honest by returning a result object rather than throwing exceptions. As a result, it's easier to deal with validation errors, and track down bugs when they do creep in.

We use field-level validation on our command and query Data Transfer Objects (DTOs), and domain-level validation inside our entities and value objects. We do this by adding the FluentValidation library to the ASP.NET processing pipeline in our api, and FluentValidation integrates with our value objects and entities. Controllers can therefore become much lighter, focused, readable and maintainable, removing all validation responsibility out of controllers and into the domain model where they belong.


### Value Objects

Encapsulation is an important part of any domain class, value objects included. Encapsulation protects application invariants: you shouldn’t be able to instantiate a value object in an invalid state (Always-Valid domain model).

In practice it means that value objects are immutable, and public setters are not used. For example, methods like NewNumber on the Phone class return a NEW Phone object, rather than mutating the existing Phone object:

        public class Phone : ValueObject
        public Phone NewNumber(string newNumber)
        {
            return new Phone(newNumber, PhoneType);
        }
#### Collections of Value Objects
        /// This entity will act as a thin wrapper on top of Phone value object
        /// with just an identifier and a reference to the owning Business.
        /// Necessary to enable searchable value object collection of phones.
        /// Although we do now have an additional entity in our domain model,
        /// we don’t ever have to expose it outside of the aggregate (Business).

### [ApiController] Attribute
... makes the next two checks in Controllers unnecessary. 
We used to have to include these checks in MANY controller methods. No more!
            
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

### Lazy Loading of Relationships/Navigation Properties
Pros:
Helps avoid partially initialized entities (which can lead to subtle bugs), so that there are no invariant violations of domain entities
More performant in some scenarios
Code simplicity (no need to remember to load explicitly via Include statements)

Cons: Possible to introduce the N+1 problem which is bad for performance. For a given page view, there should be only one database trip, not N+1.

N+1 problem ONLY HAPPENS IN READS.

So, adhere to CQRS Pattern, don't use ORM or domain model in reads. Instead of ORM reads, use plain sql or micro-ORM such as Dapper.


            return await context.Businesses
                // Tracking is not needed (and expensive) for this disconnected data collection
                // Lazy-loading is not supported for detached entities or entities that are loaded with 'AsNoTracking'.
                .AsNoTracking()
                .Include(o => o.Address)
                .Include(o => o.Contact)
                .ToArrayAsync();

### Testing
Framework of choice is <a href="https://xunit.net/">xunit</a>. With it we use <a href="https://fluentassertions.com/">Fluent Assertions</a>, which:

<ul>
<li>Improves readability of tests</li>
</ul>

                // The old way:
                // Assert.That(customer.Entity is Person);

                // The Fluent way:
                customer.Entity.Should().BeOfType<Person>();

<ul>
<li>Improves readability of test failure messages</li>
</ul>

            // If we change the test to force a failure (changed Person type to Email in the test):
            // standard output
            Assert.That(customer.Entity is Email);
            Message: 
                  Expected: True
                  But was:  False

            // Fluent output
            customer.Entity.Should().BeOfType<Email>();
            Message: 
                Expected type to be Menominee.Domain.Entities.Email, but found Menominee.Domain.Entities.Person.

            // As you may have noticed, the fluent message reveals enough info to quickly determine the cause of the failed test; without fluent assertions the message is vague and may require debugging to determine the cause.

<ul>
<li>Makes it harder to confuse expected and actual results</li>
</ul>

Test projects include unit tests, integration tests.

### Code Readability and Comments

Code should be as self-documenting as possible, and we strive to write clear, focused code that is human-readable, and attempts to adhere to the Single Responsibility Principle.
That said, some comments have been included in our source code for further explanation. We've tried to limit those comments to the Business class, its Data Transfer Objects (DTOs), repository, controller, which contain comments that are applicable to most other domain types and their related classes.