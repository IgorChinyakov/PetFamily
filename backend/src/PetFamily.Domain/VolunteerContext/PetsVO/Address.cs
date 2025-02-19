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
        public string House { get; }

        private Address(string city, string street, string house)
        {
            City = city;
            Street = street;
            House = house;
        }

        public static Result<Address> Create(string city, string street, string house)
        {
            if (string.IsNullOrWhiteSpace(city))
                return Result.Failure<Address>("City is not supposed to be empty");
            if (string.IsNullOrWhiteSpace(street))
                return Result.Failure<Address>("Street is not supposed to be empty");
            if (string.IsNullOrWhiteSpace(house))
                return Result.Failure<Address>("House is not supposed to be empty");

            return new Address(city, street, house);
        }
    }
}
