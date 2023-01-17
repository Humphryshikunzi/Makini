mkdir ./app/build -p
sudo dotnet-sdk.dotnet build "Server.csproj" -c Release -o ./app/build
cd ./app/build

sudo dotnet-sdk.dotnet _.Server.dll --urls "https://*"


