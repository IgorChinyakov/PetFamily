using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.SharedKernel
{
    public class ErrorsList : IEnumerable<Error>
    {
        private readonly List<Error> _errors;

        public ErrorsList(IEnumerable<Error> errors)
        {
            _errors = errors.ToList();
        }

        public IEnumerator<Error> GetEnumerator()
        {
            return _errors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public static implicit operator ErrorsList(List<Error> errors)
            => new(errors);

        public static implicit operator ErrorsList(Error error)
            => new([error]);
    }
}
