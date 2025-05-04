using PetFamily.SharedKernel;

namespace PetFamily.Framework
{
    public class Envelope
    {
        public object? Result { get; }
        public ErrorsList? Errors { get; }
        public DateTime TimeGenerated { get; }

        private Envelope(object? result, ErrorsList? errors)
        {
            Result = result;
            Errors = errors;
            TimeGenerated = DateTime.Now;
        }

        public static Envelope Ok(object? result = null) =>
            new(result, null);

        public static Envelope Error(ErrorsList errors) =>
            new(null, errors);
    }
}
