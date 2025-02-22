using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.Pets.Value_objects
{
    public record Address
    {
        public string City { get; }
        public string Street { get; }
        public string Apartment { get; }

        private Address(string city, string street, string apartment)
        {
            City = city;
            Street = street;
            Apartment = apartment;
        }

        public static Result<Address> Create(string city, string street, string apartment)
        {
            if (string.IsNullOrWhiteSpace(city))
                return Result.Failure<Address>("City is not supposed to be empty");
            if (string.IsNullOrWhiteSpace(street))
                return Result.Failure<Address>("Street is not supposed to be empty");
            if (string.IsNullOrWhiteSpace(apartment))
                return Result.Failure<Address>("Apartment is not supposed to be empty");

            return new Address(city, street, apartment);
        }
    }
}
