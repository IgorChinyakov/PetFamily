# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/PetFamily.Web/PetFamily.Web.csproj", "src/PetFamily.Web/"]
COPY ["src/Accounts/PetFamily.Accounts.Application/PetFamily.Accounts.Application.csproj", "src/Accounts/PetFamily.Accounts.Application/"]
COPY ["src/Shared/PetFamily.Core/PetFamily.Core.csproj", "src/Shared/PetFamily.Core/"]
COPY ["src/Files/PetFamily.Files.Contracts/PetFamily.Files.Contracts.csproj", "src/Files/PetFamily.Files.Contracts/"]
COPY ["src/Shared/PetFamily.SharedKernel/PetFamily.SharedKernel.csproj", "src/Shared/PetFamily.SharedKernel/"]
COPY ["src/Accounts/PetFamily.Accounts.Contracts/PetFamily.Accounts.Contracts.csproj", "src/Accounts/PetFamily.Accounts.Contracts/"]
COPY ["src/Accounts/PetFamily.Accounts.Domain/PetFamily.Accounts.Domain.csproj", "src/Accounts/PetFamily.Accounts.Domain/"]
COPY ["src/Accounts/PetFamily.Accounts.Infrastructure/PetFamily.Accounts.Infrastructure.csproj", "src/Accounts/PetFamily.Accounts.Infrastructure/"]
COPY ["src/Accounts/PetFamily.Accounts.Presentation/PetFamily.Accounts.Presentation.csproj", "src/Accounts/PetFamily.Accounts.Presentation/"]
COPY ["src/Shared/PetFamily.Framework/PetFamily.Framework.csproj", "src/Shared/PetFamily.Framework/"]
COPY ["src/Discussions/PetFamily.Discussions.Application/PetFamily.Discussions.Application.csproj", "src/Discussions/PetFamily.Discussions.Application/"]
COPY ["src/Discussions/PetFamily.Discussions.Contracts/PetFamily.Discussions.Contracts.csproj", "src/Discussions/PetFamily.Discussions.Contracts/"]
COPY ["src/Discussions/PetFamily.Discussions.Domain/PetFamily.Discussions.Domain.csproj", "src/Discussions/PetFamily.Discussions.Domain/"]
COPY ["src/Discussions/PetFamily.Discussions.Infrastructure/PetFamily.Discussions.Infrastructure.csproj", "src/Discussions/PetFamily.Discussions.Infrastructure/"]
COPY ["src/Discussions/PetFamily.Discussions.Presentation/PetFamily.Discussions.Presentation.csproj", "src/Discussions/PetFamily.Discussions.Presentation/"]
COPY ["src/Files/PetFamily.Files.Application/PetFamily.Files.Application.csproj", "src/Files/PetFamily.Files.Application/"]
COPY ["src/Files/PetFamily.Files.Infrastructure/PetFamily.Files.Infrastructure.csproj", "src/Files/PetFamily.Files.Infrastructure/"]
COPY ["src/Files/PetFamily.Files.Presentation/PetFamily.Files.Presentation.csproj", "src/Files/PetFamily.Files.Presentation/"]
COPY ["src/Species/PetFamily.Specieses.Application/PetFamily.Specieses.Application.csproj", "src/Species/PetFamily.Specieses.Application/"]
COPY ["src/Volunteers/PetFamily.Volunteers.Contracts/PetFamily.Volunteers.Contracts.csproj", "src/Volunteers/PetFamily.Volunteers.Contracts/"]
COPY ["src/Species/PetFamily.Specieses.Contracts/PetFamily.Specieses.Contracts.csproj", "src/Species/PetFamily.Specieses.Contracts/"]
COPY ["src/Species/PetFamily.Specieses.Domain/PetFamily.Specieses.Domain.csproj", "src/Species/PetFamily.Specieses.Domain/"]
COPY ["src/Species/PetFamily.Specieses.Infrastructure/PetFamily.Specieses.Infrastructure.csproj", "src/Species/PetFamily.Specieses.Infrastructure/"]
COPY ["src/Species/PetFamily.Specieses.Presentation/PetFamily.Specieses.Presentation.csproj", "src/Species/PetFamily.Specieses.Presentation/"]
COPY ["src/VolunteerRequests/PetFamily.VolunteerRequests.Application/PetFamily.VolunteerRequests.Application.csproj", "src/VolunteerRequests/PetFamily.VolunteerRequests.Application/"]
COPY ["src/VolunteerRequests/PetFamily.VolunteerRequests.Contracts/PetFamily.VolunteerRequests.Contracts.csproj", "src/VolunteerRequests/PetFamily.VolunteerRequests.Contracts/"]
COPY ["src/VolunteerRequests/PetFamily.VolunteerRequests.Domain/PetFamily.VolunteerRequests.Domain.csproj", "src/VolunteerRequests/PetFamily.VolunteerRequests.Domain/"]
COPY ["src/VolunteerRequests/PetFamily.VolunteerRequests.Infrastructure/PetFamily.VolunteerRequests.Infrastructure.csproj", "src/VolunteerRequests/PetFamily.VolunteerRequests.Infrastructure/"]
COPY ["src/VolunteerRequests/PetFamily.VolunteerRequests.Presentation/PetFamily.VolunteerRequests.Presentation.csproj", "src/VolunteerRequests/PetFamily.VolunteerRequests.Presentation/"]
COPY ["src/Volunteers/PetFamily.Volunteers.Application/PetFamily.Volunteers.Application.csproj", "src/Volunteers/PetFamily.Volunteers.Application/"]
COPY ["src/Volunteers/PetFamily.Volunteers.Domain/PetFamily.Volunteers.Domain.csproj", "src/Volunteers/PetFamily.Volunteers.Domain/"]
COPY ["src/Volunteers/PetFamily.Volunteers.Infrastructure/PetFamily.Volunteers.Infrastructure.csproj", "src/Volunteers/PetFamily.Volunteers.Infrastructure/"]
COPY ["src/Volunteers/PetFamily.Volunteers.Presentation/PetFamily.Volunteers.Presentation.csproj", "src/Volunteers/PetFamily.Volunteers.Presentation/"]
RUN dotnet restore "./src/PetFamily.Web/PetFamily.Web.csproj"
COPY . .
WORKDIR "/src/src/PetFamily.Web"
RUN dotnet build "./PetFamily.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PetFamily.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetFamily.Web.dll"]