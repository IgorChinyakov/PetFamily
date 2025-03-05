using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.Shared
{
    public record Error
    {
        public const string SEPARATOR = "||";
        public string Code { get; }
        public string Message { get; }
        public string? PropertyName { get; }
        public ErrorType Type { get; }

        private Error(string code, string message, ErrorType type, string? propertyName = null)
        {
            Code = code;
            Message = message;
            PropertyName = propertyName;
            Type = type;
        }

        public static Error Validation(string code, string message, string? propertyName = null)
            => new Error(code, message, ErrorType.Validation, propertyName);

        public static Error NotFound(string code, string message, string? propertyName)
            => new Error(code, message, ErrorType.NotFound, propertyName);

        public static Error Failure(string code, string message, string? propertyName)
            => new Error(code, message, ErrorType.Failure, propertyName);

        public static Error Conflict(string code, string message, string? propertyName)
            => new Error(code, message, ErrorType.Conflict, propertyName);

        public string Serialize()
            => string.Join(SEPARATOR, Code, Message, PropertyName, Type);

        public static Error Deserialize(string serialized)
        {
            var parts = serialized.Split(SEPARATOR);

            if (parts.Length < 4)
            {
                throw new ArgumentException("Invalid serialized format");
            }

            if (Enum.TryParse<ErrorType>(parts[parts.Length - 1], out var type) == false)
            {
                throw new ArgumentException("Invalid serialized format");
            }

            return new Error(parts[0], parts[1], type, parts[2]);
        }

        public ErrorsList ToErrorsList() => new([this]);
    }

    public enum ErrorType
    {
        Validation,
        NotFound,
        Failure,
        Conflict
    }
}

