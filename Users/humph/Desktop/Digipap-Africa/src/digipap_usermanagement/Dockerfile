FROM mcr.microsoft.com/dotnet/aspnet:6.0 
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /src
COPY ["DigipapUserManagement/DigipapUserManagement.csproj", "DigipapUserManagement/"]
RUN dotnet restore "DigipapUserManagement/DigipapUserManagement.csproj"
COPY . .
WORKDIR "/src/DigipapUserManagement/"
RUN dotnet build "DigipapUserManagement.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DigipapUserManagement.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DigipapUserManagement.dll"]