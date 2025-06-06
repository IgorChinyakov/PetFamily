using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.Accounts.RefreshTokens
{
    public record RefreshTokensCommand(string AccessToken, Guid RefreshToken) : ICommand;
}
