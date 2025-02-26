using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.Pets.Value_objects
{
    public record Address
    {
        public const int MAX_LENGTH = 50;

        public string City { get; }
        public string Street { get; }
        public string Apartment { get; }

        private Address(string city, string street, string apartment)
        {
            City = city;
            Street = street;
            Apartment = apartment;
        }

        public static Result<Address, Error> Create(string city, string street, string apartment)
        {
            if (string.IsNullOrWhiteSpace(city) || city.Length > MAX_LENGTH)
                return Errors.General.ValueIsInvalid("City");
            if (string.IsNullOrWhiteSpace(street) || street.Length > MAX_LENGTH)
                return Errors.General.ValueIsInvalid("Street");
            if (string.IsNullOrWhiteSpace(apartment) || apartment.Length > MAX_LENGTH)
                return Errors.General.ValueIsInvalid("Apartment");

            return new Address(city, street, apartment);
        }
    }
}
