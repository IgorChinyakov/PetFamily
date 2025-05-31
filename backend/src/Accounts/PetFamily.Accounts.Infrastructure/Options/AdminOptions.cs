using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Infrastructure.Options
{
    public class AdminOptions
    {
        public const string ADMIN = "ADMIN";

        public string UserName { get; init; } = string.Empty;

        public string Email { get; init; } = string.Empty;

        public string Password { get; init; } = string.Empty;

        public string FirstName {  get; init; } = string.Empty;

        public string SecondName {  get; init; } = string.Empty;

        public string LastName {  get; init; } = string.Empty;
    }
}
