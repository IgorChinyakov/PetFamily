using PetFamily.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Volunteers.Application.Pets.Queries.GetById
{
    public record GetPetByIdQuery(Guid PetId) : IQuery;
}
