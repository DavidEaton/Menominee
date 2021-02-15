using SharedKernel.Utilities;
using System;
using System.Collections.Generic;

namespace SharedKernel.ValueObjects
{
    public class State : ValueObject
    {
        public static readonly string StateInvalidMessage = "Drivers License details cannot be empty";
        public State(string abbreviation, string name)
        {
            try
            {
                Guard.ForNullOrEmpty(abbreviation, "abbreviation");
                Guard.ForNullOrEmpty(name, "name");
            }
            catch (Exception)
            {
                throw new ArgumentException(StateInvalidMessage);
            }

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
