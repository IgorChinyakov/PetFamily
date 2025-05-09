using PetFamily.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Specieses.Application.Breeds.Queries.GetBreedById
{
    public record GetBreedByIdQuery(Guid SpeciesId, Guid BreedId) : IQuery;
}
