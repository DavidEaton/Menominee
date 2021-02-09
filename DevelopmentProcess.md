Policy files will be maintained in the \DevelopmentProcess folder of this solution repository, where they will act as standards for our development process.

The benefits of versioned policies are:

- All policy changes will go through the change control board, so everyone will be on board with the baseline, and when policy changes
  happen, they can be systematically communicated to all affected stakeholders.

- Reduce the waste of making and recalling process decisions over and over again.  We can recall a decision by consulting the
  documents rather than interrupting someone with a question.

- Reading the process documents will be a much faster way to get new hires up to speed than transmission by tribal knowledge.

- Continual refinement of the policy documents will help us become more conscious of the development process and help us think of ways
  it can be optimized and automated.



#### This application was designed with Domain Droven Design (DDD) principles in mind.

### Value Objects

Encapsulation is an important part of any domain class, value objects included. Encapsulation protects application invariants: you shouldn’t be able to instantiate a value object in an invalid state.

In practice it means that value objects are immutable, and public setters are not used. For example, methods like NewNumber on the Phone class return a NEW Phone object, rather than mutating the existing Phone object:

        public class Phone : ValueObject
        public Phone NewNumber(string newNumber)
        {
            return new Phone(newNumber, PhoneType);
        }
#### Collections of Value Objects
        /// This entity will act as a thin wrapper on top of Phone value object
        /// with just an identifier and a reference to the owning Organization.
        /// Necessary to enable searchable value object collection of phones.
        /// Although we do now have an additional entity in our domain model,
        /// we don’t ever have to expose it outside of the aggregate (Organization).

### [ApiController] Attribute
... makes the next two checks unnecessary :)
We used to have include these checks in MANY controller methods. No more!
            
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);