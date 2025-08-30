using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Core.Options
{
    public class RabbitMQOptions
    {
        public const string RABBIT_MQ = "RabbitMQ";

        public string Host { get; init; } = string.Empty;

        public string UserName { get; init; } = string.Empty;

        public string Password { get; init; } = string.Empty;
    }
}
