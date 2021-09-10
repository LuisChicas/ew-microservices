FROM mcr.microsoft.com/dotnet/core/sdk:2.2
WORKDIR /app

COPY *.sln .
COPY EasyWalletWeb/*.csproj ./EasyWalletWeb/
COPY EasyWallet.Business/*.csproj ./EasyWallet.Business/
COPY EasyWallet.Business.Tests/*.csproj ./EasyWallet.Business.Tests/
COPY EasyWallet.Data/*.csproj ./EasyWallet.Data/
RUN dotnet restore -r linux-x64

COPY EasyWalletWeb/. ./EasyWalletWeb/
COPY EasyWallet.Business/. ./EasyWallet.Business/
COPY EasyWallet.Business.Tests/. ./EasyWallet.Business.Tests/
COPY EasyWallet.Data/. ./EasyWallet.Data/
WORKDIR /app/EasyWalletWeb
RUN dotnet publish -c Release -o out -r linux-x64

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=0 /app/EasyWalletWeb/out ./
EXPOSE 80
ENTRYPOINT ["dotnet", "EasyWalletWeb.dll"]