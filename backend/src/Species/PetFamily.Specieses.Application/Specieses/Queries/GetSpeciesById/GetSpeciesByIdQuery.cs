using PetFamily.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Specieses.Application.Specieses.Queries.GetSpeciesById
{
    public record GetSpeciesByIdQuery(Guid Id) : IQuery;
}
