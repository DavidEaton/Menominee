using Client.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Client.Models
{
    public class Person
    {
        public Person()
        {
        }

        public Person(string lastName, string firstName, string middleName = null)
        {
            LastName = lastName;
            FirstName = firstName;
            MiddleName = string.IsNullOrWhiteSpace(middleName) ? null : middleName;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        [MinLength(1)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(255)]
        [MinLength(1)]
        public string FirstName { get; set; }

        [MaxLength(255)]
        [MinLength(1)]
        public string MiddleName { get; set; }
        public Address Address { get; set; }

        [StringLength(1)]
        public string Gender { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")] 
        public DateTime? Birthday { get; set; }

        [MaxLength(15)]
        [MinLength(10)]
        public string PhonePrimary { get; set; }

        [MaxLength(15)]
        [MinLength(10)]
        public string PhoneSecondary { get; set; }

        [MaxLength(255)]
        [MinLength(5)]
        public string EmailPrimary { get; set; }

        [MaxLength(255)]
        [MinLength(5)]
        public string EmailSecondary { get; set; }

        public string NameLastFirst
        {
            get
            {
                return string.IsNullOrWhiteSpace(MiddleName) ? $"{LastName}, {FirstName}" : $"{LastName}, {FirstName} {MiddleName}";
            }
        }
        public string NameFirstLast
        {
            get
            {
                return string.IsNullOrWhiteSpace(MiddleName) ? $"{FirstName} {LastName}" : $"{FirstName} {MiddleName} {LastName}";
            }
        }


        // REMOVABLE -------------------------------------------------------------------------------------------------------------------------------

        public CustomerType CustomerType { get; set; }
        public string DriversLicence { get; set; }
        public DateTime? DlExpires { get; set; }

        [StringLength(2)]
        public string DlState { get; set; }
    }
}
