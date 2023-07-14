﻿using Menominee.Domain.Entities.Inventory;

namespace Menominee.Shared.Models.Manufacturers
{
    public class ManufacturerHelper
    {
        public static ManufacturerToWrite ConvertReadToWriteDto(ManufacturerToRead manufacturer)
        {
            return
            manufacturer is null
                ? null
                : new ManufacturerToWrite()
                {
                    Id = manufacturer.Id,
                    Code = manufacturer.Code,
                    Prefix = manufacturer.Prefix,
                    Name = manufacturer.Name
                };
        }

        public static Manufacturer ConvertWriteDtoToEntity(ManufacturerToWrite manufacturer)
        {
            return
            manufacturer is null
                ? null
                : Manufacturer.Create(
                    manufacturer.Name,
                    manufacturer.Prefix,
                    manufacturer.Code)
                .Value;
        }

        public static ManufacturerToRead ConvertToReadDto(Manufacturer manufacturer)
        {
            return
            manufacturer is null
                ? null
                : new()
                {
                    Id = manufacturer.Id,
                    Code = manufacturer.Code,
                    Prefix = manufacturer.Prefix,
                    Name = manufacturer.Name
                };
        }

        public static ManufacturerToReadInList ConvertToReadInListDto(Manufacturer manufacturer)
        {
            return
            manufacturer is null
                ? null
                : new()
                {
                    Id = manufacturer.Id,
                    Code = manufacturer.Code,
                    Prefix = manufacturer.Prefix,
                    Name = manufacturer.Name
                };
        }

        public static ManufacturerToRead ConvertReadInListToReadDto(ManufacturerToReadInList manufacturer)
        {
            return
            manufacturer is null
                ? null
                : new()
                {
                    Id = manufacturer.Id,
                    Code = manufacturer.Code,
                    Prefix = manufacturer.Prefix,
                    Name = manufacturer.Name
                };
        }

        public static ManufacturerToRead ConvertToWriteDto(Manufacturer manufacturer)
        {
            return
            manufacturer is null
                ? null
                : new()
                {
                    Id = manufacturer.Id,
                    Code = manufacturer.Code,
                    Prefix = manufacturer.Prefix,
                    Name = manufacturer.Name
                };
        }
    }
}