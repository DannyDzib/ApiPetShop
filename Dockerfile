FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
COPY /app /app
CMD ASPNETCORE_URLS=http://*:$PORT dotnet ApiPetShop.dll
