using System.Collections.Generic;

namespace SharedKernel.ValueObjects
{
    public class State : ValueObject
    {
        public State(string abbreviation, string name)
        {
            Name = name;
            Abbreviation = abbreviation;
        }

        public string Name { get; }
        public string Abbreviation { get; }

        public override string ToString()
        {
            return $"{Abbreviation} - {Name}";
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return Abbreviation;
        }
    }
}
