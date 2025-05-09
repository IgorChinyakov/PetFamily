using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Core.Abstractions
{
    public interface ICommandHandler<TResponse, in TCommand> where TCommand : ICommand
    {
        public Task<Result<TResponse, ErrorsList>> Handle(TCommand command, CancellationToken cancellationToken = default);
    }

    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        public Task<UnitResult<ErrorsList>> Handle(TCommand command, CancellationToken cancellationToken = default);
    }
}
