FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Presentation/TransactionService.Presentation/TransactionService.Api/TransactionService.Api.csproj","src/Presentation/TransactionService.Presentation/TransactionService.Api/"]
COPY ["src/Application/TransactionService.Application/TransactionService.Application.csproj","src/Application/TransactionService.Application/"]
COPY ["src/Application/TransactionService.Messages/TransactionService.Messages.csproj","src/Application/TransactionService.Messages/"]
COPY ["src/Domain/TransactionService.Domain.csproj","src/Application/TransactionService.Domain/"]
COPY ["src/Infrastructure/TransactionService.Infrastructure/TransactionService.Infrastructure.csproj","src/Infrastructure/TransactionService.Infrastructure/"]
COPY ["src/Infrastructure/TransactionService.Persistence.Sql/TransactionService.Persistence.Sql.csproj","src/Infrastructure/Persistence/TransactionService.Persistence.Sql/"]

RUN dotnet restore "src/Presentation/TransactionService.Presentation/TransactionService.Api/TransactionService.Api.csproj"
COPY . .
WORKDIR "src/Presentation/TransactionService.Presentation/TransactionService.Api"
RUN dotnet build "./TransactionService.Api.csproj" -c Release -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TransactionService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TransactionService.Api.dll"]