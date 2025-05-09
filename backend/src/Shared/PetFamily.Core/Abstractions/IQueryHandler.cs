using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Core.Abstractions
{
    public interface IQueryHandler<TResponse, in TQuery> where TQuery : IQuery
    {
        public Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken = default);
    }

    public interface IQueryHandlerWithResult<TResponse, in TQuery> where TQuery : IQuery
    {
        public Task<Result<TResponse, ErrorsList>> Handle(TQuery query, CancellationToken cancellationToken = default);
    }
}
