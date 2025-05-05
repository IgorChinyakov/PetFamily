namespace PetFamily.Volunteers.Domain.PetsVO
{
    public record PetFile
    {
        public FilePath PathToStorage { get; }

        public PetFile(FilePath pathToStorage)
        {
            PathToStorage = pathToStorage;
        }
    }
}
