using Menominee.Domain.Entities;
using Menominee.Domain.Entities.RepairOrders;
using Menominee.Shared.Models.Persons;
using Menominee.Shared.Models.Persons.PersonNames;
using System.Collections.Generic;
using System.Linq;

namespace Menominee.Shared.Models.RepairOrders.Techs
{
    public class TechnicianHelper
    {
        public static IList<RepairOrderServiceTechnicianToRead> ConvertToReadDtos(IReadOnlyList<RepairOrderServiceTechnician> technicians)
        {
            return technicians?.Select(
                technician =>
                new RepairOrderServiceTechnicianToRead()
                {
                    Id = technician.Id,
                    Employee =
                        new EmployeeToRead()
                        {
                            Id = technician.Employee.Id,
                            PersonalDetails = PersonHelper.ConvertToReadDto(technician.Employee.PersonalDetails),
                            Hired = technician.Employee.Hired,
                            Exited = technician.Employee.Exited,

                            //RoleAssignments = technician.Employee.RoleAssignments.Select(
                            //    role => RoleAssignment.Create(role).Value
                            //    )
                        }
                }).ToList()
            ?? new List<RepairOrderServiceTechnicianToRead>();
        }

        public static List<RepairOrderServiceTechnicianToWrite> CovertReadToWriteDtos(IList<RepairOrderServiceTechnicianToRead> techs)
        {
            {
                return techs?.Select(
                    technician =>
                    new RepairOrderServiceTechnicianToWrite()
                    {
                        Id = technician.Id,
                        Employee = new Techs.EmployeeToRead()
                        {
                            PersonalDetails = new PersonToRead()
                            {
                                Name = new PersonNameToRead()
                                {
                                    FirstName = technician.Employee.PersonalDetails.Name.FirstName,
                                    LastName = technician.Employee.PersonalDetails.Name.LastName,
                                    MiddleName = technician.Employee.PersonalDetails.Name?.MiddleName
                                },
                                Gender = technician.Employee.PersonalDetails.Gender
                            },
                            Hired = technician.Employee.Hired,
                            Exited = technician.Employee.Exited
                        }
                    }).ToList()
                ?? new List<RepairOrderServiceTechnicianToWrite>();
            }
        }

        internal static List<RepairOrderServiceTechnician> ConvertWriteDtosToEntities(List<RepairOrderServiceTechnicianToWrite> technicians, List<Employee> employees)
        {
            return technicians?.Select(
                technician =>
                RepairOrderServiceTechnician.Create(
                    employees.Find(x => x.Id == technician.Employee.Id))
                .Value
                ).ToList()
            ?? new List<RepairOrderServiceTechnician>();
        }

        internal static List<RepairOrderServiceTechnicianToWrite> CovertToWriteDtos(IReadOnlyList<RepairOrderServiceTechnician> technicians)
        {
            return technicians?.Select(
                technician =>
                new RepairOrderServiceTechnicianToWrite()
                {
                    Id = technician.Id,
                    Employee = new EmployeeToRead()
                    {
                        PersonalDetails = new PersonToRead()
                        {
                            Name = new PersonNameToRead()
                            {
                                FirstName = technician.Employee.PersonalDetails.Name.FirstName,
                                LastName = technician.Employee.PersonalDetails.Name.LastName,
                                MiddleName = technician.Employee.PersonalDetails.Name?.MiddleName
                            },
                            Gender = technician.Employee.PersonalDetails.Gender
                        },
                        Hired = technician.Employee.Hired,
                        Exited = technician.Employee.Exited
                    }
                }).ToList()
            ?? new List<RepairOrderServiceTechnicianToWrite>();
        }
    }
}
