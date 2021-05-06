FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS base
WORKDIR /app
RUN apk add --update tzdata
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
RUN apk add --update nodejs npm
WORKDIR /src
COPY ["Challenge/Challenge.csproj", "Challenge/"]
RUN dotnet restore "Challenge/Challenge.csproj"
COPY . .
WORKDIR "/src/Challenge"
RUN dotnet build "Challenge.csproj" -c Release -o /app/build
RUN dotnet publish "Challenge.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Challenge.dll"]