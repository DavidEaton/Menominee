using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using CustomerVehicleManagement.Shared.Models.Persons;
using CustomerVehicleManagement.Shared.Models.Persons.PersonNames;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Techs
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

    }
}
